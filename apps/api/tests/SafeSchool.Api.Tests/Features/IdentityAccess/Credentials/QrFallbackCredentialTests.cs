using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.Credentials;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Credentials;

public sealed class QrFallbackCredentialTests
{
    [Fact]
    public void QrFallbackCredential_TracksRotationSequenceWithoutRawSecret()
    {
        var qr = new QrFallbackCredential
        {
            TenantId = "school-1",
            IdentityCredentialId = Guid.NewGuid(),
            QrReference = "qr-reference-1",
            RotationSequence = 2,
            RotationReason = "Periodic rotation."
        };

        qr.RotationSequence.Should().Be(2);
        qr.QrReference.Should().NotContain("secret");
    }
}
