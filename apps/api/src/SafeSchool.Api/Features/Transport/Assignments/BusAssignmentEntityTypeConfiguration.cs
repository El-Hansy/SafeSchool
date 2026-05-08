using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.Transport.Assignments;

public static class BusAssignmentEntityTypeConfiguration
{
    public sealed class VehicleConfiguration : IEntityTypeConfiguration<TransportVehicle>
    {
        public void Configure(EntityTypeBuilder<TransportVehicle> builder)
        {
            builder.ToTable("transport_vehicles");
            builder.HasIndex(x => new { x.TenantId, x.VehicleCode }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.VehicleStatus });
        }
    }

    public sealed class AssignmentConfiguration : IEntityTypeConfiguration<StudentTransportAssignment>
    {
        public void Configure(EntityTypeBuilder<StudentTransportAssignment> builder)
        {
            builder.ToTable("student_transport_assignments");
            builder.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.AssignmentStatus });
            builder.HasIndex(x => new { x.TenantId, x.TransportRouteId });
            builder.HasIndex(x => new { x.TenantId, x.TransportVehicleId });
        }
    }
}
