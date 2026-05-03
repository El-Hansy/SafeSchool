# Quickstart: Phase 2 Attendance & Campus Access

Use this quickstart to validate that the Phase 2 planning package is complete
before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for Phase 2 scope and user stories.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, scan evidence, audit, and offline sync foundations.
- Read Phase 1 artifacts under `specs/002-identity-access/` for student
  profiles, guardian links, credentials, permissions, and credential status
  snapshots.
- Confirm `.specify/feature.json` points to
  `specs/003-attendance-campus-access`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves all planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes gates, scan points, scan
   events, campus access decisions, attendance sessions, attendance records,
   notification records, anomalies, manual reviews, offline sync batches, audit
   events, and feature settings.
3. Confirm [contracts/gate-scan-flow.md](./contracts/gate-scan-flow.md) covers
   gate management, scan point authorization, online scans, offline scan sync,
   idempotency, and scan traceability.
4. Confirm [contracts/attendance-generation.md](./contracts/attendance-generation.md)
   covers sessions, attendance generation, status rules, corrections, and
   summaries.
5. Confirm [contracts/entry-exit-notification.md](./contracts/entry-exit-notification.md)
   covers guardian eligibility, suppression, visibility, and withdrawal.
6. Confirm [contracts/anomaly-detection.md](./contracts/anomaly-detection.md)
   covers detection runs, anomaly records, assignment, resolution, dismissal,
   reopening, and audit evidence.

## Implementation Order for Later Tasks

1. Establish shared tenant, capability, permission, credential snapshot,
   idempotency, and audit guards for AttendanceAccess workflows.
2. Create Gate and Scan Point models, validation rules, migrations, contracts,
   and school administration workflows.
3. Create mobile NFC/QR scan capture with offline queue storage, source
   metadata, local time evidence, and retry-safe sync.
4. Create scan validation and reconciliation behavior for active credentials,
   gate authorization, duplicate detection, delayed sync, and campus access
   decisions.
5. Create Attendance Day or Session and Attendance Record behavior, including
   generation from accepted scans, expected population checks, and manual
   correction history.
6. Create Entry/Exit Notification Record behavior for guardian link eligibility,
   visibility, suppression, withdrawal, and guardian-facing read scope.
7. Create Attendance Anomaly detection and reviewer workflow for missing,
   duplicate, invalid, out-of-order, conflicting, late, early, and delayed
   offline cases.
8. Add web review journeys for scans, attendance, notification records,
   anomalies, traceability, and corrections.
9. Complete unit, integration, contract, authorization, tenant-isolation,
   audit, mobile offline, and critical UI journey tests.

## Validation Scenarios

### Gate Scan Flow

- Create an active gate and scan point within one school account.
- Record an online entry scan with an active NFC credential and confirm an
  allowed campus access decision is created.
- Record an exit scan through a different authorized gate in the same school
  account and confirm campus state changes remain traceable.
- Attempt scans using expired, suspended, revoked, replaced, unknown, duplicate,
  and cross-school credentials and confirm they are denied or flagged without
  normal attendance.
- Submit the same online scan or offline scan batch twice and confirm the retry
  returns the same outcome without duplicate attendance.
- Submit delayed offline scans and confirm local scan time, received time,
  scan source, and reconciliation outcome are preserved.

### Attendance Generation

- Create an active attendance session for an expected student population.
- Generate attendance from accepted entry scans and confirm Present and Late
  statuses are produced according to school account rules.
- Generate attendance for an expected student without accepted entry evidence
  and confirm Absent or Needs Review according to school account rules.
- Correct an attendance record with an authorized reviewer and reason, then
  confirm original scan-based status remains reviewable.
- Re-run generation for the same session and confirm current attendance records
  are updated without duplicates.

### Entry/Exit Notifications

- Capture an eligible entry scan for a student with an approved active guardian
  link and confirm a guardian-visible notification record is created.
- Capture a scan for a guardian link that is pending, suspended, expired,
  removed, rejected, or outside access scope and confirm visibility is
  suppressed with a reason.
- Correct or reject a scan after review and confirm guardian-visible status is
  updated or withdrawn according to school account rules.
- Confirm a guardian cannot view another student's entry/exit records.

### Attendance Anomalies

- Create missing entry, missing exit, duplicate scan, invalid credential,
  out-of-order scan, conflicting campus state, delayed offline conflict, late
  arrival, and early exit examples.
- Run anomaly detection and confirm anomaly type, severity, status, evidence,
  and affected student are recorded.
- Assign, resolve, dismiss, and reopen anomalies with reviewer permissions and
  reasons.
- Confirm anomaly resolution does not silently change attendance; attendance
  changes require a correction record.

## Expected Verification Commands

The implementation repository should provide equivalent commands once runtime
manifests exist:

```bash
dotnet test apps/api/tests/SafeSchool.Api.Tests
npm test --prefix apps/admin-web
flutter test apps/mobile
```

Contract and end-to-end validation should cover:

```bash
dotnet test apps/api/tests/SafeSchool.Api.Tests --filter AttendanceAccess
npm test --prefix apps/admin-web -- attendance-access
flutter test apps/mobile/test/features/attendance_access
```

## Readiness Criteria

- Every Phase 2 user story can be implemented independently.
- Every sensitive action has tenant, capability, role, permission, idempotency,
  and audit expectations.
- Every public route in the contracts has denial behavior for tenant mismatch,
  disabled capability, missing permission, invalid credential, and audit
  failure where applicable.
- Offline NFC/QR scan continuity, duplicate prevention, delayed sync, and clock
  evidence are planned before task generation.
- Guardian notification records remain limited to entry/exit outcomes and do
  not implement general messaging or broadcasts.
- No Phase 3 transport, Phase 4 wallet, Phase 6 request, Phase 9 messaging, or
  Phase 11 dashboard behavior is implemented as part of Phase 2.
