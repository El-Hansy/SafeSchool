using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Eta;

public sealed class EtaCalculationService(SafeSchoolDbContext dbContext)
{
    public async Task<IReadOnlyList<EtaRecordResponse>> RecalculateAsync(string tenantId, Guid tripId, EtaRecalculateRequest request, CancellationToken cancellationToken = default)
    {
        var trip = await dbContext.TransportTrips.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == tripId, cancellationToken);
        if (trip is null || trip.TripStatus != TripStatus.Active) return [];
        var latest = await dbContext.TransportLocationUpdates.Where(x => x.TenantId == tenantId && x.TransportTripId == tripId && x.AcceptanceStatus == LocationAcceptanceStatus.Accepted).OrderByDescending(x => x.ReportedAt).FirstOrDefaultAsync(cancellationToken);
        var records = request.IncludeRouteStopSequenceIds.Select((stopId, index) => new EtaRecord { TenantId = tenantId, TransportTripId = trip.Id, TransportRouteId = trip.TransportRouteId, RouteStopSequenceId = stopId, EstimatedArrivalTime = latest is null ? null : DateTimeOffset.UtcNow.AddMinutes(5 + index * 4), EtaState = latest is null ? EtaState.Unavailable : EtaState.Available, ConfidenceState = latest is null ? EtaConfidenceState.Unavailable : EtaConfidenceState.Medium, FreshnessStatus = latest is null ? LocationFreshnessStatus.Unavailable : latest.FreshnessStatus, SourceLocationUpdateId = latest?.Id }).ToList();
        dbContext.EtaRecords.AddRange(records);
        await dbContext.SaveChangesAsync(cancellationToken);
        return records.Select(x => x.ToResponse()).ToList();
    }
}
