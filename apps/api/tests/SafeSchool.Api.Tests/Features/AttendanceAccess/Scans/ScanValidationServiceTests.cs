using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Scans;

public sealed class ScanValidationServiceTests
{
    private readonly AttendanceAccessTestFixture _fixture = new();

    [Fact]
    public async Task ValidateAsync_AllowsActiveCredentialAndFlagsInvalidCredential()
    {
        await using var dbContext = _fixture.CreateDbContext();
        var seeded = await _fixture.SeedActiveGateAsync(dbContext);
        var evidence = new FakeIdentityCredentialEvidenceProvider();
        evidence.Add(AttendanceAccessTestData.ActiveCredential());
        evidence.Add(new CredentialEvidence("school-1", "student-suspended", "credential-suspended", CredentialEvidenceStatus.Suspended));
        var service = new ScanValidationService(dbContext, _fixture.Guard(dbContext, _fixture.Tenant()), evidence);

        var accepted = await service.ValidateAsync("school-1", AttendanceAccessTestData.EntryScan(seeded.Gate.Id, seeded.ScanPoint.Id));
        var flagged = await service.ValidateAsync("school-1", AttendanceAccessTestData.EntryScan(seeded.Gate.Id, seeded.ScanPoint.Id, "credential-suspended", "scan-2"));

        accepted.Allowed.Should().BeTrue();
        flagged.Status.Should().Be(ScanEventStatus.Flagged);
    }
}

