using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Anomalies;
using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Audit;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Common.Idempotency;
using SafeSchool.Api.Features.AttendanceAccess.Gates;
using SafeSchool.Api.Features.AttendanceAccess.Notifications;
using SafeSchool.Api.Features.AttendanceAccess.Reviews;
using SafeSchool.Api.Features.AttendanceAccess.Scans;

namespace SafeSchool.Api.Features.AttendanceAccess;

public static class AttendanceAccessDbContextModelBuilderExtensions
{
    public static ModelBuilder ApplyAttendanceAccessModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GateScanEntityTypeConfiguration.GateConfiguration());
        modelBuilder.ApplyConfiguration(new GateScanEntityTypeConfiguration.ScanPointConfiguration());
        modelBuilder.ApplyConfiguration(new GateScanEntityTypeConfiguration.GateScanEventConfiguration());
        modelBuilder.ApplyConfiguration(new GateScanEntityTypeConfiguration.OfflineSyncBatchConfiguration());
        modelBuilder.ApplyConfiguration(new GateScanEntityTypeConfiguration.CampusAccessDecisionConfiguration());
        modelBuilder.ApplyConfiguration(new AttendanceEntityTypeConfiguration.AttendanceSessionConfiguration());
        modelBuilder.ApplyConfiguration(new AttendanceEntityTypeConfiguration.AttendanceRecordConfiguration());
        modelBuilder.ApplyConfiguration(new EntryExitNotificationEntityTypeConfiguration.NotificationRecordConfiguration());
        modelBuilder.ApplyConfiguration(new AnomalyEntityTypeConfiguration.AttendanceAnomalyConfiguration());
        modelBuilder.ApplyConfiguration(new AnomalyEntityTypeConfiguration.ManualReviewConfiguration());
        modelBuilder.Entity<AttendanceAccessRuleSettings>().ToTable("attendance_access_rule_settings").HasIndex(x => x.TenantId);
        modelBuilder.Entity<AttendanceAccessAuditEvent>().ToTable("attendance_access_audit_events").HasIndex(x => new { x.TenantId, x.EventType, x.EventTime });
        modelBuilder.Entity<AttendanceAccessIdempotencyRecord>().ToTable("attendance_access_idempotency_records").HasIndex(x => new { x.TenantId, x.IdempotencyKey }).IsUnique();
        return modelBuilder;
    }
}

