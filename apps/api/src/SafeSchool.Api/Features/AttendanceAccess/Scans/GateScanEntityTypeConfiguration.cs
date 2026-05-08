using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeSchool.Api.Features.AttendanceAccess.Gates;

namespace SafeSchool.Api.Features.AttendanceAccess.Scans;

public static class GateScanEntityTypeConfiguration
{
    public sealed class GateConfiguration : IEntityTypeConfiguration<Gate>
    {
        public void Configure(EntityTypeBuilder<Gate> builder)
        {
            builder.ToTable("attendance_access_gates");
            builder.HasIndex(x => new { x.TenantId, x.GateCode }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.Status });
        }
    }

    public sealed class ScanPointConfiguration : IEntityTypeConfiguration<ScanPoint>
    {
        public void Configure(EntityTypeBuilder<ScanPoint> builder)
        {
            builder.ToTable("attendance_access_scan_points");
            builder.HasIndex(x => new { x.TenantId, x.GateId, x.Status });
            builder.HasIndex(x => new { x.TenantId, x.DeviceReference }).IsUnique();
        }
    }

    public sealed class GateScanEventConfiguration : IEntityTypeConfiguration<GateScanEvent>
    {
        public void Configure(EntityTypeBuilder<GateScanEvent> builder)
        {
            builder.ToTable("attendance_access_scan_events");
            builder.HasIndex(x => new { x.TenantId, x.ClientScanId }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.LocalScanTime });
            builder.HasIndex(x => new { x.TenantId, x.Status, x.Direction });
        }
    }

    public sealed class OfflineSyncBatchConfiguration : IEntityTypeConfiguration<OfflineSyncBatch>
    {
        public void Configure(EntityTypeBuilder<OfflineSyncBatch> builder)
        {
            builder.ToTable("attendance_access_offline_sync_batches");
            builder.HasIndex(x => new { x.TenantId, x.ClientBatchId }).IsUnique();
        }
    }

    public sealed class CampusAccessDecisionConfiguration : IEntityTypeConfiguration<CampusAccessDecision>
    {
        public void Configure(EntityTypeBuilder<CampusAccessDecision> builder)
        {
            builder.ToTable("attendance_access_campus_decisions");
            builder.HasIndex(x => new { x.TenantId, x.GateScanEventId }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.Decision, x.DecidedAt });
        }
    }
}

