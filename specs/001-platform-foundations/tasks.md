# Tasks: Phase 0 Platform Foundations

**Input**: Design documents from `/specs/001-platform-foundations/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/, quickstart.md

**Tests**: No separate automated test tasks are required because this feature is
a documentation and design foundation. Validation tasks are included in each
story and in the final phase.

**Implementation Note for Low-Cost Models**: Do not implement runtime web,
backend, mobile, database, API, attendance, transport, wallet, or dashboard
features. Only edit files under `specs/001-platform-foundations/` unless a task
explicitly names another path.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it edits a different file or an isolated section
- **[Story]**: User-story label for story phases only
- Every task names the exact file path to edit

## Phase 1: Setup (Shared Documentation Workspace)

**Purpose**: Create the helper documents and placeholders that keep later tasks
small, explicit, and easy to verify.

- [x] T001 Create `specs/001-platform-foundations/README.md` with links to `spec.md`, `plan.md`, `research.md`, `data-model.md`, `quickstart.md`, `contracts/foundation-decision.md`, `contracts/tenant-configuration.md`, and `contracts/scan-audit-event.md`
- [x] T002 [P] Create `specs/001-platform-foundations/foundation-areas.md` with empty sections for System Architecture, Multi-Tenant Architecture, Identity & Access Model, NFC & QR Integration, Event & Audit Logging, and Feature Flag / Tenant Configuration
- [x] T003 [P] Create `specs/001-platform-foundations/tenant-access-boundaries.md` with empty sections for School Account Boundary, Tenant-Owned Records, Actor Categories, Permission Rules, Configuration Change Approval, and Edge Cases
- [x] T004 [P] Create `specs/001-platform-foundations/scan-audit-accountability.md` with empty sections for Identity Evidence, NFC Capture, QR Fallback, Offline Sync, Duplicate and Conflict Review, and Audit Event Categories
- [x] T005 [P] Create `specs/001-platform-foundations/traceability-matrix.md` with a table header for Later Phase Requirement, Implementation Phase, Feature Module, Foundation Area, Tenant Boundary, Actor/Permission Rule, Feature Capability, Scan/Audit Evidence, Observability Expectation, and Source Link
- [x] T006 [P] Create `specs/001-platform-foundations/review-log.md` with table columns for Review Item, Source File, Reviewer Role, Status, Evidence, and Follow-Up, then update the Documentation tree in `specs/001-platform-foundations/plan.md` to include `README.md`, `foundation-areas.md`, `tenant-access-boundaries.md`, `scan-audit-accountability.md`, `traceability-matrix.md`, and `review-log.md`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish common vocabulary, identifiers, and validation rules used
by all user-story phases.

**CRITICAL**: Do not start user-story tasks until T001-T014 are complete.

- [x] T007 Add a canonical glossary to `specs/001-platform-foundations/README.md` covering School Account, Feature Capability, Actor Category, Permission Rule, Identity Evidence, Scan Event, Audit Event, Configuration Change, and Foundation Decision
- [x] T008 Add a Phase 0 scope guard to `specs/001-platform-foundations/README.md` listing all excluded Phase 1+ workflows from FR-012 in `specs/001-platform-foundations/spec.md`
- [x] T009 Add a Constitution Gate Evidence section to `specs/001-platform-foundations/review-log.md` with one row for each PASS gate from `specs/001-platform-foundations/plan.md`
- [x] T010 Add Foundation Decision ID rules to `specs/001-platform-foundations/foundation-areas.md` using `FD-001` through `FD-006`, one ID per Phase 0 foundation area
- [x] T011 Add Actor Category ID rules to `specs/001-platform-foundations/tenant-access-boundaries.md` using stable IDs for Platform Owner, School Administrator, Staff Member, Guardian, Student, and Reviewer
- [x] T012 Add Scan and Audit ID rules to `specs/001-platform-foundations/scan-audit-accountability.md` using prefixes `IE-`, `SE-`, `AE-`, and `TC-` for identity evidence, scan event, audit event, and configuration change examples
- [x] T013 Add row guidance to `specs/001-platform-foundations/traceability-matrix.md` explaining how each later-phase sample maps to implementation phase, feature module, foundation area, tenant boundary, actor/permission rule, feature capability, scan/audit evidence, observability expectation, and source link
- [x] T014 Add a "Task Execution Order" section to `specs/001-platform-foundations/quickstart.md` stating that Phase 1 and Phase 2 tasks must finish before US1, US2, or US3 tasks begin

**Checkpoint**: Shared documentation scaffolding is ready and user-story work can
start.

---

## Phase 3: User Story 1 - Approve Foundation Blueprint (Priority: P1) MVP

**Goal**: Product and engineering leadership can review one Phase 0 foundation
blueprint and confirm all six foundation areas have scope, owners, boundaries,
dependencies, acceptance criteria, and Phase 1+ exclusions.

**Independent Test**: Open `specs/001-platform-foundations/foundation-areas.md`
and verify that all six Phase 0 areas have a decision ID, scope, exclusions,
review owner, dependencies, acceptance criteria, and links to source artifacts.

### Implementation for User Story 1

- [x] T015 [US1] Populate the System Architecture section in `specs/001-platform-foundations/foundation-areas.md` with objective, scope, exclusions, review owner, dependencies, acceptance criteria, and source links to `specs/001-platform-foundations/spec.md` and `specs/001-platform-foundations/research.md`
- [x] T016 [US1] Populate the Multi-Tenant Architecture section in `specs/001-platform-foundations/foundation-areas.md` with objective, school account boundary summary, exclusions, review owner, dependencies, acceptance criteria, and source links to `specs/001-platform-foundations/data-model.md`
- [x] T017 [US1] Populate the Identity & Access Model section in `specs/001-platform-foundations/foundation-areas.md` with actor-category boundaries, exclusions for Phase 1 workflows, review owner, dependencies, acceptance criteria, and source links to `specs/001-platform-foundations/spec.md`
- [x] T018 [US1] Populate the NFC & QR Integration section in `specs/001-platform-foundations/foundation-areas.md` with scan-accountability scope, exclusions for attendance and transport outcomes, review owner, dependencies, acceptance criteria, and source links to `specs/001-platform-foundations/contracts/scan-audit-event.md`
- [x] T019 [US1] Populate the Event & Audit Logging section in `specs/001-platform-foundations/foundation-areas.md` with audit category scope, review evidence expectations, review owner, dependencies, acceptance criteria, and source links to `specs/001-platform-foundations/data-model.md`
- [x] T020 [US1] Populate the Feature Flag / Tenant Configuration section in `specs/001-platform-foundations/foundation-areas.md` with feature capability scope, configuration change ownership, exclusions, review owner, dependencies, acceptance criteria, and source links to `specs/001-platform-foundations/contracts/tenant-configuration.md`
- [x] T021 [US1] Add a Phase 1+ Exclusions table to `specs/001-platform-foundations/foundation-areas.md` that lists every excluded workflow from FR-012 and the later phase that owns it
- [x] T022 [US1] Add a Foundation Area Acceptance Matrix to `specs/001-platform-foundations/foundation-areas.md` with one row per foundation area and columns for Scope Complete, Owner Named, Boundary Defined, Dependencies Listed, Acceptance Criteria Present, and Source Links Present
- [x] T023 [P] [US1] Add a Decision Index section to `specs/001-platform-foundations/research.md` mapping each research decision to its matching Foundation Decision ID in `specs/001-platform-foundations/foundation-areas.md`
- [x] T024 [P] [US1] Add six concrete YAML examples to `specs/001-platform-foundations/contracts/foundation-decision.md`, one each for `FD-001` through `FD-006`
- [x] T025 [US1] Add US1 evidence rows to `specs/001-platform-foundations/traceability-matrix.md` for System Architecture, Multi-Tenant Architecture, Identity & Access Model, NFC & QR Integration, Event & Audit Logging, and Feature Flag / Tenant Configuration
- [x] T026 [US1] Add a "Foundation Blueprint Review" section to `specs/001-platform-foundations/quickstart.md` with step-by-step instructions for checking `specs/001-platform-foundations/foundation-areas.md`
- [x] T027 [US1] Add US1 readiness rows to `specs/001-platform-foundations/review-log.md` covering the two acceptance scenarios from `specs/001-platform-foundations/spec.md`

**Checkpoint**: US1 is complete when a reviewer can approve or reject the full
Phase 0 foundation blueprint from `foundation-areas.md` without reading hidden
context.

---

## Phase 4: User Story 2 - Validate Tenant and Access Boundaries (Priority: P2)

**Goal**: Security and operations reviewers can verify school account
boundaries, actor categories, permission rules, tenant-owned records, and
configuration ownership.

**Independent Test**: Open `specs/001-platform-foundations/tenant-access-boundaries.md`
and verify that each actor category and tenant-owned record expectation states
who can view, change, or approve access and what evidence must be retained.

### Implementation for User Story 2

- [x] T028 [US2] Define School Account boundary rules in `specs/001-platform-foundations/tenant-access-boundaries.md`, including default isolation, allowed internal campus scoping, and explicit approval for cross-school access
- [x] T029 [US2] Define tenant-owned record metadata expectations in `specs/001-platform-foundations/tenant-access-boundaries.md`, including owner, creation evidence, update evidence, review owner, and audit evidence
- [x] T030 [US2] Define Actor Category table in `specs/001-platform-foundations/tenant-access-boundaries.md` for Platform Owner, School Administrator, Staff Member, Guardian, Student, and Reviewer
- [x] T031 [US2] Define Permission Rule matrix in `specs/001-platform-foundations/tenant-access-boundaries.md` with allowed action, boundary type, approval requirement, and review evidence columns
- [x] T032 [US2] Define Configuration Change Approval flow in `specs/001-platform-foundations/tenant-access-boundaries.md` from requested to approved, rejected, applied, or rolled back
- [x] T033 [US2] Add Edge Case handling to `specs/001-platform-foundations/tenant-access-boundaries.md` for multiple campuses, multi-role users, disabled capabilities, and later-phase conflicts
- [x] T034 [US2] Add concrete validation examples to the School Account and Feature Capability sections in `specs/001-platform-foundations/data-model.md`
- [x] T035 [US2] Add allowed boundary values and sensitive-action examples to the Actor Category and Permission Rule sections in `specs/001-platform-foundations/data-model.md`
- [x] T036 [P] [US2] Add approved, rejected, applied, and rolled-back YAML examples to `specs/001-platform-foundations/contracts/tenant-configuration.md`
- [x] T037 [US2] Add tenant and access sample rows to `specs/001-platform-foundations/traceability-matrix.md` for guardian linking, permission enforcement, tenant feature configuration, and admin review samples from `PLAN.md`
- [x] T038 [US2] Add a "Tenant and Access Boundary Review" section to `specs/001-platform-foundations/quickstart.md` with reviewer steps for actor categories, permission rules, and configuration changes
- [x] T039 [US2] Add US2 readiness rows to `specs/001-platform-foundations/review-log.md` covering the two acceptance scenarios from `specs/001-platform-foundations/spec.md`

**Checkpoint**: US2 is complete when a reviewer can identify the applicable
actor category, tenant boundary, approval path, and audit evidence for each
sample sensitive action.

---

## Phase 5: User Story 3 - Establish Scan and Audit Accountability (Priority: P3)

**Goal**: School operations stakeholders can verify the shared evidence model
for NFC, QR fallback, offline scan capture, duplicate/conflict review, and audit
records before later attendance, access, or transport specs use it.

**Independent Test**: Open `specs/001-platform-foundations/scan-audit-accountability.md`
and verify that NFC capture, QR fallback, offline sync, duplicate/conflict
review, and audit categories have required evidence and review outcomes.

### Implementation for User Story 3

- [x] T040 [US3] Define Identity Evidence baseline in `specs/001-platform-foundations/scan-audit-accountability.md`, including subject type, school account, evidence type, status, verifier, and verification time
- [x] T041 [US3] Define NFC capture accountability in `specs/001-platform-foundations/scan-audit-accountability.md`, including capture source, captured time, received time, identity evidence reference, and review outcome
- [x] T042 [US3] Define QR fallback parity in `specs/001-platform-foundations/scan-audit-accountability.md`, explicitly stating that QR evidence must meet the same accountability expectations as NFC evidence
- [x] T043 [US3] Define offline scan sync states in `specs/001-platform-foundations/scan-audit-accountability.md`, including Pending, Synced, Duplicate, Conflict, Rejected, and Deferred meanings
- [x] T044 [US3] Define duplicate and conflict review outcomes in `specs/001-platform-foundations/scan-audit-accountability.md`, including accepted, rejected, needs review, and deferred outcomes
- [x] T045 [US3] Define Audit Event categories in `specs/001-platform-foundations/scan-audit-accountability.md` for Identity, Access, Tenant Configuration, Feature Availability, Scan Capture, Scan Reconciliation, and Administrative Review
- [x] T046 [US3] Add state transition examples to the Identity Evidence and Scan Event sections in `specs/001-platform-foundations/data-model.md`
- [x] T047 [US3] Add review evidence examples to the Audit Event and Configuration Change sections in `specs/001-platform-foundations/data-model.md`
- [x] T048 [P] [US3] Add offline NFC, QR fallback, duplicate scan, and conflict review YAML examples to `specs/001-platform-foundations/contracts/scan-audit-event.md`
- [x] T049 [US3] Add scan and audit sample rows to `specs/001-platform-foundations/traceability-matrix.md` for gate scans, QR identity fallback, bus boarding scans, and audit trail samples from `PLAN.md`
- [x] T050 [US3] Add a "Scan and Audit Accountability Review" section to `specs/001-platform-foundations/quickstart.md` with reviewer steps for NFC, QR fallback, offline sync, duplicates, conflicts, and audit records
- [x] T051 [US3] Add US3 readiness rows to `specs/001-platform-foundations/review-log.md` covering the two acceptance scenarios from `specs/001-platform-foundations/spec.md`

**Checkpoint**: US3 is complete when a reviewer can inspect any sample NFC,
QR fallback, offline scan, or audit case and identify the required evidence and
review outcome.

---

## Phase 6: Polish & Cross-Cutting Validation

**Purpose**: Ensure all generated foundation documentation is coherent,
traceable, and ready for review.

- [x] T052 Search all Markdown files under `specs/001-platform-foundations/` for unresolved clarification markers and record the result in `specs/001-platform-foundations/review-log.md`
- [x] T053 Verify every file created by tasks T001-T006 is linked from `specs/001-platform-foundations/README.md` and record pass/fail evidence in `specs/001-platform-foundations/review-log.md`
- [x] T054 Verify `specs/001-platform-foundations/traceability-matrix.md` contains one row for every Phase 1 candidate spec from `PLAN.md` and at least five later-phase samples, with 100% of Phase 1 rows mapped to Phase 0 foundation decisions and at least 95% of all samples mapped
- [x] T055 Compare entity names in `specs/001-platform-foundations/data-model.md` with contract names in `specs/001-platform-foundations/contracts/foundation-decision.md`, `specs/001-platform-foundations/contracts/tenant-configuration.md`, and `specs/001-platform-foundations/contracts/scan-audit-event.md`, then record mismatches or pass evidence in `specs/001-platform-foundations/review-log.md`
- [x] T056 Update `specs/001-platform-foundations/quickstart.md` with a final "Ready for Review" checklist covering US1, US2, US3, traceability, contracts, unresolved clarifications, and a 60-minute review dry-run
- [x] T057 Verify `specs/001-platform-foundations/foundation-areas.md` links each foundation area to at least one source in `specs/001-platform-foundations/spec.md`, `specs/001-platform-foundations/research.md`, `specs/001-platform-foundations/data-model.md`, or `specs/001-platform-foundations/contracts/`, then record pass/fail evidence in `specs/001-platform-foundations/review-log.md`
- [x] T058 Run a Markdown formatting review and record pass/fail notes in `specs/001-platform-foundations/review-log.md`
- [x] T059 Mark final readiness status for US1, US2, and US3 in `specs/001-platform-foundations/review-log.md`, including reviewer start time, end time, duration, and whether the 60-minute target passed

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: T001-T006 can start immediately.
- **Foundational (Phase 2)**: T007-T014 depend on the setup files from T001-T006 and block all user stories.
- **US1 (Phase 3)**: T015-T027 depend on T001-T014.
- **US2 (Phase 4)**: T028-T039 depend on T001-T014; US2 can run in parallel with US1 after Phase 2, but reviewing US2 is easier after US1.
- **US3 (Phase 5)**: T040-T051 depend on T001-T014; US3 can run in parallel with US1 or US2 after Phase 2.
- **Polish (Phase 6)**: T052-T059 depend on all desired user stories being complete.

### User Story Dependencies

- **US1 (P1)**: Independent after Phase 2; recommended MVP.
- **US2 (P2)**: Independent after Phase 2, but references glossary and ID rules from foundational tasks.
- **US3 (P3)**: Independent after Phase 2, but references glossary and ID rules from foundational tasks.

### Within Each User Story

- Complete story-specific content tasks before story-specific traceability and review-log tasks.
- Contract expansion tasks marked `[P]` can run alongside same-story content tasks when they edit different files.
- Quickstart and review-log tasks are final tasks inside each story.

---

## Parallel Opportunities

- T002-T006 can run in parallel after T001 is understood.
- T009-T013 can run in parallel after T007-T008 if different files are edited.
- T023 and T024 can run in parallel with T015-T022.
- T036 can run in parallel with T028-T033 because it edits only `contracts/tenant-configuration.md`.
- T048 can run in parallel with T040-T045 because it edits only `contracts/scan-audit-event.md`.

## Parallel Example: User Story 1

```text
Task: "T023 [P] [US1] Add a Decision Index section to specs/001-platform-foundations/research.md"
Task: "T024 [P] [US1] Add six concrete YAML examples to specs/001-platform-foundations/contracts/foundation-decision.md"
```

## Parallel Example: User Story 2

```text
Task: "T036 [P] [US2] Add approved, rejected, applied, and rolled-back YAML examples to specs/001-platform-foundations/contracts/tenant-configuration.md"
```

## Parallel Example: User Story 3

```text
Task: "T048 [P] [US3] Add offline NFC, QR fallback, duplicate scan, and conflict review YAML examples to specs/001-platform-foundations/contracts/scan-audit-event.md"
```

---

## Implementation Strategy

### MVP First (US1 Only)

1. Complete T001-T014 to create the shared workspace and rules.
2. Complete T015-T027 to produce the reviewable foundation blueprint.
3. Stop and validate US1 using the Independent Test above.

### Incremental Delivery

1. Complete Setup and Foundational tasks.
2. Complete US1 and review the full Phase 0 foundation blueprint.
3. Complete US2 and review tenant/access boundaries.
4. Complete US3 and review scan/audit accountability.
5. Complete Polish tasks and prepare for stakeholder review.

### Cheaper LLM Execution Guidance

- Work one task at a time and edit only the file named in the task.
- Preserve Phase 0 scope; do not add runtime code or later-phase product flows.
- When a task asks for examples, add concise examples that match existing entity
  and contract names.
- After each task, re-read the edited file and ensure the task's requested
  section exists with the required columns or fields.
