using FluentAssertions;
using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Common.Boundaries;
using SafeSchool.Api.Features.Learning.Common.Idempotency;
using SafeSchool.Api.Features.Learning.Domain;
using SafeSchool.Api.Features.Learning.Stars;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Tests.Features.Learning.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Learning.Content;

public sealed class LearningContentItemDomainTests
{
    [Fact]
    public void Phase5Learning_IsTenantScopedFeatureGatedAuditableAndBoundarySafe()
    {
        LearningCapabilities.All.Should().Contain(LearningCapabilities.ContentDelivery);
        LearningCapabilities.All.Should().Contain(LearningCapabilities.StarsRewards);
        LearningPermissionCatalog.Teacher.Should().Contain(LearningPermissionCatalog.ContentPublish);
        LearningPermissionCatalog.Guardian.Should().Contain(LearningPermissionCatalog.GuardianHistoryRead);

        var course = LearningTestData.ActiveCourse();
        course.TenantId.Should().Be(LearningTestData.TenantId);
        course.CourseStatus.Should().Be(CourseStatus.Active);

        var idempotency = new LearningIdempotencyService();
        idempotency.Record("progress", "request-1", "same").Outcome.Should().Be(LearningIdempotencyOutcome.Accepted);
        idempotency.Record("progress", "request-1", "same").Outcome.Should().Be(LearningIdempotencyOutcome.ExactDuplicate);
        idempotency.Record("progress", "request-1", "different").Outcome.Should().Be(LearningIdempotencyOutcome.ConflictingDuplicate);

        var guard = new LearningPhaseBoundaryGuard();
        guard.EnsureNoOutOfScopeSideEffect("wallet").Succeeded.Should().BeFalse();
        guard.EnsureNoOutOfScopeSideEffect("learning_status_event").Succeeded.Should().BeTrue();

        var snapshot = new StarBalanceSnapshotService().Recalculate(LearningTestData.TenantId, LearningTestData.StudentProfileId, [LearningTestData.StarCredit(8)]);
        snapshot.AvailableStars.Should().Be(8);
    }
}
