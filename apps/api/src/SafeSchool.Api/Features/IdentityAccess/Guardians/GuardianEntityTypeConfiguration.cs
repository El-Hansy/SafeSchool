using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public static class GuardianEntityTypeConfiguration
{
    public sealed class GuardianRecordConfiguration : IEntityTypeConfiguration<GuardianRecord>
    {
        public void Configure(EntityTypeBuilder<GuardianRecord> builder)
        {
            builder.ToTable("identity_access_guardians");
            builder.HasIndex(x => new { x.TenantId, x.DisplayName });
            builder.Property(x => x.ContactMethods).HasColumnType("text[]");
        }
    }

    public sealed class GuardianLinkConfiguration : IEntityTypeConfiguration<GuardianLink>
    {
        public void Configure(EntityTypeBuilder<GuardianLink> builder)
        {
            builder.ToTable("identity_access_guardian_links");
            builder.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.GuardianId, x.LinkStatus });
            builder.Property(x => x.AccessScope)
                .HasConversion(
                    value => JsonSerializer.Serialize(value, (JsonSerializerOptions?)null),
                    value => JsonSerializer.Deserialize<Dictionary<string, string>>(value, (JsonSerializerOptions?)null) ?? new Dictionary<string, string>())
                .HasColumnType("jsonb");
        }
    }
}
