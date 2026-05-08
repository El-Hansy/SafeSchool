# AttendanceAccess OpenAPI Notes

Phase 2 routes are mounted under `/api/v1/schools/{schoolAccountId}/attendance-access`.

- Gates: create, update, list
- Scan points: create, update
- Scans: online scan, offline sync, list, detail, trace
- Attendance: sessions, generation, records, correction, summaries
- Notifications: school review and guardian-visible entry/exit records
- Anomalies: detection runs, list, assign, resolve, dismiss, reopen

All routes are tenant-scoped and are expected to enforce capability, permission,
idempotency, audit, and no cross-tenant leakage behavior.

