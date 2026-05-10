using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Gates;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Scans;

public sealed record ScanValidationResult(bool Allowed, ScanEventStatus Status, string StudentProfileId, CredentialEvidenceStatus CredentialStatus, string Reason);

public sealed class ScanValidationService(
    SafeSchoolDbContext dbContext,
    AttendanceAccessPermissionGuard guard,
    IIdentityCredentialEvidenceProvider credentialEvidenceProvider)
{
    public async Task<ScanValidationResult> ValidateAsync(string tenantId, RecordScanRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, AttendanceAccessCapabilities.GateScanning, AttendanceAccessPermissionCatalog.ScansRecord, targetType: "GateScan", targetReference: request.ClientScanId, cancellationToken: cancellationToken);
        if (!allowed.Succeeded)
        {
            return new(false, ScanEventStatus.Denied, string.Empty, CredentialEvidenceStatus.Unknown, allowed.Errors[0].Message);
        }

        var gate = await dbContext.Gates.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == request.GateId, cancellationToken);
        var scanPoint = await dbContext.ScanPoints.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == request.ScanPointId, cancellationToken);
        if (gate is null || gate.Status != GateStatus.Active || !gate.Allows(request.Direction))
        {
            return new(false, ScanEventStatus.Denied, string.Empty, CredentialEvidenceStatus.Unknown, "Gate is not active or does not allow this direction.");
        }

        if (scanPoint is null || scanPoint.Status != ScanPointStatus.Active || scanPoint.GateId != gate.Id || !scanPoint.Allows(request.Direction))
        {
            return new(false, ScanEventStatus.Denied, string.Empty, CredentialEvidenceStatus.Unknown, "Scan point is not authorized for this gate and direction.");
        }

        var evidence = await credentialEvidenceProvider.GetEvidenceAsync(tenantId, request.CredentialReference, cancellationToken);
        return evidence.Status == CredentialEvidenceStatus.Active
            ? new(true, ScanEventStatus.Accepted, evidence.StudentProfileId, evidence.Status, "Credential is active.")
            : new(false, evidence.Status == CredentialEvidenceStatus.CrossTenant ? ScanEventStatus.Denied : ScanEventStatus.Flagged, evidence.StudentProfileId, evidence.Status, $"Credential status is {evidence.Status}.");
    }
}

