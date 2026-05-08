namespace SafeSchool.Api.Features.Transport.Common;

public enum VehicleStatus { Draft, Active, Suspended, Retired }
public enum RouteStatus { Draft, Active, Suspended, Retired }
public enum StopStatus { Draft, Active, Suspended, Retired }
public enum SequenceStatus { Active, Superseded, Suspended }
public enum ServiceDirection { Pickup, Dropoff, Both, Combined }
public enum AssignmentStatus { Draft, Active, Suspended, Expired, Removed }
public enum GuardianVisibilityState { GuardianVisible, StaffOnly, Suspended }
public enum TripStatus { Planned, Active, Paused, Completed, Cancelled, NeedsReview }
public enum ScanDirection { Boarding, Drop }
public enum TransportScanMethod { Nfc, Qr, Manual }
public enum TransportScanDecision { Accepted, Denied, Flagged, NeedsReview, Duplicate }
public enum TransportSyncStatus { Pending, Reconciled, PartiallyReconciled, Rejected, Duplicate }
public enum TransportStatusAfter { Waiting, Onboard, Dropped, NeedsReview, Unchanged }
public enum LocationProgressState { NotStarted, EnRoute, ApproachingStop, AtStop, Delayed, Completed }
public enum LocationFreshnessStatus { Current, Stale, Untrusted, Unavailable, RetainedSummaryOnly }
public enum LocationAcceptanceStatus { Accepted, Suppressed, Duplicate, Rejected }
public enum EtaState { Available, Unavailable, Stale, NeedsReview }
public enum EtaConfidenceState { High, Medium, Low, Unavailable }
public enum TransportEventType { Boarding, Drop, Delay, MaterialEtaChange, RouteChange, ReviewedCorrection }
public enum NotificationStatus { Eligible, Created, Visible, Suppressed, Withdrawn, Failed }
public enum TransportAnomalyType { MissedBoarding, MissedDrop, WrongRoute, WrongStop, DuplicateScan, InvalidCredential, OutOfOrderScan, DelayedOfflineConflict, RouteDeviation, StaleLocation, DelayedTrip, ManualReviewRequired }
public enum AnomalySeverity { Low, Medium, High, Critical }
public enum AnomalyStatus { Open, Assigned, Resolved, Dismissed, Reopened }
public enum TransportReviewStatus { NotRequired, NeedsReview, InReview, Corrected, Closed, Rejected }
public enum ManualReviewAction { Approve, Reject, Correct, Withdraw, Dismiss, Reopen }
public enum RetentionState { Detailed, SummaryOnly, ReviewHold }
public enum FeatureRuleStatus { Draft, Active, Suspended, Superseded }
public enum StudentEligibilityStatus { Active, Inactive, Missing, CrossTenant }
public enum CredentialEvidenceStatus { Active, Expired, Suspended, Revoked, Replaced, Unknown, Duplicated, CrossTenant }
public enum GuardianLinkStatus { Approved, Pending, Suspended, Expired, Removed, Rejected, OutOfScope }
