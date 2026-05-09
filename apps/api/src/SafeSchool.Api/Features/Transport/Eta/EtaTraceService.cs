using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common.Trace;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Eta;

public sealed class EtaTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<EtaTraceResponse?> TraceAsync(string tenantId, Guid etaRecordId, CancellationToken cancellationToken = default)
    {
        var eta = await dbContext.EtaRecords.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == etaRecordId, cancellationToken);
        return eta is null ? null : new EtaTraceResponse(etaRecordId, [new TransportTraceReference("Trip", eta.TransportTripId.ToString(), "Trip"), new TransportTraceReference("Stop", eta.RouteStopSequenceId.ToString(), "Route stop"), new TransportTraceReference("Location", eta.SourceLocationUpdateId?.ToString() ?? string.Empty, "Source location")]);
    }
}
