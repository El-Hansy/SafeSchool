# Contract: Delivery and Acknowledgement

This contract defines delivery attempts, channel outcomes, retries, exclusions,
read receipts, acknowledgement deadlines, overdue handling, reminders,
evidence, and audit behavior for Phase 9.

## Capabilities and Permissions

- Required capabilities:
  - `communications.delivery_tracking`
  - `communications.acknowledgements`
  - `communications.external_delivery_channels` when external channels are used
- Common permissions:
  - `communications.delivery.read`
  - `communications.delivery.retry`
  - `communications.acknowledgements.read`
  - `communications.acknowledgements.manage`
  - `communications.audit.read`

Delivery attempts and acknowledgements are tenant-scoped, recipient-scoped,
append-only where they preserve evidence, and auditable.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/communications/delivery-attempts` | Search delivery attempts |
| GET | `/api/v1/schools/{schoolAccountId}/communications/{communicationKind}/{communicationId}/delivery` | Read delivery status for one communication |
| POST | `/api/v1/schools/{schoolAccountId}/communications/delivery-attempts/{attemptId}/retry` | Retry an eligible failed delivery |
| GET | `/api/v1/schools/{schoolAccountId}/communications/acknowledgements` | Search acknowledgement records |
| GET | `/api/v1/schools/{schoolAccountId}/communications/{communicationKind}/{communicationId}/acknowledgements` | Read acknowledgement status for one communication |
| POST | `/api/v1/schools/{schoolAccountId}/communications/acknowledgements/{acknowledgementId}/waive` | Waive an acknowledgement with reviewer reason |
| POST | `/api/v1/schools/{schoolAccountId}/communications/acknowledgements/{acknowledgementId}/remind` | Mark reminder eligibility or request reminder delivery |

## Delivery Query

```yaml
communication_kind: "Broadcast"
communication_reference: "broadcast-reference"
recipient_actor_id: "actor-reference"
channel: "In App"
delivery_status: "Failed"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
page: 1
page_size: 50
```

## Delivery Retry Request

```yaml
retry_reason: "Temporary channel failure resolved."
client_request_id: "delivery-retry-unique-to-reviewer"
```

## Acknowledgement Waiver Request

```yaml
waiver_reason: "Guardian confirmed by phone and reviewer accepted evidence."
client_request_id: "acknowledgement-waiver-unique-to-reviewer"
```

## Delivery Status Response

```yaml
communication_reference: "broadcast-reference"
recipient_count: 128
delivered_count: 120
failed_count: 5
excluded_count: 3
retry_scheduled_count: 2
acknowledgement_required_count: 20
acknowledged_count: 15
overdue_count: 1
generated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Delivery reads require tenant access, enabled tracking capability, actor
  permission, and scoped filters.
- Delivery attempts require enabled channel, recipient eligibility, feature
  availability, preference policy, quiet-hour policy, duplicate suppression,
  and audit evidence.
- Failed, excluded, suppressed, or cancelled attempts cannot be marked
  delivered without a new successful attempt or reviewer correction.
- Acknowledgement state is tracked per recipient and cannot be substituted by
  aggregate read state.
- Overdue acknowledgements route to review or reminder eligibility according
  to school configuration.
- Waivers require reviewer authority and a reason.
- Delivery and acknowledgement behavior must not create side effects in
  excluded domains.
