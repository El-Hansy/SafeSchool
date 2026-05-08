using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;

namespace SafeSchool.Api.Features.AttendanceAccess.Anomalies;

public sealed class AnomalyDetectionRuleSet
{
    public IReadOnlyList<(AnomalyType Type, AnomalySeverity Severity, string Reason)> Classify(GateScanEvent scanEvent)
    {
        var results = new List<(AnomalyType, AnomalySeverity, string)>();
        if (scanEvent.Status is ScanEventStatus.Denied or ScanEventStatus.Flagged)
        {
            results.Add((AnomalyType.InvalidCredential, AnomalySeverity.High, scanEvent.DecisionReason));
        }

        if (scanEvent.Status == ScanEventStatus.Duplicate)
        {
            results.Add((AnomalyType.DuplicateScan, AnomalySeverity.Medium, "Duplicate scan evidence."));
        }

        if (scanEvent.Status == ScanEventStatus.NeedsReview)
        {
            results.Add((AnomalyType.ManualReviewRequired, AnomalySeverity.Medium, "Scan requires manual review."));
        }

        if (scanEvent.Direction == AttendanceDirection.Entry && TimeOnly.FromDateTime(scanEvent.LocalScanTime.LocalDateTime) > new TimeOnly(8, 0))
        {
            results.Add((AnomalyType.LateArrival, AnomalySeverity.Low, "Entry scan is later than configured threshold."));
        }

        if (scanEvent.Direction == AttendanceDirection.Exit && TimeOnly.FromDateTime(scanEvent.LocalScanTime.LocalDateTime) < new TimeOnly(13, 0))
        {
            results.Add((AnomalyType.EarlyExit, AnomalySeverity.Medium, "Exit scan is earlier than configured threshold."));
        }

        return results;
    }
}

