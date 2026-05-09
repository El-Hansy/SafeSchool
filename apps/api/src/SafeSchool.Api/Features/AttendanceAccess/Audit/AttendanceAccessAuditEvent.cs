using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Features.AttendanceAccess.Audit;

public sealed class AttendanceAccessAuditEvent : TenantOwnedEntity
{
    public string EventCategory { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string ActorReference { get; set; } = string.Empty;
    public string SubjectType { get; set; } = string.Empty;
    public string SubjectReference { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset EventTime { get; set; } = DateTimeOffset.UtcNow;
}

