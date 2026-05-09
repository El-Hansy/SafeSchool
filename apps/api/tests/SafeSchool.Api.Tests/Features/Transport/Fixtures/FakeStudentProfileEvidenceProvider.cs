using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;

namespace SafeSchool.Api.Tests.Features.Transport.Fixtures;

public sealed class FakeStudentProfileEvidenceProvider : IStudentProfileEvidenceProvider
{
    private readonly Dictionary<string, StudentProfileEvidence> _items = [];
    public void Add(StudentProfileEvidence evidence) => _items[$"{evidence.TenantId}:{evidence.StudentProfileId}"] = evidence;
    public Task<StudentProfileEvidence> GetEvidenceAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default) =>
        Task.FromResult(_items.GetValueOrDefault($"{tenantId}:{studentProfileId}", new StudentProfileEvidence(tenantId, studentProfileId, StudentEligibilityStatus.Active)));
}
