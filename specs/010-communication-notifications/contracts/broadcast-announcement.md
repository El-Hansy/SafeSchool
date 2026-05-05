# Contract: Broadcast and Announcement

This contract defines broadcast and announcement draft, approval, scheduling,
publication, audience resolution, recipient snapshots, correction, withdrawal,
acknowledgement requirements, and audit behavior for Phase 9.

## Capabilities and Permissions

- Required capabilities:
  - `communications.broadcasts`
  - `communications.announcements`
  - `communications.templates` when templates are used
  - `communications.moderation` when approval is required
- Common permissions:
  - `communications.broadcasts.create`
  - `communications.broadcasts.publish`
  - `communications.broadcasts.approve`
  - `communications.broadcasts.withdraw`
  - `communications.audience.resolve`
  - `communications.audit.read`

Broadcasts and announcements are tenant-scoped, audience-scoped, idempotent,
and auditable. Recipient snapshots preserve the resolved audience at
publication time.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/communications/broadcasts` | List broadcasts and announcements |
| POST | `/api/v1/schools/{schoolAccountId}/communications/broadcasts` | Create broadcast or announcement draft |
| GET | `/api/v1/schools/{schoolAccountId}/communications/broadcasts/{broadcastId}` | Read broadcast or announcement detail |
| POST | `/api/v1/schools/{schoolAccountId}/communications/broadcasts/{broadcastId}/submit` | Submit draft for approval or scheduling |
| POST | `/api/v1/schools/{schoolAccountId}/communications/broadcasts/{broadcastId}/approve` | Approve pending broadcast |
| POST | `/api/v1/schools/{schoolAccountId}/communications/broadcasts/{broadcastId}/publish` | Publish immediately or at scheduled time |
| POST | `/api/v1/schools/{schoolAccountId}/communications/broadcasts/{broadcastId}/correct` | Correct published communication |
| POST | `/api/v1/schools/{schoolAccountId}/communications/broadcasts/{broadcastId}/withdraw` | Withdraw eligible communication |
| GET | `/api/v1/schools/{schoolAccountId}/communications/broadcasts/{broadcastId}/recipients` | Read recipient snapshot |

## Broadcast Draft Request

```yaml
communication_type: "Announcement"
title: "Parent meeting reminder"
body: "The meeting starts at 5 PM."
language: "en"
priority: "Normal"
category: "School Announcement"
audience_rule_id: "grade-5-guardians"
template_id: "announcement-template-reference"
effective_from: "YYYY-MM-DDTHH:MM:SSZ"
effective_to: "YYYY-MM-DDTHH:MM:SSZ"
scheduled_publish_at: "YYYY-MM-DDTHH:MM:SSZ"
acknowledgement_required: false
client_request_id: "broadcast-draft-unique-to-caller"
```

## Publication Response

```yaml
broadcast_announcement_id: "broadcast-reference"
publication_status: "Published"
recipient_snapshot_count: 128
excluded_recipient_count: 3
suppressed_duplicate_count: 7
approval_required: false
published_at: "YYYY-MM-DDTHH:MM:SSZ"
already_processed: false
```

## Correction Request

```yaml
corrected_title: "Updated parent meeting reminder"
corrected_body: "The meeting starts at 5:30 PM."
correction_reason: "Time changed by school administration."
client_request_id: "broadcast-correction-unique-to-caller"
```

## Acceptance Rules

- Draft, approval, publication, correction, and withdrawal require tenant
  access, enabled capability, actor permission, valid audience, and audit
  evidence.
- Audience rules may target only authorized school-account recipients such as
  whole school, guardians, students, staff, grade, class, route, activity
  group, role group, source-event recipients, or manually selected permitted
  recipients.
- Publication preserves recipient snapshots with inclusion, exclusion,
  duplicate suppression, and relationship evidence.
- Approval, moderation, translation, or scheduled release requirements block
  delivery until satisfied.
- Empty audiences, cross-school recipients, restricted-detail exposure risks,
  disabled features, invalid templates, and policy limit violations reject or
  route publication to review.
- Corrections and withdrawals preserve original publication evidence.
- Broadcast behavior must not create side effects in excluded domains.
