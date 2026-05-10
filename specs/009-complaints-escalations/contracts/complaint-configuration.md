# Contract: Complaint Configuration

This contract defines feature settings, complaint categories, required fields,
confidentiality rules, owner groups, target timings, escalation rules, feedback
rules, reopen rules, versioning, and audit behavior for Phase 8.

## Capabilities and Permissions

- Required capabilities:
  - `complaints.configuration`
- Common permissions:
  - `complaints.configuration.read`
  - `complaints.configuration.manage`
  - `complaints.categories.manage`
  - `complaints.escalation_rules.manage`
  - `complaints.audit.read`

Complaint configuration is tenant-scoped, versioned, permission-scoped, and
auditable. Historical complaints preserve the configuration version that
governed each relevant action.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/complaints/configuration` | Read complaint feature settings |
| PUT | `/api/v1/schools/{schoolAccountId}/complaints/configuration` | Update complaint feature settings |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/categories` | List complaint categories |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/categories` | Create complaint category draft |
| PUT | `/api/v1/schools/{schoolAccountId}/complaints/categories/{categoryId}` | Update complaint category draft |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/categories/{categoryId}/activate` | Activate category version |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/categories/{categoryId}/archive` | Archive category version |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/escalation-rules` | List escalation rules |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/escalation-rules` | Create or update escalation rule draft |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/escalation-rules/{ruleId}/activate` | Activate escalation rule version |

## Category Configuration Request

```yaml
category_code: "TRANSPORT"
category_name: "Transport Complaint"
allowed_submitter_roles:
  - "Guardian"
  - "Student"
  - "Staff"
required_fields:
  - "description"
  - "event_window"
default_priority: "Normal"
default_confidentiality_level: "Standard"
owner_group_reference: "TransportComplaintQueue"
target_response_duration: "P1D"
target_resolution_duration: "P5D"
restricted_handling_required: false
safety_review_required: false
feedback_rule_reference: "standard-feedback"
reopen_rule_reference: "standard-reopen"
client_request_id: "category-config-unique-to-caller"
```

## Escalation Rule Request

```yaml
complaint_category_id: "complaint-category-reference"
rule_name: "High priority transport escalation"
trigger_type: "Target Expired"
trigger_condition: "target_resolution_at has passed"
target_owner_type: "Queue"
target_owner_reference: "EscalationReviewerQueue"
priority_after_escalation: "High"
target_response_duration: "PT4H"
visibility_constraints:
  - "Restricted"
client_request_id: "escalation-rule-unique-to-caller"
```

## Acceptance Rules

- Configuration changes require tenant access, enabled configuration
  capability, administrator authority, valid settings, and audit evidence.
- Category activation requires valid owner group, allowed submitter roles,
  required fields, target timings, confidentiality rules, feedback rules, and
  any required escalation routes.
- Escalation rule activation requires valid target owner, non-circular route,
  valid trigger, valid timing, and enabled escalation capability.
- Invalid owner groups, impossible target timings, circular escalations,
  disabled dependent capabilities, invalid confidentiality settings, or
  unreviewable conflict-of-interest risk reject activation with a reviewable
  reason.
- Changing a category or escalation rule creates a new version; historical
  complaints keep the version active at the time of submission,
  categorization, escalation, resolution, or reopen.
- Configuration reads and changes are audit-visible.
