# Data Model: Phase 8 Complaints & Escalations

This model defines runtime business entities for complaint submission,
categorization, assignments, investigation, escalation, resolution, feedback,
history, reviews, summaries, status events, and audit evidence. All
tenant-owned entities include `tenant_id`, `created_at`, and `updated_at`.

## Complaint

**Purpose**: A tenant-owned issue report submitted by a guardian, student, or
staff member.

**Fields**:
- `complaint_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `tracking_reference`: Human-friendly complaint reference.
- `complainant_actor_id`: Authenticated actor that submitted the complaint.
- `complainant_role`: Guardian, Student, Staff, Administrator, Other.
- `student_profile_id`: Primary affected student when applicable; additional
  students are represented through Complaint Participant records.
- `guardian_link_id`: Approved guardian relationship when applicable.
- `staff_submitter_id`: Staff submitter when applicable.
- `category_id`: Active category rule used for current classification.
- `category_version`: Category rule version applied at submission or latest
  reclassification.
- `subcategory`: Optional school-defined subcategory.
- `priority`: Low, Normal, High, Urgent.
- `confidentiality_level`: Standard, Restricted, Safeguarding, Safety,
  Medical Privacy, Staff-Only.
- `description`: Complaint statement.
- `event_window_start`: Start of complaint event window when known.
- `event_window_end`: End of complaint event window when known.
- `requested_outcome`: Requested outcome or remedy when provided.
- `complaint_status`: Submitted, Received, Triage, Assigned, In Review,
  Waiting For Information, Escalated, Resolved, Closed, Withdrawn, Reopened,
  Dismissed, Manual Review Required.
- `current_owner_type`: Queue, Actor, Role, Reviewer, None.
- `current_owner_reference`: Current owner reference.
- `target_response_at`: Target response time.
- `target_resolution_at`: Target resolution time.
- `escalation_state`: None, Eligible, Escalated, Resolved, Manual Review.
- `feedback_state`: Not Eligible, Eligible, Submitted, Reopen Requested,
  Disputed, Closed.
- `duplicate_group_reference`: Reference for exact duplicate or related
  overlapping complaint group.
- `exception_state`: None, Needs Review, Under Review, Resolved.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- May reference one primary Student Profile, Guardian Link, or Staff
  submitter.
- Has many Complaint Participants, Complaint Assignments, Investigation
  Entries, Evidence References, Escalation Events, Resolution Records,
  Complaint Feedback records, Complaint Exceptions, Manual Complaint Reviews,
  Status Events, and Audit Events.

**Validation rules**:
- Submission, read, update, assignment, escalation, resolution, feedback, and
  review operations require tenant access, enabled capability, actor
  permission, role authority, and audit evidence.
- Student complaints require enabled student submission for the selected
  category.
- Guardian complaints for student-related issues require an approved active
  guardian link unless school rules permit a restricted personal-submitter view.
- Complaints involving multiple students, guardians, staff members, or school
  units must create Complaint Participant records for every involved party and
  evaluate visibility independently per participant.
- Exact active duplicates return the existing complaint reference or are
  blocked; overlapping non-identical complaints are linked, grouped, or routed
  to manual review.
- Complaint actions must not create outcomes in attendance, campus access,
  scans, transport, wallet, learning rewards, request approvals, medical,
  emergency, document storage, global search, or broad messaging.

## Complaint Category

**Purpose**: A school-account rule set for complaint intake, triage,
assignment, escalation, feedback, and reopen behavior.

**Fields**:
- `complaint_category_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `category_code`: School-unique category code.
- `category_name`: Display name.
- `category_description`: Administrative description.
- `allowed_submitter_roles`: Guardian, Student, Staff, Administrator.
- `required_fields`: Required intake fields for this category.
- `default_priority`: Low, Normal, High, Urgent.
- `default_confidentiality_level`: Standard, Restricted, Safeguarding, Safety,
  Medical Privacy, Staff-Only.
- `owner_group_reference`: Default queue, role, or owner group.
- `target_response_duration`: Expected response window.
- `target_resolution_duration`: Expected resolution window.
- `restricted_handling_required`: Whether special privacy handling applies.
- `safety_review_required`: Whether safety or safeguarding review applies.
- `feedback_rule_reference`: Feedback rule used after resolution.
- `reopen_rule_reference`: Reopen rule used after resolution.
- `category_status`: Draft, Active, Suspended, Archived.
- `version`: Rule version.
- `effective_from`: Start of rule validity.
- `effective_to`: End of rule validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Has many Escalation Rules and Complaints.

**Validation rules**:
- Activation requires a valid owner group, valid required fields, valid timing,
  allowed submitter roles, confidentiality settings, escalation route where
  required, and enabled dependent capabilities.
- Historical complaints preserve the category version used at each relevant
  action.

## Complaint Participant

**Purpose**: A complainant, affected student, guardian, staff member, school
unit, involved party, complaint subject, investigator, resolver, reviewer, or
auditor associated with a complaint.

**Fields**:
- `complaint_participant_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Parent complaint.
- `actor_id`: Platform actor when applicable, including staff users.
- `student_profile_id`: Student participant when applicable.
- `guardian_link_id`: Guardian participant when applicable.
- `school_unit_reference`: School unit participant when applicable.
- `participant_role`: Complainant, Affected Student, Guardian, Staff
  Submitter, Subject, Involved Party, Investigator, Resolver, Reviewer,
  Auditor, School Unit.
- `visibility_scope`: Hidden, Status Only, Visible Details, Internal Details,
  Restricted Review.
- `conflict_state`: None, Potential Conflict, Confirmed Conflict, Waived By
  Reviewer.
- `participant_status`: Active, Removed, Restricted, Superseded.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Complaint subjects and conflicted participants cannot assign, decide, close,
  or view restricted complainant details unless a reviewer records a limited
  exception.
- Participant visibility cannot exceed tenant, guardian-link, student
  ownership, assignment, or review authority.
- Multi-participant complaints must preserve separate visibility scopes for
  each student, guardian, staff member, school unit, subject, investigator,
  resolver, reviewer, and auditor.

## Complaint Assignment

**Purpose**: Current and historical ownership for complaint handling.

**Fields**:
- `complaint_assignment_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Parent complaint.
- `assigned_to_type`: Queue, Role, Actor, Reviewer.
- `assigned_to_reference`: Assigned owner reference.
- `assigned_by_actor_id`: Actor or system rule that assigned the complaint.
- `assignment_reason`: Reason for assignment or reassignment.
- `prior_assignment_id`: Previous assignment.
- `assignment_status`: Active, Reassigned, Completed, Cancelled, Conflicted.
- `assigned_at`: Assignment time.
- `completed_at`: Completion time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Assignment requires enabled complaint assignment capability, valid owner
  group, actor authority, and conflict-of-interest checks.
- Reassignment preserves prior owner, reason, actor, time, and audit evidence.

## Complaint Investigation Entry

**Purpose**: A preserved complaint handling entry such as an internal note,
visible response, information request, received response, finding, corrective
action, or status change.

**Fields**:
- `complaint_investigation_entry_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Parent complaint.
- `entry_type`: Internal Note, Visible Response, Information Request,
  Complainant Response, Finding, Corrective Action, Status Change.
- `entry_summary`: Entry summary or response text.
- `visibility_level`: Internal, Complainant Visible, Reviewer Only,
  Restricted.
- `actor_id`: Actor that created the entry.
- `related_assignment_id`: Assignment context when applicable.
- `status_before`: Complaint status before entry.
- `status_after`: Complaint status after entry.
- `reason_required`: Whether a reason was required.
- `entry_reason`: Reason when required.
- `entry_time`: Entry time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Internal and restricted entries are hidden from complainants, students,
  guardians, and ordinary staff unless explicit visibility rules allow them.
- Closed evidence cannot be edited in place; corrections require review
  entries.

## Complaint Evidence Reference

**Purpose**: A permitted reference to prior school evidence without copying or
owning the source record.

**Fields**:
- `complaint_evidence_reference_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Parent complaint.
- `source_module`: Identity, Attendance, Transport, Wallet, Learning,
  Requests, Medical, Other.
- `source_record_reference`: Source-domain record identifier.
- `evidence_summary`: Minimum permitted summary.
- `visibility_level`: Internal, Complainant Visible, Reviewer Only,
  Restricted.
- `linked_by_actor_id`: Actor that linked the evidence.
- `linked_at`: Link time.
- `reference_status`: Active, Removed, Restricted, Superseded.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Evidence references require permission to view the source record and must not
  copy restricted source content into complaint records.
- Linking evidence must not modify the source domain record.

## Escalation Rule

**Purpose**: A versioned school-account rule describing escalation triggers and
routing.

**Fields**:
- `escalation_rule_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_category_id`: Category governed by the rule.
- `rule_name`: Display name.
- `trigger_type`: Priority, Safety, Safeguarding, Severe Misconduct,
  Externally Reportable, Target Expired, Disputed, Reopened, Unresolved,
  Conflict Of Interest, Manual.
- `trigger_condition`: Configured condition details.
- `target_owner_type`: Queue, Role, Actor, Reviewer.
- `target_owner_reference`: Escalation owner reference.
- `priority_after_escalation`: Priority after rule applies.
- `target_response_duration`: Escalated response window.
- `visibility_constraints`: Additional visibility restrictions.
- `rule_status`: Draft, Active, Suspended, Archived.
- `version`: Rule version.
- `effective_from`: Start of rule validity.
- `effective_to`: End of rule validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Activation requires a valid target owner, non-circular route, valid timing,
  and enabled escalation workflow capability.
- Historical escalation events preserve the rule version used.

## Escalation Event

**Purpose**: A recorded escalation of a complaint.

**Fields**:
- `escalation_event_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Parent complaint.
- `escalation_rule_id`: Rule that triggered the event when applicable.
- `trigger_source`: Rule, Manual, Target Expiry, Review.
- `escalation_reason`: Required escalation reason.
- `prior_owner_reference`: Previous owner.
- `new_owner_reference`: Escalation owner or reviewer.
- `priority_before`: Priority before escalation.
- `priority_after`: Priority after escalation.
- `target_response_at`: Escalated target response time.
- `event_status`: Pending, Active, Resolved, Cancelled, Manual Review.
- `triggered_by_actor_id`: Actor or system rule that triggered escalation.
- `triggered_at`: Event time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Escalation requires enabled escalation workflow capability and a valid route
  or manual review fallback.
- Target expiry cannot silently close, dismiss, downgrade, or hide complaints.

## Resolution Record

**Purpose**: The outcome of complaint handling.

**Fields**:
- `resolution_record_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Parent complaint.
- `resolution_type`: Resolved, Dismissed, Withdrawn, Corrected, Referred,
  No Action, Other.
- `outcome_summary`: Complainant-visible outcome summary when allowed.
- `internal_resolution_notes`: Internal outcome details.
- `corrective_action_summary`: Corrective action summary where applicable.
- `closure_reason`: Required closure reason.
- `responsible_actor_id`: Actor closing or proposing the outcome.
- `complainant_visibility`: Hidden, Summary, Full Visible Outcome.
- `feedback_eligible`: Whether feedback is allowed.
- `reopen_eligible`: Whether reopen request is allowed.
- `resolved_at`: Resolution time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Resolution requires active assignment or reviewer authority and must preserve
  original complaint and investigation history.
- Complainant-visible summaries cannot include restricted internal details.

## Complaint Feedback

**Purpose**: A complainant response to a resolution.

**Fields**:
- `complaint_feedback_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Parent complaint.
- `resolution_record_id`: Resolution being answered.
- `feedback_type`: Accepted, Dissatisfied, Rating, Comment, Dispute, Reopen
  Request.
- `feedback_rating`: Optional satisfaction score.
- `feedback_comment`: Optional comment.
- `submitted_by_actor_id`: Complainant actor.
- `feedback_status`: Submitted, Routed, Under Review, Accepted, Rejected,
  Closed.
- `reopen_requested`: Whether feedback asks to reopen the complaint.
- `reopen_reason`: Reason for reopen when applicable.
- `submitted_at`: Submission time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Feedback requires resolved complaint state, complainant eligibility, active
  feedback rules, and tenant scope.
- Reopen requests route to configured owner or reviewer while preserving the
  original resolution.

## Complaint Exception

**Purpose**: A reviewable issue involving complaint intake, assignment,
escalation, resolution, or visibility.

**Fields**:
- `complaint_exception_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Affected complaint when applicable.
- `student_profile_id`: Primary affected student when applicable.
- `exception_type`: Invalid Student, Invalid Guardian Link, Missing Required
  Field, Disabled Feature, Disabled Category, Missing Owner Group, Missing
  Escalation Route, Conflicted Owner, Stale Configuration, Duplicate Complaint,
  Cross-School Access, Restricted Evidence Reference, Missed Target Timing,
  Manual Review Required.
- `severity`: Low, Medium, High, Critical.
- `source_evidence`: Evidence that created the exception.
- `exception_status`: Open, Under Review, Resolved, Dismissed, Escalated.
- `reviewer_assignment`: Reviewer or queue when assigned.
- `resolution_reason`: Reason after resolution.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Exceptions preserve source evidence and cannot be deleted to hide an issue.
- Critical exceptions route to configured urgent review where available.

## Manual Complaint Review

**Purpose**: A reviewer action that corrects, reopens, dismisses, escalates,
resolves, or documents complaint evidence.

**Fields**:
- `manual_complaint_review_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Parent complaint.
- `complaint_exception_id`: Related exception when applicable.
- `review_action`: Correct, Reopen, Dismiss, Escalate, Resolve, Close,
  Migrate Rule Version, Document.
- `review_reason`: Required reviewer reason.
- `reviewer_actor_id`: Authorized reviewer.
- `status_before`: Status before review.
- `status_after`: Status after review.
- `visibility_change`: Visibility change when applicable.
- `reviewed_at`: Review time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Manual review requires reviewer authority and preserved original evidence.
- Rule version migration requires a reason and must preserve prior governing
  configuration.

## Complaint Review Summary

**Purpose**: A permission-scoped view of complaint counts, aging, ownership,
escalation, feedback, exceptions, and resolution outcomes.

**Fields**:
- `complaint_review_summary_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `summary_scope`: Student, Guardian, Category, Owner, Reviewer, School.
- `filter_context`: Applied filter context.
- `complaint_count`: Count in scope.
- `open_count`: Open complaint count.
- `overdue_count`: Overdue complaint count.
- `escalated_count`: Escalated complaint count.
- `feedback_pending_count`: Feedback-eligible count.
- `exception_count`: Exception count.
- `generated_for_actor_id`: Actor that requested the summary.
- `generated_at`: Generation time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Summaries apply the same tenant, guardian-link, student ownership,
  complainant ownership, assignment, and review filters as source records.
- Summary reads are audit-visible.

## Complaint Status Event

**Purpose**: Tenant-scoped evidence that complaint state changed and may be
consumed later by communication or notification capabilities.

**Fields**:
- `complaint_status_event_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `complaint_id`: Parent complaint.
- `event_type`: Submitted, Categorized, Assigned, Information Requested,
  Responded, Escalated, Target Missed, Resolved, Closed, Feedback Submitted,
  Reopen Requested, Exception Created, Review Completed.
- `event_payload_reference`: Structured status event payload reference.
- `eligible_for_notification`: Whether later communication capabilities may
  consume the event.
- `event_status`: Pending, Available, Consumed, Failed, Suppressed.
- `occurred_at`: Event time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Status events do not send messages directly.
- Sensitive complaint details are minimized according to recipient visibility
  boundaries before later communication capabilities consume them.

## School Account Feature Setting

**Purpose**: Tenant capability setting that determines whether Phase 8
complaint and escalation workflows are available.

**Fields**:
- `school_account_feature_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `feature_key`: complaint submission, categorization, assignment, escalation,
  feedback and resolution, history, configuration, or summaries.
- `feature_state`: Enabled, Disabled, Suspended.
- `effective_from`: Start of feature availability.
- `effective_to`: End of feature availability.
- `updated_by_actor_id`: Actor that changed the setting.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Backend feature checks are required before every Phase 8 workflow.
- Disabling a capability prevents new related actions and routes active
  affected complaints to configured review behavior.

## State Transitions

### Complaint Lifecycle

```text
Submitted
  -> Received
  -> Triage
  -> Assigned
  -> In Review
  -> Waiting For Information
  -> In Review
  -> Escalated
  -> In Review
  -> Resolved
  -> Closed
```

Alternate transitions:

```text
Submitted -> Withdrawn
Triage -> Manual Review Required
Assigned -> Manual Review Required
Resolved -> Reopened -> In Review
Resolved -> Dismissed -> Closed
Any non-final state -> Escalated
Any non-final state -> Manual Review Required
```

### Feedback Lifecycle

```text
Eligible -> Submitted -> Routed -> Under Review -> Accepted -> Closed
Eligible -> Submitted -> Routed -> Under Review -> Reopen Requested -> Reopened
Eligible -> Submitted -> Routed -> Under Review -> Rejected -> Closed
```

### Exception Lifecycle

```text
Open -> Under Review -> Resolved
Open -> Under Review -> Dismissed
Open -> Escalated -> Under Review -> Resolved
```
