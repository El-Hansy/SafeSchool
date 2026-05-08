# Backend Test Results

Command:

```bash
dotnet test apps/api/tests/SafeSchool.Api.Tests --no-restore
```

Result:

```text
Passed!  - Failed:     0, Passed:    23, Skipped:     0, Total:    23, Duration: 282 ms - SafeSchool.Api.Tests.dll (net9.0)
```

Notes:

- Package restore required network access for NuGet feeds and was completed before the final `--no-restore` run.
- The first sandboxed VSTest run could not bind its local test socket; the approved `dotnet test` execution path was used for the successful run.
- Coverage includes foundational tenant/capability/audit tests, student profile tests, permission enforcement tests, guardian linking tests, and credential lifecycle tests.
