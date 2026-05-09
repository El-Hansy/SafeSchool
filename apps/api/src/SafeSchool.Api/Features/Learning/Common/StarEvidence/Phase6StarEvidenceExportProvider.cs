namespace SafeSchool.Api.Features.Learning.Common.StarEvidence;

public sealed record Phase6StarEvidence(string TenantId, string StudentProfileId, int AvailableStars, int ReservedStars, DateTimeOffset FreshAt, bool ReadOnly);

public interface IPhase6StarEvidenceExportProvider
{
    Phase6StarEvidence ReadEvidence(string tenantId, string studentProfileId);
}

public sealed class DefaultPhase6StarEvidenceExportProvider : IPhase6StarEvidenceExportProvider
{
    public Phase6StarEvidence ReadEvidence(string tenantId, string studentProfileId) => new(tenantId, studentProfileId, 0, 0, DateTimeOffset.UtcNow, true);
}
