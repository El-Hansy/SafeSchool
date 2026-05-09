using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Configuration;

public sealed class LearningRuleVersionService
{
    public int CaptureCurrentVersion(RuleArea area, int currentVersion) => Math.Max(1, currentVersion);
    public int ActivateNextVersion(RuleArea area, int currentVersion) => Math.Max(1, currentVersion) + 1;
    public bool IsStale(int capturedVersion, int currentVersion) => capturedVersion < currentVersion;

    public OperationResult<int> ValidateMigration(string reason, int targetVersion) => string.IsNullOrWhiteSpace(reason)
        ? OperationResult<int>.Failure(new ValidationError("review_reason_required", "Rule migration requires a reason.", nameof(reason)))
        : OperationResult<int>.Success(Math.Max(1, targetVersion));
}
