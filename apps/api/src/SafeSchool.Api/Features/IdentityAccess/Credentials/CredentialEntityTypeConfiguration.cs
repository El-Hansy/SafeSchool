using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public static class CredentialEntityTypeConfiguration
{
    public sealed class IdentityCredentialConfiguration : IEntityTypeConfiguration<IdentityCredential>
    {
        public void Configure(EntityTypeBuilder<IdentityCredential> builder)
        {
            builder.ToTable("identity_access_credentials");
            builder.HasIndex(x => new { x.TenantId, x.CredentialReference, x.CredentialStatus });
            builder.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.CredentialType });
        }
    }

    public sealed class NfcCardCredentialConfiguration : IEntityTypeConfiguration<NfcCardCredential>
    {
        public void Configure(EntityTypeBuilder<NfcCardCredential> builder)
        {
            builder.ToTable("identity_access_nfc_card_credentials");
            builder.HasIndex(x => new { x.TenantId, x.CardReference });
        }
    }

    public sealed class QrFallbackCredentialConfiguration : IEntityTypeConfiguration<QrFallbackCredential>
    {
        public void Configure(EntityTypeBuilder<QrFallbackCredential> builder)
        {
            builder.ToTable("identity_access_qr_fallback_credentials");
            builder.HasIndex(x => new { x.TenantId, x.QrReference, x.RotationSequence });
        }
    }

    public sealed class CredentialStatusSnapshotConfiguration : IEntityTypeConfiguration<CredentialStatusSnapshot>
    {
        public void Configure(EntityTypeBuilder<CredentialStatusSnapshot> builder)
        {
            builder.ToTable("identity_access_credential_status_snapshots");
            builder.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.SnapshotExpiresAt });
        }
    }
}
