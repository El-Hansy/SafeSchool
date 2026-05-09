using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.Common;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.StudentProfiles;

public sealed class StudentProfileTests
{
    [Fact]
    public void CanTransitionTo_AllowsDraftActivationWhenDuplicateReviewIsClear()
    {
        var profile = StudentProfileTestData.ActiveStudent();
        profile.ProfileStatus = ProfileStatus.Draft;

        profile.CanTransitionTo(ProfileStatus.Active).Should().BeTrue();
    }

    [Fact]
    public void CanTransitionTo_BlocksActivationWhenDuplicateReviewIsOpen()
    {
        var profile = StudentProfileTestData.ActiveStudent();
        profile.ProfileStatus = ProfileStatus.Draft;
        profile.DuplicateReviewStatus = DuplicateReviewStatus.PossibleDuplicate;

        profile.CanTransitionTo(ProfileStatus.Active).Should().BeFalse();
    }
}
