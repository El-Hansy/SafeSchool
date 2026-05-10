# Contract: Medical Notifications

This contract defines medical notification requests, high-severity default
audiences, privacy-safe summaries, contact attempts, acknowledgements, failed
contact handling, duplicate handling, and status event behavior for Phase 7.

## Capabilities and Permissions

- Required capabilities:
  - `medical.notifications`
  - `medical.incidents` when notifications originate from incidents
- Common permissions:
  - `medical.notifications.create`
  - `medical.notifications.read`
  - `medical.notifications.contact`
  - `medical.notifications.review`
  - `medical.audit.read`

Medical notification in Phase 7 creates request, contact, acknowledgement, and
status evidence only. It does not implement general messaging, broadcast
delivery, or notification platform management.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/medical/notifications` | Create or confirm medical notification request |
| GET | `/api/v1/schools/{schoolAccountId}/medical/notifications` | List medical notification requests |
| GET | `/api/v1/schools/{schoolAccountId}/medical/notifications/{notificationId}` | Read notification request detail |
| POST | `/api/v1/schools/{schoolAccountId}/medical/notifications/{notificationId}/contact-attempts` | Record manual or delivery contact attempt |
| POST | `/api/v1/schools/{schoolAccountId}/medical/notifications/{notificationId}/acknowledgements` | Record acknowledgement |
| POST | `/api/v1/schools/{schoolAccountId}/medical/notifications/{notificationId}/review` | Correct, cancel, reopen, or review notification |
| GET | `/api/v1/schools/{schoolAccountId}/medical/notifications/{notificationId}/trace` | Trace notification to incident, contacts, reviews, status events, and audit |

## Medical Notification Request

```yaml
source_type: "Medical Incident"
source_id: "incident-reference"
student_profile_id: "student-reference"
urgency: "High"
privacy_summary: "Medical incident requires contact."
acknowledgement_required: true
client_request_id: "medical-notification-unique-to-caller"
```

## Contact Attempt Request

```yaml
recipient_role: "Guardian"
emergency_contact_id: "contact-reference"
contact_route_category: "Phone"
attempt_outcome: "Successful"
acknowledgement_state: "Acknowledged"
failure_reason: null
attempted_at: "YYYY-MM-DDTHH:MM:SSZ"
client_request_id: "contact-attempt-unique-to-caller"
```

## Notification Response

```yaml
medical_notification_request_id: "notification-reference"
source_type: "Medical Incident"
urgency: "High"
default_audience_applied: true
audience:
  - recipient_role: "Guardian"
  - recipient_role: "Emergency Contact"
  - recipient_role: "Nurse Clinic Staff"
  - recipient_role: "Emergency Coordinator"
notification_status: "Partially Acknowledged"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Notification requests require tenant access, enabled capability, source
  incident or emergency access context, actor permission, privacy-safe summary,
  audience, acknowledgement rule, and audit evidence.
- High-severity medical incidents default the audience to approved guardians,
  emergency contacts, assigned nurse or clinic staff, and the school emergency
  coordinator.
- Sensitive medical details are minimized per recipient role.
- Invalid guardian links, expired contacts, duplicate active notifications,
  closed incidents, and broad broadcast attempts are blocked or routed to
  review.
- Contact attempts preserve recipient, contact route category, attempt time,
  outcome, acknowledgement state, actor when manual, and failure reason.
- Notification requests emit medical status events for later Phase 9
  consumption without delivering general messages directly.
