using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common.Trace;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed class TransportNotificationTraceService(SafeSchoolDbContext dbContext)
{
    public async Task<TransportNotificationTraceResponse?> TraceAsync(string tenantId, Guid notificationRecordId, CancellationToken cancellationToken = default)
    {
        var record = await dbContext.TransportNotificationRecords.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == notificationRecordId, cancellationToken);
        return record is null ? null : new TransportNotificationTraceResponse(record.Id, [new TransportTraceReference("SourceEvent", record.SourceEventReference, "Source event"), new TransportTraceReference("GuardianLink", record.GuardianLinkId, "Guardian link")]);
    }
}
