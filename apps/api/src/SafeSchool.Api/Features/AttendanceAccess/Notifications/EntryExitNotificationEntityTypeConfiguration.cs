using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.AttendanceAccess.Notifications;

public static class EntryExitNotificationEntityTypeConfiguration
{
    public sealed class NotificationRecordConfiguration : IEntityTypeConfiguration<EntryExitNotificationRecord>
    {
        public void Configure(EntityTypeBuilder<EntryExitNotificationRecord> builder)
        {
            builder.ToTable("attendance_access_entry_exit_notifications");
            builder.HasIndex(x => new { x.TenantId, x.GateScanEventId, x.GuardianReference }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.GuardianReference, x.EligibilityStatus });
            builder.HasIndex(x => new { x.TenantId, x.StudentProfileId });
        }
    }
}

