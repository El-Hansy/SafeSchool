using SafeSchool.Api.Features.Transport.Common;

namespace SafeSchool.Api.Features.Transport.Scans;

public sealed class TransportStatusDecisionService
{
    public TransportStatusAfter Calculate(ScanDirection direction, TransportScanDecision decision, TransportReviewStatus reviewStatus = TransportReviewStatus.NotRequired)
    {
        if (decision == TransportScanDecision.Duplicate) return TransportStatusAfter.Unchanged;
        if (decision == TransportScanDecision.NeedsReview && reviewStatus != TransportReviewStatus.Corrected) return TransportStatusAfter.NeedsReview;
        if (decision != TransportScanDecision.Accepted && reviewStatus != TransportReviewStatus.Corrected) return TransportStatusAfter.Unchanged;
        return direction == ScanDirection.Boarding ? TransportStatusAfter.Onboard : TransportStatusAfter.Dropped;
    }
}
