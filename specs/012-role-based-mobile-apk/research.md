# Research: Phase 12 Role-Based Mobile App & APK Release

## Decision: One Role-Based Mobile App

**Decision**: Build and release one mobile app whose navigation, summaries, and
actions are resolved from tenant, role, permission, guardian link, student
self-scope, staff duty assignment, and feature availability.

**Rationale**: The product direction explicitly rejects a separate parent app.
One app avoids duplicate releases, duplicated authentication, duplicated
notification behavior, and inconsistent support evidence. Role workspaces give
guardians, students, drivers, cashiers, staff, administrators, and support users
distinct experiences while preserving one security model.

**Alternatives considered**:
- Separate guardian app: rejected because guardian access must be a role and
  permission inside the same app.
- Separate staff operations app: rejected because shared auth, releases,
  localization, support diagnostics, and cross-role accounts would duplicate
  work.
- Demo-only representative workspaces: rejected by clarification; Phase 12
  requires production-complete coverage for every listed role.

## Decision: Production-Complete Coverage For All Listed Roles

**Decision**: Plan full critical journeys for guardian, student, transport
driver, gate/access staff, canteen cashier, teacher, medical staff, complaint
handler, communication sender, document administrator, school administrator,
and platform support roles.

**Rationale**: The clarification selected production-complete coverage rather
than a sales-only or guardian-first scope. Tasks must therefore be split by
role and independently validated so production readiness is measurable per
role.

**Alternatives considered**:
- Guardian/student first: rejected because it would leave operational staff
  roles out of Phase 12.
- Five representative workspaces: rejected because it would satisfy demo needs
  but not the selected production-complete scope.

## Decision: Tenant-Scoped Mobile Permission Resolution

**Decision**: Resolve mobile visibility through active tenant, active role,
permission grant, inherited feature flags, approved student links, staff duty
assignments, release audience, and source-domain authorization.

**Rationale**: The constitution requires backend tenant, feature, role, and
permission enforcement. Mobile UI gates improve usability but cannot be treated
as authorization. Central mobile permission resolution gives consistent
answers for sign-in, navigation, deep links, cached states, notifications, and
support diagnostics.

**Alternatives considered**:
- Client-only role hiding: rejected because it does not provide a security
  boundary.
- Domain-specific visibility only: rejected because it cannot answer app shell,
  release audience, device session, and cross-feature notification decisions.

## Decision: Flutter Localization With Arabic/English And RTL

**Decision**: Arabic and English are required across all role workspaces,
forms, notifications, empty states, errors, release notes, and support messages,
with right-to-left layout validation for Arabic.

**Rationale**: Clarification selected Arabic/English with RTL support. This
affects navigation, form density, notification text, validation messages, and
demo quality, so it must be planned before implementation tasks are generated.

**Alternatives considered**:
- English only: rejected by clarification.
- Tenant-configurable language starting with English only: rejected because it
  defers layout and translation risk.
- More than two languages in Phase 12: deferred until the APK release scope is
  stable.

## Decision: Controlled APK Release Without App-Store Dependency

**Decision**: Manage APK releases through platform metadata, release audience,
version status, integrity evidence, release notes, controlled distribution
instructions, and install/version telemetry. APK artifacts are stored using the
project's low-cost object storage pattern.

**Rationale**: The spec scopes an Android production or pilot package with
controlled distribution and does not require public app-store release. Keeping
release management in the platform supports demo/pilot installation, rollback,
support review, and obsolete-version blocking without introducing a new release
management service.

**Alternatives considered**:
- Public app-store release: deferred because the spec does not require it and
  it adds external review and account workflow dependencies.
- Third-party mobile distribution service: rejected for Phase 12 because it
  adds operating cost and a new dependency before there is evidence it is
  necessary.
- Local informal build sharing: rejected because it lacks version, audience,
  revocation, integrity, and support evidence.

## Decision: Source-Domain Ownership For Mobile Actions

**Decision**: Mobile workspaces may initiate source-domain actions, but the
owning module remains authoritative for validation, side effects, audit
semantics, and outcome state.

**Rationale**: Phase 12 is an app access and release phase. It must not create
parallel implementations for attendance, access, transport, wallet, learning,
requests, medical, complaints, communications, documents, or administration.
Routing through owning workflows preserves existing invariants and audit
boundaries.

**Alternatives considered**:
- Mobile-specific duplicate workflows: rejected because they would diverge from
  domain rules and increase data integrity risk.
- Read-only mobile app: rejected because staff operational roles need permitted
  actions such as scans, POS decisions, and emergency capture.

## Decision: Offline Queue Limited To Approved Offline Workflows

**Decision**: Maintain offline queues only for workflows already approved for
offline continuity, including NFC/QR attendance or access scans, transport
boarding/drop scans, wallet POS, and emergency capture. Online-only workflows
fail safely with clear guidance.

**Rationale**: The constitution requires offline integrity for scan-dependent
flows, but the spec also prevents Phase 12 from creating new source-domain
outcomes directly. Limiting offline queues avoids accidental expansion of
offline behavior into workflows that need current authorization or server-side
confirmation.

**Alternatives considered**:
- Offline everything: rejected because it would create conflict and privacy
  risks for documents, communications, requests, complaints, and admin actions.
- Online-only mobile: rejected because it would break required scan continuity
  and POS/emergency operational use cases.

## Decision: Device Session And Support Evidence First-Class

**Decision**: Track device sessions, version checks, installs/upgrades,
denied-access attempts, role selection, sync outcomes, language context, and
support diagnostics as first-class mobile evidence.

**Rationale**: APK rollout without support evidence is hard to operate.
Support users need to identify version, install status, sign-in result, denied
access, and sync state within 2 minutes. Device evidence also supports
revocation, shared-device privacy, and obsolete-version handling.

**Alternatives considered**:
- Server logs only: rejected because logs are not tenant/support scoped and are
  difficult for authorized support users to review.
- Client-only diagnostics: rejected because lost devices, reinstall events,
  and denied-access security reviews require backend evidence.

## Decision: Admin Web Controls For Mobile Access And Releases

**Decision**: Add school/support web controls for mobile permissions, release
audiences, APK release status, install evidence, version blocking, support
diagnostics, and demo seed access.

**Rationale**: Mobile users should not self-administer sensitive access.
Schools and platform support need controlled operational surfaces to manage who
can use the app, which release they receive, and how support reviews issues.

**Alternatives considered**:
- Backend-only configuration: rejected because it prevents school operators
  from managing mobile rollout.
- Mobile self-service role assignment: rejected because it violates role and
  permission governance.
