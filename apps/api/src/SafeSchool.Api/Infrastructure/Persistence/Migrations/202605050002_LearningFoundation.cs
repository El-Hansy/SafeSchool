namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class LearningFoundationMigration
{
    public const string MigrationId = "202605050002_LearningFoundation";
    public static readonly string[] Tables =
    [
        "learning_courses",
        "learning_learning_groups",
        "learning_assignments",
        "learning_quizzes",
        "learning_star_ledger_entries",
        "learning_reward_redemptions",
        "learning_behavior_events",
        "learning_audit_events",
        "learning_status_events"
    ];
}
