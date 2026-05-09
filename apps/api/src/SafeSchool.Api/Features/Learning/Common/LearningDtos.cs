using SafeSchool.Api.Features.Learning.Common.Trace;

namespace SafeSchool.Api.Features.Learning.Common;

public sealed record LearningPagedRequest(int Page = 1, int PageSize = 25);
public sealed record LearningTypedError(string Code, string Message, string? Field = null);
public sealed record LearningCapabilityStatus(string CapabilityKey, bool Enabled, string Detail);
public sealed record LearningVisibilityPolicy(bool StudentVisible, bool GuardianVisible, bool StaffOnlyDetailsHidden);
public sealed record LearningTenantScope(string SchoolAccountId, string ActorReference, bool PlatformReviewScope = false);
public sealed record LearningSourceReference(string SourceType, string SourceId, string? SourceEventReference = null);
public sealed record LearningAuditReference(Guid AuditEventId, string EventType, DateTimeOffset EventTime);
public sealed record LearningIdempotencyKey(string Kind, string Key, string Fingerprint);
public sealed record LearningStatusEventPayload(string EventType, string SourceType, string SourceId, bool ReadyForNotification, bool ReadyForStarEvidence);
public sealed record LearningCommandResult(string Status, IReadOnlyList<LearningTraceReference> TraceLinks);
