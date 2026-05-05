# Contract: Complaint Categorization

This contract defines category evaluation, reclassification, priority,
confidentiality, assignment, conflict-of-interest detection, manual review
routing, and audit behavior for Phase 8.

## Capabilities and Permissions

- Required capabilities:
  - `complaints.categorization`
  - `complaints.assignment`
- Common permissions:
  - `complaints.categorize`
  - `complaints.assign`
  - `complaints.reclassify`
  - `complaints.read.assigned`
  - `complaints.reviews.manage`
  - `complaints.audit.read`

Categorization and assignment commands are tenant-scoped, permission-scoped,
idempotent, and auditable. Category versions determine required fields,
priority, confidentiality, owner group, target timing, escalation, and feedback
behavior.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/complaints/categories` | List active complaint categories visible to actor |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/triage` | Read complaint triage state |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/categorize` | Categorize or reclassify complaint |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/assign` | Assign or reassign complaint |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/assignment-queue` | List complaints assigned to current actor, role, or queue |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/assignment-history` | Read assignment and reclassification history |

## Categorization Request

```yaml
category_id: "complaint-category-reference"
subcategory: "Bullying"
priority: "High"
confidentiality_level: "Restricted"
classification_reason: "Complaint includes safety concern."
client_request_id: "categorization-unique-to-caller"
```

## Assignment Request

```yaml
assigned_to_type: "Queue"
assigned_to_reference: "ComplaintReviewQueue"
assignment_reason: "Category routes to complaint manager queue."
client_request_id: "assignment-unique-to-caller"
```

## Assignment Response

```yaml
complaint_id: "complaint-reference"
complaint_status: "Assigned"
category_version: 3
priority: "High"
confidentiality_level: "Restricted"
current_owner_type: "Queue"
current_owner_reference: "ComplaintReviewQueue"
target_response_at: "YYYY-MM-DDTHH:MM:SSZ"
conflict_detected: false
already_processed: false
assigned_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Categorization requires enabled categorization capability, tenant scope,
  complaint manager authority, active category, required fields, and audit
  evidence.
- Reclassification requires a reason and preserves prior category, priority,
  confidentiality, owner, target timing, and audit evidence.
- Assignment requires enabled assignment capability, valid owner group or
  actor, actor authority, and conflict-of-interest checks.
- Complaint subjects and conflicted users cannot self-assign, decide, close,
  or view restricted complainant details unless a reviewer records a limited
  exception.
- Disabled categories, missing owner groups, stale category versions, invalid
  confidentiality settings, or conflicted assignments route to manual review.
- Assignment queue responses are permission-scoped and paginated.
- Categorization and assignment must not create side effects in excluded
  domains.
