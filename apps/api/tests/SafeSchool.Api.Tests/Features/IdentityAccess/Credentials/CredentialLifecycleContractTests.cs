using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.Credentials;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Credentials;

public sealed class CredentialLifecycleContractTests
{
    [Fact]
    public void IssueNfcRequest_ContainsRetrySafeClientRequestId()
    {
        var request = new IssueNfcCredentialRequest(
            "card-ref-1",
            "Front office card",
            DateTimeOffset.UtcNow,
            null,
            "Issued after identity review.",
            "request-1");

        request.ClientRequestId.Should().Be("request-1");
    }
}
