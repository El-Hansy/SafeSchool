using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.Transport.Eta;

public static class EtaEntityTypeConfiguration
{
    public sealed class EtaRecordConfiguration : IEntityTypeConfiguration<EtaRecord>
    {
        public void Configure(EntityTypeBuilder<EtaRecord> builder)
        {
            builder.ToTable("transport_eta_records");
            builder.HasIndex(x => new { x.TenantId, x.TransportTripId, x.RouteStopSequenceId });
            builder.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.EtaState });
            builder.HasIndex(x => new { x.TenantId, x.FreshnessStatus, x.CalculatedAt });
        }
    }
}
