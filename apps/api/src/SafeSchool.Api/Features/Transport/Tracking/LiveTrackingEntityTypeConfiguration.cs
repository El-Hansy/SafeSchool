using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.Transport.Tracking;

public static class LiveTrackingEntityTypeConfiguration
{
    public sealed class LocationUpdateConfiguration : IEntityTypeConfiguration<TransportLocationUpdate>
    {
        public void Configure(EntityTypeBuilder<TransportLocationUpdate> builder)
        {
            builder.ToTable("transport_location_updates");
            builder.HasIndex(x => new { x.TenantId, x.ClientLocationId }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.TransportTripId, x.ReportedAt });
            builder.HasIndex(x => new { x.TenantId, x.RetentionState, x.ReportedAt });
        }
    }
}
