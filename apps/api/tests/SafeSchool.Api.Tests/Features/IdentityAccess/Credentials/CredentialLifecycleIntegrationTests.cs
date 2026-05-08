using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.Common;
using SafeSchool.Api.Features.IdentityAccess.Credentials;
using SafeSchool.Api.Tests.Features.IdentityAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.Credentials;

public sealed class CredentialLifecycleIntegrationTests
{
    [Fact]
    public async Task CredentialStatusSnapshotService_ExcludesSuspendedCredentials()
    {
        var fixture = new IdentityAccessTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        dbContext.IdentityCredentials.Add(new IdentityCredential
        {
            TenantId = "school-1",
            StudentProfileId = Guid.NewGuid(),
            CredentialType = CredentialType.NfcCard,
            CredentialReference = "card-1",
            CredentialStatus = CredentialStatus.Suspended,
            IssuedBy = "staff:admin",
            StatusReason = "Suspended."
        });
        await dbContext.SaveChangesAsync();
        var service = new CredentialStatusSnapshotService(dbContext);

        var snapshots = await service.CurrentAsync("school-1");

        snapshots.Should().BeEmpty();
    }
}
