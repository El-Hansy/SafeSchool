using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class StarBalanceSnapshot : TenantOwnedEntity
{
    public string StudentProfileId { get; set; } = string.Empty;
    public int AvailableStars { get; set; }
    public int ReservedStars { get; set; }
    public int ConsumedStars { get; set; }
    public int PendingReviewStars { get; set; }
    public DateTimeOffset CalculatedAt { get; set; } = DateTimeOffset.UtcNow;
}
