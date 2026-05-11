using Microsoft.EntityFrameworkCore;

namespace SafeSchool.Api.Features.Documents;

public sealed class OperationalDocumentRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
    public string SubjectReference { get; set; } = string.Empty;
    public string SourceModule { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<OperationalDocumentEvent> Events { get; set; } = [];
}

public sealed class OperationalDocumentEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public Guid DocumentId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; set; }
    public OperationalDocumentRecord? Document { get; set; }
}

public sealed class OperationalDocumentIdempotencyRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public string Fingerprint { get; set; } = string.Empty;
    public Guid DocumentId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class OperationalCertificateRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string CertificateType { get; set; } = string.Empty;
    public string SubjectReference { get; set; } = string.Empty;
    public string SourceReference { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string VerificationState { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<OperationalCertificateEvent> Events { get; set; } = [];
}

public sealed class OperationalCertificateEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public Guid CertificateId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; set; }
    public OperationalCertificateRecord? Certificate { get; set; }
}

public sealed class OperationalCertificateIdempotencyRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public string Fingerprint { get; set; } = string.Empty;
    public Guid CertificateId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class OperationalSearchLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string QueryLogReference { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public int ResultCount { get; set; }
    public string SuppressedReasons { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}

public static class DocumentOperationalModelBuilderExtensions
{
    public static void ApplyDocumentsOperationalModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationalDocumentRecord>(entity =>
        {
            entity.ToTable("operational_documents");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Reference).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Title).HasMaxLength(500).IsRequired();
            entity.Property(x => x.CategoryCode).HasMaxLength(96).IsRequired();
            entity.Property(x => x.SubjectReference).HasMaxLength(128).IsRequired();
            entity.Property(x => x.SourceModule).HasMaxLength(96).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(48).IsRequired();
            entity.Property(x => x.Visibility).HasMaxLength(64).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.Reference }).IsUnique();
            entity.HasIndex(x => new { x.TenantId, x.CategoryCode, x.UpdatedAt });
            entity.HasIndex(x => new { x.TenantId, x.Visibility, x.UpdatedAt });
        });

        modelBuilder.Entity<OperationalDocumentEvent>(entity =>
        {
            entity.ToTable("operational_document_events");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.EventType).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
            entity.HasOne(x => x.Document)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => new { x.TenantId, x.DocumentId, x.OccurredAt });
            entity.HasIndex(x => new { x.TenantId, x.EventType, x.OccurredAt });
        });

        modelBuilder.Entity<OperationalDocumentIdempotencyRecord>(entity =>
        {
            entity.ToTable("operational_document_idempotency");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Command).HasMaxLength(128).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Fingerprint).HasMaxLength(4000).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.Command, x.ClientRequestId }).IsUnique();
        });

        modelBuilder.Entity<OperationalCertificateRecord>(entity =>
        {
            entity.ToTable("operational_certificates");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Reference).HasMaxLength(128).IsRequired();
            entity.Property(x => x.CertificateType).HasMaxLength(96).IsRequired();
            entity.Property(x => x.SubjectReference).HasMaxLength(128).IsRequired();
            entity.Property(x => x.SourceReference).HasMaxLength(128).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(48).IsRequired();
            entity.Property(x => x.VerificationState).HasMaxLength(64).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.Reference }).IsUnique();
            entity.HasIndex(x => new { x.TenantId, x.SubjectReference, x.UpdatedAt });
            entity.HasIndex(x => new { x.TenantId, x.VerificationState, x.UpdatedAt });
        });

        modelBuilder.Entity<OperationalCertificateEvent>(entity =>
        {
            entity.ToTable("operational_certificate_events");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.EventType).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
            entity.HasOne(x => x.Certificate)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => new { x.TenantId, x.CertificateId, x.OccurredAt });
            entity.HasIndex(x => new { x.TenantId, x.EventType, x.OccurredAt });
        });

        modelBuilder.Entity<OperationalCertificateIdempotencyRecord>(entity =>
        {
            entity.ToTable("operational_certificate_idempotency");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Command).HasMaxLength(128).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Fingerprint).HasMaxLength(4000).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.Command, x.ClientRequestId }).IsUnique();
        });

        modelBuilder.Entity<OperationalSearchLog>(entity =>
        {
            entity.ToTable("operational_document_search_logs");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.QueryLogReference).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Text).HasMaxLength(500).IsRequired();
            entity.Property(x => x.Scope).HasMaxLength(96).IsRequired();
            entity.Property(x => x.SuppressedReasons).HasMaxLength(500).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.QueryLogReference }).IsUnique();
            entity.HasIndex(x => new { x.TenantId, x.CreatedAt });
        });
    }
}
