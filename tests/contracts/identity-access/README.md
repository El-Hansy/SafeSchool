# Identity Access Contract Tests

This folder indexes the Phase 1 Identity & Access contracts used by the backend, admin web, and mobile tests.

- Student profile contract: `specs/002-identity-access/contracts/student-profile.md`
- Permission enforcement contract: `specs/002-identity-access/contracts/permission-enforcement.md`
- Guardian linking contract: `specs/002-identity-access/contracts/guardian-linking.md`
- Credential lifecycle contract: `specs/002-identity-access/contracts/credential-lifecycle.md`

The executable tests live next to each runtime surface:

- API contract coverage: `apps/api/tests/SafeSchool.Api.Tests/Features/IdentityAccess`
- Admin route coverage: `apps/admin-web/tests/identity-access`
- Mobile snapshot coverage: `apps/mobile/test/features/identity_access`
- Cross-surface e2e scenario coverage: `tests/e2e/identity-access`
