# Wallet Verification

| Area | Command | Result |
|------|---------|--------|
| API build | `dotnet build apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj` | Passed, 0 warnings, 0 errors |
| API tests | `dotnet test apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj -v minimal` | Passed, 153 tests |
| Admin web tests | `npm test -- --run` from `apps/admin-web` | Passed, 34 test files / 34 tests |
| Admin web build | `npm run build` from `apps/admin-web` | Passed, static routes generated for `/wallet` and `/guardian/wallet` |
| Mobile tests | `flutter test` from `apps/mobile` | Passed, including wallet POS offline queue and repository tests |

`npm install` was required in the fresh worktree before Vitest could run. NPM audit reported 7 moderate vulnerabilities in the installed dependency tree; no force upgrade was applied because that can introduce breaking changes outside Phase 4 scope.
