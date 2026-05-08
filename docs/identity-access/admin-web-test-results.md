# Admin Web Test Results

Dependency setup:

```bash
npm install --prefix apps/admin-web
```

Result:

```text
added 83 packages, and audited 84 packages in 45s
7 moderate severity vulnerabilities
```

Test command:

```bash
npm test --prefix apps/admin-web
```

Result:

```text
Test Files  4 passed (4)
Tests       4 passed (4)
```

Notes:

- The first test attempt failed because `vitest` was not installed.
- The admin tests validate Identity Access route contracts for student profiles, access control, guardian linking, and credential lifecycle.
- `npm audit` follow-up is recommended before production release because install reported seven moderate vulnerabilities in the dependency tree.
