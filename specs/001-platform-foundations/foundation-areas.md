# Foundation Areas

## Foundation Decision ID Rules

| Decision ID | Foundation Area | ID Rule |
|-------------|-----------------|---------|
| FD-001 | System Architecture | Governs platform-level architectural boundaries and extraction rules. |
| FD-002 | Multi-Tenant Architecture | Governs school account isolation and internal campus scoping. |
| FD-003 | Identity & Access Model | Governs actor-category and permission-rule vocabulary. |
| FD-004 | NFC & QR Integration | Governs identity evidence, scan event, and QR fallback accountability. |
| FD-005 | Event & Audit Logging | Governs audit categories and reviewable evidence. |
| FD-006 | Feature Flag / Tenant Configuration | Governs tenant feature capability and configuration change ownership. |

## System Architecture

**Decision ID**: FD-001

**Objective**: Establish a simple modular platform baseline that later specs can
extend without creating conflicting architecture assumptions.

**Scope**: Phase 0 records the modular monolith baseline, shared artifact
expectations, product-spec traceability, and future extraction rule. Later
runtime plans decide concrete folders, packages, APIs, and deployment details.

**Exclusions**: No runtime source tree, database schema, service deployment,
mobile client, web screen, or product workflow is created by Phase 0.

**Review owner**: Product and Engineering Leadership.

**Dependencies**: `PLAN.md`, `.specify/memory/constitution.md`, this feature's
`spec.md`, and `research.md`.

**Acceptance criteria**:
- The area states that Phase 0 is a planning package, not runtime delivery.
- Later specs can identify where architecture rules come from.
- Any future microservice, queue, nonstandard storage, or broad CQRS choice
  must be justified in the later feature plan.

**Source links**: [spec.md](./spec.md), [research.md](./research.md).

## Multi-Tenant Architecture

**Decision ID**: FD-002

**Objective**: Define School Account as the default tenant boundary for all
future records and actions.

**School account boundary summary**: Every tenant-owned record belongs to one
School Account. Internal campuses, divisions, buses, classrooms, or departments
may narrow operational scope but must not weaken the parent school account
boundary. Cross-school access requires explicit platform authority, permission
evidence, and audit evidence.

**Scope**: Tenant-owned record expectations, cross-school approval, internal
campus scoping, and boundary validation language.

**Exclusions**: No school account provisioning UI, tenancy middleware, database
model, or cross-school operator workflow is implemented in Phase 0.

**Review owner**: Product, Security, and Operations Reviewers.

**Dependencies**: `data-model.md` School Account, Feature Capability,
Permission Rule, Audit Event, and Configuration Change entities.

**Acceptance criteria**:
- Every later tenant-owned entity can map to exactly one School Account.
- Cross-school access is prohibited by default.
- Internal campus scoping is documented as subordinate to the parent tenant.

**Source links**: [data-model.md](./data-model.md).

## Identity & Access Model

**Decision ID**: FD-003

**Objective**: Provide stable actor-category and permission-rule vocabulary for
later specs before detailed Phase 1 identity workflows exist.

**Actor-category boundaries**: Platform Owner may approve platform-level
foundation and cross-school changes. School Administrator operates inside a
school account. Staff Member is school-scoped and assignment-sensitive.
Guardian is delegated through approved relationships defined later. Student is
self-scoped where enabled. Reviewer acts through explicit review assignment.

**Scope**: Actor categories, permission rules, sensitive action documentation,
approval responsibility, and review evidence.

**Exclusions for Phase 1 workflows**: No student profile management, guardian
linking, NFC card provisioning, QR identity fallback provisioning, role-based
access implementation, or permission enforcement runtime is delivered here.

**Review owner**: Security Reviewer.

**Dependencies**: `spec.md` FR-005 and FR-006, and `tenant-access-boundaries.md`.

**Acceptance criteria**:
- Later specs can reuse the six canonical actor categories.
- Sensitive actions require a Permission Rule before implementation.
- A reviewer can identify the boundary and evidence for sampled access actions.

**Source links**: [spec.md](./spec.md), [tenant-access-boundaries.md](./tenant-access-boundaries.md).

## NFC & QR Integration

**Decision ID**: FD-004

**Objective**: Define shared accountability for NFC scans, QR fallback, offline
capture, sync, duplicate detection, conflicts, and review outcomes.

**Scan-accountability scope**: Scan Events record school account, capture
method, capture source, identity evidence reference, captured time, received
time, offline indicator, sync status, duplicate reference, and review outcome.
QR fallback must satisfy the same evidence expectations as NFC.

**Exclusions for attendance and transport outcomes**: Phase 0 does not create
attendance generation, campus entry or exit decisions, transport boarding,
transport drop-off, wallet payment, or emergency outcome behavior.

**Review owner**: Operations Reviewer.

**Dependencies**: `contracts/scan-audit-event.md`, `data-model.md` Identity
Evidence and Scan Event entities, and later Phase 1 identity evidence specs.

**Acceptance criteria**:
- Offline events have enough source and timing evidence for reconciliation.
- QR fallback evidence is equivalent to NFC evidence.
- Duplicate and conflict outcomes are reviewable before later specs attach
  business consequences.

**Source links**: [contracts/scan-audit-event.md](./contracts/scan-audit-event.md).

## Event & Audit Logging

**Decision ID**: FD-005

**Objective**: Define audit categories and review evidence expectations that
later specs can extend.

**Audit category scope**: Identity, Access, Tenant Configuration, Feature
Availability, Scan Capture, Scan Reconciliation, and Administrative Review.

**Review evidence expectations**: Audit Events preserve actor category, school
account when tenant-scoped, subject reference, event time, reason, and review
status. Sensitive access, configuration changes, scan reconciliation, and
administrative reviews must emit audit evidence.

**Review owner**: Security and Compliance Reviewer.

**Dependencies**: `data-model.md` Audit Event and Configuration Change
entities, `contracts/scan-audit-event.md`, and `review-log.md`.

**Acceptance criteria**:
- Every later sensitive action can map to an audit category or amend one.
- Audit records explain who acted, what changed, when, why, and under which
  school account boundary.
- Audit categories are stable but not exhaustive event-type catalogs.

**Source links**: [data-model.md](./data-model.md), [contracts/scan-audit-event.md](./contracts/scan-audit-event.md).

## Feature Flag / Tenant Configuration

**Decision ID**: FD-006

**Objective**: Establish tenant feature capability and configuration change
ownership before later product capabilities are introduced.

**Feature capability scope**: Later specs must declare whether they introduce,
read, or depend on a Feature Capability. Disabled or Suspended capabilities
must be visible as unavailable for the affected School Account.

**Configuration change ownership**: Configuration Changes require requester,
approver, reason, previous value, new value, effective time, review status, and
audit evidence before application.

**Exclusions**: Phase 0 does not build a feature-flag service, admin dashboard,
configuration UI, approval workflow engine, or runtime authorization layer.

**Review owner**: Product, Security, and Operations Reviewers.

**Dependencies**: `contracts/tenant-configuration.md`, `data-model.md` Feature
Capability and Configuration Change entities.

**Acceptance criteria**:
- Capability availability is tenant-scoped and traceable.
- Configuration changes preserve requester, approver, reason, and audit event.
- Later specs document unavailable-capability behavior.

**Source links**: [contracts/tenant-configuration.md](./contracts/tenant-configuration.md).

## Phase 1+ Exclusions

| Excluded Workflow | Owning Later Phase | Phase 0 Boundary |
|-------------------|--------------------|------------------|
| Student profile management | Phase 1: Identity & Access | Actor and identity evidence vocabulary only. |
| Guardian linking | Phase 1: Identity & Access | Delegated actor category only. |
| Card provisioning workflows | Phase 1: Identity & Access | Identity evidence expectations only. |
| Attendance generation | Phase 2: Attendance & Campus Access | Scan event evidence only. |
| Transport tracking | Phase 3: Transport & Bus Tracking | Scan and audit accountability only. |
| Wallet payments | Phase 4: Wallet & Payments | Tenant, permission, and audit rules only. |
| Learning features | Phase 5: Learning & Engagement | Tenant, actor, capability, and audit rules only. |
| Requests | Phase 6: Requests & Permissions | Permission and configuration vocabulary only. |
| Medical workflows | Phase 7: Medical & Emergency | Sensitive access and audit vocabulary only. |
| Complaints | Phase 8: Complaints & Escalations | Reviewer and audit vocabulary only. |
| Communications | Phase 9: Communication & Notifications | Capability and audit vocabulary only. |
| Documents | Phase 10: Documents & Search | Tenant, permission, audit, and capability baseline only. |
| Search | Phase 10: Documents & Search | Permission, capability, and audit baseline only. |
| Dashboards | Phase 11: Admin, Audit & Observability | Observability and audit expectations only. |

## Foundation Area Acceptance Matrix

| Foundation Area | Scope Complete | Owner Named | Boundary Defined | Dependencies Listed | Acceptance Criteria Present | Source Links Present |
|-----------------|----------------|-------------|------------------|---------------------|-----------------------------|----------------------|
| System Architecture | Yes | Yes | Yes | Yes | Yes | Yes |
| Multi-Tenant Architecture | Yes | Yes | Yes | Yes | Yes | Yes |
| Identity & Access Model | Yes | Yes | Yes | Yes | Yes | Yes |
| NFC & QR Integration | Yes | Yes | Yes | Yes | Yes | Yes |
| Event & Audit Logging | Yes | Yes | Yes | Yes | Yes | Yes |
| Feature Flag / Tenant Configuration | Yes | Yes | Yes | Yes | Yes | Yes |
