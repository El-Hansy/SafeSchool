# Transport Security Review

- Tenant boundaries enforced through school account scoped routes.
- Backend feature gates cover route/stop management, bus assignment, boarding/drop scans, live tracking, ETA, and notifications.
- Permission catalog includes staff, reviewer, driver, attendant, guardian visibility, rule, and audit scopes.
- Guardian views require approved active guardian link evidence and never expose unrelated students.
- Offline sync uses caller-stable scan, batch, request, and location identifiers.
- Detailed location retention is fixed at 30 days unless review hold is represented.
