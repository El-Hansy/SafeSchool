using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Audit;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Routes;

public sealed class TransportRouteService(SafeSchoolDbContext dbContext, TransportPermissionGuard guard, ITransportAuditWriter auditWriter)
{
    public async Task<OperationResult<RouteResponse>> CreateAsync(string tenantId, CreateRouteRequest request, CancellationToken cancellationToken = default)
    {
        var allowed = await guard.RequireAsync(tenantId, TransportCapabilities.RouteStopManagement, TransportPermissionCatalog.RoutesManage, targetType: "TransportRoute", targetReference: request.RouteCode, cancellationToken: cancellationToken);
        if (!allowed.Succeeded) return OperationResult<RouteResponse>.Failure(allowed.Errors.ToArray());
        if (await dbContext.TransportRoutes.AnyAsync(x => x.TenantId == tenantId && x.RouteCode == request.RouteCode, cancellationToken)) return OperationResult<RouteResponse>.Failure(new ValidationError("duplicate_route_code", "Route code already exists in this school account.", nameof(request.RouteCode)));
        var route = new TransportRoute { TenantId = tenantId, RouteName = request.RouteName, RouteCode = request.RouteCode, ServiceDirection = request.ServiceDirection, CampusReference = request.CampusReference, PlannedStartTime = request.PlannedStartTime, PlannedEndTime = request.PlannedEndTime, RouteStatus = request.RouteStatus };
        dbContext.TransportRoutes.Add(route);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new TransportAuditEvent { TenantId = tenantId, EventCategory = "Route", EventType = "transport.routes.create", SubjectType = "TransportRoute", SubjectReference = route.Id.ToString(), Reason = "Route created" }, cancellationToken);
        return OperationResult<RouteResponse>.Success(route.ToResponse());
    }

    public async Task<IReadOnlyList<RouteResponse>> ListAsync(string tenantId, CancellationToken cancellationToken = default) =>
        await dbContext.TransportRoutes.Where(x => x.TenantId == tenantId).OrderBy(x => x.RouteCode).Select(x => x.ToResponse()).ToListAsync(cancellationToken);

    public async Task<OperationResult<RouteResponse>> UpdateAsync(string tenantId, Guid routeId, UpdateRouteRequest request, CancellationToken cancellationToken = default)
    {
        var route = await dbContext.TransportRoutes.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == routeId, cancellationToken);
        if (route is null) return OperationResult<RouteResponse>.Failure(new ValidationError("not_found", "Route was not found in this school account."));
        if (request.RouteStatus == RouteStatus.Active)
        {
            var activeStops = await dbContext.RouteStopSequences.CountAsync(x => x.TenantId == tenantId && x.TransportRouteId == routeId && x.SequenceStatus == SequenceStatus.Active, cancellationToken);
            if (!route.CanActivate(activeStops)) return OperationResult<RouteResponse>.Failure(new ValidationError("route_not_ready", "Active routes require valid route details and active stops."));
        }
        route.RouteName = request.RouteName ?? route.RouteName;
        route.RouteStatus = request.RouteStatus ?? route.RouteStatus;
        route.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditWriter.RecordAsync(new TransportAuditEvent { TenantId = tenantId, EventCategory = "Route", EventType = "transport.routes.update", SubjectType = "TransportRoute", SubjectReference = route.Id.ToString(), Reason = request.Reason }, cancellationToken);
        return OperationResult<RouteResponse>.Success(route.ToResponse());
    }
}
