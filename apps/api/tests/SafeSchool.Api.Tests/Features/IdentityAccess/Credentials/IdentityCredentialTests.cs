using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Features.IdentityAccess.Credentials;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Credentials;

public sealed class IdentityCredentialTests
{
    [Fact]
    public void IsCurrent_OnlyAllowsActiveCredentialInsideValidityWindow()
    {
        var credential = new IdentityCredential
        {
            TenantId = "school-1",
            StudentProfileId = Guid.NewGuid(),
            CredentialType = CredentialType.NfcCard,
            CredentialReference = "card-1",
            CredentialStatus = CredentialStatus.Active,
            ValidFrom = DateTimeOffset.UtcNow.AddDays(-1),
            IssuedBy = "staff:admin",
            StatusReason = "Issued after review."
        };

        credential.IsCurrent(DateTimeOffset.UtcNow).Should().BeTrue();
    }
}
