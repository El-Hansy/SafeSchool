using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Attendance;

public sealed class AttendanceRuleEvaluatorTests
{
    [Fact]
    public void Evaluate_ClassifiesPresentLateAbsentAndEarlyExit()
    {
        var evaluator = new AttendanceRuleEvaluator();
        var session = new AttendanceSession();
        evaluator.Evaluate(session, null).Should().Be(AttendanceStatus.Absent);
        evaluator.Evaluate(session, new GateScanEvent { Status = ScanEventStatus.Accepted, LocalScanTime = DateTimeOffset.UtcNow.Date.AddHours(8).AddMinutes(30) }).Should().Be(AttendanceStatus.Late);
        evaluator.Evaluate(session, new GateScanEvent { Status = ScanEventStatus.Accepted, LocalScanTime = DateTimeOffset.UtcNow.Date.AddHours(7) }).Should().Be(AttendanceStatus.Present);
    }
}

