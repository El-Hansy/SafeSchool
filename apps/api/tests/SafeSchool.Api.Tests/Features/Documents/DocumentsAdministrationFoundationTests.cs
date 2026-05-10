using FluentAssertions;
using SafeSchool.Api.Features.Administration;
using SafeSchool.Api.Features.Documents;
using Xunit;
namespace SafeSchool.Api.Tests.Features.Documents;
public sealed class DocumentsAdministrationFoundationTests { [Fact] public void Documents_search_and_admin_are_scoped_and_no_side_effect() { DocumentCapabilities.All.Should().Contain(DocumentCapabilities.Search); new DocumentBoundaryGuard().Allows("communication_delivery").Should().BeFalse(); var doc = new DocumentWorkflowService().Upload(new DocumentCommand("Consent", "student", "student-1", "identity", "1")); doc.Evidence.Should().Contain("audit-written"); new CertificateWorkflowService().Verify("CERT-1").VerificationState.Should().Be("Verified"); new SearchWorkflowService().Search(new SearchQueryCommand("Amina", "school")).SuppressedReasons.Should().Contain("restricted-counts-hidden"); new AdministrationBoundaryGuard().Allows("wallet").Should().BeFalse(); new AdministrationWorkflowService().Dashboard("school-demo").Should().NotBeNull(); } }
