# Contract: Medical Configuration

This contract defines medical capability settings, visibility rules,
emergency access rules, break-glass rules, medication evidence rules, incident
severity rules, notification audience defaults, acknowledgement rules,
review routing, versioning, and audit behavior for Phase 7.

## Capabilities and Permissions

- Required capabilities:
  - `medical.configuration`
  - Related Phase 7 capability when a rule depends on it
- Common permissions:
  - `medical.configuration.read`
  - `medical.configuration.manage`
  - `medical.audit.read`

Configuration records are tenant-owned and versioned or revision-traceable.
Historical medical records keep the rule version active when the event
occurred unless an authorized reviewer explicitly migrates or corrects the
record with reason.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/medical/feature-settings` | Read effective Phase 7 capability settings |
| POST | `/api/v1/schools/{schoolAccountId}/medical/rule-settings` | Create draft medical rule setting |
| GET | `/api/v1/schools/{schoolAccountId}/medical/rule-settings` | List medical rule settings |
| GET | `/api/v1/schools/{schoolAccountId}/medical/rule-settings/{ruleId}` | Read medical rule setting detail |
| PATCH | `/api/v1/schools/{schoolAccountId}/medical/rule-settings/{ruleId}` | Update draft or suspended rule setting |
| POST | `/api/v1/schools/{schoolAccountId}/medical/rule-settings/{ruleId}/activate` | Activate medical rule version |
| POST | `/api/v1/schools/{schoolAccountId}/medical/rule-settings/{ruleId}/suspend` | Suspend medical rule with reason |
| GET | `/api/v1/schools/{schoolAccountId}/medical/rule-settings/{ruleId}/trace` | Trace rule versions, activation, references, and audit evidence |

## Medical Rule Request

```yaml
rule_area: "Emergency Access"
rule_payload:
  emergency_session_minutes: 30
  offline_cache_freshness_hours: 24
  break_glass_roles:
    - "Emergency Coordinator"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_to: null
change_reason: "Default emergency access policy."
client_request_id: "medical-rule-unique-to-caller"
```

## Feature Settings Response

```yaml
school_account_id: "school-reference"
medical_records_enabled: true
emergency_access_enabled: true
medical_incidents_enabled: true
medical_notifications_enabled: true
medical_history_enabled: true
medical_configuration_enabled: true
medical_review_summaries_enabled: true
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Configuration activation requires tenant access, enabled
  `medical.configuration`, required management permission, valid dependent
  capabilities, valid rule payload, and audit evidence.
- Rules are rejected when they reference disabled dependent features, invalid
  break-glass roles, emergency session durations other than the approved
  30-minute default, offline cache freshness greater than 24 hours, invalid
  medication evidence rules, unsafe visibility rules, or missing review
  routing.
- Rule changes create new versions or revision evidence instead of mutating
  historical medical records in place.
- Feature setting reads are tenant-scoped and never authorize behavior by
  themselves; backend feature enforcement remains required on every protected
  action.
- List responses are paginated and tenant-scoped.
