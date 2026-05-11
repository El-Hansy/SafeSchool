using Microsoft.EntityFrameworkCore;

namespace SafeSchool.Api.Features.Communications;

public sealed class OperationalCommunicationRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string RecipientScope { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<OperationalCommunicationEvent> Events { get; set; } = [];
}

public sealed class OperationalCommunicationEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public Guid CommunicationId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; set; }
    public OperationalCommunicationRecord? Communication { get; set; }
}

public sealed class OperationalCommunicationIdempotencyRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public string Fingerprint { get; set; } = string.Empty;
    public Guid CommunicationId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public static class CommunicationOperationalModelBuilderExtensions
{
    public static void ApplyCommunicationsOperationalModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationalCommunicationRecord>(entity =>
        {
            entity.ToTable("operational_communications");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Kind).HasMaxLength(48).IsRequired();
            entity.Property(x => x.Reference).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(48).IsRequired();
            entity.Property(x => x.Subject).HasMaxLength(500).IsRequired();
            entity.Property(x => x.Body).HasMaxLength(4000).IsRequired();
            entity.Property(x => x.RecipientScope).HasMaxLength(128).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.Reference }).IsUnique();
            entity.HasIndex(x => new { x.TenantId, x.Kind, x.UpdatedAt });
            entity.HasIndex(x => new { x.TenantId, x.Status, x.UpdatedAt });
        });

        modelBuilder.Entity<OperationalCommunicationEvent>(entity =>
        {
            entity.ToTable("operational_communication_events");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.EventType).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
            entity.HasOne(x => x.Communication)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.CommunicationId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => new { x.TenantId, x.CommunicationId, x.OccurredAt });
            entity.HasIndex(x => new { x.TenantId, x.EventType, x.OccurredAt });
        });

        modelBuilder.Entity<OperationalCommunicationIdempotencyRecord>(entity =>
        {
            entity.ToTable("operational_communication_idempotency");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Command).HasMaxLength(128).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Fingerprint).HasMaxLength(4000).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.Command, x.ClientRequestId }).IsUnique();
        });
    }
}
