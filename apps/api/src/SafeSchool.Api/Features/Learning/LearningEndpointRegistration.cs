using SafeSchool.Api.Features.Learning.Assignments;
using SafeSchool.Api.Features.Learning.Behavior;
using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Content;
using SafeSchool.Api.Features.Learning.Quizzes;
using SafeSchool.Api.Features.Learning.Reviews;
using SafeSchool.Api.Features.Learning.Stars;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Learning;

public static class LearningEndpointRegistration
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/learning";
    public const string StudentRoutePrefix = "/api/v1/students/me/learning";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/students/{studentProfileId}/learning";

    public static IServiceCollection AddLearningFeature(this IServiceCollection services)
    {
        services.AddScoped<ILearningClock, SystemLearningClock>();
        services.AddScoped<LearningOperationalService>();
        services.AddScoped<LearningFeatureGate>();
        services.AddScoped<LearningPermissionGuard>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Common.Identity.ILearningStudentProfileProvider, SafeSchool.Api.Features.Learning.Common.Identity.DefaultLearningStudentProfileProvider>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Common.Identity.ILearningGuardianLinkProvider, SafeSchool.Api.Features.Learning.Common.Identity.DefaultLearningGuardianLinkProvider>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Common.Identity.IStaffLearningAssignmentProvider, SafeSchool.Api.Features.Learning.Common.Identity.DefaultStaffLearningAssignmentProvider>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Common.StarEvidence.IPhase6StarEvidenceExportProvider, SafeSchool.Api.Features.Learning.Common.StarEvidence.DefaultPhase6StarEvidenceExportProvider>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Common.Boundaries.LearningPhaseBoundaryGuard>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Common.Idempotency.LearningIdempotencyService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Audit.ILearningAuditWriter, SafeSchool.Api.Features.Learning.Audit.LearningAuditWriter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Audit.LearningStatusEventExporter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Stars.StarBalanceSnapshotService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Courses.CourseService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Groups.LearningGroupService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Groups.StaffLearningAssignmentService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Content.ContentPublicationService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Content.ContentVisibilityService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Progress.LearningProgressService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Content.LearningContentTraceService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Content.CourseContentAuditAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Content.CourseContentStatusEventAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentCreationService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentEligibilityService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentSubmissionService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentSubmissionHistoryService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentReviewService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentExceptionService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentTraceService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentAuditAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentStatusEventAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Assignments.AssignmentLearningOutcomePublisher>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Quizzes.QuizConfigurationService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Quizzes.QuizQuestionRevisionService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Quizzes.QuizEligibilityService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Quizzes.QuizAttemptService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Quizzes.QuizScoringService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Quizzes.QuizFeedbackVisibilityService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Quizzes.QuizTraceService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Quizzes.QuizAuditAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Quizzes.QuizStatusEventAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Stars.StarRuleService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Stars.StarSourceEventService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Stars.StarLedgerService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Stars.StarReservationLifecycleService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Stars.Phase6StarEvidenceExportService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Stars.StarRewardTraceService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Stars.StarRewardAuditAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Stars.StarRewardStatusEventAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Rewards.RewardCatalogService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Rewards.RewardRedemptionService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Behavior.BehaviorCategoryService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Behavior.BehaviorEventService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Behavior.BehaviorVisibilityService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Behavior.BehaviorReviewService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Behavior.BehaviorStarImpactService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Behavior.BehaviorTraceService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Behavior.BehaviorAuditAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Behavior.BehaviorStatusEventAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Configuration.LearningRuleVersionService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Configuration.LearningConfigurationService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Configuration.LearningFeatureSettingsQueryService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Exceptions.LearningExceptionFactory>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Exceptions.LearningExceptionQueryService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Common.Visibility.LearningVisibilityService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Reviews.LearningReviewReasonValidator>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Reviews.ManualLearningReviewService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Reviews.LearningHistoryService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Reviews.LearningReviewSummaryService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Reviews.LearningLifecycleTraceService>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Reviews.LearningReviewAuditAdapter>();
        services.AddScoped<SafeSchool.Api.Features.Learning.Reviews.LearningReviewStatusEventAdapter>();
        return services;
    }

    public static IEndpointRouteBuilder MapLearningEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var schoolGroup = endpoints.MapGroup(SchoolRoutePrefix);
        var studentGroup = endpoints.MapGroup(StudentRoutePrefix);
        var guardianGroup = endpoints.MapGroup(GuardianRoutePrefix);

        schoolGroup.MapGet("/", async (string schoolAccountId, LearningOperationalService service, CancellationToken ct) => Results.Ok(await service.BoardAsync(schoolAccountId, ct)));
        studentGroup.MapGet("/", async (ITenantContext tenantContext, LearningOperationalService service, CancellationToken ct) =>
            Results.Ok(await service.StudentOverviewAsync(GuardianTenantResolver.Resolve(tenantContext), tenantContext.ActorReference ?? "anonymous", ct)));
        guardianGroup.MapGet("/", async (string studentProfileId, ITenantContext tenantContext, LearningOperationalService service, CancellationToken ct) =>
            Results.Ok(await service.GuardianOverviewAsync(GuardianTenantResolver.Resolve(tenantContext), studentProfileId, ct)));

        schoolGroup.MapCourseContentEndpoints();
        schoolGroup.MapAssignmentEndpoints();
        schoolGroup.MapQuizEndpoints();
        schoolGroup.MapStarRewardEndpoints();
        schoolGroup.MapBehaviorEndpoints();
        schoolGroup.MapLearningReviewEndpoints();

        return endpoints;
    }
}
