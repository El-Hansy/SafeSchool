using Microsoft.EntityFrameworkCore;

namespace SafeSchool.Api.Features.Medical;

public sealed class OperationalMedicalRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string RecordReference { get; set; } = string.Empty;
    public string StudentProfileId { get; set; } = string.Empty;
    public string RecordType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string VisibleSummary { get; set; } = string.Empty;
    public string RestrictedDetail { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public DateTimeOffset? ExpiresAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<OperationalMedicalEvent> Events { get; set; } = [];
}

public sealed class OperationalMedicalEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public Guid MedicalRecordId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string ActorReference { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; set; }
    public OperationalMedicalRecord? MedicalRecord { get; set; }
}

public sealed class OperationalMedicalIdempotencyRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public string Fingerprint { get; set; } = string.Empty;
    public Guid MedicalRecordId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public static class MedicalOperationalModelBuilderExtensions
{
    public static void ApplyMedicalOperationalModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationalMedicalRecord>(entity =>
        {
            entity.ToTable("operational_medical_records");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.RecordReference).HasMaxLength(64).IsRequired();
            entity.Property(x => x.StudentProfileId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.RecordType).HasMaxLength(64).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(48).IsRequired();
            entity.Property(x => x.Severity).HasMaxLength(48).IsRequired();
            entity.Property(x => x.VisibleSummary).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.RestrictedDetail).HasMaxLength(4000).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.RecordReference }).IsUnique();
            entity.HasIndex(x => new { x.TenantId, x.RecordType, x.UpdatedAt });
            entity.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.RecordType });
            entity.HasIndex(x => new { x.TenantId, x.Status, x.UpdatedAt });
        });

        modelBuilder.Entity<OperationalMedicalEvent>(entity =>
        {
            entity.ToTable("operational_medical_events");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.EventType).HasMaxLength(96).IsRequired();
            entity.Property(x => x.ActorReference).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
            entity.HasOne(x => x.MedicalRecord)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => new { x.TenantId, x.MedicalRecordId, x.OccurredAt });
            entity.HasIndex(x => new { x.TenantId, x.EventType, x.OccurredAt });
        });

        modelBuilder.Entity<OperationalMedicalIdempotencyRecord>(entity =>
        {
            entity.ToTable("operational_medical_idempotency");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Command).HasMaxLength(128).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Fingerprint).HasMaxLength(4000).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.Command, x.ClientRequestId }).IsUnique();
        });
    }
}

