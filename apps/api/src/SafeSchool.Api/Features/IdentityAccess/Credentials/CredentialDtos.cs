namespace SafeSchool.Api.Features.IdentityAccess.Credentials;

public sealed record IssueNfcCredentialRequest(string CardReference, string? CardLabel, DateTimeOffset ValidFrom, DateTimeOffset? ValidUntil, string StatusReason, string ClientRequestId);
public sealed record CreateQrCredentialRequest(string QrReference, DateTimeOffset ValidFrom, DateTimeOffset ValidUntil, string RotationReason, string ClientRequestId);
public sealed record CredentialActionRequest(string Reason, string ClientRequestId);
public sealed record CredentialResponse(Guid IdentityCredentialId, string TenantId, Guid StudentProfileId, string CredentialType, string CredentialReference, string CredentialStatus, DateTimeOffset ValidFrom, DateTimeOffset? ValidUntil, DateTimeOffset IssuedAt, string StatusReason);
public sealed record CredentialStatusSnapshotResponse(Guid CredentialStatusSnapshotId, string TenantId, Guid StudentProfileId, Guid IdentityCredentialId, string CredentialType, string CredentialStatus, DateTimeOffset ValidFrom, DateTimeOffset? ValidUntil, DateTimeOffset? RevokedAt, DateTimeOffset SnapshotGeneratedAt, DateTimeOffset SnapshotExpiresAt);
