namespace SafeSchool.Api.Features.Mobile;

public sealed class MobileLanguagePreference
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string TenantId { get; init; } = "school-demo";
    public string UserId { get; init; } = "user-demo";
    public string LanguageCode { get; init; } = "en";
    public MobileTextDirection TextDirection { get; init; } = MobileTextDirection.Ltr;
    public string Source { get; init; } = "user_selected";
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
