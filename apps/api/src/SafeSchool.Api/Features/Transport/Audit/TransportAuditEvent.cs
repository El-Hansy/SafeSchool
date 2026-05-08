using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Audit;

public sealed class TransportAuditEvent : TenantOwnedEntity
{
    public string EventCategory { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string ActorReference { get; set; } = string.Empty;
    public string SubjectType { get; set; } = string.Empty;
    public string SubjectReference { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset EventTime { get; set; } = DateTimeOffset.UtcNow;
}
