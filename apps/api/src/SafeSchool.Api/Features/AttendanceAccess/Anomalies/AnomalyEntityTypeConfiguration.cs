using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeSchool.Api.Features.AttendanceAccess.Reviews;

namespace SafeSchool.Api.Features.AttendanceAccess.Anomalies;

public static class AnomalyEntityTypeConfiguration
{
    public sealed class AttendanceAnomalyConfiguration : IEntityTypeConfiguration<AttendanceAnomaly>
    {
        public void Configure(EntityTypeBuilder<AttendanceAnomaly> builder)
        {
            builder.ToTable("attendance_access_anomalies");
            builder.HasIndex(x => new { x.TenantId, x.EvidenceReference, x.AnomalyType }).IsUnique();
            builder.HasIndex(x => new { x.TenantId, x.Status, x.Severity });
        }
    }

    public sealed class ManualReviewConfiguration : IEntityTypeConfiguration<ManualReview>
    {
        public void Configure(EntityTypeBuilder<ManualReview> builder)
        {
            builder.ToTable("attendance_access_manual_reviews");
            builder.HasIndex(x => new { x.TenantId, x.TargetType, x.TargetReference });
            builder.HasIndex(x => new { x.TenantId, x.Status });
        }
    }
}

