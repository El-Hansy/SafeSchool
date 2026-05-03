# Quickstart: Phase 3 Transport & Bus Tracking

Use this quickstart to validate that the Phase 3 planning package is complete
before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for Phase 3 scope, clarifications, and user stories.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, scan evidence, audit, and offline sync foundations.
- Read Phase 1 artifacts under `specs/002-identity-access/` for student
  profiles, guardian links, credentials, permissions, and credential status
  snapshots.
- Read Phase 2 artifacts under `specs/003-attendance-campus-access/` only to
  preserve campus attendance and access boundaries; Phase 3 transport must not
  generate attendance or campus access outcomes.
- Confirm `.specify/feature.json` points to
  `specs/004-transport-bus-tracking`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves all planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes vehicles, routes, stops,
   route stop sequences, student transport assignments, active trips, offline
   scan sync batches, boarding/drop scan events, location updates, ETA records,
   notification records, anomalies, manual reviews, rule settings, feature
   settings, and audit events.
3. Confirm [contracts/route-stop-management.md](./contracts/route-stop-management.md)
   covers route, stop, route version, stop sequence, and traceability behavior.
4. Confirm [contracts/bus-assignment.md](./contracts/bus-assignment.md) covers
   vehicles, student assignments, guardian transport plan visibility, and
   assignment traceability.
5. Confirm [contracts/boarding-drop-scan.md](./contracts/boarding-drop-scan.md)
   covers online scans, offline sync, idempotency, invalid credentials,
   assignment mismatch anomalies, and scan traceability.
6. Confirm [contracts/live-tracking.md](./contracts/live-tracking.md) covers
   trip lifecycle, authorized mobile tracking devices, active-trip overlap,
   guardian live-location windows, and retention.
7. Confirm [contracts/eta-calculation.md](./contracts/eta-calculation.md)
   covers ETA calculation, freshness, confidence, stale progress, and guardian
   ETA visibility.
8. Confirm [contracts/transport-notification.md](./contracts/transport-notification.md)
   covers guardian eligibility, suppression, withdrawal, visibility, and
   notification traceability.
9. Confirm [contracts/transport-anomaly-review.md](./contracts/transport-anomaly-review.md)
   covers anomaly detection, assignment, resolution, dismissal, reopening,
   manual corrections, and audit evidence.

## Implementation Order for Later Tasks

1. Establish shared tenant, capability, permission, Phase 1 identity/credential,
   guardian visibility, idempotency, retention, and audit guards for Transport
   workflows.
2. Create Transport Vehicle, Transport Route, Transport Stop, and Route Stop
   Sequence models, validation rules, migrations, contracts, and school
   administration workflows.
3. Create Student Transport Assignment behavior for active students, active
   routes, valid stops, validity dates, guardian visibility, and review history.
4. Create Transport Trip lifecycle behavior, including route version capture,
   assigned bus and staff, authorized mobile tracking device, and active-trip
   overlap prevention.
5. Create mobile NFC/QR boarding/drop scan capture with offline queue storage,
   source metadata, local time evidence, and retry-safe sync.
6. Create scan validation and reconciliation behavior for active credentials,
   active assignments, route/stop matching, invalid credentials, duplicate
   scans, delayed sync, and needs-review anomalies.
7. Create mobile live tracking submission from authorized staff or vehicle
   mobile devices and staff-visible active-trip progress.
8. Create guardian transport visibility windows: pickup ETA before boarding,
   exact live bus location only while the linked student is onboard, and drop
   status after drop.
9. Create ETA calculation behavior for active trips, ordered stops, trusted
   progress, freshness, confidence, and unavailable or review states.
10. Create Transport Notification Record behavior for guardian link eligibility,
    visibility, suppression, withdrawal, and corrected outcomes.
11. Create Transport Anomaly detection and reviewer workflow for missed,
    duplicate, invalid, out-of-order, delayed, stale, deviated, wrong-route,
    wrong-stop, and manual-review-required cases.
12. Create detailed location retention behavior: retain detailed active-trip
    location history for 30 days, then keep trip summaries and audit evidence
    unless an approved review hold applies.
13. Add web review journeys for routes, assignments, trips, scans, tracking,
    ETA, notification records, anomalies, traceability, and corrections.
14. Complete unit, integration, contract, authorization, tenant-isolation,
    audit, mobile offline/location, retention, and critical UI journey tests.

## Validation Scenarios

### Route & Stop Management

- Create an active route with at least five ordered pickup stops in one school
  account.
- Update one stop sequence and confirm the new route version is active while
  prior values remain reviewable.
- Attempt to activate a route without active stops and confirm activation is
  rejected with readiness reason.
- Attempt to use a cross-school route or stop and confirm the request is denied
  without exposing cross-tenant existence.

### Bus Assignment

- Create an active vehicle and assign ten active students to an active route
  with pickup and drop stops.
- Confirm guardian-visible plans are shown only for approved active guardian
  links with transport visibility scope.
- Attempt assignments for inactive students, inactive routes, suspended
  vehicles, invalid date ranges, and cross-school references and confirm each
  is prevented from activation.

### Boarding/Drop Scan

- Start an active trip and record an online boarding scan using an active NFC
  credential for an assigned student.
- Record a drop scan for the same student and confirm the transport status
  changes remain traceable.
- Attempt scans using expired, suspended, revoked, replaced, unknown,
  duplicated, and cross-school credentials and confirm they are denied or
  flagged without normal boarding/drop status.
- Scan a student without an active assignment or at the wrong route or stop and
  confirm a needs-review anomaly is created while normal status and guardian
  notification are withheld.
- Submit the same online scan or offline scan batch twice and confirm the retry
  returns the same outcome without duplicate transport status.
- Submit delayed offline scans and confirm local scan time, received time,
  source, trip, route, stop, and reconciliation outcome are preserved.

### Live Tracking

- Start one active trip for a route using a bus and authorized mobile tracking
  device, then submit accepted mobile location updates.
- Start a second active trip for the same route using a different bus and
  tracking device and confirm it is allowed.
- Attempt to start another active trip using a bus or tracking device already
  associated with an active trip and confirm it is blocked.
- Submit stale, untrusted, out-of-trip, or cross-school location updates and
  confirm they are suppressed or marked unavailable with reasons.
- Confirm staff can view current trip progress when authorized.

### Guardian Visibility

- Before accepted boarding, confirm the guardian sees pickup ETA and trip
  status without exact live bus location.
- After accepted boarding and before accepted or reviewed drop, confirm the
  guardian sees exact live bus location for only the linked student's active
  trip.
- After accepted or reviewed drop, confirm the guardian sees drop status and no
  longer sees exact live bus location.
- Confirm a guardian cannot view another student's transport plan, exact
  location, ETA, notification, or scan status.

### ETA Calculation

- Calculate ETA for upcoming stops on an active trip with current trusted
  progress and confirm ETA records include freshness and confidence.
- Make location evidence stale or inconsistent and confirm ETA is unavailable,
  stale, or needs review rather than unsupported.
- Change ETA materially for a linked student and confirm eligible guardian
  notification records are created or made visible according to settings.

### Transport Notifications

- Capture eligible boarding, drop, delay, and material ETA events for a student
  with an approved active guardian link and confirm guardian notification
  records are created.
- Capture events for inactive guardian links, out-of-scope guardian links,
  denied scans, unresolved anomalies, stale location evidence, and disabled
  notifications and confirm suppression records include reasons.
- Correct or reject a transport event after review and confirm guardian-visible
  status is withdrawn or corrected according to school account rules.

### Transport Anomalies and Reviews

- Create missed boarding, missed drop, wrong route, wrong stop, duplicate scan,
  invalid credential, out-of-order scan, delayed offline conflict, route
  deviation, stale location, and delayed trip examples.
- Run anomaly detection and confirm anomaly type, severity, status, evidence,
  and affected student or trip are recorded.
- Assign, resolve, dismiss, and reopen anomalies with reviewer permissions and
  reasons.
- Correct a scan, trip, ETA, notification, or anomaly through Manual Transport
  Review and confirm original evidence remains reviewable.
- Confirm anomaly resolution does not create campus attendance or campus access
  outcomes.

### Retention and Audit

- Confirm detailed active-trip location history older than 30 days is reduced
  to trip summaries and audit evidence unless an approved review hold applies.
- Confirm sensitive route, assignment, trip, scan, location, ETA, notification,
  anomaly, review, retention, and access-denial actions emit audit evidence.
- Confirm a failed audit write prevents sensitive mutation rather than allowing
  unaudited transport changes.

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
dotnet test apps/api/tests/SafeSchool.Api.Tests --filter Transport
npm test --prefix apps/admin-web -- transport
flutter test apps/mobile/test/features/transport
```

## Readiness Criteria

- Every Phase 3 user story can be implemented independently.
- Every sensitive action has tenant, capability, role, permission, idempotency,
  guardian visibility, retention, and audit expectations.
- Every public route in the contracts has denial behavior for tenant mismatch,
  disabled capability, missing permission, invalid credential, assignment
  mismatch, stale tracking evidence, and audit failure where applicable.
- Offline NFC/QR scan continuity, duplicate prevention, delayed sync, clock
  evidence, active-trip overlap, and location retention are planned before task
  generation.
- Guardian exact live bus location is limited to the linked student's onboard
  interval.
- Transport notification records remain limited to transport outcomes and do
  not implement general messaging or broadcasts.
- No Phase 2 campus attendance, Phase 2 campus access, Phase 4 wallet, Phase 6
  request, Phase 9 messaging, Phase 10 document/search, or Phase 11 dashboard
  behavior is implemented as part of Phase 3.
