namespace SafeSchool.Api.Features.Learning.Common;

public static class LearningPermissionCatalog
{
    public const string CourseManage = "learning.course.manage";
    public const string ContentPublish = "learning.content.publish";
    public const string ContentRead = "learning.content.read";
    public const string ProgressWrite = "learning.progress.write";
    public const string ProgressRead = "learning.progress.read";
    public const string AssignmentManage = "learning.assignment.manage";
    public const string AssignmentSubmit = "learning.assignment.submit";
    public const string AssignmentReview = "learning.assignment.review";
    public const string AssignmentRead = "learning.assignment.read";
    public const string QuizManage = "learning.quiz.manage";
    public const string QuizAttempt = "learning.quiz.attempt";
    public const string QuizReview = "learning.quiz.review";
    public const string QuizRead = "learning.quiz.read";
    public const string StarsRead = "learning.stars.read";
    public const string StarsManage = "learning.stars.manage";
    public const string RewardsRead = "learning.rewards.read";
    public const string RewardsManage = "learning.rewards.manage";
    public const string RewardsRedeem = "learning.rewards.redeem";
    public const string BehaviorRead = "learning.behavior.read";
    public const string BehaviorCreate = "learning.behavior.create";
    public const string BehaviorReview = "learning.behavior.review";
    public const string HistoryRead = "learning.history.read";
    public const string GuardianHistoryRead = "learning.guardian_history.read";
    public const string ConfigurationRead = "learning.configuration.read";
    public const string ConfigurationManage = "learning.configuration.manage";
    public const string ReviewsManage = "learning.reviews.manage";
    public const string SummariesRead = "learning.summaries.read";
    public const string AuditRead = "learning.audit.read";
    public const string PlatformReview = "learning.platform_review";

    public static readonly string[] Teacher = [CourseManage, ContentPublish, ContentRead, ProgressRead, AssignmentManage, AssignmentReview, AssignmentRead, QuizManage, QuizReview, QuizRead, BehaviorCreate, BehaviorRead, HistoryRead, SummariesRead];
    public static readonly string[] Student = [ContentRead, ProgressWrite, ProgressRead, AssignmentSubmit, AssignmentRead, QuizAttempt, QuizRead, StarsRead, RewardsRead, RewardsRedeem, BehaviorRead, HistoryRead];
    public static readonly string[] Guardian = [ContentRead, ProgressRead, AssignmentRead, QuizRead, StarsRead, RewardsRead, BehaviorRead, GuardianHistoryRead];
    public static readonly string[] RewardManager = [StarsRead, StarsManage, RewardsRead, RewardsManage, ReviewsManage, SummariesRead];
    public static readonly string[] BehaviorReviewer = [BehaviorRead, BehaviorCreate, BehaviorReview, ReviewsManage, SummariesRead];
    public static readonly string[] Auditor = [HistoryRead, SummariesRead, AuditRead, PlatformReview];
}
