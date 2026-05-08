using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;

public sealed class FakeExpectedStudentPopulationProvider : IExpectedStudentPopulationProvider
{
    public IReadOnlyList<string> Students { get; set; } = ["student-active"];

    public Task<IReadOnlyList<string>> GetExpectedStudentsAsync(string tenantId, string populationRule, CancellationToken cancellationToken = default) =>
        Task.FromResult(Students);
}

