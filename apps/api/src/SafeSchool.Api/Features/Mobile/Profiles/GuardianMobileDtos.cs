namespace SafeSchool.Api.Features.Mobile;

public sealed record GuardianMobileSummary(string StudentId, string StudentName, IReadOnlyList<string> EnabledModules, IReadOnlyList<string> Actions);
