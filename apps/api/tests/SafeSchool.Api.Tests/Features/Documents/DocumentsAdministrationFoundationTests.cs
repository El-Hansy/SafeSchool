using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Administration;
using SafeSchool.Api.Features.Documents;
using SafeSchool.Api.Infrastructure.Persistence;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Documents;

public sealed class DocumentsAdministrationFoundationTests
{
    [Fact]
    public async Task Documents_search_and_admin_are_scoped_and_no_side_effect()
    {
        await using var dbContext = CreateDbContext();
        var documents = new DocumentWorkflowService(dbContext);
        var certificates = new CertificateWorkflowService(dbContext);
        var search = new SearchWorkflowService(dbContext);

        DocumentCapabilities.All.Should().Contain(DocumentCapabilities.Search);
        new DocumentBoundaryGuard().Allows("communication_delivery").Should().BeFalse();

        var doc = await documents.UploadAsync("school-demo", new DocumentCommand("Consent", "guardian-consent", "student-1", "identity", "1"));
        doc.Evidence.Should().Contain("audit-written");
        (await documents.UploadAsync("school-demo", new DocumentCommand("Consent", "guardian-consent", "student-1", "identity", "1")))
            .Evidence.Should().Contain("idempotency_duplicate");

        var cert = await certificates.IssueAsync("school-demo", new CertificateCommand("attendance", "student-1", "attendance", "cert-1"));
        (await certificates.VerifyAsync("school-demo", cert.Reference)).VerificationState.Should().Be("Verified");

        (await search.SearchAsync("school-demo", new SearchQueryCommand("Consent", "school"))).SuppressedReasons.Should().Contain("restricted-counts-hidden");
        new AdministrationBoundaryGuard().Allows("wallet").Should().BeFalse();
        (await new AdministrationWorkflowService(dbContext).DashboardAsync("school-demo")).Should().NotBeNull();
    }

    private static SafeSchoolDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new SafeSchoolDbContext(options);
    }
}
