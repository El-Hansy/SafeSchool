# AttendanceAccess Security Review

- Tenant context is required for all school-scoped routes.
- Feature gates cover gate scanning, attendance generation, entry/exit
  notifications, and anomaly detection.
- Permission constants cover gate, scan, attendance, notification, guardian,
  anomaly, and audit workflows.
- Cross-tenant credentials and records are denied or hidden.
- Offline sync uses client scan and batch identifiers to prevent duplicates.
- Sensitive actions write access decisions or audit evidence.

