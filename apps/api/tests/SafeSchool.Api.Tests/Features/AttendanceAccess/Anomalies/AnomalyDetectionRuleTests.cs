using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Anomalies;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Anomalies;

public sealed class AnomalyDetectionRuleTests
{
    [Fact]
    public void Classify_CreatesInvalidCredentialAndLateArrivalAnomalies()
    {
        var scan = new GateScanEvent { Status = ScanEventStatus.Flagged, Direction = AttendanceDirection.Entry, LocalScanTime = DateTimeOffset.UtcNow.Date.AddHours(9), DecisionReason = "invalid credential" };
        var results = new AnomalyDetectionRuleSet().Classify(scan);
        results.Select(x => x.Type).Should().Contain([AnomalyType.InvalidCredential, AnomalyType.LateArrival]);
    }
}

