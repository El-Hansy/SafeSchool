# Traceability Matrix

## Row Guidance

Each row maps a later-phase sample to the Phase 0 rule set that governs it.
Use the columns as follows:

- **Later Phase Requirement**: Name the candidate feature, workflow, or
  sensitive action.
- **Implementation Phase**: Use the phase name from `PLAN.md`.
- **Feature Module**: Name the module or spec area from the later phase.
- **Foundation Area**: Use one of the six Phase 0 areas and include the
  Foundation Decision ID where possible.
- **Tenant Boundary**: State school-scoped, platform-scoped, self-scoped,
  delegated, assignment-scoped, or internal campus scope.
- **Actor/Permission Rule**: Identify the actor category or Permission Rule.
- **Feature Capability**: Name the capability or say "declared by later spec".
- **Scan/Audit Evidence**: Identify required Identity Evidence, Scan Event,
  Audit Event, Configuration Change, or none.
- **Observability Expectation**: State the minimum review evidence.
- **Source Link**: Link to the relevant artifact or `PLAN.md`.

| Later Phase Requirement | Implementation Phase | Feature Module | Foundation Area | Tenant Boundary | Actor/Permission Rule | Feature Capability | Scan/Audit Evidence | Observability Expectation | Source Link |
|-------------------------|----------------------|----------------|-----------------|-----------------|-----------------------|-------------------|---------------------|----------------------------|-------------|
| Student Profile Spec | Phase 1: Identity & Access | Student Profile | FD-003 Identity & Access Model | School-scoped | AC-002, AC-003, PR-003 | Declared by later spec | Identity Evidence, Audit Event | Profile-sensitive changes are auditable and school-scoped. | [PLAN.md](../../PLAN.md) |
| Guardian Linking Spec | Phase 1: Identity & Access | Guardian Linking | FD-003 Identity & Access Model | Delegated | AC-004, PR-004 | Declared by later spec | Identity Evidence, Audit Event | Relationship approval evidence controls guardian scope. | [PLAN.md](../../PLAN.md) |
| NFC Card Provisioning Spec | Phase 1: Identity & Access | NFC Card Provisioning | FD-004 NFC & QR Integration | School-scoped | AC-002, AC-003, PR-003 | Declared by later spec | Identity Evidence, Audit Event | Card evidence is verified, rejected, or revoked with audit. | [PLAN.md](../../PLAN.md) |
| QR Identity Fallback Spec | Phase 1: Identity & Access | QR Identity Fallback | FD-004 NFC & QR Integration | School-scoped | AC-002, AC-003, PR-003 | Declared by later spec | Identity Evidence, Audit Event | QR evidence meets NFC-equivalent accountability. | [PLAN.md](../../PLAN.md) |
| Role-Based Access Spec | Phase 1: Identity & Access | Role-Based Access | FD-003 Identity & Access Model | School-scoped and platform-scoped where approved | AC-001, AC-002, AC-006, PR-001, PR-002 | Declared by later spec | Permission Rule, Audit Event | Sensitive permission changes record actor, reason, and result. | [PLAN.md](../../PLAN.md) |
| Permission Enforcement Spec | Phase 1: Identity & Access | Permission Enforcement | FD-003 Identity & Access Model | School-scoped, self-scoped, delegated, assignment-scoped | PR-001 through PR-007 | Declared by later spec | Permission Rule, Audit Event | Denied and approved sensitive actions are reviewable. | [PLAN.md](../../PLAN.md) |
| Gate Scan Flow | Phase 2: Attendance & Campus Access | Gate Scan Flow | FD-004 NFC & QR Integration | School-scoped, internal campus scope | AC-003, PR-003 | campus-access.gate-scan | Identity Evidence, Scan Event, Audit Event | Offline, duplicate, and conflict states are reviewable before outcomes. | [PLAN.md](../../PLAN.md) |
| QR Identity Fallback at Gate | Phase 2: Attendance & Campus Access | Gate Scan Flow | FD-004 NFC & QR Integration | School-scoped | AC-003, PR-003 | campus-access.qr-fallback | Identity Evidence, Scan Event, Audit Event | QR fallback uses same accountability as NFC. | [PLAN.md](../../PLAN.md) |
| Bus Boarding Scan | Phase 3: Transport & Bus Tracking | Boarding/Drop Scan | FD-004 NFC & QR Integration | School-scoped, route assignment scope | AC-003, PR-003 | transport.boarding-scan | Identity Evidence, Scan Event, Audit Event | Capture, sync, duplicate, and conflict review are preserved. | [PLAN.md](../../PLAN.md) |
| Wallet Transaction Approval | Phase 4: Wallet & Payments | Wallet Ledger | FD-002 Multi-Tenant Architecture | School-scoped | AC-002, AC-003, PR-003 | wallet.ledger | Audit Event | Payment actions remain tenant-scoped and sensitive changes are audited. | [PLAN.md](../../PLAN.md) |
| Emergency Access Review | Phase 7: Medical & Emergency | Emergency Access | FD-003 Identity & Access Model | School-scoped with assigned emergency review | AC-006, PR-006 | medical.emergency-access | Audit Event | Emergency access has reviewer evidence and reason. | [PLAN.md](../../PLAN.md) |
| Tenant Feature Configuration | Phase 11: Admin, Audit & Observability | Tenant Feature Configuration | FD-006 Feature Flag / Tenant Configuration | School-scoped | AC-002, AC-001, PR-002 | tenant.configuration | Configuration Change, Audit Event | Requested, approved, applied, rejected, or rolled back states are traceable. | [contracts/tenant-configuration.md](./contracts/tenant-configuration.md) |
| Admin Review Sample | Phase 11: Admin, Audit & Observability | Audit Trail | FD-005 Event & Audit Logging | School-scoped or platform-scoped with approval | AC-006, PR-006 | audit.review | Audit Event | Reviewers can inspect actor, action, result, reason, and follow-up. | [data-model.md](./data-model.md) |
