# Contract: Assignment Tracking

This contract defines assignment creation, submission, resubmission, grading,
late handling, visibility, correction evidence, idempotency, and audit behavior
for Phase 5.

## Capabilities and Permissions

- Required capabilities:
  - `learning.assignments`
  - `learning.progress_history` when history is read
- Common permissions:
  - `learning.assignments.manage`
  - `learning.assignments.review`
  - `learning.assignments.submit`
  - `learning.assignments.read`
  - `learning.guardian_history.read`
  - `learning.audit.read`

Assignment commands are tenant-scoped and auditable. Submission attempts are
preserved as history and must not overwrite prior evidence.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/learning/assignments` | Create an assignment |
| GET | `/api/v1/schools/{schoolAccountId}/learning/assignments` | List assignments |
| GET | `/api/v1/students/me/learning/assignments` | List student-visible assignments |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/learning/assignments` | List guardian-visible linked-student assignments |
| POST | `/api/v1/students/me/learning/assignments/{assignmentId}/submissions` | Submit or resubmit assignment evidence |
| POST | `/api/v1/schools/{schoolAccountId}/learning/assignments/{assignmentId}/submissions/{submissionId}/review` | Grade, return, excuse, or route a submission to review |
| GET | `/api/v1/schools/{schoolAccountId}/learning/assignments/{assignmentId}/trace` | Trace assignment to submissions, stars, exceptions, reviews, and audit evidence |

## Assignment Request

```yaml
course_id: "course-reference"
learning_group_id: "group-reference"
title: "Fraction worksheet"
instructions: "Submit completed worksheet evidence."
required_evidence: "Worksheet response"
due_at: "YYYY-MM-DDTHH:MM:SSZ"
opens_at: "YYYY-MM-DDTHH:MM:SSZ"
closes_at: "YYYY-MM-DDTHH:MM:SSZ"
late_policy: "Route Late To Review"
review_policy: "Grade"
client_request_id: "assignment-unique-to-caller"
```

## Submission Request

```yaml
submitted_evidence_reference: "student-work-reference"
submission_note: "Completed worksheet."
client_request_id: "submission-unique-to-student"
```

## Review Request

```yaml
review_action: "Grade"
grade_value: "92"
feedback_summary: "Strong work. Review question 4."
review_reason: "Teacher grading"
client_request_id: "submission-review-unique-to-reviewer"
```

## Submission Response

```yaml
assignment_submission_id: "submission-reference"
assignment_id: "assignment-reference"
student_profile_id: "student-reference"
attempt_number: 1
submission_status: "Submitted"
submitted_at: "YYYY-MM-DDTHH:MM:SSZ"
late_state: "On Time"
guardian_visible: true
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Assignment creation requires tenant access, enabled `learning.assignments`,
  active course or group scope, staff authority, required evidence, due window,
  and audit evidence.
- Student submission requires active student status, assigned learning scope,
  open submission window or configured late behavior, and idempotent client
  identity.
- Resubmission is allowed only when assignment rules permit it and preserves
  all prior submission evidence.
- Late, missing-evidence, duplicate, withdrawn, excused, ineligible, disabled,
  and cross-school submissions are blocked, marked late, excused, or routed to
  review according to school rules.
- Grading and feedback visibility require teacher/reviewer permission and do
  not expose staff-only details to guardians or students unless configured.
- Accepted submissions and feedback cannot be edited or deleted directly;
  corrections append review evidence.
