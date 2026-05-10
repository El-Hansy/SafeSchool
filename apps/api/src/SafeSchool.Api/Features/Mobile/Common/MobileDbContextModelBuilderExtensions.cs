using Microsoft.EntityFrameworkCore;

namespace SafeSchool.Api.Features.Mobile;

public static class MobileDbContextModelBuilderExtensions
{
    public static void ApplyMobileModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MobileUserProfile>().HasIndex(x => new { x.TenantId, x.UserId }).IsUnique();
        modelBuilder.Entity<RoleWorkspace>().HasIndex(x => new { x.TenantId, x.WorkspaceCode }).IsUnique();
        modelBuilder.Entity<RoleWorkspaceAction>().HasIndex(x => new { x.TenantId, x.WorkspaceCode, x.ActionCode });
        modelBuilder.Entity<MobilePermissionGrant>().HasIndex(x => new { x.TenantId, x.SubjectId, x.WorkspaceCode, x.PermissionCode, x.GrantStatus });
        modelBuilder.Entity<TenantMobileFeatureAvailability>().HasIndex(x => x.TenantId).IsUnique();
        modelBuilder.Entity<MobileLanguagePreference>().HasIndex(x => new { x.TenantId, x.UserId }).IsUnique();
        modelBuilder.Entity<DeviceSession>().HasIndex(x => new { x.TenantId, x.UserId, x.DeviceId, x.SessionStatus });
        modelBuilder.Entity<ApkRelease>().HasIndex(x => x.VersionCode).IsUnique();
        modelBuilder.Entity<ReleaseAudience>().HasIndex(x => new { x.ReleaseId, x.TenantId, x.AudienceType, x.AudienceRef });
        modelBuilder.Entity<InstallOrUpgradeEvent>().HasIndex(x => new { x.TenantId, x.UserId, x.DeviceId, x.VersionCode, x.OccurredAt });
        modelBuilder.Entity<OfflineActionQueue>().HasIndex(x => new { x.TenantId, x.DeviceId, x.SourceFeatureCode, x.ClientActionId }).IsUnique();
        modelBuilder.Entity<MobileAuditEvent>().HasIndex(x => new { x.TenantId, x.ActorUserId, x.EventType, x.TargetType, x.TargetId, x.OccurredAt });
    }
}
