# Contract: Notification Center

This contract defines notification source event intake, notification
generation, recipient-scoped notification reads, read-state changes,
acknowledgements, correction, withdrawal, reply-policy display, duplicate
suppression, restricted-detail minimization, and audit behavior for Phase 9.

## Capabilities and Permissions

- Required capabilities:
  - `communications.notification_center`
  - `communications.delivery_tracking` when delivery state is shown
  - `communications.acknowledgements` when acknowledgement is required
- Common permissions:
  - `communications.notifications.read.own`
  - `communications.notifications.read.staff`
  - `communications.notifications.generate`
  - `communications.notifications.acknowledge`
  - `communications.notifications.correct`
  - `communications.notifications.withdraw`
  - `communications.audit.read`

Notification records are tenant-scoped, recipient-scoped, idempotent where
they mutate state, and auditable. Source events are accepted only as read-only
communication triggers.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/communications/source-events` | Accept an eligible source event for notification generation |
| GET | `/api/v1/schools/{schoolAccountId}/communications/notifications` | List staff-visible notifications within authorized scope |
| GET | `/api/v1/schools/{schoolAccountId}/communications/notifications/{notificationId}` | Read staff-visible notification detail |
| POST | `/api/v1/schools/{schoolAccountId}/communications/notifications/{notificationId}/read` | Mark a notification read |
| POST | `/api/v1/schools/{schoolAccountId}/communications/notifications/{notificationId}/acknowledge` | Acknowledge a required notification |
| POST | `/api/v1/schools/{schoolAccountId}/communications/notifications/{notificationId}/correct` | Correct an eligible notification with preserved original evidence |
| POST | `/api/v1/schools/{schoolAccountId}/communications/notifications/{notificationId}/withdraw` | Withdraw an eligible notification with reason and recipient impact |
| GET | `/api/v1/guardians/me/notifications` | List guardian-visible notifications |
| GET | `/api/v1/guardians/me/notifications/{notificationId}` | Read guardian-visible notification detail |
| POST | `/api/v1/guardians/me/notifications/{notificationId}/read` | Mark guardian notification read |
| POST | `/api/v1/guardians/me/notifications/{notificationId}/acknowledge` | Acknowledge guardian notification |
| GET | `/api/v1/students/me/notifications` | List student-visible notifications where enabled |
| GET | `/api/v1/students/me/notifications/{notificationId}` | Read student-visible notification detail |
| POST | `/api/v1/students/me/notifications/{notificationId}/read` | Mark student notification read |
| POST | `/api/v1/students/me/notifications/{notificationId}/acknowledge` | Acknowledge student notification |

## Source Event Request

```yaml
source_module: "Attendance"
source_record_reference: "attendance-event-reference"
source_event_type: "EntryRecorded"
source_event_version: 1
student_profile_id: "student-reference"
event_priority: "Normal"
event_summary: "Student arrived on campus."
restricted_detail_level: "Recipient Summary"
communication_eligible: true
dedupe_key: "attendance-entry-student-date"
client_request_id: "source-event-unique-to-source-module"
```

## Notification Query

```yaml
student_profile_id: "student-reference"
category: "Attendance"
priority: "Normal"
read_state: "Unread"
acknowledgement_state: "Pending"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
page: 1
page_size: 50
```

## Acknowledgement Request

```yaml
acknowledgement_note: "Reviewed by guardian."
client_request_id: "notification-acknowledgement-unique-to-recipient"
```

## Notification Correction Request

```yaml
corrected_title: "Updated attendance notice"
corrected_summary: "Student arrival time was corrected by attendance staff."
correction_reason: "Source attendance event was corrected."
recipient_impact_note: "Recipients who already read the prior notification retain correction history."
client_request_id: "notification-correction-unique-to-actor"
```

## Notification Withdrawal Request

```yaml
withdrawal_reason: "Source event was withdrawn by originating module."
recipient_impact_note: "Notification remains visible in history as withdrawn."
client_request_id: "notification-withdrawal-unique-to-actor"
```

## Notification Response

```yaml
notification_id: "notification-reference"
source_module: "Attendance"
category: "Attendance"
priority: "Normal"
title: "Attendance update"
summary: "Student arrived on campus."
student_profile_id: "student-reference"
notification_status: "Delivered"
reply_policy: "No Reply"
support_action:
  action_type: "Configured Support Route"
  label: "Contact school office"
  target_reference: "school-office-support"
correction_of_notification_id: null
correction_reason: null
withdrawal_reason: null
read_state: "Unread"
acknowledgement_required: false
acknowledgement_state: "Not Required"
delivery_state: "Delivered"
visible_detail_level: "Recipient Summary"
corrected_at: null
withdrawn_at: null
generated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Every route requires tenant access, enabled capability, role permission, and
  backend feature enforcement.
- Source events must be tenant-scoped, marked communication eligible,
  permission-safe, and idempotent by source event reference or dedupe key.
- Guardian notification reads require an approved active guardian link unless
  the notification is addressed directly to the guardian outside student
  context.
- Student notification reads require student self-scope and enabled student
  notification rules.
- Restricted source details are minimized, withheld, or routed to review based
  on recipient authority.
- Duplicate source events return the existing notification result or preserve
  suppression evidence without sending duplicates.
- Notification corrections and withdrawals preserve original title, summary,
  source reference, recipient snapshot, actor, time, reason, recipient impact,
  audit evidence, and lifecycle events.
- Notification responses must identify whether replies are allowed. No Reply
  notifications reject reply attempts; Support Route notifications expose only
  the configured action and do not mutate the originating source workflow.
- Notification behavior must not create side effects in excluded domains.
