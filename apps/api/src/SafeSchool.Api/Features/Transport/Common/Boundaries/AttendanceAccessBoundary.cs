namespace SafeSchool.Api.Features.Transport.Common.Boundaries;

public sealed record CampusContextReference(string SchoolAccountId, string CampusReference, string SeparationReason);

public interface IAttendanceAccessBoundary
{
    Task<CampusContextReference> GetCampusContextAsync(string tenantId, string campusReference, CancellationToken cancellationToken = default);
    Task AssertTransportDoesNotMutateAttendanceAsync(string tenantId, string sourceReference, CancellationToken cancellationToken = default);
}

public sealed class AttendanceAccessBoundary : IAttendanceAccessBoundary
{
    public Task<CampusContextReference> GetCampusContextAsync(string tenantId, string campusReference, CancellationToken cancellationToken = default) =>
        Task.FromResult(new CampusContextReference(tenantId, campusReference, "Transport reads campus context only."));

    public Task AssertTransportDoesNotMutateAttendanceAsync(string tenantId, string sourceReference, CancellationToken cancellationToken = default) => Task.CompletedTask;
}
