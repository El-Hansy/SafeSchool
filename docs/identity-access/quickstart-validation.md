# Quickstart Validation

Validation date: 2026-05-06

## Commands

| Command | Result |
|---------|--------|
| `dotnet test apps/api/tests/SafeSchool.Api.Tests --no-restore` | Passed: 23 tests |
| `npm test --prefix apps/admin-web` | Passed: 4 tests |
| `flutter test` from `apps/mobile` | Passed: 3 tests |
| `apps/admin-web/node_modules/.bin/vitest run tests/e2e/identity-access/identity-access.e2e.spec.ts --root .` | Passed: 1 test |

## Scenario Evidence

| Quickstart area | Evidence | Status |
|-----------------|----------|--------|
| Student Profile | Student profile domain, duplicate detector, contract, integration, and admin route tests pass. | Pass |
| Permission Enforcement | Permission guard/evaluator, role assignment, access decision, and audit writer tests pass. | Pass |
| Guardian Linking | Guardian record, guardian link lifecycle, visibility integration, and admin route tests pass. | Pass |
| Credentials | NFC/QR credential domain, status snapshot integration, admin route, and mobile cache/snapshot tests pass. | Pass |
| Cross-surface e2e scenarios | E2E scenario spec validates tenant isolation, disabled capabilities, denied mutation evidence, guardian visibility, and credential status scope. | Pass |

## Limitations

- A live API host, PostgreSQL database, and browser/mobile device session were not started in this workspace.
- The validation is automated implementation-level coverage rather than a deployed-environment smoke test.
- NFC/QR secrets are not exercised against real hardware; Phase 1 only exposes credential status as current identity evidence.
