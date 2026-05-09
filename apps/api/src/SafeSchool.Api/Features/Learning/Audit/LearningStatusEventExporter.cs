using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Audit;

public sealed class LearningStatusEventExporter
{
    public LearningStatusEvent Export(string tenantId, LearningStatusEventType eventType, string sourceType, string sourceId, bool starEligible = false) => new()
    {
        TenantId = tenantId,
        StatusEventType = eventType,
        SourceType = sourceType,
        SourceId = sourceId,
        ReadyForNotification = true,
        ReadyForStarEvidence = starEligible,
        Payload = $"{eventType}:{sourceType}:{sourceId}"
    };
}
