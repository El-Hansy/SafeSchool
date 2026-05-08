using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Notifications;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Notifications;

public sealed class EntryExitNotificationContractTests
{
    [Fact]
    public void ResponseDto_ExposesVisibilityFields()
    {
        typeof(NotificationRecordResponse).GetProperties().Select(x => x.Name).Should().Contain(["GuardianReference", "EligibilityStatus", "SuppressionReason"]);
    }
}

