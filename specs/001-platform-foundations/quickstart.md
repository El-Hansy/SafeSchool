# Quickstart: Validate Phase 0 Platform Foundations

Use this quickstart to review the Phase 0 planning artifacts before generating
implementation tasks.

## 1. Confirm Scope

Read [spec.md](./spec.md) and verify that the feature is limited to Phase 0:

- System Architecture
- Multi-Tenant Architecture
- Identity & Access Model
- NFC & QR Integration
- Event & Audit Logging
- Feature Flag / Tenant Configuration

Confirm that Phase 1 and later product workflows are excluded.

## 2. Review Constitution Gates

Open [plan.md](./plan.md) and check the Constitution Check section.

Every gate must be marked PASS:

- Spec-first delivery
- Phase and module mapping
- Multi-tenancy and feature flags
- Security and authorization
- Data and API contracts
- Offline NFC integrity
- Observability and testing
- Simplicity and cost

If any gate fails, stop and amend the spec or plan before continuing.

## 3. Review Research Decisions

Open [research.md](./research.md) and verify that each decision includes:

- Decision
- Rationale
- Alternatives considered

Pay special attention to school account tenancy, actor categories, feature
capability configuration, scan accountability, and audit event categories.

## 4. Validate the Data Model

Open [data-model.md](./data-model.md) and sample each entity:

- School Account
- Feature Capability
- Actor Category
- Permission Rule
- Identity Evidence
- Scan Event
- Audit Event
- Configuration Change
- Foundation Decision

For each entity, confirm that fields, relationships, validation rules, and
state transitions are sufficient for later specs to reference.

## 5. Check Contracts

Review all files in [contracts/](./contracts/):

- [foundation-decision.md](./contracts/foundation-decision.md)
- [tenant-configuration.md](./contracts/tenant-configuration.md)
- [scan-audit-event.md](./contracts/scan-audit-event.md)

Confirm each contract has required records and acceptance rules.

## 6. Sample Later-Phase Traceability

Choose at least five candidate later-phase requirements from `PLAN.md`, such as
guardian linking, gate scans, boarding scans, wallet transactions, or emergency
access.

For each sample, identify:

- The relevant foundation area
- The tenant boundary
- The actor category or permission rule
- The feature capability or configuration impact
- The scan or audit evidence needed, when applicable

At least 95% of sampled requirements must map to one or more Phase 0
foundation decisions.

## 7. Ready for Tasks

Proceed to `/speckit.tasks` only when:

- The Constitution Check passes.
- No unresolved clarification markers remain.
- Phase 0 scope is preserved.
- The contracts and data model are reviewable by product and engineering.

## Task Execution Order

When running `/speckit-implement`, complete Phase 1 setup tasks and Phase 2
foundational tasks before starting US1, US2, or US3.

Required order:

1. Finish T001 through T006 so the documentation workspace exists.
2. Finish T007 through T014 so glossary, scope guard, ID rules, traceability
   guidance, constitution evidence, and quickstart order are in place.
3. Complete US1 tasks T015 through T027.
4. Complete US2 tasks T028 through T039.
5. Complete US3 tasks T040 through T051.
6. Complete polish tasks T052 through T059 and record final readiness.

## Foundation Blueprint Review

Use [foundation-areas.md](./foundation-areas.md) to validate US1.

1. Confirm FD-001 through FD-006 are present and each maps to one Phase 0
   foundation area.
2. For each foundation area, check objective, scope, exclusions, review owner,
   dependencies, acceptance criteria, and source links.
3. Verify the Phase 1+ Exclusions table lists every workflow excluded by
   `spec.md` FR-012.
4. Verify the Foundation Area Acceptance Matrix marks every required column as
   complete.
5. Sample a later proposal in [traceability-matrix.md](./traceability-matrix.md)
   and confirm reviewers can find the governing foundation decision without
   creating a new category.

## Tenant and Access Boundary Review

Use [tenant-access-boundaries.md](./tenant-access-boundaries.md) to validate
US2.

1. Confirm School Account is the default boundary and cross-school access
   requires explicit approval.
2. Check that tenant-owned records define owner, creation evidence, update
   evidence, review owner, and audit evidence.
3. Verify actor categories AC-001 through AC-006 cover Platform Owner, School
   Administrator, Staff Member, Guardian, Student, and Reviewer.
4. Review Permission Rules PR-001 through PR-007 and confirm each sensitive
   action states boundary, approval, and review evidence.
5. Walk a configuration change from requested to approved, rejected, applied,
   or rolled back and confirm the Audit Event requirement is clear.

## Scan and Audit Accountability Review

Use [scan-audit-accountability.md](./scan-audit-accountability.md) to validate
US3.

1. Confirm ID prefixes IE-, SE-, AE-, and TC- are defined.
2. Verify Identity Evidence includes subject type, school account, evidence
   type, status, verifier, and verification time.
3. Confirm NFC capture includes capture source, captured time, received time,
   identity evidence reference, offline indicator, and review outcome.
4. Confirm QR fallback must meet the same accountability expectations as NFC.
5. Review Offline Sync states: Pending, Synced, Duplicate, Conflict, Rejected,
   and Deferred.
6. Confirm duplicate and conflict review outcomes preserve original evidence
   and record Audit Events.

## Ready for Review

Use this final checklist before stakeholder review:

- [ ] US1 foundation blueprint covers all six Phase 0 foundation areas.
- [ ] US2 tenant and access boundaries identify actor category, boundary,
  approval path, and audit evidence for each sensitive sample.
- [ ] US3 scan and audit accountability identifies evidence and review outcome
  for NFC, QR fallback, offline, duplicate, and conflict samples.
- [ ] Traceability matrix contains every Phase 1 candidate spec from `PLAN.md`
  and at least five later-phase samples.
- [ ] Contracts contain concrete examples for foundation decisions, tenant
  configuration changes, and scan/audit evidence.
- [ ] No unresolved clarification markers remain.
- [ ] Review log records the 60-minute dry-run result for US1, US2, and US3.
