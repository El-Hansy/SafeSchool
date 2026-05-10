namespace SafeSchool.Api.Features.Mobile;

public sealed record StudentMobileSummary(string StudentId, string StudentName, IReadOnlyList<string> EnabledModules, IReadOnlyList<string> Actions);
