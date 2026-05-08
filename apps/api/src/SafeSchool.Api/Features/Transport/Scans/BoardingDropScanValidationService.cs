using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Assignments;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;
using SafeSchool.Api.Features.Transport.Trips;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Scans;

public sealed record BoardingDropScanValidationResult(bool Allowed, TransportScanDecision Decision, string StudentProfileId, Guid? AssignmentId, string Reason);

public sealed class BoardingDropScanValidationService(SafeSchoolDbContext dbContext, TransportTripScanContextService tripContext, ITransportCredentialEvidenceProvider credentialProvider)
{
    public async Task<BoardingDropScanValidationResult> ValidateAsync(string tenantId, BoardingDropScanRequest request, CancellationToken cancellationToken = default)
    {
        var tripResult = await tripContext.RequireActiveTripAsync(tenantId, request.TransportTripId, request.TrackingDeviceReference, cancellationToken);
        if (!tripResult.Succeeded) return new(false, TransportScanDecision.Denied, string.Empty, null, tripResult.Errors[0].Message);
        var credential = await credentialProvider.GetEvidenceAsync(tenantId, request.CredentialReference, cancellationToken);
        if (credential.Status != CredentialEvidenceStatus.Active) return new(false, credential.Status == CredentialEvidenceStatus.CrossTenant ? TransportScanDecision.Denied : TransportScanDecision.Flagged, credential.StudentProfileId, null, $"Credential status is {credential.Status}.");
        var trip = tripResult.Value!;
        var assignment = await dbContext.StudentTransportAssignments.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.StudentProfileId == credential.StudentProfileId && x.TransportRouteId == trip.TransportRouteId && x.AssignmentStatus == AssignmentStatus.Active, cancellationToken);
        if (assignment is null) return new(false, TransportScanDecision.NeedsReview, credential.StudentProfileId, null, "Student does not have an active assignment for this route.");
        return new(true, TransportScanDecision.Accepted, credential.StudentProfileId, assignment.Id, "Credential active, trip active, and assignment matched.");
    }
}
