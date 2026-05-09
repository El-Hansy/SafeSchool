using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Anomalies;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Anomalies;

public sealed class AttendanceAnomalyLifecycleTests
{
    [Fact]
    public void CanTransitionTo_RequiresValidLifecycle()
    {
        var anomaly = new AttendanceAnomaly { Status = AnomalyStatus.New };
        anomaly.CanTransitionTo(AnomalyStatus.Assigned).Should().BeTrue();
        anomaly.CanTransitionTo(AnomalyStatus.Reopened).Should().BeFalse();
    }
}

