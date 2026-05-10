namespace SafeSchool.Api.Features.Mobile;

public sealed class TeacherMobileAdapter
{
    public StaffMobileWorkspace Workspace() => new(MobileRoleCodes.Teacher, "Learning, attendance, and behavior updates", ["view_class", "record_behavior", "review_assignment"], false);
}
