namespace SafeSchool.Api.Features.IdentityAccess.Common;

public enum ProfileStatus { Draft, Active, Suspended, Deactivated, Archived }
public enum DuplicateReviewStatus { Clear, PossibleDuplicate, ConfirmedDuplicate, Resolved }
public enum GuardianStatus { Active, Suspended, Deactivated, Archived }
public enum GuardianIdentityReviewStatus { Unverified, Verified, Rejected, Suspended }
public enum GuardianLinkStatus { Pending, Approved, Suspended, Expired, Rejected, Removed }
public enum CredentialStatus { Proposed, Active, Suspended, Replaced, Expired, Revoked }
public enum CredentialType { NfcCard, QrFallback }
public enum RoleStatus { Draft, Active, Suspended, Deprecated }
public enum PermissionStatus { Active, Deprecated, Retired }
public enum AssignmentStatus { Pending, Active, Suspended, Revoked, Expired }
public enum AccessDecisionResult { Allowed, Denied }
public enum AuditReviewStatus { Recorded, Reviewed, Escalated, Corrected }
public enum FeatureAvailabilityStatus { Proposed, Enabled, Disabled, Suspended, Retired }
