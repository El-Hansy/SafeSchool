namespace SafeSchool.Api.Features.Mobile;

public sealed record StaffMobileWorkspace(string RoleCode, string Duty, IReadOnlyList<string> CriticalActions, bool OfflineAllowed);
