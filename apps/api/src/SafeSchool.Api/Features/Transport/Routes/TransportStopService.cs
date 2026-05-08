using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Audit;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Routes;

public sealed class TransportStopService(SafeSchoolDbContext dbContext, TransportPermissionGuard guard, ITransportAuditWriter auditWriter)
{
    public async Task<OperationResult<StopResponse>> CreateAsync(string tenantId, CreateStopRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, TransportCapabilities.RouteStopManagement, TransportPermissionCatalog.RoutesManage, targetType: "TransportStop", targetReference: request.StopCode, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<StopResponse>.Failure(allowed.Errors.ToArray());
        var stop = new TransportStop { TenantId = tenantId, StopName = request.StopName, StopCode = request.StopCode, StopReference = request.StopReference, PickupAllowed = request.PickupAllowed, DropAllowed = request.DropAllowed, StopStatus = request.StopStatus };
        dbContext.TransportStops.Add(stop);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new TransportAuditEvent { TenantId = tenantId, EventCategory = "Route", EventType = "transport.stops.create", SubjectType = "TransportStop", SubjectReference = stop.Id.ToString(), Reason = "Stop created" }, cancellationToken);
        return OperationResult<StopResponse>.Success(stop.ToResponse());
    }

    public async Task<IReadOnlyList<StopResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.TransportStops.Where(x => x.TenantId == tenantId).OrderBy(x => x.StopName).Select(x => x.ToResponse()).ToListAsync(cancellationToken);

    public async Task<OperationResult<StopResponse>> UpdateAsync(string tenantId, Guid stopId, UpdateStopRequest request, CancellationToken cancellationToken = default)
    {
        var stop = await dbContext.TransportStops.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == stopId, cancellationToken);
        if (stop is null) return OperationResult<StopResponse>.Failure(new ValidationError("not_found", "Stop was not found in this school account."));
        stop.StopName = request.StopName ?? stop.StopName;
        stop.StopStatus = request.StopStatus ?? stop.StopStatus;
        stop.PickupAllowed = request.PickupAllowed ?? stop.PickupAllowed;
        stop.DropAllowed = request.DropAllowed ?? stop.DropAllowed;
        stop.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<StopResponse>.Success(stop.ToResponse());
    }
}
