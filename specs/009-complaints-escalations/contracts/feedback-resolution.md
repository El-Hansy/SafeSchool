# Contract: Feedback and Resolution

This contract defines investigation entries, information requests,
complainant-visible responses, internal notes, resolution records, feedback,
reopen requests, correction, idempotency, and audit behavior for Phase 8.

## Capabilities and Permissions

- Required capabilities:
  - `complaints.feedback_resolution`
- Common permissions:
  - `complaints.investigate`
  - `complaints.resolve`
  - `complaints.feedback.submit`
  - `complaints.reopen.request`
  - `complaints.reviews.manage`
  - `complaints.audit.read`

Investigation and resolution records are tenant-scoped, append-only where they
record evidence, permission-scoped, and auditable. Internal details are
separated from complainant-visible responses.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/timeline` | Read authorized complaint timeline |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/entries` | Add investigation entry, information request, or visible response |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/resolve` | Record resolution proposal or closure |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/review` | Correct, reopen, dismiss, escalate, resolve, or close with reviewer reason |
| POST | `/api/v1/guardians/me/complaints/{complaintId}/feedback` | Submit guardian feedback or reopen request |
| POST | `/api/v1/students/me/complaints/{complaintId}/feedback` | Submit student feedback or reopen request where enabled |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/resolution` | Read authorized resolution detail |

## Investigation Entry Request

```yaml
entry_type: "Visible Response"
entry_summary: "School is reviewing the complaint and requested more details."
visibility_level: "Complainant Visible"
status_after: "Waiting For Information"
entry_reason: "More information needed from complainant."
client_request_id: "investigation-entry-unique-to-caller"
```

## Resolution Request

```yaml
resolution_type: "Resolved"
outcome_summary: "The complaint was reviewed and corrective action was taken."
internal_resolution_notes: "Restricted internal details."
corrective_action_summary: "Complainant-visible corrective action summary."
closure_reason: "Investigation completed."
complainant_visibility: "Summary"
feedback_eligible: true
reopen_eligible: true
client_request_id: "resolution-unique-to-caller"
```

## Feedback Request

```yaml
feedback_type: "Dissatisfied"
feedback_rating: 2
feedback_comment: "The outcome did not address the transport concern."
reopen_requested: true
reopen_reason: "Issue continues after resolution."
client_request_id: "feedback-unique-to-caller"
```

## Acceptance Rules

- Investigation entries require tenant access, enabled capability, assignment
  or resolver authority, allowed status transition, visibility level, and audit
  evidence.
- Complainant-visible responses must not include restricted internal notes,
  conflict review details, safety review notes, or staff-only details.
- Resolution requires active assignment or reviewer authority, resolution type,
  closure reason, outcome visibility, and preserved history.
- Feedback requires resolved complaint state, complainant eligibility, active
  feedback rules, and tenant scope.
- Reopen requests route to the configured owner or reviewer while preserving
  the original resolution.
- Corrections and reopen actions append review evidence and cannot delete the
  original complaint, investigation entries, resolution, or feedback.
- Duplicate resolution, feedback, and review retries return the existing result
  without creating duplicate final outcomes.
