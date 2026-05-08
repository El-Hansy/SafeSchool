using SafeSchool.Api.Features.IdentityAccess.Common;

namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public sealed class GuardianLink : TenantOwnedEntity
{
    public Guid StudentProfileId { get; set; }
    public Guid GuardianId { get; set; }
    public required string RelationshipType { get; set; }
    public Dictionary<string, string> AccessScope { get; set; } = new();
    public GuardianLinkStatus LinkStatus { get; set; } = GuardianLinkStatus.Pending;
    public DateTimeOffset ValidFrom { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ValidUntil { get; set; }
    public required string RequestedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public required string ReviewReason { get; set; }

    public bool GrantsStudentVisibility(DateTimeOffset now) =>
        LinkStatus == GuardianLinkStatus.Approved
        && ValidFrom <= now
        && (ValidUntil is null || ValidUntil > now)
        && AccessScope.TryGetValue("student_profile", out var scope)
        && string.Equals(scope, "read", StringComparison.OrdinalIgnoreCase);
}
