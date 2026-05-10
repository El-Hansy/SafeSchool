namespace SafeSchool.Api.Features.Mobile;

public sealed class StudentMobileService
{
    public StudentMobileSummary Summary() => new("student-self", "Student Demo", ["learning", "communications", "complaints", "documents", "certificates"], ["view_learning", "submit_complaint", "view_documents"]);
}
