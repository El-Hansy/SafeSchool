namespace SafeSchool.Api.Features.AttendanceAccess.Common;

public enum AttendanceDirection { Entry, Exit }
public enum GateStatus { Draft, Active, Suspended, Decommissioned }
public enum ScanPointStatus { Pending, Active, Suspended, Retired }
public enum ScanMethod { Nfc, Qr, ManualReview }
public enum CredentialEvidenceStatus { Active, Expired, Suspended, Revoked, Replaced, Unknown, CrossTenant }
public enum ScanEventStatus { Accepted, Denied, Flagged, Duplicate, NeedsReview }
public enum CampusAccessDecisionOutcome { Allowed, Denied, Flagged, NeedsReview }
public enum CampusState { Unknown, OnCampus, OffCampus, NeedsReview }
public enum OfflineSyncStatus { Pending, Accepted, Rejected, Duplicate, Conflict }
public enum AttendanceSessionStatus { Draft, Active, Generated, Reopened, Closed }
public enum AttendanceStatus { Present, Late, Absent, EarlyExit, NeedsReview, Excused }
public enum NotificationEligibilityStatus { Eligible, Suppressed, Visible, Attempted, Failed, Withdrawn }
public enum AnomalyType { MissingEntry, MissingExit, DuplicateScan, InvalidCredential, OutOfOrderScan, ConflictingCampusState, DelayedOfflineConflict, LateArrival, EarlyExit, ManualReviewRequired }
public enum AnomalySeverity { Low, Medium, High, Critical }
public enum AnomalyStatus { New, Assigned, InReview, Resolved, Dismissed, Escalated, Reopened }
public enum ManualReviewStatus { Requested, InReview, Approved, Rejected, Corrected, Dismissed }
public enum ReviewAction { Assign, Resolve, Dismiss, Reopen, Correct }

