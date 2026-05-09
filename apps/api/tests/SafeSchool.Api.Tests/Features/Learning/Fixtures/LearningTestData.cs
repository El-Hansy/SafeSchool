using SafeSchool.Api.Features.Learning.Common;
using SafeSchool.Api.Features.Learning.Domain;

namespace SafeSchool.Api.Tests.Features.Learning.Fixtures;

public static class LearningTestData
{
    public const string TenantId = "school-1";
    public const string OtherTenantId = "school-2";
    public const string StudentProfileId = "student-1";
    public const string GuardianActorId = "guardian-1";
    public const string TeacherActorId = "teacher-1";

    public static Course ActiveCourse() => new() { TenantId = TenantId, CourseCode = "MATH-5", CourseName = "Math 5", CourseStatus = CourseStatus.Active };
    public static LearningGroup ActiveGroup() => new() { TenantId = TenantId, GroupName = "Grade 5A", GroupStatus = LearningGroupStatus.Active };
    public static LearningContentItem PublishedContent() => new() { TenantId = TenantId, Title = "Fractions", ResourceReference = "resource:fraction-video", ContentStatus = ContentStatus.Published };
    public static Assignment ActiveAssignment() => new() { TenantId = TenantId, Title = "Fractions worksheet", RequiredEvidence = "upload", AssignmentStatus = AssignmentStatus.Active };
    public static Quiz ActiveQuiz() => new() { TenantId = TenantId, QuizTitle = "Fractions quiz", QuizStatus = QuizStatus.Active };
    public static StarLedgerEntry StarCredit(int amount = 5) => new() { TenantId = TenantId, StudentProfileId = StudentProfileId, Direction = StarLedgerDirection.Credit, Amount = amount, SourceType = "assignment", SourceReference = "assignment-1" };
    public static RewardCatalogItem ActiveReward() => new() { TenantId = TenantId, RewardCode = "RWD-1", Title = "Library pass", StarCost = 10, RewardStatus = RewardStatus.Active };
    public static BehaviorCategory PositiveBehaviorCategory() => new() { TenantId = TenantId, CategoryCode = "TEAM", Title = "Teamwork", Classification = BehaviorClassification.Positive, CategoryStatus = RuleStatus.Active };
}
