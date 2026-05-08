using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.Transport.Routes;

public static class RouteStopEntityTypeConfiguration
{
    public sealed class RouteConfiguration : IEntityTypeConfiguration<TransportRoute>
    {
        public void Configure(EntityTypeBuilder<TransportRoute> builder)
        {
            builder.ToTable("transport_routes");
            builder.HasIndex(x => new { x.TenantId, x.RouteCode }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.RouteStatus });
        }
    }

    public sealed class StopConfiguration : IEntityTypeConfiguration<TransportStop>
    {
        public void Configure(EntityTypeBuilder<TransportStop> builder)
        {
            builder.ToTable("transport_stops");
            builder.HasIndex(x => new { x.TenantId, x.StopCode });
            builder.HasIndex(x => new { x.TenantId, x.StopStatus });
        }
    }

    public sealed class RouteStopSequenceConfiguration : IEntityTypeConfiguration<RouteStopSequence>
    {
        public void Configure(EntityTypeBuilder<RouteStopSequence> builder)
        {
            builder.ToTable("transport_route_stop_sequences");
            builder.HasIndex(x => new { x.TenantId, x.TransportRouteId, x.ServiceDirection, x.RouteVersion, x.SequenceNumber }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.TransportStopId });
        }
    }
}
