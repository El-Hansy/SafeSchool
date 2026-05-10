using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Learning.Audit;
using SafeSchool.Api.Features.Learning.Common.Idempotency;
using SafeSchool.Api.Features.Learning.Domain;

namespace SafeSchool.Api.Features.Learning.Persistence;

public static class LearningEntityTypeConfigurations
{
    public static ModelBuilder ApplyLearningEntityConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().ToTable("learning_courses").HasIndex(x => x.TenantId);
        modelBuilder.Entity<LearningGroup>().ToTable("learning_learning_groups").HasIndex(x => x.TenantId);
        modelBuilder.Entity<LearningGroupMembership>().ToTable("learning_learning_group_memberships").HasIndex(x => x.TenantId);
        modelBuilder.Entity<StaffLearningAssignment>().ToTable("learning_staff_learning_assignments").HasIndex(x => x.TenantId);
        modelBuilder.Entity<LearningContentItem>().ToTable("learning_learning_content_items").HasIndex(x => x.TenantId);
        modelBuilder.Entity<LearningProgressEvent>().ToTable("learning_learning_progress_events").HasIndex(x => x.TenantId);
        modelBuilder.Entity<Assignment>().ToTable("learning_assignments").HasIndex(x => x.TenantId);
        modelBuilder.Entity<AssignmentSubmission>().ToTable("learning_assignment_submissions").HasIndex(x => x.TenantId);
        modelBuilder.Entity<Quiz>().ToTable("learning_quizs").HasIndex(x => x.TenantId);
        modelBuilder.Entity<QuizQuestion>().ToTable("learning_quiz_questions").HasIndex(x => x.TenantId);
        modelBuilder.Entity<QuizAttempt>().ToTable("learning_quiz_attempts").HasIndex(x => x.TenantId);
        modelBuilder.Entity<QuizResponse>().ToTable("learning_quiz_responses").HasIndex(x => x.TenantId);
        modelBuilder.Entity<StarRuleSetting>().ToTable("learning_star_rule_settings").HasIndex(x => x.TenantId);
        modelBuilder.Entity<StarLedgerEntry>().ToTable("learning_star_ledger_entrys").HasIndex(x => x.TenantId);
        modelBuilder.Entity<StarBalanceSnapshot>().ToTable("learning_star_balance_snapshots").HasIndex(x => x.TenantId);
        modelBuilder.Entity<RewardCatalogItem>().ToTable("learning_reward_catalog_items").HasIndex(x => x.TenantId);
        modelBuilder.Entity<RewardRedemption>().ToTable("learning_reward_redemptions").HasIndex(x => x.TenantId);
        modelBuilder.Entity<BehaviorCategory>().ToTable("learning_behavior_categorys").HasIndex(x => x.TenantId);
        modelBuilder.Entity<BehaviorEvent>().ToTable("learning_behavior_events").HasIndex(x => x.TenantId);
        modelBuilder.Entity<LearningException>().ToTable("learning_learning_exceptions").HasIndex(x => x.TenantId);
        modelBuilder.Entity<ManualLearningReview>().ToTable("learning_manual_learning_reviews").HasIndex(x => x.TenantId);
        modelBuilder.Entity<LearningRuleSetting>().ToTable("learning_learning_rule_settings").HasIndex(x => x.TenantId);
        modelBuilder.Entity<LearningReviewSummary>().ToTable("learning_learning_review_summarys").HasIndex(x => x.TenantId);
        modelBuilder.Entity<SchoolAccountFeatureSetting>().ToTable("learning_school_account_feature_settings").HasIndex(x => x.TenantId);
        modelBuilder.Entity<Course>().HasIndex(x => new { x.TenantId, x.CourseCode }).IsUnique();
        modelBuilder.Entity<LearningGroupMembership>().HasIndex(x => new { x.TenantId, x.LearningGroupId, x.StudentProfileId, x.MembershipStatus });
        modelBuilder.Entity<AssignmentSubmission>().HasIndex(x => new { x.TenantId, x.AssignmentId, x.StudentProfileId, x.ClientRequestId });
        modelBuilder.Entity<QuizAttempt>().HasIndex(x => new { x.TenantId, x.QuizId, x.StudentProfileId, x.ClientRequestId });
        modelBuilder.Entity<StarLedgerEntry>().HasIndex(x => new { x.TenantId, x.SourceType, x.SourceReference, x.IdempotencyKey });
        modelBuilder.Entity<LearningIdempotencyRecord>().ToTable("learning_idempotency_records").HasIndex(x => new { x.TenantId, x.IdempotencyKind, x.IdempotencyKey }).IsUnique();
        modelBuilder.Entity<LearningAuditEvent>().ToTable("learning_audit_events").HasIndex(x => new { x.TenantId, x.EventType, x.EventTime });
        modelBuilder.Entity<LearningStatusEvent>().ToTable("learning_status_events").HasIndex(x => new { x.TenantId, x.StatusEventType, x.SourceType, x.SourceId });
        return modelBuilder;
    }
}
