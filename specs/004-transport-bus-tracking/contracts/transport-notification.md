# Contract: Transport Notification

This contract defines guardian notification eligibility, visibility,
suppression, withdrawal, and traceability for Phase 3 transport events.

## Capabilities and Permissions

- Required capabilities:
  - `transport.notifications`
- Common permissions:
  - `transport.notifications.read`
  - `transport.guardian_visibility.read`
  - `transport.scans.read`
  - `transport.eta.read`
  - `transport.audit.read`

Phase 3 creates transport notification records and suppression records. General
messaging, broadcasts, and channel management belong to a later communication
phase.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/transport/notification-records` | Review notification records with filters |
| GET | `/api/v1/schools/{schoolAccountId}/transport/notification-records/{notificationRecordId}` | Read notification record detail |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/transport/notifications` | Read guardian-visible transport notifications |
| POST | `/api/v1/schools/{schoolAccountId}/transport/notification-records/{notificationRecordId}/withdraw` | Withdraw or correct a guardian-visible notification after review |
| GET | `/api/v1/schools/{schoolAccountId}/transport/notification-records/{notificationRecordId}/trace` | Trace notification to event, guardian link, trip, anomaly, review, and audit outcomes |

## Notification Record Response

```yaml
transport_notification_record_id: "notification-reference"
school_account_id: "school-account-reference"
guardian_record_id: "guardian-reference"
guardian_link_id: "guardian-link-reference"
student_profile_id: "student-profile-reference"
transport_trip_id: "trip-reference"
event_type: "Boarding"
source_event_reference: "scan-event-reference"
notification_status: "Visible"
suppression_reason: null
visible_status: "Student boarded the bus."
created_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Guardian Notification Response

```yaml
student_profile_id: "student-profile-reference"
notifications:
  - transport_notification_record_id: "notification-reference"
    event_type: "Boarding"
    visible_status: "Student boarded the bus."
    event_time: "YYYY-MM-DDTHH:MM:SSZ"
    transport_trip_id: "trip-reference"
    route_label: "North Morning Route"
```

## Withdraw Request

```yaml
withdrawal_reason: "Scan was corrected after reviewer approval."
replacement_visible_status: "Boarding status corrected."
client_request_id: "request-unique-to-caller"
```

## Acceptance Rules

- Transport notification records require `transport.notifications` to be
  enabled.
- Eligible event types include accepted or reviewed boarding, accepted or
  reviewed drop, delay, material ETA change, route change, and reviewed
  correction.
- Notification eligibility requires an approved active guardian link, transport
  visibility scope, accepted or reviewed source evidence, and school account
  settings.
- Denied, unresolved, stale, untrusted, wrong-route, wrong-stop, or
  needs-review evidence is suppressed until reviewer approval.
- Suppression records must include the school account, guardian or link when
  known, student, event type, source evidence, suppression reason, and time.
- Guardian notification list responses must never expose unrelated students,
  routes, trips, or guardian links.
- Withdrawal or correction requires reviewer permission, reason, and audit
  evidence.
- Notification record lists must be paginated and support filtering by student,
  guardian, trip, event type, status, source event, suppression reason, and
  creation time.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Suppress notification workflow and record feature capability reason |
| Guardian link inactive or out of scope | Suppress visibility and record reason |
| Source event unresolved or denied | Suppress notification until review approves |
| Source location stale or untrusted | Suppress exact location and record reason |
| Actor lacks review permission | Deny withdrawal or correction and record required permission |
| Guardian requests another student | Deny without exposing student existence |
| Cross-school event reference | Deny without exposing cross-tenant existence |
| Audit write fails for eligibility or withdrawal | Reject sensitive change rather than allowing unaudited mutation |
