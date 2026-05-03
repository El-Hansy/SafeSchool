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
