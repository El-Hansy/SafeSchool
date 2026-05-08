using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Gates;

public sealed class ScanPointService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<ScanPointResponse>> RegisterAsync(string tenantId, CreateScanPointRequest request, CancellationToken cancellationToken = default)
    {
        var gate = await dbContext.Gates.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == request.GateId, cancellationToken);
        if (gate is null || gate.Status != GateStatus.Active)
        {
            return OperationResult<ScanPointResponse>.Failure(new ValidationError("invalid_gate", "Scan point requires an active gate in the same school account."));
        }

        var scanPoint = new ScanPoint
        {
            TenantId = tenantId,
            GateId = request.GateId,
            DeviceReference = request.DeviceReference,
            AssignedActorReference = request.AssignedActorReference,
            OfflineAllowed = request.OfflineAllowed,
            AllowsEntry = gate.AllowsEntry,
            AllowsExit = gate.AllowsExit,
            Status = ScanPointStatus.Active
        };
        dbContext.ScanPoints.Add(scanPoint);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<ScanPointResponse>.Success(scanPoint.ToResponse());
    }

    public async Task<OperationResult<ScanPointResponse>> UpdateAsync(string tenantId, Guid scanPointId, UpdateScanPointRequest request, CancellationToken cancellationToken = default)
    {
        var scanPoint = await dbContext.ScanPoints.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == scanPointId, cancellationToken);
        if (scanPoint is null)
        {
            return OperationResult<ScanPointResponse>.Failure(new ValidationError("not_found", "Scan point was not found in this school account."));
        }

        if (request.Status is not null && !scanPoint.CanTransitionTo(request.Status.Value))
        {
            return OperationResult<ScanPointResponse>.Failure(new ValidationError("invalid_transition", "Scan point cannot move to the requested status."));
        }

        scanPoint.Status = request.Status ?? scanPoint.Status;
        scanPoint.OfflineAllowed = request.OfflineAllowed ?? scanPoint.OfflineAllowed;
        scanPoint.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<ScanPointResponse>.Success(scanPoint.ToResponse());
    }
}

public static class ScanPointMapping
{
    public static ScanPointResponse ToResponse(this ScanPoint scanPoint) => new(scanPoint.Id, scanPoint.GateId, scanPoint.DeviceReference, scanPoint.AssignedActorReference, scanPoint.Status, scanPoint.OfflineAllowed);
}

