# Contract: Behavior Logging

This contract defines behavior category configuration, behavior event creation,
visibility, dispute and correction handling, star impact, duplicate handling,
and audit behavior for Phase 5.

## Capabilities and Permissions

- Required capabilities:
  - `learning.behavior_logging`
  - `learning.stars_rewards` when behavior affects stars
- Common permissions:
  - `learning.behavior.read`
  - `learning.behavior.create`
  - `learning.behavior.review`
  - `learning.guardian_history.read`
  - `learning.audit.read`

Behavior events are engagement evidence only. They do not create medical,
complaint, emergency, or disciplinary case-management workflows.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/learning/behavior/categories` | Create behavior category |
| POST | `/api/v1/schools/{schoolAccountId}/learning/behavior/events` | Record behavior event |
| GET | `/api/v1/schools/{schoolAccountId}/learning/behavior/events` | List behavior events with filters |
| GET | `/api/v1/students/me/learning/behavior` | List student-visible behavior events |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/learning/behavior` | List guardian-visible linked-student behavior events |
| POST | `/api/v1/schools/{schoolAccountId}/learning/behavior/events/{eventId}/review` | Correct, dismiss, dispute, or resolve behavior event |
| GET | `/api/v1/schools/{schoolAccountId}/learning/behavior/events/{eventId}/trace` | Trace behavior event to stars, exceptions, reviews, and audit evidence |

## Behavior Category Request

```yaml
category_name: "Class participation"
behavior_classification: "Positive"
default_severity: "Low"
default_visibility_policy: "Guardian Summary"
star_rule_setting_id: "star-rule-reference"
client_request_id: "behavior-category-unique-to-caller"
```

## Behavior Event Request

```yaml
student_profile_id: "student-reference"
behavior_category_id: "category-reference"
classification: "Positive"
severity: "Low"
source_context: "Course"
staff_note: "Participated actively in group work."
visibility_policy: "Guardian Summary"
occurred_at: "YYYY-MM-DDTHH:MM:SSZ"
client_request_id: "behavior-event-unique-to-caller"
```

## Behavior Review Request

```yaml
review_action: "Correct"
review_reason: "Recorded for the wrong severity."
resulting_visibility_policy: "Staff Only"
client_request_id: "behavior-review-unique-to-reviewer"
```

## Behavior Event Response

```yaml
behavior_event_id: "behavior-event-reference"
student_profile_id: "student-reference"
classification: "Positive"
severity: "Low"
review_state: "Accepted"
guardian_visible: true
related_star_ledger_entry_id: "ledger-reference"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Behavior creation requires tenant access, enabled behavior logging, active
  student status, staff authority for the student/group/course or reviewer
  scope, valid category, visibility policy, and audit evidence.
- Sensitive behavior categories default to restricted student/guardian
  visibility unless school rules explicitly allow disclosure.
- Behavior events that affect stars use active star rules and preserve the
  behavior-to-ledger trace.
- Duplicate behavior event requests are idempotent by client request and source
  context.
- Disputed or corrected events preserve the original event, reason, actor,
  visibility, star impact, and resulting status.
- Behavior logging must not create medical, emergency, complaint, request,
  wallet, attendance, transport, document, or messaging outcomes.
