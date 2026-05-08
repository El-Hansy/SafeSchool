using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.AttendanceAccess.Attendance;

public static class AttendanceEntityTypeConfiguration
{
    public sealed class AttendanceSessionConfiguration : IEntityTypeConfiguration<AttendanceSession>
    {
        public void Configure(EntityTypeBuilder<AttendanceSession> builder)
        {
            builder.ToTable("attendance_access_sessions");
            builder.HasIndex(x => new { x.TenantId, x.AttendanceDate, x.CampusReference });
            builder.HasIndex(x => new { x.TenantId, x.GenerationStatus });
        }
    }

    public sealed class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {
            builder.ToTable("attendance_access_records");
            builder.HasIndex(x => new { x.TenantId, x.AttendanceSessionId, x.StudentProfileId }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.Status });
        }
    }
}

