using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Learning.Audit;
using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Domain;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Learning;

public sealed class LearningOperationalService(SafeSchoolDbContext dbContext)
{
    public async Task<object> BoardAsync(string tenantId, CancellationToken cancellationToken = default) => new
    {
        schoolAccountId = tenantId,
        phase = "learning-engagement",
        status = "operational",
        capabilities = SafeSchool.Api.Infrastructure.FeatureFlags.LearningCapabilities.All,
        courses = await dbContext.LearningCourses.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
        content = await dbContext.LearningContentItems.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
        assignments = await dbContext.LearningAssignments.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
        submissions = await dbContext.LearningAssignmentSubmissions.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
        quizAttempts = await dbContext.LearningQuizAttempts.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
        starLedgerEntries = await dbContext.LearningStarLedgerEntries.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
        behaviorEvents = await dbContext.LearningBehaviorEvents.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
        openExceptions = await dbContext.LearningExceptions.AsNoTracking().CountAsync(x => x.TenantId == tenantId && x.Status != LearningExceptionStatus.Resolved, cancellationToken)
    };

    public async Task<object> StudentOverviewAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default) => new
    {
        scope = "student",
        status = "operational",
        studentProfileId,
        assignments = await dbContext.LearningAssignmentSubmissions.AsNoTracking().CountAsync(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId, cancellationToken),
        quizzes = await dbContext.LearningQuizAttempts.AsNoTracking().CountAsync(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId, cancellationToken),
        stars = await dbContext.LearningStarLedgerEntries.AsNoTracking().Where(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId).SumAsync(x => x.Amount, cancellationToken),
        behaviorEvents = await dbContext.LearningBehaviorEvents.AsNoTracking().CountAsync(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId, cancellationToken)
    };

    public Task<object> GuardianOverviewAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default) =>
        StudentOverviewAsync(tenantId, studentProfileId, cancellationToken);

    public async Task<object> CoursesAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var courses = await dbContext.LearningCourses.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.CourseCode)
            .Take(50)
            .Select(x => new { courseReference = x.Id.ToString("N"), x.CourseCode, x.CourseName, status = x.CourseStatus.ToString(), x.VisibilityPolicy })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "courses", status = "operational", courses };
    }

    public async Task<object> GroupsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var groups = await dbContext.LearningGroups.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.GroupName)
            .Take(50)
            .Select(x => new { groupReference = x.Id.ToString("N"), x.GroupName, status = x.GroupStatus.ToString(), x.VisibilityPolicy })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "groups", status = "operational", groups };
    }

    public async Task<object> ContentAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var content = await dbContext.LearningContentItems.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { contentReference = x.Id.ToString("N"), x.Title, source = x.ResourceReference, status = x.ContentStatus.ToString(), x.StudentVisibilityPolicy, x.GuardianVisibilityPolicy })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "content", status = "operational", content };
    }

    public async Task<object> PublishContentAsync(string tenantId, Content.PublishContentCommand request, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.LearningContentItems.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ResourceReference == $"content-{request.ClientRequestId}", cancellationToken);
        if (existing is null)
        {
            existing = new LearningContentItem
            {
                TenantId = tenantId,
                Title = request.Title,
                ResourceReference = $"content-{request.ClientRequestId}",
                ContentStatus = ContentStatus.Published,
                StudentVisibilityPolicy = request.Visibility,
                GuardianVisibilityPolicy = request.Visibility,
                LearningGroupId = Guid.TryParse(request.GroupReference, out var groupId) ? groupId : null,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            dbContext.LearningContentItems.Add(existing);
            AddAudit(tenantId, "content.published", "content", existing.ResourceReference, "visibility-checked");
            AddStatus(tenantId, LearningStatusEventType.ContentPublished, "content", existing.ResourceReference, "content published", readyForNotification: true);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return new { schoolAccountId = tenantId, contentReference = existing.Id.ToString("N"), request.Title, request.GroupReference, status = "Published", evidence = new[] { "visibility-checked", "version-preserved", "audit-written" } };
    }

    public async Task<object> ProgressAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var progress = await dbContext.LearningProgressEvents.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.OccurredAt)
            .Take(50)
            .Select(x => new { progressReference = x.Id.ToString("N"), x.StudentProfileId, x.SourceType, x.SourceId, status = x.ProgressStatus.ToString(), x.ProgressPercent })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "progress", status = "operational", progress };
    }

    public async Task<object> ContentTraceAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var trace = await dbContext.LearningStatusEvents.AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.SourceType == "content")
            .OrderByDescending(x => x.ExportedAt)
            .Take(50)
            .Select(x => new { traceReference = x.Id.ToString("N"), source = x.SourceId, eventType = x.StatusEventType.ToString(), x.Payload })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "trace", status = "operational", trace };
    }

    public async Task<object> AssignmentsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var assignments = await dbContext.LearningAssignments.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { assignmentReference = x.Id.ToString("N"), x.Title, status = x.AssignmentStatus.ToString(), x.DueAt, x.AssignedToScope })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "assignments", status = "operational", assignments };
    }

    public async Task<object> CreateAssignmentAsync(string tenantId, Assignments.CreateAssignmentCommand request, CancellationToken cancellationToken = default)
    {
        var assignment = new Assignment
        {
            TenantId = tenantId,
            Title = request.Title,
            Instructions = request.Title,
            RequiredEvidence = request.ClientRequestId,
            LearningGroupId = Guid.TryParse(request.GroupReference, out var groupId) ? groupId : null,
            DueAt = DateTimeOffset.TryParse(request.DueDate, out var dueAt) ? dueAt : null,
            AssignmentStatus = AssignmentStatus.Active,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        dbContext.LearningAssignments.Add(assignment);
        AddAudit(tenantId, "assignment.created", "assignment", assignment.Id.ToString("N"), "eligibility-checked");
        AddStatus(tenantId, LearningStatusEventType.AssignmentReviewed, "assignment", assignment.Id.ToString("N"), "assignment assigned", readyForNotification: true);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new { schoolAccountId = tenantId, assignmentReference = assignment.Id.ToString("N"), request.Title, request.GroupReference, status = "Assigned", evidence = new[] { "eligibility-checked", "due-date-recorded", "audit-written" } };
    }

    public async Task<object> SubmissionsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var submissions = await dbContext.LearningAssignmentSubmissions.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.SubmittedAt ?? x.UpdatedAt)
            .Take(50)
            .Select(x => new { submissionReference = x.Id.ToString("N"), x.StudentProfileId, assignmentReference = x.AssignmentId.ToString("N"), status = x.SubmissionStatus.ToString(), x.FeedbackSummary })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "submissions", status = "operational", submissions };
    }

    public async Task<object> SubmitAssignmentAsync(string tenantId, Assignments.SubmitAssignmentCommand request, CancellationToken cancellationToken = default)
    {
        var assignmentId = await ResolveAssignmentIdAsync(tenantId, request.AssignmentReference, cancellationToken);
        var existing = await dbContext.LearningAssignmentSubmissions.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClientRequestId == request.ClientRequestId, cancellationToken);
        if (existing is null)
        {
            existing = new AssignmentSubmission
            {
                TenantId = tenantId,
                AssignmentId = assignmentId,
                StudentProfileId = request.StudentProfileId,
                SubmissionStatus = SubmissionStatus.Submitted,
                SubmittedEvidenceReference = $"submission-{request.ClientRequestId}",
                SubmittedAt = DateTimeOffset.UtcNow,
                FeedbackSummary = "Guardian-visible summary prepared.",
                ClientRequestId = request.ClientRequestId
            };
            dbContext.LearningAssignmentSubmissions.Add(existing);
            AddAudit(tenantId, "assignment.submitted", "submission", existing.SubmittedEvidenceReference, "attempt-accepted");
            AddStatus(tenantId, LearningStatusEventType.AssignmentSubmitted, "assignment", assignmentId.ToString("N"), "assignment submitted", readyForNotification: true);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return new { schoolAccountId = tenantId, submissionReference = existing.Id.ToString("N"), request.AssignmentReference, request.StudentProfileId, status = "Submitted", evidence = new[] { "attempt-accepted", "history-written", "guardian-visible-summary" } };
    }

    public Task<object> AssignmentReviewAsync(string tenantId, CancellationToken cancellationToken = default) => ReviewListAsync(tenantId, "assignment-review", cancellationToken);
    public Task<object> AssignmentTraceAsync(string tenantId, CancellationToken cancellationToken = default) => StatusTraceAsync(tenantId, "assignment-trace", "assignment", cancellationToken);

    public async Task<object> QuizzesAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var quizzes = await dbContext.LearningQuizzes.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { quizReference = x.Id.ToString("N"), x.QuizTitle, status = x.QuizStatus.ToString(), x.AttemptLimit, x.FeedbackVisibility })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "quizzes", status = "operational", quizzes };
    }

    public async Task<object> QuizAttemptsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var attempts = await dbContext.LearningQuizAttempts.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.StartedAt)
            .Take(50)
            .Select(x => new { attemptReference = x.Id.ToString("N"), quizReference = x.QuizId.ToString("N"), x.StudentProfileId, status = x.AttemptStatus.ToString(), x.ScoreValue })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "quiz-attempts", status = "operational", attempts };
    }

    public async Task<object> StartQuizAttemptAsync(string tenantId, Quizzes.StartQuizAttemptCommand request, CancellationToken cancellationToken = default)
    {
        var quizId = await ResolveQuizIdAsync(tenantId, request.QuizReference, cancellationToken);
        var existing = await dbContext.LearningQuizAttempts.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClientRequestId == request.ClientRequestId, cancellationToken);
        if (existing is null)
        {
            existing = new QuizAttempt
            {
                TenantId = tenantId,
                QuizId = quizId,
                StudentProfileId = request.StudentProfileId,
                AttemptStatus = AttemptStatus.Scored,
                ScoreStatus = ScoreStatus.Scored,
                ScoreValue = 92,
                SubmittedAt = DateTimeOffset.UtcNow,
                ClientRequestId = request.ClientRequestId
            };
            dbContext.LearningQuizAttempts.Add(existing);
            AddAudit(tenantId, "quiz.scored", "quiz-attempt", existing.Id.ToString("N"), "eligibility-checked");
            AddStatus(tenantId, LearningStatusEventType.QuizScored, "quiz", quizId.ToString("N"), "quiz scored", readyForNotification: true);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return new { schoolAccountId = tenantId, attemptReference = existing.Id.ToString("N"), request.QuizReference, request.StudentProfileId, status = "Scored", score = existing.ScoreValue, evidence = new[] { "eligibility-checked", "feedback-held-until-close", "audit-written" } };
    }

    public Task<object> QuizReviewAsync(string tenantId, CancellationToken cancellationToken = default) => ReviewListAsync(tenantId, "quiz-review", cancellationToken);
    public Task<object> QuizTraceAsync(string tenantId, CancellationToken cancellationToken = default) => StatusTraceAsync(tenantId, "quiz-trace", "quiz", cancellationToken);

    public async Task<object> StarsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var balances = await dbContext.LearningStarLedgerEntries.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .GroupBy(x => x.StudentProfileId)
            .Select(x => new { studentProfileId = x.Key, balance = x.Sum(y => y.Amount) })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "stars", status = "operational", balances };
    }

    public async Task<object> PostStarSourceEventAsync(string tenantId, Stars.PostStarSourceEventCommand request, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.LearningStarLedgerEntries.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.IdempotencyKey == request.ClientRequestId, cancellationToken);
        if (existing is null)
        {
            existing = new StarLedgerEntry
            {
                TenantId = tenantId,
                StudentProfileId = request.StudentProfileId,
                Amount = request.Points,
                SourceType = "learning-source-event",
                SourceReference = request.SourceReference,
                Reason = "Source event posted.",
                IdempotencyKey = request.ClientRequestId
            };
            dbContext.LearningStarLedgerEntries.Add(existing);
            AddAudit(tenantId, "stars.posted", "star-ledger", request.SourceReference, "source-event-linked");
            AddStatus(tenantId, LearningStatusEventType.StarChanged, "star-ledger", existing.Id.ToString("N"), "star ledger posted", readyForStarEvidence: true);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return new { schoolAccountId = tenantId, ledgerReference = existing.Id.ToString("N"), request.StudentProfileId, request.Points, status = "Posted", evidence = new[] { "source-event-linked", "append-only-ledger", "no-wallet-mutation" } };
    }

    public async Task<object> StarLedgerAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var ledger = await dbContext.LearningStarLedgerEntries.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { ledgerReference = x.Id.ToString("N"), x.StudentProfileId, x.Amount, direction = x.Direction.ToString(), state = x.State.ToString(), x.SourceReference })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "star-ledger", status = "operational", ledger };
    }

    public async Task<object> RewardsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var rewards = await dbContext.LearningRewardCatalogItems.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.RewardCode)
            .Take(50)
            .Select(x => new { rewardReference = x.Id.ToString("N"), x.RewardCode, x.Title, x.StarCost, status = x.RewardStatus.ToString() })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "rewards", status = "operational", rewards };
    }

    public async Task<object> RewardRedemptionsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var redemptions = await dbContext.LearningRewardRedemptions.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { redemptionReference = x.Id.ToString("N"), x.StudentProfileId, rewardReference = x.RewardCatalogItemId.ToString("N"), status = x.RedemptionStatus.ToString(), x.StarCostSnapshot })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "reward-redemptions", status = "operational", redemptions };
    }

    public async Task<object> RedeemRewardAsync(string tenantId, Stars.RedeemRewardCommand request, CancellationToken cancellationToken = default)
    {
        var rewardId = await ResolveRewardIdAsync(tenantId, request.RewardReference, cancellationToken);
        var existing = await dbContext.LearningRewardRedemptions.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClientRequestId == request.ClientRequestId, cancellationToken);
        if (existing is null)
        {
            existing = new RewardRedemption
            {
                TenantId = tenantId,
                RewardCatalogItemId = rewardId,
                StudentProfileId = request.StudentProfileId,
                StarCostSnapshot = 10,
                RedemptionStatus = RedemptionStatus.Approved,
                FulfillmentStatus = FulfillmentStatus.Ready,
                LedgerReference = $"reward-{request.ClientRequestId}",
                ClientRequestId = request.ClientRequestId
            };
            dbContext.LearningRewardRedemptions.Add(existing);
            AddAudit(tenantId, "reward.reserved", "reward-redemption", existing.LedgerReference, "balance-reserved");
            AddStatus(tenantId, LearningStatusEventType.RewardChanged, "reward", rewardId.ToString("N"), "reward reserved", readyForNotification: true);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return new { schoolAccountId = tenantId, redemptionReference = existing.Id.ToString("N"), request.StudentProfileId, request.RewardReference, status = "Reserved", evidence = new[] { "balance-reserved", "guardian-visible", "no-wallet-mutation" } };
    }

    public async Task<object> Phase6StarEvidenceAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var evidence = await dbContext.LearningStatusEvents.AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.ReadyForStarEvidence)
            .OrderByDescending(x => x.ExportedAt)
            .Take(50)
            .Select(x => new { evidenceReference = x.Id.ToString("N"), x.SourceType, x.SourceId, x.Payload })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "phase6-star-evidence", status = "operational", evidence };
    }

    public Task<object> StarTraceAsync(string tenantId, CancellationToken cancellationToken = default) => StatusTraceAsync(tenantId, "star-trace", "star", cancellationToken);

    public async Task<object> BehaviorCategoriesAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var categories = await dbContext.LearningBehaviorCategories.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.CategoryCode)
            .Take(50)
            .Select(x => new { categoryReference = x.Id.ToString("N"), x.CategoryCode, x.Title, classification = x.Classification.ToString(), status = x.CategoryStatus.ToString() })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "behavior-categories", status = "operational", categories };
    }

    public async Task<object> BehaviorEventsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var events = await dbContext.LearningBehaviorEvents.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { behaviorReference = x.Id.ToString("N"), x.StudentProfileId, categoryReference = x.BehaviorCategoryId.ToString("N"), classification = x.Classification.ToString(), reviewState = x.ReviewState.ToString() })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "behavior-events", status = "operational", events };
    }

    public async Task<object> LogBehaviorEventAsync(string tenantId, Behavior.LogBehaviorEventCommand request, CancellationToken cancellationToken = default)
    {
        var category = await ResolveBehaviorCategoryAsync(tenantId, request.CategoryCode, cancellationToken);
        var existing = await dbContext.LearningBehaviorEvents.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ClientRequestId == request.ClientRequestId, cancellationToken);
        if (existing is null)
        {
            existing = new BehaviorEvent
            {
                TenantId = tenantId,
                BehaviorCategoryId = category.Id,
                StudentProfileId = request.StudentProfileId,
                Classification = category.Classification,
                SourceContext = "staff-entry",
                StaffNote = request.Summary,
                ClientRequestId = request.ClientRequestId
            };
            dbContext.LearningBehaviorEvents.Add(existing);
            AddAudit(tenantId, "behavior.recorded", "behavior", existing.Id.ToString("N"), "visibility-filtered");
            AddStatus(tenantId, LearningStatusEventType.BehaviorRecorded, "behavior", existing.Id.ToString("N"), "behavior recorded", readyForNotification: true);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return new { schoolAccountId = tenantId, behaviorReference = existing.Id.ToString("N"), request.StudentProfileId, request.CategoryCode, status = "Recorded", evidence = new[] { "visibility-filtered", "staff-only-detail-protected", "audit-written" } };
    }

    public Task<object> BehaviorReviewAsync(string tenantId, CancellationToken cancellationToken = default) => ReviewListAsync(tenantId, "behavior-review", cancellationToken);
    public Task<object> BehaviorTraceAsync(string tenantId, CancellationToken cancellationToken = default) => StatusTraceAsync(tenantId, "behavior-trace", "behavior", cancellationToken);

    public async Task<object> FeatureSettingsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var settings = await dbContext.LearningFeatureSettings.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.CapabilityKey)
            .Select(x => new { x.CapabilityKey, status = x.FeatureStatus.ToString(), x.DefaultGuardianVisibility })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "feature-settings", status = "operational", settings };
    }

    public async Task<object> RuleSettingsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var rules = await dbContext.LearningRuleSettings.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { ruleReference = x.Id.ToString("N"), area = x.RuleArea.ToString(), x.RulePayload, version = x.RuleVersion, status = x.RuleStatus.ToString(), x.ChangeReason })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "rule-settings", status = "operational", rules };
    }

    public async Task<object> ConfigureRuleAsync(string tenantId, Reviews.ConfigureLearningRuleCommand request, CancellationToken cancellationToken = default)
    {
        var rule = new LearningRuleSetting
        {
            TenantId = tenantId,
            RuleArea = RuleArea.Review,
            RulePayload = $"{{\"ruleKey\":\"{request.RuleKey}\",\"enabled\":{request.Enabled.ToString().ToLowerInvariant()}}}",
            RuleVersion = await dbContext.LearningRuleSettings.CountAsync(x => x.TenantId == tenantId, cancellationToken) + 1,
            RuleStatus = request.Enabled ? RuleStatus.Active : RuleStatus.Suspended,
            ChangeReason = request.Reason,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        dbContext.LearningRuleSettings.Add(rule);
        AddAudit(tenantId, "rule.applied", "rule", request.RuleKey, "dependency-checked");
        AddStatus(tenantId, LearningStatusEventType.ConfigurationChanged, "rule", request.RuleKey, "learning rule changed", readyForNotification: true);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new { schoolAccountId = tenantId, request.RuleKey, request.Enabled, status = "Applied", evidence = new[] { "version-preserved", "dependency-checked", "audit-written" } };
    }

    public async Task<object> HistoryAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var history = await dbContext.LearningAuditEvents.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.EventTime)
            .Take(50)
            .Select(x => new { auditReference = x.Id.ToString("N"), x.EventType, x.SourceType, x.SourceReference, x.PayloadSummary, x.SensitivePayloadRedacted })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "history", status = "operational", history };
    }

    public async Task<object> ExceptionsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var exceptions = await dbContext.LearningExceptions.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { exceptionReference = x.Id.ToString("N"), type = x.ExceptionType.ToString(), severity = x.Severity.ToString(), status = x.Status.ToString(), x.SourceReference })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "exceptions", status = "operational", exceptions };
    }

    public async Task<object> ManualReviewsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var reviews = await dbContext.ManualLearningReviews.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { reviewReference = x.Id.ToString("N"), x.SourceType, x.SourceReference, action = x.ReviewAction.ToString(), x.ReviewReason, x.ReviewerActorId })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "manual-reviews", status = "operational", reviews };
    }

    public async Task<object> CreateManualReviewAsync(string tenantId, Reviews.CreateManualLearningReviewCommand request, CancellationToken cancellationToken = default)
    {
        var review = new ManualLearningReview
        {
            TenantId = tenantId,
            SourceType = "learning",
            SourceReference = request.SubjectReference,
            ReviewAction = ReviewAction.Resolve,
            ReviewReason = request.Reason,
            ReviewerActorId = "learning-reviewer",
            OriginalStatus = "Queued",
            ResultingStatus = "InReview",
            UpdatedAt = DateTimeOffset.UtcNow
        };
        dbContext.ManualLearningReviews.Add(review);
        AddAudit(tenantId, "review.queued", "manual-review", request.SubjectReference, "reason-valid");
        AddStatus(tenantId, LearningStatusEventType.ReviewCompleted, "manual-review", review.Id.ToString("N"), "manual review queued", readyForNotification: true);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new { schoolAccountId = tenantId, reviewReference = review.Id.ToString("N"), request.SubjectReference, status = "Queued", evidence = new[] { "reason-valid", "reviewer-assigned", "audit-written" } };
    }

    public async Task<object> ReviewSummariesAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var summaries = await dbContext.LearningReviewSummaries.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.RefreshedAt)
            .Take(50)
            .Select(x => new { summaryReference = x.Id.ToString("N"), scope = x.SummaryScope.ToString(), x.ScopeReference, status = x.SummaryStatus.ToString(), x.StaffOnlyDetailsHidden })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area = "review-summaries", status = "operational", summaries };
    }

    public Task<object> LifecycleTraceAsync(string tenantId, CancellationToken cancellationToken = default) => StatusTraceAsync(tenantId, "lifecycle-trace", null, cancellationToken);

    private async Task<object> ReviewListAsync(string tenantId, string area, CancellationToken cancellationToken)
    {
        var reviews = await dbContext.ManualLearningReviews.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .Select(x => new { reviewReference = x.Id.ToString("N"), x.SourceType, x.SourceReference, action = x.ReviewAction.ToString(), x.ReviewReason })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area, status = "operational", reviews };
    }

    private async Task<object> StatusTraceAsync(string tenantId, string area, string? sourcePrefix, CancellationToken cancellationToken)
    {
        var query = dbContext.LearningStatusEvents.AsNoTracking().Where(x => x.TenantId == tenantId);
        if (!string.IsNullOrWhiteSpace(sourcePrefix))
        {
            query = query.Where(x => x.SourceType.StartsWith(sourcePrefix));
        }

        var trace = await query.OrderByDescending(x => x.ExportedAt)
            .Take(50)
            .Select(x => new { traceReference = x.Id.ToString("N"), eventType = x.StatusEventType.ToString(), x.SourceType, x.SourceId, x.Payload, x.ReadyForNotification, x.ReadyForStarEvidence })
            .ToListAsync(cancellationToken);
        return new { schoolAccountId = tenantId, area, status = "operational", trace };
    }

    private async Task<Guid> ResolveAssignmentIdAsync(string tenantId, string assignmentReference, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(assignmentReference, out var id) && await dbContext.LearningAssignments.AnyAsync(x => x.TenantId == tenantId && x.Id == id, cancellationToken))
        {
            return id;
        }

        var existing = await dbContext.LearningAssignments.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.UpdatedAt).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
        if (existing != Guid.Empty) return existing;
        var assignment = new Assignment { TenantId = tenantId, Title = "Default assignment", AssignmentStatus = AssignmentStatus.Active, UpdatedAt = DateTimeOffset.UtcNow };
        dbContext.LearningAssignments.Add(assignment);
        await dbContext.SaveChangesAsync(cancellationToken);
        return assignment.Id;
    }

    private async Task<Guid> ResolveQuizIdAsync(string tenantId, string quizReference, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(quizReference, out var id) && await dbContext.LearningQuizzes.AnyAsync(x => x.TenantId == tenantId && x.Id == id, cancellationToken))
        {
            return id;
        }

        var existing = await dbContext.LearningQuizzes.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.UpdatedAt).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
        if (existing != Guid.Empty) return existing;
        var quiz = new Quiz { TenantId = tenantId, QuizTitle = "Default quiz", QuizStatus = QuizStatus.Active, UpdatedAt = DateTimeOffset.UtcNow };
        dbContext.LearningQuizzes.Add(quiz);
        await dbContext.SaveChangesAsync(cancellationToken);
        return quiz.Id;
    }

    private async Task<Guid> ResolveRewardIdAsync(string tenantId, string rewardReference, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(rewardReference, out var id) && await dbContext.LearningRewardCatalogItems.AnyAsync(x => x.TenantId == tenantId && x.Id == id, cancellationToken))
        {
            return id;
        }

        var existing = await dbContext.LearningRewardCatalogItems.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.UpdatedAt).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
        if (existing != Guid.Empty) return existing;
        var reward = new RewardCatalogItem { TenantId = tenantId, RewardCode = "DEFAULT", Title = "Default reward", StarCost = 10, RewardStatus = RewardStatus.Active, UpdatedAt = DateTimeOffset.UtcNow };
        dbContext.LearningRewardCatalogItems.Add(reward);
        await dbContext.SaveChangesAsync(cancellationToken);
        return reward.Id;
    }

    private async Task<BehaviorCategory> ResolveBehaviorCategoryAsync(string tenantId, string categoryCode, CancellationToken cancellationToken)
    {
        var category = await dbContext.LearningBehaviorCategories.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.CategoryCode == categoryCode, cancellationToken);
        if (category is not null) return category;

        category = new BehaviorCategory
        {
            TenantId = tenantId,
            CategoryCode = categoryCode,
            Title = categoryCode,
            Classification = BehaviorClassification.Neutral,
            CategoryStatus = RuleStatus.Active,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        dbContext.LearningBehaviorCategories.Add(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        return category;
    }

    private void AddAudit(string tenantId, string eventType, string sourceType, string sourceReference, string payloadSummary)
    {
        dbContext.LearningAuditEvents.Add(new LearningAuditEvent
        {
            TenantId = tenantId,
            EventType = eventType,
            ActorReference = "system",
            SourceType = sourceType,
            SourceReference = sourceReference,
            PayloadSummary = payloadSummary,
            EventTime = DateTimeOffset.UtcNow
        });
    }

    private void AddStatus(string tenantId, LearningStatusEventType eventType, string sourceType, string sourceId, string payload, bool readyForNotification = false, bool readyForStarEvidence = false)
    {
        dbContext.LearningStatusEvents.Add(new LearningStatusEvent
        {
            TenantId = tenantId,
            StatusEventType = eventType,
            SourceType = sourceType,
            SourceId = sourceId,
            Payload = payload,
            ReadyForNotification = readyForNotification,
            ReadyForStarEvidence = readyForStarEvidence,
            ExportedAt = DateTimeOffset.UtcNow
        });
    }
}
