using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Features.Transport.Common.Identity;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Eta;

public sealed class GuardianEtaVisibilityService(SafeSchoolDbContext dbContext, ITransportGuardianLinkProvider guardianLinkProvider)
{
    public async Task<OperationResult<GuardianEtaResponse>> ReadAsync(string tenantId, string guardianReference, string studentProfileId, Guid tripId, CancellationToken cancellationToken = default)
    {
        var link = await guardianLinkProvider.GetLinkAsync(tenantId, guardianReference, studentProfileId, cancellationToken);
        if (link.Status != GuardianLinkStatus.Approved || !link.HasTransportVisibilityScope) return OperationResult<GuardianEtaResponse>.Failure(new ValidationError("guardian_scope_denied", "Guardian cannot view this ETA."));
        var eta = await dbContext.EtaRecords.Where(x => x.TenantId == tenantId && x.TransportTripId == tripId && x.StudentProfileId == studentProfileId).OrderByDescending(x => x.CalculatedAt).FirstOrDefaultAsync(cancellationToken);
        return OperationResult<GuardianEtaResponse>.Success(new GuardianEtaResponse(studentProfileId, tripId, eta?.RouteStopSequenceId ?? Guid.Empty, "Before Boarding", eta?.ToResponse(), false));
    }
}
