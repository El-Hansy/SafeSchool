# Data Model: Phase 6 Requests & Permissions

This model defines runtime business entities for permission requests, outing
details, early leave details, request type configuration, workflow templates,
workflow decisions, guardian consent, star-based permission rules, pickup
evidence, exceptions, manual reviews, summaries, and audit evidence. All
tenant-owned entities include `tenant_id`, `created_at`, and `updated_at`.

## Permission Request

**Purpose**: A tenant-owned request for outing, early leave, or another
school-defined permission type.

**Fields**:
- `permission_request_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Student from Phase 1.
- `request_type_id`: Request type active at submission.
- `request_type_version`: Version snapshot used by this request.
- `requester_actor_id`: Student, guardian, or staff actor that initiated the request.
- `requester_role`: Student, Guardian, Staff.
- `guardian_link_id`: Guardian relationship when guardian-initiated or consented.
- `reason`: Request reason supplied by requester.
- `requested_start_at`: Requested start or departure time.
- `requested_end_at`: Requested end, return, or release window end.
- `request_status`: Draft, Submitted, Pending Consent, Pending Approval,
  Information Requested, Escalated, Manual Review, Approved, Denied, Withdrawn,
  Expired, Closed, Reopened.
- `workflow_template_version_id`: Workflow version governing the request.
- `current_workflow_step_id`: Active step when pending.
- `current_assignee_scope`: Role, actor, or reviewer queue currently assigned.
- `duplicate_review_state`: None, Exact Duplicate Blocked, Overlap Needs Review.
- `star_rule_evaluation_id`: Star evaluation when applicable.
- `exception_state`: None, Needs Review, Resolved, Dismissed.
- `submitted_at`: Submission time.
- `finalized_at`: Final approval or denial time.
- `withdrawn_at`: Withdrawal time when applicable.
- `closed_at`: Closure time when applicable.
- `created_by`: Actor that created the request.
- `updated_by`: Actor that last changed the request.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account and one Student Profile.
- References one Request Type and one Workflow Template Version.
- Has zero or one Outing Request Detail or Early Leave Detail.
- Has many Workflow Decisions, Guardian Consent Records, Request Exceptions,
  Manual Request Reviews, Request Review Summary rows, and Audit Events.
- May reference one Star Rule Evaluation.

**Validation rules**:
- Create, submit, withdraw, decide, correct, and view operations require tenant
  access, feature capability, request type eligibility, student eligibility,
  role permission, and guardian link validation where applicable.
- Exact active duplicates for the same student, request type, requested date,
  and requested time window are blocked.
- Overlapping non-identical active requests for the same student and request
  type route to manual review.
- Request records cannot cross school account boundaries.
- Approved decision history cannot be edited or deleted directly.

**State transitions**:
- Draft -> Submitted when required fields pass validation.
- Submitted -> Pending Consent when guardian consent is required.
- Submitted or Pending Consent -> Pending Approval when workflow approval
  steps are active.
- Pending Approval -> Information Requested when an approver requests more
  information.
- Pending Approval -> Escalated or Manual Review when a step expires or an
  escalation rule triggers.
- Pending Consent, Pending Approval, Information Requested, Escalated, or
  Manual Review -> Approved when all required decisions pass.
- Pending Consent, Pending Approval, Information Requested, Escalated, or
  Manual Review -> Denied when a required denial decision is final.
- Draft, Submitted, Pending Consent, Pending Approval, Information Requested,
  Escalated, or Manual Review -> Withdrawn when the original requester is
  allowed to withdraw.
- Approved -> Closed when closure or return expectations are complete.
- Approved -> Expired when approval validity ends without required closure.
- Approved, Denied, Withdrawn, Expired, or Closed -> Reopened only through
  authorized manual review with reason.

## Outing Request Detail

**Purpose**: Request-specific details for leaving campus or participating in an
outing.

**Fields**:
- `outing_request_detail_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `permission_request_id`: Parent request.
- `destination_or_purpose`: Destination or outing purpose.
- `departure_at`: Requested departure time.
- `expected_return_at`: Requested return time.
- `supervision_expectation`: Supervision note when required by request type.
- `transport_expectation`: Transport expectation when required by request type.
- `guardian_consent_required`: Whether consent is required for this request.
- `closure_required`: Whether return or closure status must be recorded.
- `closure_status`: Not Required, Pending, Returned, Closed By Review,
  Overdue, Needs Review.
- `closure_note`: Optional reviewer or staff note.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Permission Request.
- May be referenced by Request Exceptions, Manual Request Reviews, and Audit
  Events.

**Validation rules**:
- Destination or purpose, departure, and expected return are required when the
  request type marks them required.
- Outing detail must use the same tenant as the parent request.
- Closure status changes require explicit permission and audit evidence.

## Early Leave Detail

**Purpose**: Request-specific details for releasing a student before the normal
end time.

**Fields**:
- `early_leave_detail_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `permission_request_id`: Parent request.
- `release_at`: Requested early leave release time.
- `release_reason`: Early leave reason.
- `guardian_consent_required`: Whether guardian consent is required.
- `pickup_evidence_id`: Pickup evidence required before release eligibility.
- `approval_expires_at`: Expiry time for the approved release evidence.
- `release_eligibility_status`: Not Eligible, Pending Verification,
  Release Eligible, Expired, Blocked, Needs Review.
- `release_eligibility_note`: Staff or reviewer note.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Permission Request.
- References one Pickup Evidence record when pickup verification is required.
- May be referenced by Request Exceptions, Manual Request Reviews, and Audit
  Events.

**Validation rules**:
- Release eligibility requires approved request status, unexpired approval,
  guardian-selected authorized pickup person, and staff verification note.
- Release eligibility is read-only evidence and does not create attendance,
  entry, exit, scan, or transport outcomes.
- Missing or invalid pickup evidence routes to review or blocks release
  eligibility.

**State transitions**:
- Not Eligible -> Pending Verification after final approval when pickup
  verification is required.
- Pending Verification -> Release Eligible when staff verification note is
  recorded for the authorized pickup person.
- Release Eligible -> Expired when approval expiry passes.
- Any non-final state -> Blocked or Needs Review when guardian, pickup, student,
  tenant, or feature validation fails.

## Request Type

**Purpose**: School account configuration for a request category and its
required fields, initiation rules, workflow, consent, star rules, and expiry
behavior.

**Fields**:
- `request_type_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `request_type_code`: School-unique code.
- `request_type_name`: Display name.
- `request_category`: Outing, Early Leave, Permission, Star Permission.
- `allowed_initiator_roles`: Student, Guardian, Staff role list.
- `required_field_schema`: Versioned required field definition.
- `guardian_consent_policy`: One Authorized Guardian, Primary Guardian,
  All Decision-Capable Guardians, Manual Review, or custom school policy.
- `workflow_template_id`: Active workflow template.
- `star_permission_rule_id`: Optional star rule.
- `expiry_policy`: Request and approval expiry settings.
- `duplicate_policy`: Exact duplicate block and overlap review policy.
- `closure_policy`: Whether closure or return status is required.
- `request_type_status`: Draft, Active, Suspended, Retired.
- `version`: Active version number.
- `created_by`: Actor that created the type.
- `updated_by`: Actor that last changed the type.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Has many Permission Requests.
- References active Workflow Template and optional Star Permission Rule.
- Produces Audit Events when created, activated, suspended, retired, or
  versioned.

**Validation rules**:
- Active request types require at least one allowed initiator role, valid
  required field definitions, a valid workflow path, valid consent policy, and
  enabled dependent feature capabilities.
- Request type codes are unique within a school account while active.
- Historical requests preserve the request type version active at submission.

## Workflow Template

**Purpose**: A versioned school account approval definition for request
routing.

**Fields**:
- `workflow_template_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `template_name`: Human-readable name.
- `template_status`: Draft, Active, Suspended, Retired.
- `active_version_id`: Current active version.
- `created_by`: Actor that created the template.
- `updated_by`: Actor that last changed the template.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Has many Workflow Template Versions.
- Is referenced by Request Types and Permission Requests.

**Validation rules**:
- Activation requires a valid template version with no circular steps, at least
  one valid approval or consent path, possible escalation timing, and enabled
  dependent features.
- Historical requests preserve their workflow version unless migrated through
  authorized review.

## Workflow Step

**Purpose**: A specific approval, guardian consent, information request,
escalation, delegation, or review point inside a workflow version.

**Fields**:
- `workflow_step_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `workflow_template_version_id`: Version that owns the step.
- `step_sequence`: Ordered position when applicable.
- `step_type`: Approval, Guardian Consent, Information Request, Escalation,
  Delegation, Manual Review.
- `assigned_role`: Role expected to act.
- `assigned_actor_id`: Specific actor when assigned directly.
- `required_permission`: Permission required to act.
- `decision_options`: Allowed decisions for the step.
- `reason_required`: Whether action requires a reason.
- `expiry_duration`: Optional configured expiry interval.
- `escalation_target`: Role, actor, or review queue used on expiry.
- `next_step_rules`: Conditional routing rules.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Workflow Template Version.
- Has many Workflow Decisions.
- May create Request Exceptions when invalid, expired, or conflicted.

**Validation rules**:
- A step must have a valid actor, role, or review queue assignment.
- Expired steps route pending requests to manual review or configured
  escalation and must not auto-approve or auto-deny by default.
- Circular step routing is rejected before activation.

## Workflow Decision

**Purpose**: An approval, denial, information request, consent decision,
escalation, delegation, or review outcome recorded by an authorized actor.

**Fields**:
- `workflow_decision_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `permission_request_id`: Request being decided.
- `workflow_step_id`: Active step being decided.
- `actor_id`: Actor recording the decision.
- `decision_type`: Approve, Deny, Request Information, Delegate, Escalate,
  Mark Manual Review, Consent Approve, Consent Deny.
- `decision_status`: Accepted, Rejected, Already Processed, Needs Review.
- `decision_reason`: Required reason when configured.
- `next_request_status`: Resulting request status.
- `next_step_id`: Next active step when applicable.
- `idempotency_key`: Caller-stable decision identity.
- `decided_at`: Decision time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Permission Request and Workflow Step.
- May create Guardian Consent Records, Request Exceptions, Manual Request
  Reviews, and Audit Events.

**Validation rules**:
- Decisions require tenant access, feature availability, assigned active step,
  actor permission, non-final request state, and idempotent request identity.
- Duplicate, out-of-order, cross-school, final-state, withdrawn, expired, or
  concurrently conflicting decisions are rejected, returned as already
  processed, or routed to review without corrupting the request outcome.
- Accepted decisions are append-only.

## Guardian Consent Record

**Purpose**: Evidence that an eligible guardian approved, denied, or was
required to decide a student request.

**Fields**:
- `guardian_consent_record_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `permission_request_id`: Request requiring consent.
- `student_profile_id`: Student.
- `guardian_link_id`: Approved active guardian relationship.
- `guardian_actor_id`: Guardian actor.
- `consent_policy`: One Authorized Guardian, Primary Guardian, All
  Decision-Capable Guardians, Manual Review, or school-specific policy.
- `consent_decision`: Approved, Denied, Not Required, Pending, Needs Review.
- `decision_reason`: Optional guardian or reviewer reason.
- `decided_at`: Guardian decision time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Permission Request.
- References one Guardian Link from Phase 1.
- May be tied to a Workflow Decision.

**Validation rules**:
- Default guardian consent is satisfied by any one authorized guardian.
- Stricter request type rules override the default.
- Guardian action requires approved active guardian link and tenant scope.

## Star Permission Rule

**Purpose**: A versioned school account rule for star-gated request eligibility.

**Fields**:
- `star_permission_rule_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `rule_name`: Human-readable name.
- `request_type_id`: Request type using the rule.
- `required_star_threshold`: Minimum eligible stars when applicable.
- `star_cost`: Stars reserved and consumed when applicable.
- `eligible_student_group`: Optional school-defined student group.
- `manual_review_behavior`: Route To Review, Deny, Block Submission.
- `unavailable_evidence_behavior`: Route To Review, Deny, Block Submission.
- `rule_status`: Draft, Active, Suspended, Retired.
- `version`: Rule version.
- `valid_from`: Start of active validity.
- `valid_to`: End of active validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- References one Request Type.
- Has many Star Rule Evaluations.

**Validation rules**:
- Activation requires enabled `requests.star_rules` and available Phase 5 star
  evidence capability for request types that need automated evaluation.
- Historical requests preserve the star rule version used at submission.
- Phase 6 does not store or mutate star balances directly.

## Star Rule Evaluation

**Purpose**: Captured outcome of applying a star permission rule to a request.

**Fields**:
- `star_rule_evaluation_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `permission_request_id`: Request being evaluated.
- `star_permission_rule_id`: Rule used.
- `rule_version`: Rule version snapshot.
- `phase5_evidence_reference`: External star evidence reference.
- `available_star_count`: Count reported by Phase 5 when available.
- `required_star_threshold`: Threshold snapshot.
- `star_cost`: Cost snapshot.
- `evaluation_status`: Not Required, Eligible, Insufficient, Evidence
  Unavailable, Reserved, Consumed, Released, Failed, Needs Review.
- `reservation_reference`: Phase 5 reservation outcome reference.
- `consumption_reference`: Phase 5 consumption outcome reference.
- `release_reference`: Phase 5 release outcome reference.
- `review_reason`: Reason for review or failure.
- `evaluated_at`: Evaluation time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Permission Request and Star Permission Rule.
- May create Request Exceptions, Manual Request Reviews, and Audit Events.

**Validation rules**:
- Requests with a star cost reserve stars at submission, consume reserved stars
  only on final approval, and release reservations on denial, withdrawal, or
  expiry.
- Missing or stale Phase 5 evidence follows the request type's configured
  unavailable-evidence behavior and never invents a star balance.

## Pickup Evidence

**Purpose**: Guardian-selected pickup person and staff verification evidence
needed before an approved early leave request is treated as release-eligible.

**Fields**:
- `pickup_evidence_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `permission_request_id`: Related early leave request.
- `selected_by_guardian_link_id`: Guardian that selected the pickup person.
- `pickup_person_name`: Authorized pickup person name.
- `pickup_person_relationship`: Relationship to student.
- `authorization_status`: Authorized, Blocked, Expired, Needs Review.
- `staff_verification_note`: Staff verification note.
- `verified_by_actor_id`: Staff actor that recorded verification.
- `verified_at`: Verification time.
- `verification_status`: Pending, Verified, Rejected, Expired, Needs Review.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Permission Request and Early Leave Detail.
- References guardian link and verifying staff actor.
- May produce Request Exceptions and Audit Events.

**Validation rules**:
- Release eligibility requires authorized pickup person and staff verification
  note.
- Phase 6 does not require government ID image capture or NFC/QR release scan.

## Request Exception

**Purpose**: A reviewable issue involving invalid, conflicting, expired,
duplicate, unauthorized, stale, or manual-review-required request evidence.

**Fields**:
- `request_exception_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `permission_request_id`: Affected request.
- `student_profile_id`: Affected student when applicable.
- `exception_type`: Missing Consent, Invalid Guardian Link, Expired Approval,
  Exact Duplicate, Overlap Needs Review, Conflicting Decision,
  Out-Of-Order Decision, Stale Workflow Version, Missing Star Evidence,
  Insufficient Stars, Disabled Feature, Invalid Pickup Evidence,
  Cross-School Access Attempt, Manual Review Required.
- `severity`: Info, Warning, Critical.
- `source_evidence_reference`: Source evidence for the exception.
- `exception_status`: Open, Assigned, Resolved, Dismissed, Reopened.
- `reviewer_assignment`: Role, actor, or queue.
- `resolution_reason`: Required on resolution or dismissal.
- `detected_at`: Detection time.
- `resolved_at`: Resolution time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Permission Request.
- May reference Workflow Decision, Guardian Consent Record, Star Rule
  Evaluation, Pickup Evidence, Manual Request Review, and Audit Events.

**Validation rules**:
- Exceptions cannot be deleted to hide evidence.
- Resolution requires reviewer permission, reason, and audit evidence.

## Manual Request Review

**Purpose**: Reviewer action that corrects, reopens, resolves, escalates,
closes, or documents a request exception.

**Fields**:
- `manual_request_review_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `permission_request_id`: Reviewed request.
- `request_exception_id`: Related exception when applicable.
- `reviewer_actor_id`: Reviewer.
- `review_action`: Correct, Reopen, Close, Resolve Exception, Dismiss
  Exception, Escalate, Migrate Workflow Version.
- `review_reason`: Required reason.
- `previous_request_status`: Status before review.
- `resulting_request_status`: Status after review.
- `previous_workflow_version_id`: Prior workflow version when migrated.
- `resulting_workflow_version_id`: New workflow version when migrated.
- `reviewed_at`: Review time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Permission Request.
- May resolve one or more Request Exceptions.
- Produces Audit Events.

**Validation rules**:
- Manual reviews require explicit reviewer permission and tenant access.
- Original request and decision history remain preserved.

## Request Review Summary

**Purpose**: Permission-scoped view of request counts, pending assignments,
final outcomes, exception states, star-rule outcomes, and review status.

**Fields**:
- `request_review_summary_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `summary_scope`: Student, Guardian, Request Type, Assignee, Workflow,
  Exception, Star Rule.
- `scope_reference`: Identifier for the summary scope.
- `pending_count`: Number pending.
- `approved_count`: Number approved.
- `denied_count`: Number denied.
- `withdrawn_count`: Number withdrawn.
- `exception_open_count`: Open exception count.
- `manual_review_count`: Manual review count.
- `star_rule_needs_review_count`: Star-rule review count.
- `latest_evidence_at`: Most recent evidence time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Derived from Permission Requests, Workflow Decisions, Request Exceptions,
  Star Rule Evaluations, Manual Request Reviews, and Audit Events.

**Validation rules**:
- Summaries are tenant-scoped and permission-scoped.
- Guardian summaries include only linked-student records allowed by guardian
  visibility rules.

## School Account Feature Setting

**Purpose**: Tenant capability setting that determines whether Phase 6 workflows
are available.

**Fields**:
- `school_account_feature_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `capability_key`: Requests capability key.
- `enabled`: Whether the capability is enabled.
- `effective_from`: Start time.
- `effective_to`: End time.
- `changed_by`: Actor that changed the setting.
- `change_reason`: Reason for change.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Governs Request Types, Permission Requests, Workflow Templates, Star Rules,
  History, Configuration, and Summaries.

**Validation rules**:
- Backend operations must check feature availability before mutating or
  exposing Phase 6 workflow data.
- UI gates are usability controls only and do not replace backend checks.
