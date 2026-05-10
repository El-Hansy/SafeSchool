# Contract: Complaint History and Review

This contract defines complaint history search, complaint summaries,
exceptions, manual reviews, corrections, lifecycle trace, privacy filtering,
and audit behavior for Phase 8.

## Capabilities and Permissions

- Required capabilities:
  - `complaints.history`
  - `complaints.review_summaries`
- Common permissions:
  - `complaints.history.read`
  - `complaints.reviews.manage`
  - `complaints.exceptions.read`
  - `complaints.exceptions.resolve`
  - `complaints.summaries.read`
  - `complaints.audit.read`

History, summaries, exceptions, and review actions are tenant-scoped,
permission-scoped, and auditable. They apply the same privacy filters as source
complaint records.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/complaints/history` | Search complaint history with filters |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/summaries` | Read complaint review summaries |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/exceptions` | List complaint exceptions |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/exceptions` | Read exceptions for complaint |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/exceptions/{exceptionId}/review` | Resolve, dismiss, or escalate complaint exception |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/manual-review` | Correct, reopen, dismiss, escalate, resolve, close, or migrate rule version |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/trace` | Trace complaint lifecycle and audit evidence |

## History Query

```yaml
student_profile_id: "student-reference"
complainant_actor_id: "actor-reference"
guardian_link_id: "guardian-link-reference"
staff_submitter_id: "staff-reference"
category_id: "category-reference"
priority: "High"
confidentiality_level: "Restricted"
complaint_status: "Escalated"
owner_reference: "ComplaintReviewQueue"
escalation_state: "Escalated"
target_timing_state: "Overdue"
feedback_state: "Reopen Requested"
exception_type: "Conflicted Owner"
review_state: "Under Review"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
page: 1
page_size: 50
```

## Manual Review Request

```yaml
review_action: "Correct"
review_reason: "Corrected category after reviewer analysis."
resulting_status: "In Review"
visibility_change: "Restricted"
client_request_id: "manual-review-unique-to-reviewer"
```

## Exception Review Request

```yaml
review_action: "Resolve"
resolution_reason: "Assigned to non-conflicted reviewer."
client_request_id: "exception-review-unique-to-reviewer"
```

## Summary Response

```yaml
summary_scope: "Category"
complaint_count: 25
open_count: 12
overdue_count: 3
escalated_count: 2
feedback_pending_count: 4
exception_count: 1
generated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- History and summary routes require tenant access, enabled capability, actor
  permission, and audit evidence.
- Filters include student, complainant, guardian, staff submitter, category,
  priority, confidentiality, status, owner, escalation state, date range,
  target timing state, feedback state, exception type, and review state.
- Guardian and student history views hide restricted internal notes,
  conflicted-party details, safety review notes, staff-only details, and
  records outside approved links or self-scope.
- Manual review requires reviewer authority and a required reason.
- Corrections, reopen actions, dismissals, escalations, resolutions, closures,
  and rule migrations preserve original complaint evidence.
- Exceptions preserve source evidence and cannot be deleted to hide an issue.
- Lifecycle trace must connect submission, categorization, assignment,
  investigation entries, escalation, resolution, feedback, reopen, review,
  summaries, status events, and audit evidence.
