using Microsoft.EntityFrameworkCore;

namespace SafeSchool.Api.Features.Requests;

public sealed class OperationalRequestRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string TrackingReference { get; set; } = string.Empty;
    public string StudentProfileId { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string RequestedOutcome { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public string SubmitterRole { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string VisibleSummary { get; set; } = string.Empty;
    public DateTimeOffset? StartsAt { get; set; }
    public DateTimeOffset? EndsAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<OperationalRequestEvent> Events { get; set; } = [];
}

public sealed class OperationalRequestEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public Guid RequestId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string ActorReference { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; set; }
    public OperationalRequestRecord? Request { get; set; }
}

public sealed class OperationalRequestIdempotencyRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TenantId { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string ClientRequestId { get; set; } = string.Empty;
    public string Fingerprint { get; set; } = string.Empty;
    public Guid RequestId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public static class RequestOperationalModelBuilderExtensions
{
    public static void ApplyRequestsOperationalModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationalRequestRecord>(entity =>
        {
            entity.ToTable("operational_requests");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.TrackingReference).HasMaxLength(64).IsRequired();
            entity.Property(x => x.StudentProfileId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.RequestType).HasMaxLength(64).IsRequired();
            entity.Property(x => x.Reason).HasMaxLength(2000).IsRequired();
            entity.Property(x => x.RequestedOutcome).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.SubmitterRole).HasMaxLength(48).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(48).IsRequired();
            entity.Property(x => x.Priority).HasMaxLength(48).IsRequired();
            entity.Property(x => x.VisibleSummary).HasMaxLength(1000).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.TrackingReference }).IsUnique();
            entity.HasIndex(x => new { x.TenantId, x.Status, x.UpdatedAt });
            entity.HasIndex(x => new { x.TenantId, x.SubmitterRole, x.UpdatedAt });
            entity.HasIndex(x => new { x.TenantId, x.RequestType, x.UpdatedAt });
        });

        modelBuilder.Entity<OperationalRequestEvent>(entity =>
        {
            entity.ToTable("operational_request_events");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.EventType).HasMaxLength(96).IsRequired();
            entity.Property(x => x.ActorReference).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
            entity.HasOne(x => x.Request)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => new { x.TenantId, x.RequestId, x.OccurredAt });
            entity.HasIndex(x => new { x.TenantId, x.EventType, x.OccurredAt });
        });

        modelBuilder.Entity<OperationalRequestIdempotencyRecord>(entity =>
        {
            entity.ToTable("operational_request_idempotency");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TenantId).HasMaxLength(96).IsRequired();
            entity.Property(x => x.Command).HasMaxLength(128).IsRequired();
            entity.Property(x => x.ClientRequestId).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Fingerprint).HasMaxLength(4000).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.Command, x.ClientRequestId }).IsUnique();
        });
    }
}

