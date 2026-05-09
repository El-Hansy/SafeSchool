namespace SafeSchool.Api.Infrastructure.FeatureFlags;

public static class LearningCapabilities
{
    public const string ContentDelivery = "learning.content_delivery";
    public const string Assignments = "learning.assignments";
    public const string Quizzes = "learning.quizzes";
    public const string StarsRewards = "learning.stars_rewards";
    public const string Rewards = "learning.rewards";
    public const string BehaviorLogging = "learning.behavior_logging";
    public const string ProgressHistory = "learning.progress_history";
    public const string Configuration = "learning.configuration";
    public const string ReviewSummaries = "learning.review_summaries";

    public static readonly string[] All =
    [
        ContentDelivery,
        Assignments,
        Quizzes,
        StarsRewards,
        Rewards,
        BehaviorLogging,
        ProgressHistory,
        Configuration,
        ReviewSummaries
    ];
}
