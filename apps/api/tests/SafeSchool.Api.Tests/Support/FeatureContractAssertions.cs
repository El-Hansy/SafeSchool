using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Administration;
using SafeSchool.Api.Features.Communications;
using SafeSchool.Api.Features.Complaints;
using SafeSchool.Api.Features.Documents;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Tests.Support;

public static class FeatureContractAssertions
{
    public static void AdministrationContractsHold()
    {
        AdministrationCapabilities.All.Should().Contain(AdministrationCapabilities.AuditTrail);
        new AdministrationBoundaryGuard().Allows("wallet").Should().BeFalse();

        var service = new AdministrationWorkflowService();
        JsonSerializer.Serialize(service.Dashboard("school-live"))
            .Should().Contain("school-live").And.Contain("demo-ready");
        JsonSerializer.Serialize(service.Configure(new ConfigurationCommand("mobile.app", true, "pilot", "cfg-1")))
            .Should().Contain("dependency-validated");
        JsonSerializer.Serialize(service.ExportAudit(new ExportCommand("tenant", "audit request", "exp-1")))
            .Should().Contain("audit-export-exp-1");
        JsonSerializer.Serialize(service.OpenIncident("alert-1"))
            .Should().Contain("owner-assigned");
    }

    public static void CommunicationContractsHold()
    {
        CommunicationCapabilities.All.Should().Contain(CommunicationCapabilities.NotificationCenter);
        new CommunicationBoundaryGuard().Allows("complaint_resolution").Should().BeFalse();

        using var dbContext = CreateDbContext();
        var service = new CommunicationWorkflowService(dbContext);
        Await(service.AcceptSourceEventAsync("school-live", new NotificationSourceEventRequest("complaints", "CMP-1", "status", "req-1")))
            .Evidence.Should().Contain("source-read-only");
        Await(service.AcceptSourceEventAsync("school-live", new NotificationSourceEventRequest("complaints", "CMP-1", "status", "req-1")))
            .Evidence.Should().Contain("idempotency_duplicate");
        Await(service.SendMessageAsync("school-live", new MessageRequest("Safety", "Update", "guardians", "msg-1")))
            .Evidence.Should().Contain("moderation-checked");
        Await(service.PublishBroadcastAsync("school-live", new BroadcastRequest("Safety", "Update", "guardians", "b1")))
            .Evidence.Should().Contain("audience-snapshot");
        Await(service.NotificationCenterAsync("school-live", "guardian")).Should().NotBeEmpty();
    }

    public static void ComplaintContractsHold()
    {
        ComplaintCapabilities.All.Should().Contain(ComplaintCapabilities.Submission);
        new ComplaintBoundaryGuard().Allows("wallet").Should().BeFalse();

        using var dbContext = CreateDbContext();
        var service = new ComplaintWorkflowService(dbContext);
        var request = new ComplaintSubmissionRequest("student-1", "safety", "Concern", "Review", "req-1");
        var submitted = Await(service.SubmitAsync("school-live", request));
        submitted.AuditTrail.Should().Contain("audit-written");
        Await(service.SubmitAsync("school-live", request)).Status.Should().Be("Received");
        Await(service.SubmitAsync("school-live", request with { Description = "Changed" })).Status.Should().Be("ManualReviewRequired");
        Await(service.AssignAsync("school-live", submitted.ComplaintId, new ComplaintActionRequest("assign", "owner"))).AuditTrail.Should().Contain("owner");
        Await(service.EscalateAsync("school-live", submitted.ComplaintId, new ComplaintActionRequest("escalate", "sla"))).Status.Should().Be("Escalated");
        Await(service.ResolveAsync("school-live", submitted.ComplaintId, new ComplaintActionRequest("resolve", "fixed"))).VisibleSummary.Should().Contain("Resolution");
    }

    public static void DocumentContractsHold()
    {
        DocumentCapabilities.All.Should().Contain(DocumentCapabilities.Search);
        new DocumentBoundaryGuard().Allows("wallet").Should().BeFalse();

        using var dbContext = CreateDbContext();
        var documents = new DocumentWorkflowService(dbContext);
        var uploaded = Await(documents.UploadAsync("school-live", new DocumentCommand("Consent", "guardian-consent", "student-1", "medical", "doc-1")));
        uploaded
            .Evidence.Should().Contain("metadata-validated");
        Await(documents.UploadAsync("school-live", new DocumentCommand("Consent", "guardian-consent", "student-1", "medical", "doc-1")))
            .Evidence.Should().Contain("idempotency_duplicate");
        Await(documents.PlaceHoldAsync("school-live", uploaded.Reference, new DocumentActionCommand("hold", "legal")))
            .Status.Should().Be("LegalHold");
        Await(documents.ExportAsync("school-live", uploaded.Reference, new DocumentActionCommand("export", "audit")))
            .Evidence.Should().Contain("scope-validated");

        var certificates = new CertificateWorkflowService(dbContext);
        var issued = Await(certificates.IssueAsync("school-live", new CertificateCommand("attendance", "student-1", "attendance", "cert-1")));
        issued
            .Evidence.Should().Contain("audit-written");
        Await(certificates.VerifyAsync("school-live", issued.Reference)).VerificationState.Should().Be("Verified");

        var search = new SearchWorkflowService(dbContext);
        Await(search.SearchAsync("school-live", new SearchQueryCommand("Consent", "guardian"))).SuppressedReasons.Should().Contain("restricted-counts-hidden");
        JsonSerializer.Serialize(Await(search.OpenAsync("school-live", uploaded.Reference))).Should().Contain("permission-revalidated");
    }

    private static SafeSchoolDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new SafeSchoolDbContext(options);
    }

    private static T Await<T>(Task<T> task) => task.GetAwaiter().GetResult();
}
