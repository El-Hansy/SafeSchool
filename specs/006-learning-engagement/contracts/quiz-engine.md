# Contract: Quiz Engine

This contract defines quiz creation, activation, attempt start and completion,
scoring, feedback visibility, idempotency, review handling, and audit behavior
for Phase 5.

## Capabilities and Permissions

- Required capabilities:
  - `learning.quizzes`
  - `learning.progress_history` when quiz history is read
- Common permissions:
  - `learning.quizzes.manage`
  - `learning.quizzes.review`
  - `learning.quizzes.attempt`
  - `learning.quizzes.read`
  - `learning.guardian_history.read`
  - `learning.audit.read`

Quiz attempts are tenant-scoped, student-scoped, and idempotent. Question and
scoring revisions are preserved after attempts exist.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/learning/quizzes` | Create a draft quiz |
| POST | `/api/v1/schools/{schoolAccountId}/learning/quizzes/{quizId}/activate` | Activate quiz revision |
| POST | `/api/v1/schools/{schoolAccountId}/learning/quizzes/{quizId}/suspend` | Suspend quiz with reason |
| GET | `/api/v1/students/me/learning/quizzes` | List student-visible quizzes |
| POST | `/api/v1/students/me/learning/quizzes/{quizId}/attempts/start` | Start a quiz attempt |
| POST | `/api/v1/students/me/learning/quizzes/{quizId}/attempts/{attemptId}/submit` | Submit quiz responses |
| POST | `/api/v1/schools/{schoolAccountId}/learning/quizzes/{quizId}/attempts/{attemptId}/review` | Review, score, correct, or void an attempt |
| GET | `/api/v1/schools/{schoolAccountId}/learning/quizzes/{quizId}/trace` | Trace quiz to attempts, scoring, stars, exceptions, reviews, and audit evidence |

## Quiz Request

```yaml
course_id: "course-reference"
learning_group_id: "group-reference"
quiz_title: "Fractions quiz"
opens_at: "YYYY-MM-DDTHH:MM:SSZ"
closes_at: "YYYY-MM-DDTHH:MM:SSZ"
attempt_limit: 2
time_limit_minutes: 30
scoring_policy: "Hybrid"
feedback_visibility: "Teacher Released"
questions:
  - question_type: "Multiple Choice"
    prompt_reference: "question-reference"
    answer_key_reference: "answer-key-reference"
    points_possible: 5
    required: true
client_request_id: "quiz-unique-to-caller"
```

## Attempt Start Response

```yaml
quiz_attempt_id: "attempt-reference"
quiz_id: "quiz-reference"
student_profile_id: "student-reference"
attempt_number: 1
attempt_status: "Started"
started_at: "YYYY-MM-DDTHH:MM:SSZ"
expires_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Attempt Submit Request

```yaml
responses:
  - quiz_question_id: "question-reference"
    response_reference: "answer-reference"
client_request_id: "quiz-submit-unique-to-student"
```

## Attempt Result Response

```yaml
quiz_attempt_id: "attempt-reference"
attempt_status: "Scored"
score_status: "Auto Scored"
score_value: 88
feedback_visible: false
submitted_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Quiz activation requires tenant access, enabled `learning.quizzes`, staff
  authority, target learners, active question revision, valid schedule, valid
  attempt limit, scoring policy, feedback policy, and audit evidence.
- Student attempt start requires active student status, eligible learning
  scope, active quiz, valid attempt window, available attempt count, and
  idempotent client identity.
- Attempt submission validates required answers, timer/window state, duplicate
  completion, and student ownership.
- Expired, duplicate, over-limit, out-of-window, missing-answer, ineligible, or
  cross-school attempts are rejected, finalized, or routed to review according
  to quiz rules without corrupting prior attempts.
- Scoring corrections create new review evidence and preserve the original
  question set and answer key revision used by the attempt.
- Feedback visibility follows quiz settings and role permission.
