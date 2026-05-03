# Contract: Entry/Exit Notification

This contract defines guardian entry/exit notification eligibility, suppression,
visibility records, and review behavior for Phase 2. It does not define a
general messaging or broadcast system.

## Capabilities and Permissions

- Required capabilities:
  - `attendance_access.entry_exit_notifications`
- Common permissions:
  - `attendance_access.notifications.read`
  - `attendance_access.guardian_entry_exit.read`
  - `attendance_access.scans.read`
  - `attendance_access.attendance.read`
  - `attendance_access.audit.read`

Guardian visibility requires an approved active Guardian Link from Phase 1 with
an access scope that allows entry/exit updates.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/notification-records` | Review entry/exit notification records with filters |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/notification-records/{notificationRecordId}` | Read notification record detail |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/students/{studentProfileId}/entry-exit-notifications` | Review notification records for a student |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/guardians/me/entry-exit-notifications` | Guardian view of eligible linked-student entry/exit records |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/notification-records/{notificationRecordId}/withdraw` | Withdraw or suppress a notification after review |

Notification records are created by scan and attendance workflows. The
contracts above expose review and guardian visibility behavior.

## Notification Record Response

```yaml
entry_exit_notification_record_id: "notification-record-reference"
school_account_id: "school-account-reference"
student_profile_id: "student-profile-reference"
guardian_id: "guardian-reference"
guardian_link_id: "guardian-link-reference"
gate_scan_event_id: "scan-event-reference"
campus_access_decision_id: "campus-access-decision-reference"
attendance_record_id: "attendance-record-reference"
direction: "Entry"
eligibility_status: "Visible"
suppression_reason: null
guardian_visible_time: "YYYY-MM-DDTHH:MM:SSZ"
channel_reference: "configured-entry-exit-channel"
created_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Suppressed Notification Record

```yaml
entry_exit_notification_record_id: "notification-record-reference"
school_account_id: "school-account-reference"
student_profile_id: "student-profile-reference"
guardian_id: "guardian-reference"
guardian_link_id: "guardian-link-reference"
gate_scan_event_id: "scan-event-reference"
direction: "Exit"
eligibility_status: "Suppressed"
suppression_reason: "Guardian link does not allow entry/exit visibility."
guardian_visible_time: null
channel_reference: null
created_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Withdraw Notification Request

```yaml
withdrawal_reason: "Scan was corrected after review."
replacement_status: "Withdrawn"
client_request_id: "request-unique-to-caller"
```

## Acceptance Rules

- Entry/exit notification behavior requires
  `attendance_access.entry_exit_notifications` to be enabled.
- A notification record can be visible to a guardian only when the guardian has
  an approved active Guardian Link with entry/exit visibility scope.
- Guardian records without eligible active links must receive suppression
  records or no visible output according to school account rules.
- Denied, unresolved, or needs-review scans suppress notification until school
  rules allow a reviewed outcome to be visible.
- Corrected, rejected, or withdrawn scan outcomes must update notification
  visibility according to school account rules.
- Guardian-facing list responses must include only linked students and records
  visible under the active guardian link scope.
- Review list responses must be paginated and support filtering by student,
  guardian, direction, eligibility status, suppression reason, scan time, and
  created time.
- Phase 2 notification records may reference configured channels, but general
  messaging, broadcasts, subscription preferences, and channel vendor
  management remain out of scope.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Suppress or deny notification workflow and record feature capability reason |
| Guardian link inactive | Suppress notification and record link state |
| Guardian link outside scope | Suppress notification and record access scope reason |
| Scan denied or unresolved | Suppress visibility until review produces an allowed outcome |
| Actor lacks review permission | Deny notification record review and record required permission |
| Guardian requests another student's record | Deny without exposing cross-student or cross-tenant existence |
| Notification withdrawal lacks reason | Reject withdrawal with validation details |
| Audit write fails for visibility change | Reject visibility change rather than allowing unaudited mutation |
