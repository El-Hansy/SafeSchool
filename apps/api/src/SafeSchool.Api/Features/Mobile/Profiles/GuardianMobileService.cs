namespace SafeSchool.Api.Features.Mobile;

public sealed class GuardianMobileService
{
    public IReadOnlyList<GuardianMobileSummary> Summaries() =>
    [
        new("student-amina", "Amina Hassan", ["attendance", "transport", "wallet", "learning", "requests", "complaints", "communications", "documents", "certificates"], ["submit_request", "submit_complaint", "view_notifications"]),
        new("student-omar", "Omar Saleh", ["attendance", "transport", "learning", "communications"], ["submit_complaint", "view_notifications"])
    ];
}
