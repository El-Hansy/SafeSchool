namespace SafeSchool.Api.Features.IdentityAccess.Guardians;

public sealed record GuardianRequest(string DisplayName, IReadOnlyList<string> ContactMethods, string ClientRequestId);
public sealed record GuardianUpdateRequest(string? DisplayName, IReadOnlyList<string>? ContactMethods, string ReviewReason, string ClientRequestId);
public sealed record GuardianResponse(Guid GuardianId, string TenantId, string DisplayName, string GuardianStatus, string IdentityReviewStatus);
public sealed record GuardianLinkRequest(Guid GuardianId, string RelationshipType, Dictionary<string, string> AccessScope, DateTimeOffset ValidFrom, DateTimeOffset? ValidUntil, string LinkStatus, string ReviewReason, string ClientRequestId);
public sealed record GuardianLinkStateRequest(string ReviewReason, string ClientRequestId);
public sealed record GuardianLinkResponse(Guid GuardianLinkId, string TenantId, Guid StudentProfileId, Guid GuardianId, string RelationshipType, Dictionary<string, string> AccessScope, string LinkStatus, DateTimeOffset ValidFrom, DateTimeOffset? ValidUntil, string ReviewReason);
