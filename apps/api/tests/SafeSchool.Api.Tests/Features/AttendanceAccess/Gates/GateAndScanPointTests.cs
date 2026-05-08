using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Gates;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Gates;

public sealed class GateAndScanPointTests
{
    [Fact]
    public void GateAndScanPoint_ValidateLifecycleAndDirections()
    {
        var gate = new Gate { Status = GateStatus.Draft, AllowsEntry = true, AllowsExit = false };
        gate.CanTransitionTo(GateStatus.Active).Should().BeTrue();
        gate.Allows(AttendanceDirection.Entry).Should().BeTrue();
        gate.Allows(AttendanceDirection.Exit).Should().BeFalse();

        var scanPoint = new ScanPoint { Status = ScanPointStatus.Pending };
        scanPoint.CanTransitionTo(ScanPointStatus.Active).Should().BeTrue();
    }
}

