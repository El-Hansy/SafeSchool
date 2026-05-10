# Data Model: Phase 1 Identity & Access

This model defines the runtime business entities for student identity,
guardian access, credentials, roles, permissions, access decisions, and audit
evidence. All tenant-owned entities include `tenant_id`, `created_at`, and
`updated_at`.

## Student Profile

**Purpose**: The trusted student identity record for one school account.

**Fields**:
- `student_profile_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `school_student_number`: School-scoped student identifier.
- `external_identity_references`: Optional configured identifiers approved by
  the school account.
- `legal_name`: Official student name.
- `preferred_name`: Optional display name.
- `date_of_birth`: Date used for identity review when allowed by school policy.
- `grade_level`: Current grade or year group.
- `campus_or_division`: Optional internal assignment within the school account.
- `enrollment_status`: Applied, Enrolled, Transferred, Withdrawn, Graduated.
- `profile_status`: Draft, Active, Suspended, Deactivated, Archived.
- `duplicate_review_status`: Clear, Possible Duplicate, Confirmed Duplicate,
  Resolved.
- `created_by`: Actor that created the record.
- `updated_by`: Actor that last changed the record.
- `review_reason`: Business reason for the current reviewed state.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Has many Guardian Links.
- Has many Identity Credentials.
- Produces Audit Events for profile changes.

**Validation rules**:
- Active profiles must be unique by `tenant_id` plus configured active identity
  identifiers.
- A profile cannot become Active while duplicate review status is Possible
  Duplicate or Confirmed Duplicate.
- Deactivation must preserve profile history, credential history, and guardian
  link history.
- Cross-school visibility is denied unless an explicit platform-level review
  permission allows inspection.

**State transitions**:
- Draft -> Active when required identity details pass validation.
- Draft -> Archived when abandoned before activation.
- Active -> Suspended when temporarily unavailable for identity use.
- Active or Suspended -> Deactivated when no longer current.
- Deactivated -> Archived after retention and review conditions are met.

## Guardian Record

**Purpose**: A guardian identity record that may be linked to one or more
students in a school account.

**Fields**:
- `guardian_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `display_name`: Guardian name.
- `contact_methods`: Approved contact references.
- `identity_review_status`: Unverified, Verified, Rejected, Suspended.
- `guardian_status`: Active, Suspended, Deactivated, Archived.
- `created_by`: Actor that created the record.
- `updated_by`: Actor that last changed the record.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- Has many Guardian Links.
- Produces Audit Events for identity review and status changes.

**Validation rules**:
- A Guardian Record grants no student visibility without an approved active
  Guardian Link.
- Guardian records cannot be linked across school accounts.
- Suspended or deactivated guardians cannot receive new active links.

## Guardian Link

**Purpose**: Defines a guardian's relationship and access scope for one student.

**Fields**:
- `guardian_link_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Linked student.
- `guardian_id`: Linked guardian.
- `relationship_type`: Parent, Legal Guardian, Authorized Relative, Sponsor, or
  Other School-Approved Relationship.
- `access_scope`: Approved information categories and actions.
- `link_status`: Pending, Approved, Suspended, Expired, Rejected, Removed.
- `valid_from`: When access may begin.
- `valid_until`: Optional expiry time.
- `requested_by`: Actor requesting the link.
- `approved_by`: Actor approving or rejecting the link.
- `review_reason`: Business reason for current state.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Student Profile.
- Belongs to one Guardian Record.
- Produces Audit Events for lifecycle changes.
- Is evaluated by Access Decisions when a guardian attempts student access.

**Validation rules**:
- Only Approved links within their validity window can grant guardian access.
- Suspended, expired, rejected, or removed links deny guardian access.
- Access scope must be explicit; blank or unknown scope denies student data
  visibility.
- Link lifecycle changes require a reason and audit event.

**State transitions**:
- Pending -> Approved when reviewed and accepted.
- Pending -> Rejected when reviewed and denied.
- Approved -> Suspended during dispute or temporary hold.
- Suspended -> Approved when restored.
- Approved or Suspended -> Removed when no longer valid.
- Approved -> Expired when the validity window ends.

## Identity Credential

**Purpose**: Shared credential concept for NFC cards and QR fallback identity
evidence.

**Fields**:
- `identity_credential_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Assigned student.
- `credential_type`: NFC Card or QR Fallback.
- `credential_reference`: Non-secret reference used for lookup.
- `credential_status`: Proposed, Active, Suspended, Replaced, Expired,
  Revoked.
- `issued_by`: Actor that issued the credential.
- `issued_at`: Issue time.
- `valid_from`: When the credential becomes usable.
- `valid_until`: Optional expiry time.
- `replaced_by_credential_id`: Replacement credential when applicable.
- `status_reason`: Business reason for current status.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Student Profile.
- Has one subtype record when subtype-specific attributes are needed.
- Produces Audit Events for lifecycle changes.
- Is included in Credential Status Snapshots for later scan flows.

**Validation rules**:
- Active credentials must belong to an active student profile in the same school
  account.
- A credential cannot be Active outside its validity window.
- Suspended, replaced, expired, or revoked credentials are unavailable as
  current identity evidence.
- Credential lifecycle changes require permission checks, feature availability
  checks, and audit events.

**State transitions**:
- Proposed -> Active when issued.
- Active -> Suspended during temporary hold.
- Suspended -> Active when restored.
- Active or Suspended -> Replaced when a new credential supersedes it.
- Active or Suspended -> Revoked when permanently invalidated.
- Active or Suspended -> Expired when validity ends.

## NFC Card Credential

**Purpose**: A physical card identity credential tied to exactly one active
student identity at a time.

**Fields**:
- `nfc_card_credential_id`: Stable subtype identifier.
- `identity_credential_id`: Parent Identity Credential.
- `card_reference`: Non-secret card reference or fingerprint.
- `card_label`: Optional human-readable label.
- `provisioning_status`: Pending, Provisioned, Failed, Replaced, Revoked.
- `last_verified_at`: Last successful credential status verification.

**Relationships**:
- Extends Identity Credential.
- May be replaced by another NFC Card Credential.

**Validation rules**:
- One active NFC card reference can be assigned to only one active student
  profile within a school account at a time.
- Lost or replaced cards must be suspended or revoked before a replacement is
  activated.
- Raw card secrets must not be exposed in UI or audit details.

## QR Fallback Credential

**Purpose**: A time-bound fallback identity credential used only when QR
fallback is enabled for the school account.

**Fields**:
- `qr_fallback_credential_id`: Stable subtype identifier.
- `identity_credential_id`: Parent Identity Credential.
- `qr_reference`: Non-secret QR reference or fingerprint.
- `rotation_sequence`: Monotonic sequence for replacements.
- `rotation_reason`: Business reason for rotation.
- `last_presented_at`: Last known presentation time when available.

**Relationships**:
- Extends Identity Credential.
- May replace a previous QR Fallback Credential for the same student.

**Validation rules**:
- QR fallback credentials require the `identity.qr_fallback` capability to be
  enabled for the school account.
- QR fallback credentials must have a validity window.
- Rotation creates a new credential and marks the prior active QR credential as
  Replaced or Revoked.

## Role

**Purpose**: A named responsibility set for platform or school users.

**Fields**:
- `role_id`: Stable identifier.
- `tenant_id`: Owning school account when tenant-scoped; null only for
  platform-level roles.
- `role_key`: Stable role name.
- `display_name`: Human-readable role name.
- `role_scope`: Platform, School Account, Self, or Delegated.
- `role_status`: Draft, Active, Suspended, Deprecated.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Has many Role Permissions.
- Has many Actor Role Assignments.
- Produces Audit Events when changed.

**Validation rules**:
- Tenant-scoped roles cannot grant access outside their school account.
- Deprecated or suspended roles cannot be assigned to new actors.
- Role changes require role administration permission.

## Permission

**Purpose**: A specific action or read scope that can be granted through a role
or assignment.

**Fields**:
- `permission_id`: Stable identifier.
- `permission_key`: Stable action key.
- `description`: Business meaning.
- `permission_scope`: Platform, School Account, Student Profile, Guardian Link,
  Credential, Role, Permission, Audit, or Self.
- `sensitive_action`: Whether the action requires audit evidence.
- `permission_status`: Active, Deprecated, Retired.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to many Roles through Role Permissions.
- Is evaluated by Access Decisions.

**Validation rules**:
- Sensitive permissions must produce audit evidence when used for changes.
- Deprecated or retired permissions cannot be newly granted.

## Actor Role Assignment

**Purpose**: Connects an authenticated actor to one role in a school account or
platform review context.

**Fields**:
- `actor_role_assignment_id`: Stable identifier.
- `tenant_id`: School account for tenant-scoped assignment.
- `actor_reference`: User or service actor reference.
- `role_id`: Assigned role.
- `assignment_status`: Pending, Active, Suspended, Revoked, Expired.
- `valid_from`: When assignment starts.
- `valid_until`: Optional expiry time.
- `assigned_by`: Actor that granted the role.
- `review_reason`: Business reason.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Role.
- Produces Audit Events for assignment lifecycle changes.
- Is evaluated by Access Decisions.

**Validation rules**:
- Only Active assignments within their validity window can contribute
  permissions.
- Tenant-scoped assignments must match the active school account.
- Assignment changes require role administration permission and audit evidence.

## Access Decision

**Purpose**: Records allow or deny outcomes for sensitive Phase 1 actions.

**Fields**:
- `access_decision_id`: Stable identifier.
- `tenant_id`: School account affected.
- `actor_reference`: User or service actor that attempted the action.
- `attempted_action`: Permission key or action name.
- `target_type`: Student Profile, Guardian Link, Credential, Role, Permission,
  Audit Event, or Feature Capability.
- `target_reference`: Target entity reference.
- `decision`: Allowed or Denied.
- `decision_reason`: Explanation of the result.
- `role_sources`: Roles or assignments considered.
- `feature_capability_key`: Capability checked when applicable.
- `decided_at`: Decision time.

**Relationships**:
- May reference Student Profile, Guardian Link, Identity Credential, Role,
  Permission, or Feature Capability.
- Produces or is referenced by Audit Events for sensitive denials and changes.

**Validation rules**:
- Denied sensitive actions must be recorded with enough context for review.
- Access Decisions must include tenant and feature capability context when the
  action is tenant-scoped.
- A missing permission, missing tenant access, inactive role assignment, or
  disabled feature capability must result in denial.

## Audit Event

**Purpose**: Reviewable evidence for important Phase 1 changes and denials.

**Fields**:
- `audit_event_id`: Stable identifier.
- `tenant_id`: School account affected.
- `event_category`: Student Profile, Guardian Link, Credential, Role,
  Permission, Access Decision, Feature Capability, or Administrative Review.
- `event_type`: Specific action name.
- `actor_reference`: Actor responsible for the event.
- `subject_type`: Entity type affected.
- `subject_reference`: Entity affected.
- `previous_value_summary`: Summary of prior state when applicable.
- `new_value_summary`: Summary of new state when applicable.
- `reason`: Business reason or review note.
- `access_decision_id`: Related Access Decision when applicable.
- `event_time`: Event time.
- `review_status`: Recorded, Reviewed, Escalated, Corrected.

**Relationships**:
- May reference any Phase 1 entity.
- Supports quickstart and compliance review scenarios.

**Validation rules**:
- Sensitive profile, guardian, credential, role, permission, feature-gated, and
  access-denial events must produce Audit Events.
- Audit Events must preserve enough context to identify who acted, what changed,
  which school account was affected, and why.

## School Account Feature Setting

**Purpose**: Tenant capability setting inherited from Phase 0 and used to gate
Phase 1 workflows.

**Fields**:
- `feature_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `capability_key`: Phase 1 capability key.
- `availability_status`: Proposed, Enabled, Disabled, Suspended, Retired.
- `effective_at`: Start time for the current status.
- `approved_by`: Actor approving the status.
- `reason`: Business reason.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Gates Student Profile, Guardian Link, Credential, Role, and Permission
  workflows.
- Produces Audit Events when changed.

**Validation rules**:
- A disabled or suspended capability denies its related workflow even when the
  actor otherwise has permission.
- Capability checks must be enforced before business changes execute.

## Credential Status Snapshot

**Purpose**: Read-optimized evidence package used by later offline NFC/QR scan
flows to determine whether a credential is current identity evidence.

**Fields**:
- `credential_status_snapshot_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `identity_credential_id`: Credential represented.
- `student_profile_id`: Assigned student.
- `credential_type`: NFC Card or QR Fallback.
- `credential_status`: Current lifecycle status.
- `valid_from`: Credential validity start.
- `valid_until`: Credential validity end when applicable.
- `revoked_at`: Revocation time when applicable.
- `snapshot_generated_at`: Snapshot generation time.
- `snapshot_expires_at`: Time after which later offline flows must refresh when
  possible.

**Relationships**:
- Derived from Identity Credential and Student Profile.
- Consumed by later attendance, campus access, and transport plans.

**Validation rules**:
- Snapshots cannot mark suspended, replaced, expired, or revoked credentials as
  current identity evidence.
- Snapshots must include enough tenant, student, status, validity, and
  revocation evidence for later offline scan rules.
- Snapshots do not define attendance, campus access, or transport outcomes.
