# Phase 0 Research: Platform Foundations

## Decision: Treat Phase 0 as a foundation planning package

**Rationale**: The feature spec explicitly limits scope to the six Phase 0
foundation areas from `PLAN.md` and excludes later product workflows. Planning
artifacts must therefore define shared decisions, data concepts, review
contracts, and validation steps rather than runtime product behavior.

**Alternatives considered**:
- Combine all phases into one platform plan: rejected because the user requested
  Phase 0 only and the constitution requires spec-first delivery by phase.
- Start implementation scaffolding now: rejected because `/speckit.plan` stops
  at planning artifacts and Phase 0 is not a user-facing release.

## Decision: Use school account as the default tenant boundary

**Rationale**: The spec defines school account isolation as the default boundary
for tenant-owned data and actions. This supports multi-school SaaS operation
while allowing later specs to add internal campus or operational scoping without
weakening the parent boundary.

**Alternatives considered**:
- Campus as top-level tenant: rejected because a school may contain multiple
  campuses or operational units and still require unified administration.
- Global records by default: rejected because it creates cross-school exposure
  risk and violates the constitution's tenant isolation rule.

## Decision: Define actor categories before detailed roles

**Rationale**: Phase 0 must support later identity and access specs without
implementing role-based access workflows. Actor categories give later specs a
stable vocabulary for platform owner, school administrator, staff member,
guardian, student, and reviewer responsibilities.

**Alternatives considered**:
- Fully enumerate every role now: rejected because detailed role and permission
  enforcement belongs to Phase 1.
- Leave actors to each later spec: rejected because inconsistent actor names
  would make tenant, audit, and configuration rules harder to review.

## Decision: Document feature capability as a tenant configuration record

**Rationale**: The constitution requires every feature to be tenant-aware and
feature-flag controlled. Phase 0 needs a shared concept for enabling, disabling,
reviewing, and changing capabilities per school account with traceable
ownership.

**Alternatives considered**:
- Boolean flags only: rejected because review ownership, reason, status, and
  history are required for operational accountability.
- Product-specific configuration per later feature only: rejected because later
  specs need a common baseline for feature availability.

## Decision: Use scan event accountability without defining business outcomes

**Rationale**: Phase 0 covers NFC and QR integration expectations, offline scan
evidence, sync status, duplicate detection, and review outcome. Attendance,
transport, wallet, and access business effects are explicitly later-phase
scope.

**Alternatives considered**:
- Define attendance generation now: rejected because it belongs to Phase 2.
- Define transport boarding behavior now: rejected because it belongs to Phase 3.
- Ignore scan data until later phases: rejected because scan identity and audit
  expectations must be shared across later flows.

## Decision: Model audit events as reviewable evidence categories

**Rationale**: Phase 0 must support traceability for identity, access,
configuration, feature availability, scan capture, scan reconciliation, and
administrative review. Defining event categories now allows later specs to add
specific event types consistently.

**Alternatives considered**:
- Treat audit logging as an implementation detail: rejected because auditability
  is a constitution principle and a Phase 0 foundation spec.
- Define every future event type now: rejected because later specs need room to
  define product-specific events.

## Decision: Keep reference frames as optional context only

**Rationale**: `PLAN.md` states the frames in `docs/references/frames/` are
visual inspiration only. Phase 0 can cite them as context for scan and school
operations, but they must not become acceptance criteria or fixed flows.

**Alternatives considered**:
- Derive foundation requirements from frames: rejected because product decisions
  and specs supersede visual inspiration.
- Ignore frames entirely: rejected because they may help reviewers understand
  real-world NFC and entry/exit context.

## Decision: No unresolved technical clarifications remain

**Rationale**: The current constitution provides the relevant technical baseline
for future implementation, and the Phase 0 feature is a documentation/design
package. Unknown implementation details are deliberately deferred to later
feature plans where runtime behavior is in scope.

**Alternatives considered**:
- Add clarification markers for future stack choices: rejected because the
  constitution already defines the baseline and no runtime component is created
  here.
