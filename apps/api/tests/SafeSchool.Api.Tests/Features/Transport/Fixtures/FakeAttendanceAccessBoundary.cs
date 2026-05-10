using SafeSchool.Api.Features.Transport.Common.Boundaries;

namespace SafeSchool.Api.Tests.Features.Transport.Fixtures;

public sealed class FakeAttendanceAccessBoundary : IAttendanceAccessBoundary
{
    public bool MutationAttempted { get; private set; }
    public Task<CampusContextReference> GetCampusContextAsync(string tenantId, string campusReference, CancellationToken cancellationToken = default) => Task.FromResult(new CampusContextReference(tenantId, campusReference, "fake context"));
    public Task AssertTransportDoesNotMutateAttendanceAsync(string tenantId, string sourceReference, CancellationToken cancellationToken = default)
    {
        MutationAttempted = false;
        return Task.CompletedTask;
    }
}
