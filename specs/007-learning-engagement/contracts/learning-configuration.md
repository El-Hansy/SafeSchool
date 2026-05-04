# Contract: Learning Configuration

This contract defines learning feature settings, course/group rule settings,
assignment policies, quiz policies, star and reward policies, behavior
visibility rules, review routing, activation validation, versioning, and audit
behavior for Phase 5.

## Capabilities and Permissions

- Required capabilities:
  - `learning.configuration`
  - Related Phase 5 capability when a rule depends on it
- Common permissions:
  - `learning.configuration.read`
  - `learning.configuration.manage`
  - `learning.audit.read`

Configuration records are tenant-owned and versioned or revision-traceable.
Historical learning records keep the rule version active when the event
occurred unless an authorized reviewer explicitly migrates or corrects the
record with reason.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/learning/feature-settings` | Read effective Phase 5 capability settings |
| POST | `/api/v1/schools/{schoolAccountId}/learning/rule-settings` | Create draft learning rule setting |
| GET | `/api/v1/schools/{schoolAccountId}/learning/rule-settings` | List learning rule settings |
| GET | `/api/v1/schools/{schoolAccountId}/learning/rule-settings/{ruleId}` | Read learning rule setting detail |
| PATCH | `/api/v1/schools/{schoolAccountId}/learning/rule-settings/{ruleId}` | Update draft or suspended rule setting |
| POST | `/api/v1/schools/{schoolAccountId}/learning/rule-settings/{ruleId}/activate` | Activate rule version |
| POST | `/api/v1/schools/{schoolAccountId}/learning/rule-settings/{ruleId}/suspend` | Suspend rule with reason |
| GET | `/api/v1/schools/{schoolAccountId}/learning/rule-settings/{ruleId}/trace` | Trace rule versions, activation, references, and audit evidence |

## Learning Rule Request

```yaml
rule_area: "Assignment"
rule_payload:
  late_policy: "Route Late To Review"
  resubmission_policy: "Teacher Allowed"
  guardian_feedback_visibility: "Summary"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_to: null
change_reason: "Default assignment policy."
client_request_id: "learning-rule-unique-to-caller"
```

## Feature Settings Response

```yaml
school_account_id: "school-reference"
course_content_delivery_enabled: true
assignment_tracking_enabled: true
quiz_engine_enabled: true
stars_rewards_enabled: true
behavior_logging_enabled: true
learning_history_enabled: true
learning_configuration_enabled: true
learning_review_summaries_enabled: true
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Configuration activation requires tenant access, enabled
  `learning.configuration`, required management permission, valid dependent
  capabilities, valid rule payload, and audit evidence.
- Rule settings are rejected when they reference disabled dependent features,
  impossible due windows, invalid quiz attempt limits, invalid star or reward
  behavior, invalid visibility settings, or missing review routing.
- Rule changes create new versions or revision evidence instead of mutating
  historical learning records in place.
- Feature setting reads are tenant-scoped and never authorize behavior by
  themselves; backend feature enforcement remains required on every mutation.
- List responses are paginated and tenant-scoped.
