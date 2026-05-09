using SafeSchool.Api.Features.Learning.Common.StarEvidence;

namespace SafeSchool.Api.Tests.Features.Learning.Fixtures;

public sealed class FakePhase6StarEvidenceExportProvider : IPhase6StarEvidenceExportProvider
{
    public int AvailableStars { get; set; } = 15;
    public int ReservedStars { get; set; } = 3;
    public Phase6StarEvidence ReadEvidence(string tenantId, string studentProfileId) => new(tenantId, studentProfileId, AvailableStars, ReservedStars, DateTimeOffset.UtcNow, true);
}
