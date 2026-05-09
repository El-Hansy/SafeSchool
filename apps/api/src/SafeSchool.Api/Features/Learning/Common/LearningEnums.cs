namespace SafeSchool.Api.Features.Learning.Common;

public enum CourseStatus { Draft, Active, Suspended, Archived }
public enum LearningGroupStatus { Draft, Active, Suspended, Archived }
public enum MembershipStatus { Active, Suspended, Removed, Expired }
public enum StaffAssignmentRole { Teacher, TeachingAssistant, Coordinator, Reviewer, RewardManager, BehaviorReviewer, Auditor }
public enum StaffAssignmentStatus { Active, Suspended, Removed, Expired }
public enum ContentStatus { Draft, Published, Withdrawn, Expired, Archived }
public enum CompletionPolicy { ViewOnly, CompletionRequired, ManualReview }
public enum ProgressStatus { Started, InProgress, Completed, Corrected, Reviewed }
public enum AssignmentStatus { Draft, Active, Closed, Suspended, Archived }
public enum LatePolicy { AllowLate, BlockLate, RouteLateToReview, ExcuseAllowed }
public enum ReviewPolicy { Grade, FeedbackOnly, Completion, ManualReview }
public enum SubmissionStatus { Draft, Submitted, Resubmitted, Returned, Graded, Excused, Late, Withdrawn, NeedsReview }
public enum QuizStatus { Draft, Active, Suspended, Retired, Archived }
public enum QuestionType { MultipleChoice, ShortAnswer, TrueFalse, ManualReview }
public enum AttemptStatus { Started, Submitted, Expired, Scored, NeedsReview, Corrected, Voided }
public enum ScoreStatus { Pending, Scored, NeedsReview, Corrected, Voided }
public enum ScoringPolicy { AutoScore, ManualReview, Hybrid }
public enum FeedbackVisibility { Immediate, AfterClose, TeacherReleased, Hidden }
public enum StarLedgerDirection { Credit, Debit, Reserve, Consume, Release, Correct, Expire }
public enum StarLedgerState { Posted, Reserved, Consumed, Released, Corrected, Expired, NeedsReview }
public enum RewardStatus { Draft, Active, Suspended, Retired, Expired }
public enum RedemptionStatus { Requested, Approved, Fulfilled, Cancelled, Denied, Released, NeedsReview }
public enum FulfillmentStatus { Pending, Ready, Fulfilled, Cancelled, Denied }
public enum BehaviorClassification { Positive, Corrective, Neutral }
public enum BehaviorSeverity { Low, Medium, High, Critical }
public enum BehaviorReviewState { Accepted, NeedsReview, Disputed, Corrected, Dismissed }
public enum LearningExceptionType { InvalidEnrollment, InactiveStudent, MissingEvidence, LateSubmission, DuplicateSubmission, DuplicateQuizAttempt, ScoringConflict, StaleRule, MissingSourceEvidence, DuplicateStarAward, InsufficientStars, UnavailableReward, InvalidBehaviorRecord, DisabledFeature, CrossSchoolAccess, ManualReviewRequired }
public enum LearningExceptionSeverity { Low, Medium, High, Critical }
public enum LearningExceptionStatus { Open, Assigned, Resolved, Dismissed, Escalated, Reopened }
public enum ReviewAction { Correct, Reopen, Close, Resolve, Dismiss, Escalate, MigrateRuleVersion, Withdraw, Deny, Cancel, ChangeSensitiveVisibility }
public enum RuleArea { Content, Assignment, Quiz, Star, Reward, Behavior, Visibility, Review }
public enum RuleStatus { Draft, Active, Suspended, Retired, Superseded }
public enum SummaryScope { Student, Course, Group, Assignment, Quiz, Star, Reward, Behavior, Reviewer }
public enum SummaryStatus { Current, NeedsReview, Closed, RetentionReduced }
public enum LearningStatusEventType { ContentPublished, ProgressChanged, AssignmentSubmitted, AssignmentReviewed, QuizCompleted, QuizScored, StarChanged, RewardChanged, BehaviorRecorded, ExceptionOpened, ReviewCompleted, ConfigurationChanged, SummaryRefreshed, TraceRead }
public enum LearningFeatureStatus { Proposed, Enabled, Disabled, Suspended, Retired }
public enum GuardianLearningLinkStatus { Approved, Pending, Suspended, Expired, Removed, Rejected, RestrictedVisibility, StaffOnlyVisibility, OutOfScope }
public enum LearningStudentProfileStatus { Active, Inactive, Suspended, Graduated, Transferred, Duplicated, Missing, CrossTenant }
public enum LearningIdempotencyOutcome { Accepted, ExactDuplicate, ConflictingDuplicate }
