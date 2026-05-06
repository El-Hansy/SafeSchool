# Tenant and Access Boundaries

## Actor Category ID Rules

| Actor Category ID | Actor Category | Stable Use |
|-------------------|----------------|------------|
| AC-001 | Platform Owner | Platform-wide governance, foundation amendments, and explicitly approved cross-school access. |
| AC-002 | School Administrator | School-scoped administration and tenant configuration requests. |
| AC-003 | Staff Member | School-scoped work limited by assignment, role, and later feature permission. |
| AC-004 | Guardian | Delegated access through later approved guardian relationships. |
| AC-005 | Student | Self-scoped access where the school account and later feature allow it. |
| AC-006 | Reviewer | Assigned product, security, compliance, or operations review authority. |

## School Account Boundary

| Rule | Requirement | Review Evidence |
|------|-------------|-----------------|
| Default isolation | Data and actions stay inside one School Account unless explicitly approved otherwise. | Foundation Decision FD-002 and the relevant Permission Rule. |
| Internal campus scoping | Campuses, divisions, classrooms, buses, or departments may narrow access inside the same School Account. | Scope note on the tenant-owned record and audit event for sensitive action. |
| Cross-school access | Cross-school access is prohibited unless a Platform Owner-approved Permission Rule exists. | Configuration Change with reason, approver, effective time, and Audit Event. |
| Later spec boundary | Each later spec identifies its tenant scope and whether records are school-scoped, self-scoped, delegated, or platform-scoped. | Traceability row linking the later requirement to FD-002 and FD-003. |

## Tenant-Owned Records

Every later tenant-owned record must state the metadata below before runtime
implementation begins.

| Metadata | Requirement | Review Owner | Audit Evidence |
|----------|-------------|--------------|----------------|
| Owner | Exactly one School Account unless explicit platform scope is documented. | Security Reviewer | Audit Event for creation or boundary change. |
| Creation evidence | Actor category, source, created time, and reason where sensitive. | Feature Review Owner | Audit Event or source-domain creation evidence. |
| Update evidence | Actor category, update time, changed fields summary, and reason where sensitive. | Feature Review Owner | Audit Event or Configuration Change. |
| Review owner | Actor category responsible for approving access or configuration changes. | Product and Security Reviewers | Review-log row or approval record. |
| Audit evidence | Sensitive reads, writes, approvals, denials, scan reconciliation, and configuration changes must be reviewable. | Compliance Reviewer | Audit Event category from FD-005. |

## Actor Categories

| Actor Category ID | Actor Category | Default Boundary | Can View | Can Change | Can Approve Access | Evidence Required |
|-------------------|----------------|------------------|----------|------------|--------------------|-------------------|
| AC-001 | Platform Owner | Platform-wide with explicit purpose | Foundation and cross-school governance records | Foundation decisions and platform configuration | Cross-school and foundation amendments | Configuration Change and Audit Event |
| AC-002 | School Administrator | One School Account | School-scoped configuration and operational records | Tenant configuration requests and school settings | School-scoped configuration when policy allows | Audit Event and approval trace |
| AC-003 | Staff Member | One School Account and assignment | Assigned student, class, transport, learning, finance, medical, complaint, or communication records as later specs allow | Assigned workflow records as later specs allow | Usually no, unless role-specific later spec permits | Permission Rule and source assignment |
| AC-004 | Guardian | Delegated relationship | Guardian-visible student records after Phase 1 relationship approval | Guardian-submitted records where later specs allow | No platform or school-wide approval | Guardian relationship evidence and Audit Event |
| AC-005 | Student | Self-only | Student-visible self records where school capability allows | Student-submitted records where later specs allow | No approval authority | Identity Evidence and later self-scope rule |
| AC-006 | Reviewer | Assigned review scope | Records under explicit product, security, compliance, or operations review | Review status, comments, or decisions within assignment | Review decisions inside assignment | Review assignment and Audit Event |

## Permission Rules

| Permission Rule | Actor Category | Allowed Action | Boundary Type | Approval Requirement | Review Evidence |
|-----------------|----------------|----------------|---------------|----------------------|-----------------|
| PR-001 | Platform Owner | Amend accepted Foundation Decision | Platform-scoped | Product and Engineering Leadership review | Configuration Change, Audit Event, updated foundation artifact |
| PR-002 | School Administrator | Request tenant feature availability change | School-scoped | Platform Owner or assigned Reviewer approval before application | Tenant Configuration Change and Audit Event |
| PR-003 | Staff Member | Perform later sensitive school workflow | School-scoped and assignment-scoped | Defined by the later feature spec | Permission Rule, assignment evidence, Audit Event |
| PR-004 | Guardian | Access delegated student-visible records | Delegated | Approved guardian relationship from Phase 1 | Relationship evidence and Audit Event |
| PR-005 | Student | Access enabled self-scope records | Self-scoped | School capability and later feature self-scope rule | Identity Evidence and Audit Event where sensitive |
| PR-006 | Reviewer | Review scan, access, configuration, or audit evidence | Assigned review scope | Review assignment required | Review assignment, decision note, Audit Event |
| PR-007 | Platform Owner | Approve temporary cross-school access | Platform-scoped | Explicit purpose and time-bound approval required | Configuration Change and Audit Event |

## Configuration Change Approval

1. **Requested**: An actor submits a Configuration Change with school account,
   change type, prior value, new value, reason, requested by, and effective
   target.
2. **Reviewed**: The required approver checks tenant boundary, actor category,
   impacted capability, sensitive-action risk, and later-feature dependency.
3. **Approved**: The approver records approval, effective time, and audit event.
4. **Rejected**: The reviewer records rejection reason; no partial change is
   applied.
5. **Applied**: The approved change becomes active and remains linked to its
   audit event.
6. **Rolled Back**: A follow-up Configuration Change records rollback reason,
   previous value, new value, approver, effective time, and audit event.

## Edge Cases

| Edge Case | Handling |
|-----------|----------|
| Multiple campuses | Treat campuses as internal scope under one School Account; do not allow cross-school leakage through campus labels. |
| Multi-role users | Evaluate all roles through explicit Permission Rules and record the effective permission source for sensitive actions. |
| Disabled capabilities | Later specs must show the capability as unavailable and must not silently perform the workflow. |
| Later-phase conflicts | The later spec must conform to Phase 0 or document an approved amendment before planning. |
| Cross-school emergency support | Requires Platform Owner approval, time-bounded purpose, reviewer evidence, and audit event. |
| Guardian relationship changes | Later Phase 1 relationship evidence controls delegated access; Phase 0 only defines the category and evidence requirement. |
