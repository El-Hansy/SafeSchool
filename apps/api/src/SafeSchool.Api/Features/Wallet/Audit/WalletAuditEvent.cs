using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Audit;

public sealed class WalletAuditEvent : TenantOwnedEntity
{
    public string EventCategory { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string SubjectType { get; set; } = string.Empty;
    public string SubjectReference { get; set; } = string.Empty;
    public string ActorReference { get; set; } = "system";
    public string Reason { get; set; } = string.Empty;
    public string CorrelationReference { get; set; } = string.Empty;
    public DateTimeOffset EventTime { get; set; } = DateTimeOffset.UtcNow;
}
