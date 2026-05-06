# Data Model: Phase 0 Platform Foundations

This model defines shared business entities for later specs. It does not create
runtime schemas by itself.

## School Account

**Purpose**: Defines the tenant boundary for one school or school organization.

**Fields**:
- `school_account_id`: Stable identifier for the school account.
- `display_name`: Human-readable school or organization name.
- `operational_scope`: Description of campuses, divisions, or internal units
  covered by the account.
- `status`: Draft, Active, Suspended, Archived.
- `created_at`: When the account boundary was established.
- `updated_at`: When the boundary definition last changed.
- `review_owner`: Actor category responsible for approving boundary changes.

**Relationships**:
- Has many Feature Capabilities.
- Has many Configuration Changes.
- Owns tenant-scoped Identity Evidence, Scan Events, and Audit Events.

**Validation rules**:
- Every tenant-owned entity MUST reference exactly one School Account.
- Cross-school access MUST require an explicit approved Permission Rule.
- Internal campus scoping MUST NOT override the parent School Account boundary.

**Validation examples**:
- A campus gate scan belongs to the parent School Account even when the later
  spec adds a campus or gate identifier.
- A platform support review across schools requires a Platform Owner-approved
  Permission Rule and Audit Event before data from another School Account is
  visible.

## Feature Capability

**Purpose**: Represents a named capability that can be enabled, disabled, or
reviewed for a School Account.

**Fields**:
- `capability_key`: Stable capability name used by later specs.
- `school_account_id`: Owning School Account.
- `availability_status`: Proposed, Enabled, Disabled, Suspended, Retired.
- `requested_by`: Actor category or reviewer requesting availability.
- `approved_by`: Actor category or reviewer approving availability.
- `reason`: Business reason for the current status.
- `effective_at`: When the status starts.
- `review_due_at`: When the capability must be reviewed again.
- `updated_at`: When the capability record last changed.

**Relationships**:
- Belongs to one School Account.
- Is changed through Configuration Changes.
- Produces Audit Events when availability changes.

**Validation rules**:
- A capability cannot be marked Enabled without approval evidence.
- Disabled or Suspended capabilities MUST be visible to later specs as
  unavailable for the affected School Account.
- Later specs MUST document whether they introduce, read, or depend on a Feature
  Capability.

**Validation examples**:
- If `attendance.gate-scan` is Disabled for a School Account, the later gate
  scan spec must surface unavailable capability behavior instead of processing
  the workflow.
- If `transport.boarding-scan` is Suspended, offline scan evidence may still be
  preserved for review, but the later transport spec must define whether a
  boarding outcome is deferred or rejected.

**State transitions**:
- Proposed -> Enabled when approved.
- Proposed -> Disabled when rejected or deferred.
- Enabled -> Suspended when temporarily unavailable.
- Suspended -> Enabled when restored.
- Enabled or Suspended -> Retired when permanently removed.

## Actor Category

**Purpose**: Provides shared role vocabulary for access and review decisions.

**Fields**:
- `actor_category_key`: Stable category name.
- `description`: Scope of responsibility.
- `default_boundary`: School Account, platform-wide, self-only, or delegated.
- `sensitive_action_notes`: Rules later specs must consider.

**Relationships**:
- Referenced by Permission Rules.
- Referenced by Configuration Changes and Audit Events.

**Validation rules**:
- Actor categories do not grant access by themselves; a Permission Rule must
  define allowed actions.
- Later specs MUST use existing actor categories unless an amendment adds a new
  category.

**Allowed boundary values**:
- Platform-wide
- School Account
- Internal campus scope
- Assignment-scoped
- Delegated
- Self-only
- Assigned review scope

**Sensitive-action examples**:
- Platform Owner approves a cross-school support review.
- School Administrator requests a tenant capability change.
- Staff Member performs a school-scoped sensitive workflow under assignment.
- Guardian opens a delegated student-visible record.
- Student opens a self-scope record where enabled.
- Reviewer resolves a duplicate scan conflict.

## Permission Rule

**Purpose**: Describes who may perform or approve a sensitive action.

**Fields**:
- `permission_key`: Stable rule identifier.
- `actor_category_key`: Actor category covered by the rule.
- `school_account_id`: Tenant boundary when the rule is tenant-specific.
- `allowed_action`: Business action covered by the rule.
- `approval_required`: Whether a separate reviewer must approve the action.
- `review_evidence`: Evidence required for audit review.
- `effective_status`: Draft, Active, Deprecated.

**Relationships**:
- References Actor Category.
- May reference School Account.
- Produces Audit Events when used for sensitive actions.

**Validation rules**:
- Sensitive actions MUST have an active Permission Rule before implementation.
- Permission Rules MUST identify whether access is school-scoped,
  platform-scoped, self-scoped, or delegated.

**Allowed boundary values**:
- Platform-scoped
- School-scoped
- Campus-scoped inside a School Account
- Assignment-scoped
- Delegated relationship
- Self-scoped
- Assigned review scope

**Sensitive-action examples**:
- Enabling a Feature Capability.
- Approving temporary cross-school access.
- Reviewing a rejected guardian access attempt.
- Reconciling an offline Scan Event conflict.
- Amending an accepted Foundation Decision.

## Identity Evidence

**Purpose**: Captures the common evidence needed to connect a person,
credential, scan method, or fallback method to a School Account.

**Fields**:
- `identity_evidence_id`: Stable evidence identifier.
- `school_account_id`: Owning School Account.
- `subject_type`: Student, Guardian, Staff, Device, Card, QR Credential, or
  Reviewer.
- `subject_reference`: Business reference to the subject.
- `evidence_type`: NFC, QR, Manual Review, Administrative Record, or Imported
  Record.
- `evidence_status`: Proposed, Verified, Rejected, Revoked.
- `verified_by`: Actor category or reviewer that verified the evidence.
- `verified_at`: Verification time.

**Relationships**:
- Belongs to School Account.
- May be referenced by Scan Events.
- Produces Audit Events when verified, rejected, or revoked.

**Validation rules**:
- Identity Evidence MUST identify the School Account boundary.
- NFC and QR fallback evidence MUST meet equivalent review expectations.
- Card provisioning and guardian linking workflows are excluded from Phase 0.

**State transition examples**:
- Proposed -> Verified when a reviewer accepts evidence for later use.
- Proposed -> Rejected when the evidence cannot be trusted.
- Verified -> Revoked when the credential, relationship, or source becomes
  invalid for later specs.

## Scan Event

**Purpose**: Represents a captured NFC or QR identity interaction and its review
status.

**Fields**:
- `scan_event_id`: Stable event identifier.
- `school_account_id`: School Account associated with the event.
- `capture_method`: NFC or QR.
- `capture_source`: Device, station, or actor category that captured the event.
- `captured_at`: Time claimed by the capture source.
- `received_at`: Time the event became available for review.
- `offline_indicator`: Whether the event was captured while offline.
- `sync_status`: Pending, Synced, Duplicate, Conflict, Rejected.
- `duplicate_reference`: Related Scan Event when duplicate detection applies.
- `review_outcome`: Accepted, Rejected, Needs Review, Deferred.

**Relationships**:
- Belongs to School Account.
- May reference Identity Evidence.
- Produces Audit Events during capture, sync, duplicate detection, conflict
  review, and final review.

**Validation rules**:
- Offline events MUST include enough timing and source evidence for later
  reconciliation.
- QR fallback events MUST carry the same accountability expectations as NFC
  events.
- Attendance, access, transport, and wallet business outcomes are excluded from
  Phase 0.

**State transitions**:
- Pending -> Synced when accepted for review.
- Pending or Synced -> Duplicate when matched to an existing event.
- Pending or Synced -> Conflict when evidence conflicts with existing records.
- Conflict -> Accepted, Rejected, or Deferred after review.

**State transition examples**:
- Offline NFC capture starts as Pending, becomes Synced when received, and then
  becomes Accepted after later review.
- QR fallback starts as Pending, becomes Conflict if the credential no longer
  matches the expected School Account, and then becomes Rejected with audit
  evidence.

## Audit Event

**Purpose**: Provides reviewable evidence for important identity, access,
configuration, scan, and administrative activity.

**Fields**:
- `audit_event_id`: Stable event identifier.
- `school_account_id`: School Account affected, when tenant-scoped.
- `event_category`: Identity, Access, Tenant Configuration, Feature
  Availability, Scan Capture, Scan Reconciliation, Administrative Review.
- `actor_category_key`: Actor category associated with the event.
- `subject_reference`: Entity or decision affected by the event.
- `event_time`: Time the event occurred.
- `reason`: Business reason or review note.
- `review_status`: Recorded, Reviewed, Escalated, Corrected.

**Relationships**:
- May reference any foundation entity.
- Supports Foundation Decisions and later spec validation.

**Validation rules**:
- Sensitive access, configuration changes, scan reconciliation, and
  administrative reviews MUST produce Audit Events.
- Audit Events MUST preserve enough context for reviewers to understand who did
  what, for which School Account, and why.

**Review evidence examples**:
- A duplicate Scan Event creates an Audit Event in the Scan Reconciliation
  category with the duplicate reference and reviewer reason.
- A denied cross-school access attempt creates an Access Audit Event with actor
  category, boundary, result, and reason.

## Configuration Change

**Purpose**: Records changes to School Account settings or Feature Capability
availability.

**Fields**:
- `configuration_change_id`: Stable change identifier.
- `school_account_id`: Affected School Account.
- `change_type`: Tenant Boundary, Feature Availability, Permission Rule,
  Identity Evidence Rule, Audit Rule, or Other Foundation Decision.
- `requested_by`: Actor category requesting the change.
- `approved_by`: Actor category approving the change.
- `change_reason`: Business reason for the change.
- `previous_value_summary`: Summary of prior state.
- `new_value_summary`: Summary of new state.
- `effective_at`: When the change takes effect.
- `review_status`: Proposed, Approved, Rejected, Applied, Rolled Back.

**Relationships**:
- Belongs to School Account.
- May update Feature Capability, Permission Rule, or Foundation Decision.
- Produces Audit Events.

**Validation rules**:
- Tenant boundary and feature availability changes MUST include requester,
  approver, reason, and effective time.
- Rejected or rolled back changes MUST retain review evidence.

**Review evidence examples**:
- An approved capability change records previous value, new value, approver,
  effective time, and Audit Event reference.
- A rolled-back change records the rollback reason and preserves the original
  applied change for review.

## Foundation Decision

**Purpose**: Captures a rule or boundary that later phase specs must reference
or amend.

**Fields**:
- `decision_id`: Stable decision identifier.
- `foundation_area`: System Architecture, Multi-Tenant Architecture, Identity &
  Access Model, NFC & QR Integration, Event & Audit Logging, or Feature Flag /
  Tenant Configuration.
- `decision_statement`: The rule or boundary.
- `rationale`: Why the decision exists.
- `affected_entities`: Foundation entities affected by the decision.
- `review_owner`: Actor category responsible for review.
- `status`: Proposed, Accepted, Superseded, Deprecated.
- `amended_by`: Later decision that supersedes it, when applicable.

**Relationships**:
- References one or more foundation areas.
- May be amended by Configuration Changes.
- Must be referenced by later specs when relevant.

**Validation rules**:
- Every accepted Foundation Decision MUST have a rationale and review owner.
- Later specs MUST reference applicable Foundation Decisions or document an
  amendment before planning.
