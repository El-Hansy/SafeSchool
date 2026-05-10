# Contract: Escalation Workflow

This contract defines rule-based escalation, manual escalation, high-risk
routing, target expiry routing, conflict-of-interest escalation, manual review
fallback, idempotency, and audit behavior for Phase 8.

## Capabilities and Permissions

- Required capabilities:
  - `complaints.escalation`
- Common permissions:
  - `complaints.escalate`
  - `complaints.escalations.read`
  - `complaints.escalations.review`
  - `complaints.reviews.manage`
  - `complaints.audit.read`

Escalation events are append-only, tenant-scoped, permission-scoped,
idempotent, and auditable. Target expiry routes to escalation or manual review
and must not silently close, dismiss, downgrade, or hide complaints.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/escalations` | Read complaint escalation history |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/escalate` | Manually escalate complaint |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/evaluate-escalation` | Evaluate configured escalation rules |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/expire-target` | Apply target response or resolution expiry routing |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/escalation-queue` | List complaints escalated to current actor, role, or queue |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/escalation-trace` | Trace escalation rules, events, owners, exceptions, and audit |

## Manual Escalation Request

```yaml
escalation_reason: "Complaint is unresolved and disputed."
target_owner_type: "Role"
target_owner_reference: "EscalationReviewer"
priority_after_escalation: "High"
client_request_id: "manual-escalation-unique-to-caller"
```

## Expiry Routing Request

```yaml
expired_target_type: "Resolution"
expiry_detected_at: "YYYY-MM-DDTHH:MM:SSZ"
client_request_id: "target-expiry-unique-to-system"
```

## Escalation Response

```yaml
escalation_event_id: "escalation-event-reference"
complaint_id: "complaint-reference"
trigger_source: "Target Expiry"
event_status: "Active"
prior_owner_reference: "ComplaintResolverQueue"
new_owner_reference: "EscalationReviewerQueue"
priority_after: "High"
target_response_at: "YYYY-MM-DDTHH:MM:SSZ"
manual_review_required: false
automatic_final_decision_created: false
already_processed: false
triggered_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Escalation requires tenant access, enabled escalation capability, actor
  permission or configured rule authority, non-final complaint state where
  required, and audit evidence.
- Escalation triggers include high priority, safety, safeguarding, severe
  misconduct, externally reportable, overdue, disputed, unresolved, repeatedly
  reopened, conflicted, and manual conditions.
- Missing routes, disabled owners, conflicted reviewers, stale rules, final
  complaints, or cross-school exposure route to manual review with exception
  evidence.
- Urgent safety, medical, emergency, or external authority signals route to
  configured urgent review but must not create medical incidents, emergency
  access sessions, or external authority reports.
- Duplicate escalation retries return the existing event without creating a
  second escalation.
- Escalation queue responses are permission-scoped and paginated.
