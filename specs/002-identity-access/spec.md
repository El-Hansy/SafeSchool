# Feature Specification: Phase 1 Identity & Access

**Feature Branch**: `002-identity-access`
**Created**: 2026-05-03
**Status**: Draft
**Input**: User description: "Read PLAN.md and create a specification for Phase 1: Identity & Access"

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 1: Identity & Access
- **Feature Module(s)**: Student Profile, Guardian Linking, NFC Card Provisioning, QR Identity Fallback, Role-Based Access, Permission Enforcement
- **Tenant Scope**: All student profiles, guardian relationships, identity credentials, roles, permissions, access decisions, and audit evidence belong to one school account and must not be visible or actionable outside that school account unless a platform-level role explicitly allows review.
- **Feature Flag(s)**: Student profile management, guardian linking, NFC card provisioning, QR identity fallback, role administration, and permission enforcement must respect each school account's enabled capabilities before users can access the related workflow.
- **Security/Roles**: Platform owners, school administrators, authorized staff, guardians, students, and reviewers must have explicit permissions for each identity action; sensitive profile, guardian, credential, role, and permission changes must be enforced by the system and denied when the actor lacks authority.
- **Offline/NFC Impact**: This phase establishes NFC cards and QR fallback as identity credentials only. It must define credential status, expiry, revocation, and offline-verifiable evidence needed by later scan flows, without generating attendance, campus access, transport, wallet, or notification outcomes.
- **Observability**: The system must emit reviewable audit evidence for student profile changes, guardian link lifecycle changes, credential issuance and revocation, QR fallback lifecycle changes, role and permission changes, and access denials.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Maintain Student Identity Profile (Priority: P1)

As a school administrator, I need to create and maintain a verified student
identity profile so the school has one trusted identity record that later
attendance, transport, wallet, learning, and safety workflows can reference.

**Why this priority**: Student identity is the anchor for every later school
operation. Without a reliable student profile, guardian access, card issuance,
and permission decisions cannot be trusted.

**Independent Test**: Create a student identity profile for a school account,
update controlled profile details, and confirm the active profile remains
unique, tenant-scoped, and reviewable.

**Acceptance Scenarios**:

1. **Given** a school administrator is authorized for a school account, **When** they create a student profile with all required identity details, **Then** the student profile is available within that school account with a clear active status and review history.
2. **Given** an active student profile already exists with the configured school identity identifiers, **When** a user attempts to create another active profile for the same student in the same school account, **Then** the system prevents duplicate activation and records the review reason.
3. **Given** a student profile requires correction, **When** an authorized user updates controlled profile details, **Then** the current profile reflects the approved details and the prior values remain reviewable.

---

### User Story 2 - Enforce Identity Permissions (Priority: P1)

As a security reviewer, I need identity and access actions to be allowed only
for users with the right role and permission so student information and
credentials are protected consistently across schools.

**Why this priority**: Permission enforcement is required before sensitive
identity operations can be safely used by administrators, staff, guardians, or
reviewers.

**Independent Test**: Run representative identity actions using platform owner,
school administrator, staff, guardian, student, and reviewer accounts, then
confirm allowed actions succeed and unauthorized actions are denied with
reviewable reasons.

**Acceptance Scenarios**:

1. **Given** a staff member lacks permission to change guardian relationships, **When** they attempt to add or remove a guardian link, **Then** the system denies the action and records the denied permission.
2. **Given** a user has multiple roles in the same school account, **When** they perform an identity action, **Then** the system applies the effective permissions for that school account and records the access decision.
3. **Given** a school account has a Phase 1 capability disabled, **When** an otherwise authorized user attempts to use that workflow, **Then** the system blocks the action because the capability is not enabled for that school account.

---

### User Story 3 - Link Guardians to Students (Priority: P2)

As a school administrator, I need to link guardians to student identity profiles
with relationship and access scope so guardians can view only the students and
information they are approved to access.

**Why this priority**: Guardian relationships are essential for family-facing
features, but they depend on trusted student profiles and permission
enforcement being in place first.

**Independent Test**: Create, approve, update, and remove a guardian link for a
student profile, then confirm guardian visibility changes according to the link
state and access scope.

**Acceptance Scenarios**:

1. **Given** a verified student profile and a guardian record in the same school account, **When** an authorized user creates an approved guardian link with relationship type and access scope, **Then** the guardian can access only the approved student information for that link.
2. **Given** a guardian link is pending, suspended, expired, rejected, or removed, **When** the guardian attempts to access student information through that link, **Then** the system prevents access and records the link state that caused the decision.
3. **Given** a guardian relationship is disputed or no longer valid, **When** an authorized user suspends or removes the link, **Then** the guardian loses access to that student's information while the change remains reviewable.

---

### User Story 4 - Manage NFC and QR Identity Credentials (Priority: P3)

As a school administrator or authorized staff member, I need to issue, suspend,
replace, revoke, and review student NFC cards and QR fallback credentials so
students can be identified by approved credentials in later scan-based flows.

**Why this priority**: Credentials make identity usable in physical school
operations, but this phase must first establish identity, guardian, and access
rules before scan outcomes are introduced in later phases.

**Independent Test**: Issue an NFC card and QR fallback credential for a student,
change each credential's status, and confirm only active, unexpired credentials
are considered valid identity evidence.

**Acceptance Scenarios**:

1. **Given** a verified active student profile, **When** an authorized user issues an NFC card to that student, **Then** the card is associated with exactly one active student identity in the school account and has a reviewable credential status.
2. **Given** an NFC card is lost, suspended, replaced, expired, or revoked, **When** the credential is checked for identity use, **Then** the system treats the card as unavailable for current identity evidence and records the reason.
3. **Given** QR fallback is enabled for the school account, **When** an authorized user creates or rotates a QR fallback credential, **Then** the credential has a clear validity window, status, and review history.
4. **Given** QR fallback is disabled for the school account, **When** a user attempts to create or use a QR fallback credential, **Then** the system blocks the workflow and records the capability decision.

---

### Edge Cases

- A school account has multiple campuses or divisions; identity records remain owned by the parent school account while optional internal assignment can limit local visibility.
- A student transfers into, within, or out of a school account; the prior identity state, credential state, and guardian links must remain reviewable without creating conflicting active identities.
- Two student records appear to match based on configured identifiers but require human review before merge, rejection, or activation.
- A guardian has approved links to multiple students, or multiple guardians are linked to one student; visibility must be calculated per link and per school account.
- A guardian link is disputed by the school or family; the link can be suspended while preserving evidence and preventing unauthorized access.
- A user belongs to more than one school account; role and permission decisions must be scoped to the active school account.
- A user has multiple roles in one school account; the access decision must show which role or permission allowed or denied the action.
- A credential is presented after suspension, expiry, replacement, or revocation; it must not be treated as active identity evidence.
- QR fallback is requested while NFC is available, unavailable, or disabled; the result must follow school account configuration and credential status.
- Identity credential evidence is used while a device is offline in a later phase; this phase must provide enough status and validity information for later offline scan rules without defining scan outcomes.
- A Phase 1 capability is disabled for a school account; users must not be able to bypass the unavailable workflow through another identity action.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized users to create, view, update, deactivate, and review student identity profiles within a school account.
- **FR-002**: The system MUST prevent more than one active student identity profile for the same configured school identity identifiers within a school account unless an authorized review resolves the conflict.
- **FR-003**: The system MUST preserve review history for student identity changes, including who requested the change, who approved or rejected it when approval is required, the timing, and the business reason.
- **FR-004**: The system MUST allow authorized users to create and manage guardian records and guardian links to student profiles within the same school account.
- **FR-005**: Each guardian link MUST include relationship type, access scope, lifecycle state, validity period when applicable, and review history.
- **FR-006**: The system MUST ensure guardians can access only student information allowed by an approved active guardian link and the school account's enabled capabilities.
- **FR-007**: The system MUST allow authorized users to issue, suspend, replace, expire, and revoke NFC card credentials for verified active student profiles.
- **FR-008**: The system MUST ensure each active NFC card credential is associated with exactly one active student identity in the school account at a time.
- **FR-009**: The system MUST allow QR fallback identity credentials only when enabled for the school account and only for verified active student profiles.
- **FR-010**: Each QR fallback credential MUST have a validity window, lifecycle state, rotation or replacement evidence when applicable, and revocation evidence.
- **FR-011**: The system MUST define the Phase 1 actor categories and permissions required for profile management, guardian linking, credential management, role administration, permission administration, review, and read-only inspection.
- **FR-012**: The system MUST enforce role and permission decisions before every sensitive student profile, guardian link, credential, role, and permission action.
- **FR-013**: The system MUST record denied access decisions with the school account, actor, attempted action, relevant target, denial reason, and time of decision.
- **FR-014**: The system MUST respect school account feature configuration before allowing student profile, guardian linking, NFC card provisioning, QR fallback, role administration, or permission administration workflows.
- **FR-015**: The system MUST keep all Phase 1 records scoped to a school account and prevent cross-school visibility or action unless an explicit platform-level review role permits it.
- **FR-016**: The system MUST provide reviewers with a chronological history of student profile, guardian link, NFC credential, QR fallback credential, role, permission, and access-denial events.
- **FR-017**: The system MUST expose enough identity credential status, ownership, validity, and revocation evidence for later attendance, campus access, and transport scan flows to determine whether a credential can be considered current identity evidence.
- **FR-018**: The system MUST explicitly exclude attendance generation, campus entry or exit decisions, bus boarding outcomes, wallet payment behavior, learning features, request workflows, medical workflows, complaint workflows, communications, document management, and search behavior from Phase 1 deliverable scope.

### Key Entities *(include if feature involves data)*

- **Student Profile**: The school-owned identity record for a student, including required identity details, active status, internal school identifiers, optional campus or division assignment, and review history.
- **Guardian Record**: A school-owned identity record for a guardian who may be linked to one or more students in that school account.
- **Guardian Link**: The relationship between a guardian and a student profile, including relationship type, access scope, lifecycle state, validity period, and review evidence.
- **Identity Credential**: A credential assigned to a student profile for identity evidence, represented by an NFC card credential or QR fallback credential.
- **NFC Card Credential**: A physical card identity credential tied to one student profile at a time, with issue, replacement, suspension, expiry, and revocation state.
- **QR Fallback Credential**: A fallback identity credential tied to one student profile, governed by school account configuration, validity window, status, rotation, and revocation rules.
- **Role**: A named set of responsibilities available within a school account or platform review context.
- **Permission**: A specific allowed action or read scope used to decide whether an actor may perform a Phase 1 workflow.
- **Access Decision**: The recorded allow or deny outcome for a sensitive identity or access action, including actor, school account, action, target, reason, and timing.
- **Audit Event**: Reviewable evidence of important Phase 1 activity, including profile, guardian, credential, role, permission, and access-denial changes.
- **School Account Feature Setting**: A tenant capability setting that determines whether Phase 1 workflows are available for a school account.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: School administrators can create a complete student identity profile with required information and duplicate checks in under 5 minutes during review testing.
- **SC-002**: 100% of sampled unauthorized profile, guardian, credential, role, and permission actions are denied with a clear reviewable reason.
- **SC-003**: 95% of sampled guardian relationship cases can be approved, suspended, removed, or rejected using the specified link state, access scope, and review evidence without additional clarification.
- **SC-004**: Authorized staff can determine the current active, suspended, expired, replaced, or revoked credential status for a student in under 60 seconds.
- **SC-005**: 100% of sampled student profile, guardian link, credential, role, permission, and access-denial changes have chronological audit evidence available to reviewers.
- **SC-006**: Duplicate active student identity attempts within the same school account are detected before activation in 100% of sampled duplicate scenarios.
- **SC-007**: 100% of sampled Phase 2 and Phase 3 scan-related identity checks can determine whether an NFC or QR credential should be treated as current identity evidence without defining attendance, campus access, or transport outcomes in this phase.

## Assumptions

- Phase 1 builds on the Phase 0 foundation rules for school account isolation, actor categories, tenant configuration, identity evidence, and audit expectations.
- The school account is the default tenant boundary for all Phase 1 identity and access records.
- School administrators are the primary actors for student profile, guardian link, and credential management unless the school account grants specific staff permissions.
- Guardian access requires an approved active guardian link to a student profile; having a guardian record alone is not enough to view student information.
- Students may have read-only access to their own limited identity information only when the school account enables that capability and permissions allow it.
- NFC cards and QR fallback credentials are identity evidence only in Phase 1; attendance, campus entry and exit, transport boarding, wallet payment, and notification outcomes belong to later phases.
- QR fallback is optional per school account and may be disabled even when NFC card provisioning is enabled.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 1 requirements.
