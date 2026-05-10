using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common.Trace;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Scans;

public sealed class BoardingDropScanTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<BoardingDropScanTraceResponse?> TraceAsync(string tenantId, Guid scanEventId, CancellationToken cancellationToken = default)
    {
        var scan = await dbContext.BoardingDropScanEvents.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == scanEventId, cancellationToken);
        if (scan is null) return null;
        return new BoardingDropScanTraceResponse(scanEventId, [new TransportTraceReference("Trip", scan.TransportTripId.ToString(), "Trip"), new TransportTraceReference("Assignment", scan.StudentTransportAssignmentId?.ToString() ?? string.Empty, "Assignment"), new TransportTraceReference("Audit", scanEventId.ToString(), "Scan audit")]);
    }
}
