using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Scans;

public sealed class CampusAccessDecisionService(SafeSchoolDbContext dbContext)
{
    public async Task<CampusAccessDecision> DecideAsync(GateScanEvent scanEvent, CredentialEvidenceStatus credentialStatus, CancellationToken cancellationToken = default)
    {
        var decision = scanEvent.Status switch
        {
            ScanEventStatus.Accepted => CampusAccessDecisionOutcome.Allowed,
            ScanEventStatus.Flagged => CampusAccessDecisionOutcome.Flagged,
            ScanEventStatus.NeedsReview => CampusAccessDecisionOutcome.NeedsReview,
            _ => CampusAccessDecisionOutcome.Denied
        };

        var record = new CampusAccessDecision
        {
            TenantId = scanEvent.TenantId,
            GateScanEventId = scanEvent.Id,
            Decision = decision,
            DecisionReason = scanEvent.DecisionReason,
            FeatureCapabilityKey = AttendanceAccessCapabilities.GateScanning,
            PermissionKey = AttendanceAccessPermissionCatalog.ScansRecord,
            CredentialStatusUsed = credentialStatus,
            CampusStateAfter = decision == CampusAccessDecisionOutcome.Allowed
                ? scanEvent.Direction == AttendanceDirection.Entry ? CampusState.OnCampus : CampusState.OffCampus
                : CampusState.NeedsReview
        };
        dbContext.CampusAccessDecisions.Add(record);
        await dbContext.SaveChangesAsync(cancellationToken);
        return record;
    }
}

