namespace SafeSchool.Api.Features.AttendanceAccess.Common;

public sealed class AttendanceAccessRuleSettings : TenantOwnedEntity
{
    public TimeOnly EntryWindowStart { get; set; } = new(7, 0);
    public TimeOnly EntryWindowEnd { get; set; } = new(8, 0);
    public TimeOnly LateAfter { get; set; } = new(8, 0);
    public TimeOnly EarlyExitBefore { get; set; } = new(13, 0);
    public int OfflineClockDriftToleranceMinutes { get; set; } = 15;
    public bool GuardianNotificationsEnabled { get; set; } = true;
    public string Version { get; set; } = "v1";
}

