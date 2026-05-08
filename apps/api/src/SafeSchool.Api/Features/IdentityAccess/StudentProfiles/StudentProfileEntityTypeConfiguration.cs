using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

public sealed class StudentProfileEntityTypeConfiguration : IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> builder)
    {
        builder.ToTable("identity_access_student_profiles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.SchoolStudentNumber, x.ProfileStatus })
            .HasDatabaseName("ix_student_profiles_tenant_number_status");
        builder.Property(x => x.ExternalIdentityReferences).HasColumnType("text[]");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }
}
