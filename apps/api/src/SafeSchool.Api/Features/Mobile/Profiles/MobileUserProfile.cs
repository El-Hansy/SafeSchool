namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileUserProfile
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public string UserId { get; init; } = "user-demo";
    public string ActiveTenantId { get; init; } = "school-demo";
    public string ActiveRoleCode { get; init; } = MobileRoleCodes.Guardian;
    public string AvailableTenantIds { get; init; } = "school-demo";
    public string AvailableRoleCodes { get; init; } = string.Join(',', MobileRoleCodes.All);
    public string LinkedStudentIds { get; init; } = "student-amina,student-omar";
    public MobileAccessStatus MobileAccessStatus { get; init; } = MobileAccessStatus.Active;
    public DateTimeOffset LastResolvedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
