namespace SafeSchool.Api.Infrastructure.Tenancy;

public interface ITenantContext
{
    string? TenantId { get; set; }
    string? ActorReference { get; set; }
    bool HasPlatformReviewScope { get; set; }
}

public sealed class TenantContext : ITenantContext
{
    public string? TenantId { get; set; }
    public string? ActorReference { get; set; }
    public bool HasPlatformReviewScope { get; set; }
}
