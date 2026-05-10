namespace SafeSchool.Api.Features.Mobile;

public static class StudentMobileEndpoints
{
    public static RouteGroupBuilder MapStudentMobileEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/student/summary", (StudentMobileService service) => Results.Ok(service.Summary()));
        return group;
    }
}
