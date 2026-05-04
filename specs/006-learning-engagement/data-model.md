# Data Model: Phase 5 Learning & Engagement

## Course

**Purpose**: A tenant-owned learning container for curriculum, content,
assignments, quizzes, learning groups, progress records, and review history.

**Fields**:
- `course_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `course_code`: School-scoped unique code.
- `course_name`: Display name.
- `description`: Optional course overview.
- `course_status`: Draft, Active, Suspended, Archived.
- `visibility_policy`: Student and guardian visibility setting.
- `effective_from`, `effective_to`: Active date range.
- `created_by_actor_id`, `updated_by_actor_id`: Staff actor references.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Has many Learning Groups, Learning Content Items, Assignments, Quizzes,
  Learning Rule Settings, Learning Exceptions, and Learning Review Summaries.

**Validation Rules**:
- Course code is unique within a school account.
- Active courses require at least one authorized owner or coordinator.
- Suspended or archived courses cannot receive new content, assignments, or
  quiz activations unless reopened by an authorized reviewer.

## Learning Group

**Purpose**: A tenant-owned student and staff grouping used to target learning
content and activities.

**Fields**:
- `learning_group_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `course_id`: Optional course association.
- `group_name`: Display name.
- `group_status`: Draft, Active, Suspended, Archived.
- `visibility_policy`: Guardian and student visibility setting.
- `effective_from`, `effective_to`: Membership date range.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Has many Learning Group Memberships and Staff Learning Assignments.
- Targets Content Items, Assignments, Quizzes, Star Rules, Reward eligibility,
  and Behavior visibility where configured.

**Validation Rules**:
- Active groups require at least one active student or an explicit empty-group
  review reason.
- Group membership cannot include students outside the school account.
- Staff assignment cannot grant visibility outside the school account.

## Learning Group Membership

**Purpose**: A student's membership in a learning group.

**Fields**:
- `learning_group_membership_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `learning_group_id`: Group reference.
- `student_profile_id`: Student reference from Phase 1.
- `membership_status`: Active, Suspended, Removed, Expired.
- `effective_from`, `effective_to`: Membership window.
- `review_reason`: Required when manually suspended or removed.
- `created_at`, `updated_at`: Audit timestamps.

**Validation Rules**:
- Active membership requires an active student profile.
- Overlapping active membership for the same student and group is rejected or
  treated as already processed.
- Removing a student does not delete historical submissions, attempts, stars,
  rewards, behavior events, or progress evidence.

## Staff Learning Assignment

**Purpose**: Staff authority over a course or learning group.

**Fields**:
- `staff_learning_assignment_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `actor_id`: Staff actor reference.
- `course_id`, `learning_group_id`: Scope reference.
- `assignment_role`: Teacher, Teaching Assistant, Coordinator, Reviewer.
- `assignment_status`: Active, Suspended, Removed, Expired.
- `effective_from`, `effective_to`: Assignment window.
- `created_at`, `updated_at`: Audit timestamps.

**Validation Rules**:
- Staff actions require active assignment or explicit school administrator /
  platform reviewer authority.
- Removed staff retain traceability on prior actions but cannot create new
  learning records in that scope.

## Learning Content Item

**Purpose**: A lesson, activity, or resource reference assigned to learners.

**Fields**:
- `learning_content_item_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `course_id`, `learning_group_id`: Optional targeting scope.
- `title`, `description`: Required display fields.
- `resource_reference`: Resource pointer or activity reference; not a broad
  document storage record.
- `content_status`: Draft, Published, Withdrawn, Expired, Archived.
- `release_at`, `expires_at`: Visibility window.
- `completion_policy`: View Only, Completion Required, Manual Review.
- `student_visibility_policy`, `guardian_visibility_policy`: Allowed details.
- `content_revision`: Revision reference.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Has many Learning Progress Events and Learning Exceptions.
- May be a source for Star Ledger Entries through active Star Rule Settings.

**Validation Rules**:
- Published content requires title, target learners, release window, resource
  reference, and active course or group scope.
- Withdrawn content remains visible in history only to authorized users.
- Content cannot be shown to unassigned students or unrelated guardians.

## Learning Progress Event

**Purpose**: Evidence that a student viewed, started, resumed, completed, or
had progress corrected for learning work.

**Fields**:
- `learning_progress_event_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `student_profile_id`: Student reference.
- `course_id`, `learning_group_id`: Learning scope.
- `source_type`: Content, Assignment, Quiz, Behavior, Manual Correction.
- `source_id`: Source record reference.
- `progress_status`: Started, In Progress, Completed, Corrected, Reviewed.
- `progress_percent`: Optional progress value.
- `source_event_reference`: Idempotency/source reference.
- `visibility_policy`: Guardian/student visibility.
- `occurred_at`: Business event time.
- `created_at`: Persistence time.

**Validation Rules**:
- Progress events require assigned student scope or reviewer correction
  authority.
- Exact duplicate progress event references are ignored or returned as already
  processed.
- Progress events must not create attendance, transport, wallet, or request
  outcomes.

## Assignment

**Purpose**: A teacher-created learning task assigned to students or groups.

**Fields**:
- `assignment_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `course_id`, `learning_group_id`: Learning scope.
- `title`, `instructions`: Required content.
- `required_evidence`: Required submission evidence description.
- `assigned_to_scope`: Student, Group, Course.
- `due_at`: Due date.
- `opens_at`, `closes_at`: Submission window.
- `late_policy`: Allow Late, Block Late, Route Late To Review, Excuse Allowed.
- `review_policy`: Grade, Feedback Only, Completion, Manual Review.
- `assignment_status`: Draft, Active, Closed, Suspended, Archived.
- `configuration_revision`: Rule revision active at assignment creation.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Has many Assignment Submissions, Learning Progress Events, Star Ledger
  Entries, Learning Exceptions, and Manual Learning Reviews.

**Validation Rules**:
- Active assignments require target learners, instructions, due date, and
  active assignment capability.
- Due date changes create reviewable evidence and do not hide prior
  submissions.
- Closed assignments reject new submissions unless late or review policy allows
  them.

## Assignment Submission

**Purpose**: A student's submitted work evidence and review state.

**Fields**:
- `assignment_submission_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `assignment_id`: Assignment reference.
- `student_profile_id`: Student reference.
- `attempt_number`: Submission sequence.
- `submission_status`: Draft, Submitted, Resubmitted, Returned, Graded,
  Excused, Late, Withdrawn, Needs Review.
- `submitted_evidence_reference`: Student work reference.
- `submitted_at`: Submission time.
- `grade_value`: Optional grade or rubric outcome.
- `feedback_summary`: Teacher-visible and student/guardian allowed feedback.
- `reviewer_actor_id`: Staff reviewer.
- `client_request_id`: Idempotency key.
- `created_at`, `updated_at`: Audit timestamps.

**State Transitions**:
- Draft -> Submitted.
- Submitted -> Resubmitted when assignment rules allow another attempt.
- Submitted or Resubmitted -> Returned, Graded, Excused, Late, or Needs Review.
- Submitted, Resubmitted, or Returned -> Withdrawn when allowed.
- Any active state -> Corrected through Manual Learning Review.

**Validation Rules**:
- Student must be eligible for the assignment at submission time.
- Duplicate `client_request_id` returns the existing submission result.
- Prior submissions and feedback are append-only evidence and must not be
  overwritten by resubmission.

## Quiz

**Purpose**: A controlled assessment with question set, schedule, attempt, and
scoring rules.

**Fields**:
- `quiz_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `course_id`, `learning_group_id`: Learning scope.
- `quiz_title`, `description`: Display fields.
- `quiz_status`: Draft, Active, Suspended, Retired, Archived.
- `opens_at`, `closes_at`: Attempt window.
- `attempt_limit`: Maximum attempts per eligible student.
- `time_limit_minutes`: Optional timer.
- `scoring_policy`: Auto Score, Manual Review, Hybrid.
- `feedback_visibility`: Immediate, After Close, Teacher Released, Hidden.
- `question_set_revision`: Version of active questions and answers.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Has many Quiz Questions, Quiz Attempts, Learning Progress Events, Star Ledger
  Entries, and Learning Exceptions.

**Validation Rules**:
- Active quizzes require an active question set and target learners.
- Suspended or retired quizzes reject new attempts.
- Question or answer key corrections create a new revision and preserve old
  attempt evidence.

## Quiz Question

**Purpose**: A question inside a quiz revision.

**Fields**:
- `quiz_question_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `quiz_id`: Quiz reference.
- `question_set_revision`: Revision reference.
- `question_type`: Multiple Choice, Short Answer, True/False, Manual Review.
- `prompt_reference`: Question text or resource reference.
- `answer_key_reference`: Expected answer or scoring reference.
- `points_possible`: Score value.
- `display_order`: Stable ordering.
- `required`: Whether answer is required.

**Validation Rules**:
- Active question revisions cannot be mutated in place after attempts exist.
- Manual review questions require reviewer scoring before final scored state.

## Quiz Attempt

**Purpose**: A student's attempt at a quiz.

**Fields**:
- `quiz_attempt_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `quiz_id`: Quiz reference.
- `student_profile_id`: Student reference.
- `attempt_number`: Sequence for the student and quiz.
- `attempt_status`: Started, Submitted, Expired, Scored, Needs Review,
  Corrected, Voided.
- `started_at`, `submitted_at`, `expires_at`: Attempt timing.
- `score_value`: Optional score.
- `score_status`: Pending, Auto Scored, Manually Reviewed, Corrected.
- `feedback_released_at`: Feedback visibility time.
- `client_request_id`: Idempotency key.
- `created_at`, `updated_at`: Audit timestamps.

**State Transitions**:
- Started -> Submitted.
- Started -> Expired when timer or close window is exceeded.
- Submitted -> Scored or Needs Review.
- Scored or Needs Review -> Corrected through review.
- Any non-final attempt -> Voided by authorized reviewer with reason.

**Validation Rules**:
- Attempt requires active quiz, active eligible student, available attempt
  count, and valid time window.
- Duplicate attempt completion is idempotent.
- Expired and over-limit attempts must not corrupt prior scored attempts.

## Quiz Response

**Purpose**: A student's answer evidence for a quiz attempt.

**Fields**:
- `quiz_response_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `quiz_attempt_id`: Attempt reference.
- `quiz_question_id`: Question reference.
- `response_reference`: Answer value or evidence reference.
- `response_status`: Answered, Missing, Flagged, Reviewed.
- `points_awarded`: Optional score.
- `reviewer_note`: Optional staff note.
- `created_at`, `updated_at`: Audit timestamps.

**Validation Rules**:
- Required questions must have an answer or produce a missing answer outcome
  according to quiz rules.
- Responses are preserved with the question revision used for the attempt.

## Star Rule Setting

**Purpose**: School-account rules for awarding, reversing, reserving,
consuming, releasing, capping, expiring, or reviewing stars.

**Fields**:
- `star_rule_setting_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `rule_name`: Display name.
- `source_type`: Content, Assignment, Quiz, Behavior, Manual Award.
- `eligible_student_scope`: Student, Group, Course, School.
- `star_amount`: Positive or negative star impact allowed by rule.
- `award_cap`: Optional per-student or per-period cap.
- `valid_from`, `valid_to`: Active range.
- `review_behavior`: Auto Award, Route To Review, Block.
- `rule_status`: Draft, Active, Suspended, Retired.
- `rule_version`: Version used by ledger entries.
- `created_at`, `updated_at`: Audit timestamps.

**Validation Rules**:
- Active rules require enabled star capability and valid source type.
- Rule changes create a new version and do not alter historical ledger entries.
- A source event can trigger only one active matching automatic award unless a
  reviewer explicitly records a correction.

## Star Ledger Entry

**Purpose**: Append-only evidence for student star credits, debits,
reservations, consumption, releases, corrections, or expiry.

**Fields**:
- `star_ledger_entry_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `student_profile_id`: Student reference.
- `source_type`: Assignment, Quiz, Content, Behavior, Reward, Manual Review,
  Phase 6 Reservation, Expiry.
- `source_id`: Source record reference.
- `star_rule_setting_id`, `rule_version`: Rule reference when applicable.
- `ledger_direction`: Credit, Debit, Reserve, Consume, Release, Correct, Expire.
- `star_amount`: Integer star quantity.
- `balance_state`: Available, Reserved, Consumed, Released, Corrected, Pending.
- `reason`: Required business reason.
- `client_request_id`: Idempotency key.
- `created_by_actor_id`: Actor or system source.
- `created_at`: Ledger time.

**Relationships**:
- May reference Reward Redemptions, Behavior Events, Assignment Submissions,
  Quiz Attempts, Learning Progress Events, Manual Learning Reviews, or Phase 6
  star evidence consumers.

**Validation Rules**:
- Ledger entries are append-only and cannot be edited or deleted directly.
- Star debits, reservations, and consumption cannot exceed available stars
  unless routed to review by configured rule.
- Duplicate source and client request identities return the existing ledger
  result.

## Star Balance Snapshot

**Purpose**: A reviewable read model derived from star ledger entries.

**Fields**:
- `star_balance_snapshot_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `student_profile_id`: Student reference.
- `available_stars`: Spendable star total.
- `reserved_stars`: Stars reserved for pending rewards or later permission
  workflows.
- `consumed_stars`: Lifetime consumed total.
- `pending_review_stars`: Stars awaiting reviewer outcome.
- `snapshot_source_time`: Last ledger event included.
- `created_at`, `updated_at`: Snapshot timestamps.

**Validation Rules**:
- Snapshot must be reconstructable from append-only ledger evidence.
- Snapshot reads never become the authority when ledger entries disagree.

## Reward Catalog Item

**Purpose**: A tenant-owned reward definition that students can redeem with
stars when eligible.

**Fields**:
- `reward_catalog_item_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `reward_name`, `description`: Display details.
- `eligible_student_scope`: Student, Group, Course, School.
- `star_cost`: Required star cost.
- `available_from`, `available_to`: Availability window.
- `inventory_limit`: Optional fulfillment limit.
- `redemption_limit`: Optional per-student or total limit.
- `reward_status`: Draft, Active, Suspended, Retired, Expired.
- `fulfillment_policy`: Staff Fulfilled, Auto Fulfilled, Review Required.
- `created_at`, `updated_at`: Audit timestamps.

**Validation Rules**:
- Active rewards require positive star cost, eligibility scope, availability
  window, and enabled star/reward capability.
- Suspended, retired, or expired rewards cannot accept new redemptions.

## Reward Redemption

**Purpose**: A student's request or staff-created record to spend stars on a
reward.

**Fields**:
- `reward_redemption_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `reward_catalog_item_id`: Reward reference.
- `student_profile_id`: Student reference.
- `redemption_status`: Requested, Approved, Fulfilled, Cancelled, Denied,
  Released, Needs Review.
- `star_cost_snapshot`: Cost at redemption.
- `star_ledger_entry_id`: Reservation or consumption entry reference.
- `fulfillment_status`: Pending, Fulfilled, Failed, Cancelled.
- `requested_by_actor_id`: Student or staff actor.
- `review_reason`: Required for denial, cancellation, or review.
- `client_request_id`: Idempotency key.
- `created_at`, `updated_at`: Audit timestamps.

**State Transitions**:
- Requested -> Approved or Needs Review.
- Approved -> Fulfilled, Cancelled, or Released.
- Requested or Needs Review -> Denied.
- Any active state -> Corrected through Manual Learning Review.

**Validation Rules**:
- Student eligibility and available stars are checked before reservation or
  consumption.
- Reward redemption does not create wallet or payment activity.
- Duplicate redemption requests are idempotent.

## Behavior Category

**Purpose**: School-account configuration for behavior and engagement event
classification.

**Fields**:
- `behavior_category_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `category_name`: Display name.
- `behavior_classification`: Positive, Corrective, Neutral.
- `default_severity`: Low, Medium, High, Critical.
- `default_visibility_policy`: Student/guardian visibility.
- `star_rule_setting_id`: Optional star effect rule.
- `category_status`: Draft, Active, Suspended, Retired.
- `created_at`, `updated_at`: Audit timestamps.

**Validation Rules**:
- Active categories require enabled behavior logging.
- Sensitive categories default to restricted guardian/student visibility unless
  school rules explicitly permit disclosure.

## Behavior Event

**Purpose**: A positive, corrective, or neutral engagement record for a student.

**Fields**:
- `behavior_event_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `student_profile_id`: Student reference.
- `behavior_category_id`: Category reference.
- `classification`: Positive, Corrective, Neutral.
- `severity`: Low, Medium, High, Critical.
- `source_context`: Course, Group, Assignment, Quiz, Manual.
- `staff_note`: Staff-entered detail.
- `visibility_policy`: Student/guardian visibility.
- `review_state`: Accepted, Needs Review, Disputed, Corrected, Dismissed.
- `related_star_ledger_entry_id`: Optional star impact.
- `client_request_id`: Idempotency key.
- `created_by_actor_id`: Staff actor reference.
- `occurred_at`, `created_at`, `updated_at`: Timestamps.

**Validation Rules**:
- Actor must have active staff authority for the student, group, course, or
  reviewer role.
- Duplicate behavior events are idempotent by source and client request.
- Star effects use active Star Rule Settings and preserve traceability.

## Learning Exception

**Purpose**: A reviewable issue in learning, quiz, star, reward, or behavior
workflows.

**Fields**:
- `learning_exception_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `student_profile_id`: Optional affected student.
- `source_type`: Course, Content, Assignment, Quiz, Star, Reward, Behavior,
  Access, Configuration.
- `source_id`: Related record reference.
- `exception_type`: Invalid Enrollment, Inactive Student, Missing Evidence,
  Late Submission, Duplicate Submission, Duplicate Quiz Attempt, Scoring
  Conflict, Stale Rule, Missing Source Evidence, Duplicate Star Award,
  Insufficient Stars, Unavailable Reward, Invalid Behavior Record, Disabled
  Feature, Cross-School Access, Manual Review Required.
- `severity`: Low, Medium, High, Critical.
- `exception_status`: Open, Assigned, Resolved, Dismissed, Escalated, Reopened.
- `source_evidence_reference`: Trace reference.
- `reviewer_assignment`: Optional reviewer queue or actor.
- `resolution_reason`: Required when resolved or dismissed.
- `created_at`, `updated_at`: Audit timestamps.

**Validation Rules**:
- Exceptions are tenant-scoped and permission-scoped.
- Resolving or dismissing requires reviewer authority and reason.
- Original source records remain preserved.

## Manual Learning Review

**Purpose**: A reviewer action that corrects, reopens, resolves, escalates, or
documents a learning exception.

**Fields**:
- `manual_learning_review_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `learning_exception_id`: Optional exception reference.
- `source_type`, `source_id`: Record under review.
- `review_action`: Correct, Reopen, Close, Resolve, Dismiss, Escalate, Migrate
  Rule Version.
- `review_reason`: Required reason.
- `original_status`: Status before review.
- `resulting_status`: Status after review.
- `reviewer_actor_id`: Reviewer reference.
- `created_at`: Review time.

**Validation Rules**:
- Reviewer must have explicit review permission.
- Review actions preserve original source evidence and append correction
  evidence.

## Learning Rule Setting

**Purpose**: Versioned or revision-traceable school-account configuration for
learning and engagement workflows.

**Fields**:
- `learning_rule_setting_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `rule_area`: Content, Assignment, Quiz, Stars, Rewards, Behavior, Visibility,
  Review.
- `rule_version`: Version reference.
- `rule_status`: Draft, Active, Suspended, Retired.
- `rule_payload`: Structured rule configuration.
- `valid_from`, `valid_to`: Effective window.
- `change_reason`: Required on activation or suspension.
- `created_at`, `updated_at`: Audit timestamps.

**Validation Rules**:
- Activation requires enabled learning configuration capability and valid
  dependent capabilities.
- Historical records retain the rule version active when the event occurred.

## Learning Review Summary

**Purpose**: Permission-scoped summary of learning progress, assignment status,
quiz outcomes, star balances, rewards, behavior, exceptions, and reviews.

**Fields**:
- `learning_review_summary_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `summary_scope`: Student, Course, Group, Assignment, Quiz, Star, Reward,
  Behavior, Reviewer.
- `student_profile_id`, `course_id`, `learning_group_id`: Optional filters.
- `assignment_counts`, `quiz_counts`, `star_counts`, `reward_counts`,
  `behavior_counts`, `exception_counts`: Summary values.
- `latest_evidence_at`: Most recent event in scope.
- `visibility_scope`: Staff, Student, Guardian, Reviewer, Platform Reviewer.
- `created_at`, `updated_at`: Summary timestamps.

**Validation Rules**:
- Summary reads enforce the same visibility boundaries as underlying records.
- Summary values never expose cross-school records or staff-only details to
  students or guardians.

## Learning Status Event

**Purpose**: Exportable evidence for later communication/notification
capabilities and Phase 6 star evidence consumers.

**Fields**:
- `learning_status_event_id`: Stable identifier.
- `school_account_id`: Tenant owner.
- `student_profile_id`: Optional student reference.
- `source_type`, `source_id`: Related learning source.
- `event_type`: Content Published, Progress Changed, Assignment Submitted,
  Assignment Reviewed, Quiz Completed, Quiz Scored, Star Changed, Reward
  Changed, Behavior Recorded, Exception Opened, Review Completed.
- `event_payload_reference`: Minimal trace payload.
- `eligible_for_notification`: Whether Phase 9 may consume it.
- `eligible_for_phase6_star_evidence`: Whether Phase 6 may consume it.
- `occurred_at`, `created_at`: Event timestamps.

**Validation Rules**:
- Events are append-only and tenant-scoped.
- Events do not deliver messages and do not approve requests directly.
