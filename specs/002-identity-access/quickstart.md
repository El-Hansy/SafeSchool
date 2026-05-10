# Quickstart: Phase 1 Identity & Access

Use this quickstart to validate that the Phase 1 planning package is complete
before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for Phase 1 scope and user stories.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, scan evidence, and audit foundations.
- Confirm `.specify/feature.json` points to `specs/002-identity-access`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves all planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes student profile, guardian,
   credential, role, permission, access decision, audit, and feature setting
   entities.
3. Confirm [contracts/student-profile.md](./contracts/student-profile.md)
   covers student profile creation, duplicate checks, updates, deactivation,
   and history.
4. Confirm [contracts/guardian-linking.md](./contracts/guardian-linking.md)
   covers guardian records, guardian links, link state, access scope, and
   guardian-visible student lists.
5. Confirm [contracts/credential-lifecycle.md](./contracts/credential-lifecycle.md)
   covers NFC card credentials, QR fallback credentials, lifecycle state,
   retry-safe commands, and status snapshots.
6. Confirm [contracts/permission-enforcement.md](./contracts/permission-enforcement.md)
   covers roles, permissions, role assignments, access decisions, and audit
   events.

## Implementation Order for Later Tasks

1. Establish shared tenant, capability, permission, and audit guards for
   IdentityAccess workflows.
2. Create Student Profile data model, validation rules, duplicate detection,
   migrations, contracts, and administrator workflow.
3. Create Role, Permission, Role Assignment, Access Decision, and audit review
   behavior needed by every other Phase 1 story.
4. Create Guardian Record and Guardian Link lifecycle behavior with
   relationship-specific access scope.
5. Create NFC Card Credential and QR Fallback Credential lifecycle behavior,
   including status snapshots for later scan flows.
6. Add web administration journeys for student profiles, guardian links,
   credential lifecycle, role administration, and review history.
7. Add mobile credential support only for NFC/QR provisioning or validation
   workflows that require device-native behavior.
8. Complete unit, integration, contract, authorization, tenant-isolation,
   audit, and critical UI/mobile journey tests.

## Validation Scenarios

### Student Profile

- Create an active student profile with configured identifiers and confirm it
  is tenant-scoped.
- Attempt to create a second active profile with the same active identifiers in
  the same school account and confirm activation is blocked.
- Update controlled profile details and confirm previous values remain
  reviewable.
- Deactivate a profile and confirm guardian and credential history remains
  available to reviewers.

### Permission Enforcement

- Attempt profile, guardian, credential, role, and permission actions using
  actors with and without the required permission.
- Confirm unauthorized attempts are denied before data changes occur.
- Confirm denied sensitive actions create Access Decision and Audit Event
  evidence.
- Confirm a user with roles in multiple school accounts is evaluated only
  against the active school account.

### Guardian Linking

- Create a Guardian Record without a link and confirm it grants no student
  visibility.
- Create an Approved Guardian Link with explicit access scope and confirm the
  guardian sees only allowed student information.
- Suspend, expire, reject, and remove links and confirm access is denied with
  link-state evidence.

### Credentials

- Issue an NFC card to an active student and confirm one active card reference
  maps to one active student identity in the school account.
- Suspend, restore, replace, expire, and revoke credentials and confirm status
  snapshots reflect only current identity evidence.
- Create and rotate a QR fallback credential only when
  `identity.qr_fallback` is enabled.
- Disable QR fallback and confirm QR creation or use is blocked with feature
  capability evidence.

## Expected Verification Commands

The implementation repository should provide equivalent commands once runtime
manifests exist:

```bash
dotnet test apps/api/tests/SafeSchool.Api.Tests
npm test --prefix apps/admin-web
flutter test apps/mobile
```

Contract and end-to-end validation should cover:

```bash
dotnet test apps/api/tests/SafeSchool.Api.Tests --filter IdentityAccess
npm test --prefix apps/admin-web -- identity-access
flutter test apps/mobile/test/features/identity_access
```

## Readiness Criteria

- Every Phase 1 user story can be implemented independently.
- Every sensitive action has tenant, capability, role, permission, and audit
  expectations.
- Every public route in the contracts has a denial behavior for tenant mismatch,
  disabled capability, and missing permission.
- Credential lifecycle outputs enough status, validity, and revocation evidence
  for later offline scan plans.
- No Phase 2 or Phase 3 attendance, campus access, or transport outcome is
  implemented as part of Phase 1.
