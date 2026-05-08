using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Common.Identity;

public sealed record StudentProfileEvidence(string TenantId, string StudentProfileId, StudentEligibilityStatus Status, string DisplayName = "");

public interface IStudentProfileEvidenceProvider
{
    Task<StudentProfileEvidence> GetEvidenceAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default);
}

public sealed class DefaultStudentProfileEvidenceProvider : IStudentProfileEvidenceProvider
{
    public Task<StudentProfileEvidence> GetEvidenceAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default) =>
        Task.FromResult(new StudentProfileEvidence(tenantId, studentProfileId, StudentEligibilityStatus.Active));
}
