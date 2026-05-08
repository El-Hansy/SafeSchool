using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Transport.Notifications;

public sealed class TransportNotificationService(SafeSchoolDbContext dbContext, TransportNotificationEligibilityService eligibilityService)
{
    public async Task<TransportNotificationRecordResponse> CreateAsync(string tenantId, string guardianReference, string studentProfileId, TransportEventType eventType, string sourceReference, Guid? tripId, bool sourceAcceptedOrReviewed, CancellationToken cancellationToken = default)
    {
        var eligibility = await eligibilityService.EvaluateAsync(tenantId, guardianReference, studentProfileId, eventType, sourceAcceptedOrReviewed, cancellationToken);
        var existing = await dbContext.TransportNotificationRecords.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.SourceEventReference == sourceReference && x.StudentProfileId == studentProfileId && x.EventType == eventType, cancellationToken);
        if (existing is not null) return existing.ToResponse();
        var record = new TransportNotificationRecord { TenantId = tenantId, GuardianRecordId = guardianReference, GuardianLinkId = guardianReference, StudentProfileId = studentProfileId, TransportTripId = tripId, EventType = eventType, SourceEventReference = sourceReference, NotificationStatus = eligibility.Eligible ? NotificationStatus.Visible : NotificationStatus.Suppressed, SuppressionReason = eligibility.Eligible ? string.Empty : eligibility.Reason, VisibleStatus = eligibility.Eligible ? $"Transport event: {eventType}" : string.Empty };
        dbContext.TransportNotificationRecords.Add(record);
        await dbContext.SaveChangesAsync(cancellationToken);
        return record.ToResponse();
    }
}
