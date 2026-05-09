using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Domain;

namespace SafeSchool.Api.Features.Learning.Fixtures;

public static class LearningSeedData
{
    public const string TenantId = "school-demo";
    public const string StudentProfileId = "student-amina";
    public const string GuardianActorId = "guardian-1";
    public const string TeacherActorId = "teacher-1";

    public static Course Course => new() { TenantId = TenantId, CourseCode = "SCI-5", CourseName = "Science 5", CourseStatus = CourseStatus.Active };
    public static LearningGroup Group => new() { TenantId = TenantId, GroupName = "Grade 5A", GroupStatus = LearningGroupStatus.Active };
    public static LearningContentItem Content => new() { TenantId = TenantId, Title = "Solar System", ResourceReference = "resource:solar-system", ContentStatus = ContentStatus.Published };
    public static Assignment Assignment => new() { TenantId = TenantId, Title = "Planet worksheet", RequiredEvidence = "photo-or-upload", AssignmentStatus = AssignmentStatus.Active };
    public static Quiz Quiz => new() { TenantId = TenantId, QuizTitle = "Planet quiz", QuizStatus = QuizStatus.Active };
    public static StarRuleSetting StarRule => new() { TenantId = TenantId, StarAmount = 5, RuleStatus = RuleStatus.Active };
    public static RewardCatalogItem Reward => new() { TenantId = TenantId, RewardCode = "LIB-PASS", Title = "Library priority pass", StarCost = 10, RewardStatus = RewardStatus.Active };
    public static BehaviorCategory BehaviorCategory => new() { TenantId = TenantId, CategoryCode = "TEAMWORK", Title = "Teamwork", Classification = BehaviorClassification.Positive, CategoryStatus = RuleStatus.Active };
}
