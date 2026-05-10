# Contract and E2E Test Results

API contract coverage command: `dotnet test apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj --filter Transport`

Result: Passed. 53 Transport API tests passed.

E2E command: `./apps/admin-web/node_modules/.bin/vitest run tests/e2e/transport/transport.e2e.spec.ts`

Result: Passed. 1 e2e test file, 1 test, 0 failed.
