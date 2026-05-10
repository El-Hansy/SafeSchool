# Contract: Communication History and Review

This contract defines communication history search, review summaries,
exceptions, moderation reviews, corrections, lifecycle trace, privacy
filtering, and audit behavior for Phase 9.

## Capabilities and Permissions

- Required capabilities:
  - `communications.history`
  - `communications.review_summaries`
  - `communications.moderation`
- Common permissions:
  - `communications.history.read`
  - `communications.summaries.read`
  - `communications.exceptions.read`
  - `communications.exceptions.resolve`
  - `communications.moderation.review`
  - `communications.audit.read`

History, summaries, exceptions, moderation, and review actions are
tenant-scoped, permission-scoped, and auditable. They apply the same privacy
filters as source communication records.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/communications/history` | Search communication history with filters |
| GET | `/api/v1/schools/{schoolAccountId}/communications/summaries` | Read communication review summaries |
| GET | `/api/v1/schools/{schoolAccountId}/communications/exceptions` | List communication exceptions |
| GET | `/api/v1/schools/{schoolAccountId}/communications/{communicationKind}/{communicationId}/exceptions` | Read exceptions for one communication |
| POST | `/api/v1/schools/{schoolAccountId}/communications/exceptions/{exceptionId}/review` | Resolve, dismiss, or escalate communication exception |
| GET | `/api/v1/schools/{schoolAccountId}/communications/moderation-queue` | List communications awaiting moderation |
| POST | `/api/v1/schools/{schoolAccountId}/communications/moderation/{reviewId}/review` | Approve, reject, correct, withdraw, or document moderation item |
| GET | `/api/v1/schools/{schoolAccountId}/communications/{communicationKind}/{communicationId}/trace` | Trace communication lifecycle and audit evidence |

## History Query

```yaml
student_profile_id: "student-reference"
guardian_link_id: "guardian-link-reference"
sender_actor_id: "sender-reference"
recipient_actor_id: "recipient-reference"
audience_rule_id: "audience-rule-reference"
source_module: "Complaints"
category: "Complaint Update"
priority: "High"
message_type: "Notification"
delivery_state: "Failed"
read_state: "Unread"
acknowledgement_state: "Overdue"
moderation_state: "Pending"
exception_type: "Delivery Failure"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
page: 1
page_size: 50
```

## Moderation Review Request

```yaml
review_action: "Approve"
review_reason: "Message content is appropriate for selected audience."
visibility_change: "Recipient Visible"
client_request_id: "moderation-review-unique-to-reviewer"
```

## Exception Review Request

```yaml
review_action: "Resolve"
resolution_reason: "Recipient link was corrected and delivery retried."
client_request_id: "communication-exception-review-unique-to-reviewer"
```

## Summary Response

```yaml
summary_scope: "Category"
sent_count: 250
delivered_count: 238
failed_count: 7
read_count: 190
acknowledged_count: 80
overdue_count: 3
moderated_count: 6
withdrawn_count: 1
corrected_count: 2
exception_count: 5
pending_count: 4
generated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- History, summary, exception, moderation, and trace routes require tenant
  access, enabled capability, actor permission, scoped filters, and audit
  evidence.
- Filters include student, guardian, staff member, sender, recipient, audience,
  source module, category, priority, message type, delivery state, read state,
  acknowledgement state, moderation state, exception type, and date range.
- Guardian and student history views hide restricted source details,
  reviewer-only content, staff-only details, and records outside approved links
  or self-scope.
- Moderation review requires reviewer authority and a required reason.
- Corrections, withdrawals, approvals, rejections, resolutions, and escalations
  preserve original communication evidence.
- Exceptions preserve source evidence and cannot be deleted to hide an issue.
- Lifecycle trace must connect source event or author, template, audience
  resolution, recipient snapshots, delivery attempts, read state,
  acknowledgements, moderation decisions, exceptions, corrections, summaries,
  lifecycle events, and audit evidence.
