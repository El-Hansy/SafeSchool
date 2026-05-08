using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.StudentProfiles;
using SafeSchool.Api.Tests.Features.IdentityAccess.Fixtures;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.StudentProfiles;

public sealed class DuplicateStudentProfileDetectorTests
{
    [Fact]
    public async Task CheckAsync_FindsActiveStudentNumberInsideSameTenant()
    {
        var fixture = new IdentityAccessTestFixture();
        await using var dbContext = fixture.CreateDbContext();
        dbContext.StudentProfiles.Add(StudentProfileTestData.ActiveStudent());
        await dbContext.SaveChangesAsync();
        var detector = new DuplicateStudentProfileDetector(dbContext);

        var result = await detector.CheckAsync("school-1", new DuplicateCheckRequest("S-1001", []));

        result.MatchingProfileIds.Should().HaveCount(1);
    }
}
