# Contract: Learning History and Review

This contract defines learning history search, exception review, manual
corrections, reopenings, review summaries, lifecycle traceability, privacy
filtering, and audit behavior for Phase 5.

## Capabilities and Permissions

- Required capabilities:
  - `learning.progress_history`
  - `learning.review_summaries` for summaries
  - `learning.configuration` when rule migration is requested
- Common permissions:
  - `learning.history.read`
  - `learning.guardian_history.read`
  - `learning.reviews.manage`
  - `learning.summaries.read`
  - `learning.audit.read`

History and review routes are tenant-scoped, permission-scoped, paginated, and
privacy-filtered for students and guardians.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/learning/history` | Search school learning history |
| GET | `/api/v1/students/me/learning/history` | Search student-visible own learning history |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/learning/history` | Search guardian-visible linked-student learning history |
| GET | `/api/v1/schools/{schoolAccountId}/learning/exceptions` | List learning exceptions |
| GET | `/api/v1/schools/{schoolAccountId}/learning/exceptions/{exceptionId}` | Read exception detail |
| POST | `/api/v1/schools/{schoolAccountId}/learning/reviews` | Correct, reopen, close, resolve, dismiss, escalate, or migrate a learning record |
| GET | `/api/v1/schools/{schoolAccountId}/learning/review-summaries` | Read learning review summaries |
| GET | `/api/v1/schools/{schoolAccountId}/learning/trace/{sourceType}/{sourceId}` | Trace a learning lifecycle record |

## History Query

```yaml
student_profile_id: "student-reference"
course_id: "course-reference"
learning_group_id: "group-reference"
assignment_status: "Submitted"
quiz_result_status: "Scored"
star_outcome: "Awarded"
reward_status: "Fulfilled"
behavior_category: "Class participation"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
exception_state: "Open"
page: 1
page_size: 25
```

## Manual Review Request

```yaml
source_type: "Assignment Submission"
source_id: "submission-reference"
review_action: "Correct"
review_reason: "Teacher correction after review."
resulting_status: "Graded"
client_request_id: "learning-review-unique-to-reviewer"
```

## Review Summary Response

```yaml
summary_scope: "Student"
student_profile_id: "student-reference"
assignment_counts:
  submitted: 8
  missing: 1
quiz_counts:
  scored: 4
star_counts:
  available: 42
reward_counts:
  fulfilled: 2
behavior_counts:
  positive: 5
exception_counts:
  open: 1
latest_evidence_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- History reads require tenant access, enabled learning history capability, role
  permission, and visibility filtering by school account, guardian link,
  student ownership, course/group assignment, reviewer scope, or platform
  review authority.
- Guardian and student views hide staff-only notes, internal reviewer
  assignment, sensitive behavior details, and restricted feedback unless school
  visibility rules permit disclosure.
- Manual review actions require explicit reviewer permission, idempotent client
  identity, required reason, and audit evidence.
- Corrections, reopenings, dismissals, escalations, and rule migrations preserve
  original learning records and append review evidence.
- Summary reads enforce the same visibility boundaries as underlying records.
- Trace responses link source records to progress, submissions, quiz attempts,
  stars, rewards, behavior, exceptions, reviews, status events, and audit
  evidence where applicable.
