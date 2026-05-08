using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeSchool.Api.Features.Transport.Anomalies;
using SafeSchool.Api.Features.Transport.Reviews;
using SafeSchool.Api.Features.Transport.Trips;

namespace SafeSchool.Api.Features.Transport.Scans;

public static class BoardingDropScanEntityTypeConfiguration
{
    public sealed class TransportTripConfiguration : IEntityTypeConfiguration<TransportTrip>
    {
        public void Configure(EntityTypeBuilder<TransportTrip> builder)
        {
            builder.ToTable("transport_trips");
            builder.HasIndex(x => new { x.TenantId, x.TransportRouteId, x.TripStatus });
            builder.HasIndex(x => new { x.TenantId, x.TransportVehicleId, x.TripStatus });
            builder.HasIndex(x => new { x.TenantId, x.TrackingDeviceReference, x.TripStatus });
        }
    }

    public sealed class ScanEventConfiguration : IEntityTypeConfiguration<BoardingDropScanEvent>
    {
        public void Configure(EntityTypeBuilder<BoardingDropScanEvent> builder)
        {
            builder.ToTable("transport_boarding_drop_scan_events");
            builder.HasIndex(x => new { x.TenantId, x.ClientScanId }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.TransportTripId, x.ScanDirection });
            builder.HasIndex(x => new { x.TenantId, x.CredentialReference });
        }
    }

    public sealed class SyncBatchConfiguration : IEntityTypeConfiguration<OfflineTransportScanSyncBatch>
    {
        public void Configure(EntityTypeBuilder<OfflineTransportScanSyncBatch> builder)
        {
            builder.ToTable("transport_offline_scan_sync_batches");
            builder.HasIndex(x => new { x.TenantId, x.ClientBatchId }).IsUnique();
        }
    }

    public sealed class AnomalyConfiguration : IEntityTypeConfiguration<TransportAnomaly>
    {
        public void Configure(EntityTypeBuilder<TransportAnomaly> builder)
        {
            builder.ToTable("transport_anomalies");
            builder.HasIndex(x => new { x.TenantId, x.TransportTripId, x.Status });
            builder.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.Status });
        }
    }

    public sealed class ManualReviewConfiguration : IEntityTypeConfiguration<ManualTransportReview>
    {
        public void Configure(EntityTypeBuilder<ManualTransportReview> builder)
        {
            builder.ToTable("transport_manual_reviews");
            builder.HasIndex(x => new { x.TenantId, x.SourceRecordType, x.SourceRecordReference });
        }
    }
}
