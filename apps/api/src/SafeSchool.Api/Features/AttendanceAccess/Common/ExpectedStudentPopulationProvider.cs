namespace SafeSchool.Api.Features.AttendanceAccess.Common;

public interface IExpectedStudentPopulationProvider
{
    Task<IReadOnlyList<string>> GetExpectedStudentsAsync(string tenantId, string populationRule, CancellationToken cancellationToken = default);
}

public sealed class DefaultExpectedStudentPopulationProvider : IExpectedStudentPopulationProvider
{
    public Task<IReadOnlyList<string>> GetExpectedStudentsAsync(string tenantId, string populationRule, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<string> students = string.IsNullOrWhiteSpace(populationRule)
            ? ["student-default"]
            : populationRule.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return Task.FromResult(students);
    }
}

