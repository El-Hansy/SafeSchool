namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public sealed record RoleRequest(string RoleKey, string DisplayName, string RoleScope, string ReviewReason, string ClientRequestId);
public sealed record RoleResponse(Guid RoleId, string TenantId, string RoleKey, string DisplayName, string RoleScope, string RoleStatus);
public sealed record PermissionResponse(Guid PermissionId, string PermissionKey, string Description, string PermissionScope, bool SensitiveAction);
public sealed record ReplaceRolePermissionsRequest(IReadOnlyList<string> PermissionKeys, string ReviewReason, string ClientRequestId);
public sealed record RoleAssignmentRequest(string ActorReference, Guid RoleId, string AssignmentStatus, DateTimeOffset ValidFrom, DateTimeOffset? ValidUntil, string ReviewReason, string ClientRequestId);
public sealed record AccessDecisionResponse(Guid AccessDecisionId, string TenantId, string ActorReference, string AttemptedAction, string TargetType, string TargetReference, string Decision, string DecisionReason, DateTimeOffset DecidedAt);
public sealed record AuditEventResponse(Guid AuditEventId, string TenantId, string EventCategory, string EventType, string ActorReference, string SubjectType, string SubjectReference, string Reason, DateTimeOffset EventTime);
