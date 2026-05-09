using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Tracking;

public sealed class LocationRetentionService(SafeSchoolDbContext dbContext)
{
    public async Task<int> ConvertExpiredDetailedLocationsAsync(string tenantId, DateTimeOffset now, CancellationToken cancellationToken = default)
    {
        var cutoff = now.AddDays(-30);
        var updates = await dbContext.TransportLocationUpdates.Where(x => x.TenantId == tenantId && x.RetentionState == RetentionState.Detailed && x.ReportedAt < cutoff).ToListAsync(cancellationToken);
        foreach (var update in updates)
        {
            update.ConvertToSummary();
            update.LocationReference = "summary-retained";
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        return updates.Count;
    }
}
