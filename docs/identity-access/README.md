# Identity Access Implementation

Phase 1 Identity & Access builds the runtime foundations for school-scoped student profiles, guardian links, NFC card credentials, QR fallback credentials, role administration, permission enforcement, audit events, and access decisions.

Primary artifacts:

- Spec: `specs/002-identity-access/spec.md`
- Plan: `specs/002-identity-access/plan.md`
- Tasks: `specs/002-identity-access/tasks.md`
- Data model: `specs/002-identity-access/data-model.md`
- Quickstart: `specs/002-identity-access/quickstart.md`
- Contracts: `specs/002-identity-access/contracts/`

Runtime surfaces:

- Backend API: `apps/api/src/SafeSchool.Api/Features/IdentityAccess`
- Admin web: `apps/admin-web/src/app/(school)/identity-access`
- Mobile credential support: `apps/mobile/lib/features/identity_access`
- Tests: `apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess`, `apps/admin-web/tests/identity-access`, `apps/mobile/test/features/identity_access`

Phase 1 intentionally does not implement attendance generation, campus access outcomes, bus boarding, wallet payments, documents, communications, or search. Credentials are exposed only as current identity evidence for later phases.
