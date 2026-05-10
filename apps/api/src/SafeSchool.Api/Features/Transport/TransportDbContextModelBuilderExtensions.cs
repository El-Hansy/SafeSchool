using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Assignments;
using SafeSchool.Api.Features.Transport.Audit;
using SafeSchool.Api.Features.Transport.Common.Idempotency;
using SafeSchool.Api.Features.Transport.Eta;
using SafeSchool.Api.Features.Transport.Notifications;
using SafeSchool.Api.Features.Transport.Routes;
using SafeSchool.Api.Features.Transport.Rules;
using SafeSchool.Api.Features.Transport.Scans;
using SafeSchool.Api.Features.Transport.Tracking;

namespace SafeSchool.Api.Features.Transport;

public static class TransportDbContextModelBuilderExtensions
{
    public static ModelBuilder ApplyTransportModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RouteStopEntityTypeConfiguration.RouteConfiguration());
        modelBuilder.ApplyConfiguration(new RouteStopEntityTypeConfiguration.StopConfiguration());
        modelBuilder.ApplyConfiguration(new RouteStopEntityTypeConfiguration.RouteStopSequenceConfiguration());
        modelBuilder.ApplyConfiguration(new BusAssignmentEntityTypeConfiguration.VehicleConfiguration());
        modelBuilder.ApplyConfiguration(new BusAssignmentEntityTypeConfiguration.AssignmentConfiguration());
        modelBuilder.ApplyConfiguration(new BoardingDropScanEntityTypeConfiguration.TransportTripConfiguration());
        modelBuilder.ApplyConfiguration(new BoardingDropScanEntityTypeConfiguration.ScanEventConfiguration());
        modelBuilder.ApplyConfiguration(new BoardingDropScanEntityTypeConfiguration.SyncBatchConfiguration());
        modelBuilder.ApplyConfiguration(new BoardingDropScanEntityTypeConfiguration.AnomalyConfiguration());
        modelBuilder.ApplyConfiguration(new BoardingDropScanEntityTypeConfiguration.ManualReviewConfiguration());
        modelBuilder.ApplyConfiguration(new LiveTrackingEntityTypeConfiguration.LocationUpdateConfiguration());
        modelBuilder.ApplyConfiguration(new EtaEntityTypeConfiguration.EtaRecordConfiguration());
        modelBuilder.ApplyConfiguration(new TransportNotificationEntityTypeConfiguration.NotificationRecordConfiguration());
        modelBuilder.Entity<TransportRuleSetting>().ToTable("transport_rule_settings").HasIndex(x => new { x.TenantId, x.Status });
        modelBuilder.Entity<TransportAuditEvent>().ToTable("transport_audit_events").HasIndex(x => new { x.TenantId, x.EventType, x.EventTime });
        modelBuilder.Entity<TransportIdempotencyRecord>().ToTable("transport_idempotency_records").HasIndex(x => new { x.TenantId, x.IdempotencyKind, x.IdempotencyKey }).IsUnique();
        return modelBuilder;
    }
}
