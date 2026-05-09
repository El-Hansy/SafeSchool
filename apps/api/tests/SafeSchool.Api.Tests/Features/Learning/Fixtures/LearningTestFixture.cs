using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Common.Boundaries;
using SafeSchool.Api.Features.Learning.Common.Idempotency;
using SafeSchool.Api.Features.Learning.Stars;

namespace SafeSchool.Api.Tests.Features.Learning.Fixtures;

public sealed class LearningTestFixture
{
    public LearningPhaseBoundaryGuard BoundaryGuard { get; } = new();
    public LearningIdempotencyService Idempotency { get; } = new();
    public StarBalanceSnapshotService StarBalances { get; } = new();
    public IReadOnlyList<string> Capabilities => SafeSchool.Api.Infrastructure.FeatureFlags.LearningCapabilities.All;
    public IReadOnlyList<string> TeacherPermissions => LearningPermissionCatalog.Teacher;
}
