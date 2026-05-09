using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Routes;

public sealed class RouteStopSequenceService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<IReadOnlyList<RouteStopSequenceResponse>>> ReplaceAsync(string tenantId, Guid routeId, ReplaceRouteStopSequenceRequest request, CancellationToken cancellationToken = default)
    {
        var route = await dbContext.TransportRoutes.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == routeId, cancellationToken);
        if (route is null) return OperationResult<IReadOnlyList<RouteStopSequenceResponse>>.Failure(new ValidationError("not_found", "Route was not found."));
        if (request.Stops.Count == 0) return OperationResult<IReadOnlyList<RouteStopSequenceResponse>>.Failure(new ValidationError("empty_sequence", "At least one stop is required."));
        if (request.Stops.Select(x => x.SequenceNumber).Distinct().Count() != request.Stops.Count) return OperationResult<IReadOnlyList<RouteStopSequenceResponse>>.Failure(new ValidationError("duplicate_sequence_number", "Sequence numbers must be unique."));
        var stopIds = request.Stops.Select(x => x.TransportStopId).ToArray();
        var stops = await dbContext.TransportStops.Where(x => x.TenantId == tenantId && stopIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
        if (stops.Count != stopIds.Length || stops.Values.Any(x => x.StopStatus != StopStatus.Active || !x.Allows(request.ServiceDirection))) return OperationResult<IReadOnlyList<RouteStopSequenceResponse>>.Failure(new ValidationError("inactive_or_mismatched_stop", "All sequence stops must be active and allow the requested direction."));
        var newVersion = route.NextVersion();
        await dbContext.RouteStopSequences.Where(x => x.TenantId == tenantId && x.TransportRouteId == routeId && x.SequenceStatus == SequenceStatus.Active).ExecuteUpdateAsync(x => x.SetProperty(s => s.SequenceStatus, SequenceStatus.Superseded), cancellationToken);
        route.RouteVersion = newVersion;
        var sequences = request.Stops.OrderBy(x => x.SequenceNumber).Select(item => new RouteStopSequence { TenantId = tenantId, TransportRouteId = routeId, TransportStopId = item.TransportStopId, ServiceDirection = request.ServiceDirection, SequenceNumber = item.SequenceNumber, PlannedArrivalOffset = item.PlannedArrivalOffset, PlannedDepartureOffset = item.PlannedDepartureOffset, RouteVersion = newVersion }).ToList();
        dbContext.RouteStopSequences.AddRange(sequences);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<IReadOnlyList<RouteStopSequenceResponse>>.Success(sequences.Select(x => x.ToResponse()).ToList());
    }
}
