# Contract: Approval Workflow

This contract defines workflow step assignment, approval decisions, denials,
information requests, delegation, escalation, expiry routing, idempotency, and
decision traceability for Phase 6.

## Capabilities and Permissions

- Required capabilities:
  - `requests.approval_workflow`
- Common permissions:
  - `requests.workflows.read`
  - `requests.decisions.act`
  - `requests.reviews.manage`
  - `requests.requests.read`
  - `requests.audit.read`

Workflow decisions are append-only, tenant-scoped, and idempotent. Expired
workflow steps route requests to manual review or configured escalation while
keeping the request pending; they must not auto-approve or auto-deny by
default.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/workflow` | Read active workflow state and decision history |
| GET | `/api/v1/schools/{schoolAccountId}/requests/workflow-queue` | List requests assigned to the current actor or role |
| POST | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/decisions` | Approve, deny, request information, delegate, escalate, or mark manual review |
| POST | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/workflow/expire-step` | Apply configured expiry routing for an active step |
| POST | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/workflow/delegate` | Delegate an active step when allowed by workflow rules |
| GET | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/workflow/trace` | Trace workflow steps, decisions, exceptions, reviews, and audit evidence |

## Decision Request

```yaml
workflow_step_id: "active-step-reference"
decision_type: "Approve"
decision_reason: "Student meets outing requirements."
delegate_to_actor_id: null
request_information_note: null
client_request_id: "decision-unique-to-caller"
```

## Decision Response

```yaml
workflow_decision_id: "decision-reference"
permission_request_id: "request-reference"
workflow_step_id: "step-reference"
decision_status: "Accepted"
decision_type: "Approve"
actor_id: "actor-reference"
next_request_status: "Pending Approval"
next_step_id: "next-step-reference"
already_processed: false
decided_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Expiry Routing Request

```yaml
workflow_step_id: "expired-step-reference"
expiry_detected_at: "YYYY-MM-DDTHH:MM:SSZ"
client_request_id: "expiry-unique-to-system"
```

## Expiry Routing Response

```yaml
permission_request_id: "request-reference"
workflow_step_id: "expired-step-reference"
request_status: "Manual Review"
pending_state_preserved: true
automatic_final_decision_created: false
review_assignment: "Queue:RequestReview"
escalation_reason: "Configured step expiry reached without decision."
```

## Acceptance Rules

- Decisions require tenant access, enabled `requests.approval_workflow`,
  assigned active step, required permission, non-final request state, and
  idempotent client identity.
- Duplicate decision retries return the existing outcome without creating a
  second decision.
- Unauthorized, out-of-order, cross-school, withdrawn, expired, or final-state
  decisions are rejected or routed to review without corrupting decision
  history.
- Decision reasons are required when the workflow step marks them required.
- Accepted decisions cannot be edited or deleted directly.
- Expiry routing must create reviewable evidence and must not silently approve
  or deny the request.
- Workflow queue responses are permission-scoped and paginated.
