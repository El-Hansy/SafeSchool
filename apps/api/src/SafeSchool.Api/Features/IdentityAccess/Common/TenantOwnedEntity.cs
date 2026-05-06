namespace SafeSchool.Api.Features.IdentityAccess.Common;

public abstract class TenantOwnedEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string TenantId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
