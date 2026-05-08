using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Reviews;

public sealed class ManualReview : TenantOwnedEntity
{
    public string TargetType { get; set; } = string.Empty;
    public string TargetReference { get; set; } = string.Empty;
    public ManualReviewStatus Status { get; set; } = ManualReviewStatus.Requested;
    public string ReviewerReference { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}

