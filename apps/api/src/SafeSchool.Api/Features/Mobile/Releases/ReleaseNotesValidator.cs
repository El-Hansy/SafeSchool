namespace SafeSchool.Api.Features.Mobile;

public sealed class ReleaseNotesValidator
{
    public bool HasArabicAndEnglish(string releaseNotes) => !string.IsNullOrWhiteSpace(releaseNotes);
}
