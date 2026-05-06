# Phase 0 Platform Foundations

This package is the reviewable baseline for Phase 0. It defines the shared
foundation that later School NFC specs must reference before they introduce
runtime web, backend, mobile, database, or product workflow behavior.

## Artifact Index

| Artifact | Purpose |
|----------|---------|
| [README.md](./README.md) | Entry point, artifact index, glossary, and scope guard. |
| [spec.md](./spec.md) | Phase 0 user stories, requirements, edge cases, entities, and success criteria. |
| [plan.md](./plan.md) | Implementation plan, constitution checks, structure decision, and scope constraints. |
| [research.md](./research.md) | Planning decisions and alternatives for Phase 0 boundaries. |
| [data-model.md](./data-model.md) | Shared business entities that later specs reference. |
| [quickstart.md](./quickstart.md) | Reviewer workflow and validation checklist. |
| [contracts/foundation-decision.md](./contracts/foundation-decision.md) | Required record shape for foundation decisions. |
| [contracts/tenant-configuration.md](./contracts/tenant-configuration.md) | Required record shape for tenant configuration changes. |
| [contracts/scan-audit-event.md](./contracts/scan-audit-event.md) | Required scan and audit evidence for later scan workflows. |
| [foundation-areas.md](./foundation-areas.md) | Six foundation areas with owners, boundaries, dependencies, and acceptance criteria. |
| [tenant-access-boundaries.md](./tenant-access-boundaries.md) | Tenant boundary, actor, permission, and configuration approval rules. |
| [scan-audit-accountability.md](./scan-audit-accountability.md) | Identity evidence, NFC/QR scan, offline sync, duplicate/conflict, and audit categories. |
| [traceability-matrix.md](./traceability-matrix.md) | Later-phase samples mapped back to Phase 0 decisions. |
| [review-log.md](./review-log.md) | Constitution, readiness, and validation evidence. |

## Canonical Glossary

| Term | Canonical Meaning |
|------|-------------------|
| School Account | The tenant boundary for one school or school organization. Every tenant-owned record belongs to exactly one school account unless a later spec documents explicit platform authority. |
| Feature Capability | A named tenant capability that can be proposed, enabled, disabled, suspended, retired, or reviewed for a school account. |
| Actor Category | Shared access vocabulary for Platform Owner, School Administrator, Staff Member, Guardian, Student, and Reviewer. Actor categories do not grant access by themselves. |
| Permission Rule | A documented rule that states which actor category may perform or approve a sensitive action, within which boundary, and with what review evidence. |
| Identity Evidence | Evidence connecting a person, credential, scan method, fallback method, device, or reviewer to a school account. |
| Scan Event | A captured NFC or QR identity interaction, including timing, source, offline status, sync status, duplicate/conflict state, and review outcome. |
| Audit Event | Reviewable evidence showing who did what, for which school account, when, why, and with what review status. |
| Configuration Change | A traceable change to a school account boundary, feature capability, permission rule, identity evidence rule, audit rule, or foundation decision. |
| Foundation Decision | A documented Phase 0 rule or boundary that later specs must reference or formally amend before planning. |

## Phase 0 Scope Guard

Phase 0 is a documentation and governance package only. It does not implement
or deliver the following Phase 1 and later workflows from `spec.md` FR-012:

| Excluded Workflow | Owning Later Phase |
|-------------------|--------------------|
| Student profile management | Phase 1: Identity & Access |
| Guardian linking | Phase 1: Identity & Access |
| Card provisioning workflows | Phase 1: Identity & Access |
| Attendance generation | Phase 2: Attendance & Campus Access |
| Transport tracking | Phase 3: Transport & Bus Tracking |
| Wallet payments | Phase 4: Wallet & Payments |
| Learning features | Phase 5: Learning & Engagement |
| Requests | Phase 6: Requests & Permissions |
| Medical workflows | Phase 7: Medical & Emergency |
| Complaints | Phase 8: Complaints & Escalations |
| Communications | Phase 9: Communication & Notifications |
| Documents | Phase 10: Documents & Search |
| Search | Phase 10: Documents & Search |
| Dashboards | Phase 11: Admin, Audit & Observability |

Later specs may reference Phase 0 decisions. They must not treat this package
as approval to implement a later workflow without its own reviewed spec.
