using System.Text.Json;
using FluentAssertions;
using SafeSchool.Api.Features.Administration;
using SafeSchool.Api.Features.Communications;
using SafeSchool.Api.Features.Complaints;
using SafeSchool.Api.Features.Documents;

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

        var idempotency = new CommunicationIdempotencyService();
        var service = new CommunicationWorkflowService(idempotency);
        service.AcceptSourceEvent(new NotificationSourceEventRequest("complaints", "CMP-1", "status", "req-1"))
            .Evidence.Should().Contain("source-read-only");
        service.AcceptSourceEvent(new NotificationSourceEventRequest("complaints", "CMP-1", "status", "req-1"))
            .Status.Should().Be(CommunicationIdempotencyOutcome.Duplicate.ToString());
        service.SendMessage(new MessageRequest("Safety", "Update", "guardians", "msg-1"))
            .Evidence.Should().Contain("moderation-checked");
        service.PublishBroadcast(new BroadcastRequest("Safety", "Update", "guardians", "b1"))
            .Evidence.Should().Contain("audience-snapshot");
        CommunicationWorkflowService.NotificationCenter("guardian").Single().Recipients.Should().Contain("guardian");
    }

    public static void ComplaintContractsHold()
    {
        ComplaintCapabilities.All.Should().Contain(ComplaintCapabilities.Submission);
        new ComplaintBoundaryGuard().Allows("wallet").Should().BeFalse();

        var idempotency = new ComplaintIdempotencyService();
        var service = new ComplaintWorkflowService(idempotency);
        var request = new ComplaintSubmissionRequest("student-1", "safety", "Concern", "Review", "req-1");
        service.Submit("school-live", request).AuditTrail.Should().Contain("audit-written");
        service.Submit("school-live", request).Status.Should().Be("Received");
        service.Submit("school-live", request with { Description = "Changed" }).Status.Should().Be("ManualReviewRequired");
        service.Assign("cmp-1", new ComplaintActionRequest("assign", "owner")).AuditTrail.Should().Contain("owner");
        service.Escalate("cmp-1", new ComplaintActionRequest("escalate", "sla")).Status.Should().Be("Escalated");
        service.Resolve("cmp-1", new ComplaintActionRequest("resolve", "fixed")).VisibleSummary.Should().Contain("Resolution");
    }

    public static void DocumentContractsHold()
    {
        DocumentCapabilities.All.Should().Contain(DocumentCapabilities.Search);
        new DocumentBoundaryGuard().Allows("wallet").Should().BeFalse();

        var documents = new DocumentWorkflowService();
        documents.Upload(new DocumentCommand("Consent", "medical", "student-1", "medical", "doc-1"))
            .Evidence.Should().Contain("metadata-validated");
        documents.PlaceHold("doc-1", new DocumentActionCommand("hold", "legal"))
            .Status.Should().Be("LegalHold");
        documents.Export("doc-1", new DocumentActionCommand("export", "audit"))
            .Evidence.Should().Contain("scope-validated");

        var certificates = new CertificateWorkflowService();
        certificates.Issue(new CertificateCommand("attendance", "student-1", "attendance", "cert-1"))
            .Evidence.Should().Contain("audit-written");
        certificates.Verify("cert-1").VerificationState.Should().Be("Verified");

        var search = new SearchWorkflowService();
        search.Search(new SearchQueryCommand("Amina", "guardian")).SuppressedReasons.Should().Contain("restricted-counts-hidden");
        JsonSerializer.Serialize(search.Open("entry-1")).Should().Contain("permission-revalidated");
    }
}
