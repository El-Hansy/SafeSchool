# Contract: Communication Configuration

This contract defines communication feature settings, templates, audience
rules, notification rules, channel rules, quiet-hour behavior, acknowledgement
rules, moderation rules, preference policies, versioning, and audit behavior
for Phase 9.

## Capabilities and Permissions

- Required capabilities:
  - `communications.configuration`
  - `communications.templates`
  - `communications.preferences`
  - `communications.moderation`
- Common permissions:
  - `communications.configuration.read`
  - `communications.configuration.manage`
  - `communications.templates.manage`
  - `communications.audience_rules.manage`
  - `communications.preferences.manage`
  - `communications.audit.read`

Communication configuration is tenant-scoped, versioned, permission-scoped,
and auditable. Historical communications preserve the configuration version
that governed each relevant action.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/communications/configuration` | Read communication feature settings |
| PUT | `/api/v1/schools/{schoolAccountId}/communications/configuration` | Update communication feature settings |
| GET | `/api/v1/schools/{schoolAccountId}/communications/templates` | List communication templates |
| POST | `/api/v1/schools/{schoolAccountId}/communications/templates` | Create template draft |
| PUT | `/api/v1/schools/{schoolAccountId}/communications/templates/{templateId}` | Update template draft |
| POST | `/api/v1/schools/{schoolAccountId}/communications/templates/{templateId}/activate` | Activate template version |
| POST | `/api/v1/schools/{schoolAccountId}/communications/templates/{templateId}/archive` | Archive template version |
| GET | `/api/v1/schools/{schoolAccountId}/communications/audience-rules` | List audience rules |
| POST | `/api/v1/schools/{schoolAccountId}/communications/audience-rules` | Create audience rule draft |
| POST | `/api/v1/schools/{schoolAccountId}/communications/audience-rules/{ruleId}/activate` | Activate audience rule |
| GET | `/api/v1/guardians/me/communication-preferences` | Read guardian preferences |
| PUT | `/api/v1/guardians/me/communication-preferences` | Update guardian optional preferences |
| GET | `/api/v1/students/me/communication-preferences` | Read student preferences where enabled |
| PUT | `/api/v1/students/me/communication-preferences` | Update student optional preferences where enabled |

## Feature Settings Request

```yaml
direct_messaging_enabled: true
staff_guardian_messaging_enabled: true
student_messaging_enabled: false
broadcasts_enabled: true
announcements_enabled: true
notification_center_enabled: true
external_delivery_enabled: true
templates_enabled: true
delivery_tracking_enabled: true
acknowledgements_enabled: true
preferences_enabled: true
history_enabled: true
moderation_enabled: true
review_summaries_enabled: true
default_quiet_hours:
  starts_at: "21:00"
  ends_at: "07:00"
mandatory_categories:
  - "Safety"
  - "Emergency"
  - "Attendance"
bulk_recipient_limit: 500
client_request_id: "communication-settings-unique-to-admin"
```

## Template Request

```yaml
template_code: "ATTENDANCE_ENTRY"
template_name: "Attendance Entry Notification"
communication_kind: "Notification"
category: "Attendance"
language: "en"
title_template: "Attendance update"
body_template: "{{student_name}} arrived on campus."
required_variables:
  - "student_name"
default_priority: "Normal"
default_audience_rule_id: "approved-guardians"
restricted_detail_policy: "Summary Only"
acknowledgement_behavior: "None"
quiet_hour_behavior: "Delay Optional"
channel_behavior:
  in_app: true
  push: true
  email: false
moderation_required: false
client_request_id: "template-config-unique-to-admin"
```

## Preference Request

```yaml
preferences:
  - category: "School Announcement"
    channel: "Email"
    preference_state: "Enabled"
  - category: "Learning"
    channel: "Push"
    preference_state: "Disabled"
client_request_id: "preference-update-unique-to-user"
```

## Acceptance Rules

- Configuration changes require tenant access, enabled configuration
  capability, administrator authority, valid settings, and audit evidence.
- Template activation requires valid title, body, variables, language,
  audience, priority, restricted-detail policy, acknowledgement behavior,
  quiet-hour behavior, channel behavior, and dependent capabilities.
- Audience rule activation requires valid recipient criteria, valid recipient
  types, no cross-school expansion, and restricted-detail policy.
- Preference updates apply only to optional categories and cannot disable
  mandatory categories governed by school policy.
- Invalid templates, missing audience rules, unsupported language variants,
  disabled channels, invalid quiet-hour behavior, or mandatory category
  conflicts reject activation with a reviewable reason.
- Configuration changes create new versions; historical communications keep
  the version active at generation, publication, delivery, or acknowledgement.
