# Contract: Request Configuration

This contract defines request type configuration, workflow template management,
workflow version activation, required fields, consent policies, escalation
rules, duplicate policies, star rule attachment, and feature gating for Phase 6.

## Capabilities and Permissions

- Required capabilities:
  - `requests.configuration`
  - `requests.approval_workflow`
  - Related request capabilities when activated request types depend on them
- Common permissions:
  - `requests.workflows.read`
  - `requests.workflows.manage`
  - `requests.star_rules.read`
  - `requests.requests.read`
  - `requests.audit.read`

Configuration records are tenant-owned and versioned. Historical requests keep
the request type and workflow version active at submission unless an authorized
review explicitly migrates or reopens the request with reason.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/requests/request-types` | Create a draft request type |
| GET | `/api/v1/schools/{schoolAccountId}/requests/request-types` | List request types |
| GET | `/api/v1/schools/{schoolAccountId}/requests/request-types/{requestTypeId}` | Read request type detail |
| PATCH | `/api/v1/schools/{schoolAccountId}/requests/request-types/{requestTypeId}` | Update a draft or suspended request type |
| POST | `/api/v1/schools/{schoolAccountId}/requests/request-types/{requestTypeId}/activate` | Activate request type version |
| POST | `/api/v1/schools/{schoolAccountId}/requests/request-types/{requestTypeId}/suspend` | Suspend request type with reason |
| POST | `/api/v1/schools/{schoolAccountId}/requests/workflow-templates` | Create a draft workflow template |
| GET | `/api/v1/schools/{schoolAccountId}/requests/workflow-templates` | List workflow templates |
| GET | `/api/v1/schools/{schoolAccountId}/requests/workflow-templates/{workflowTemplateId}` | Read workflow template detail |
| PATCH | `/api/v1/schools/{schoolAccountId}/requests/workflow-templates/{workflowTemplateId}` | Update a draft workflow template |
| POST | `/api/v1/schools/{schoolAccountId}/requests/workflow-templates/{workflowTemplateId}/activate` | Activate workflow template version |
| GET | `/api/v1/schools/{schoolAccountId}/requests/feature-settings` | Read effective Phase 6 capability settings |

## Request Type Configuration

```yaml
request_type_code: "EARLY_LEAVE"
request_type_name: "Early Leave"
request_category: "Early Leave"
allowed_initiator_roles:
  - "Guardian"
  - "Staff"
required_field_schema:
  required_fields:
    - "release_at"
    - "release_reason"
    - "pickup_person"
guardian_consent_policy: "One Authorized Guardian"
workflow_template_id: "workflow-template-reference"
star_permission_rule_id: null
expiry_policy:
  approval_valid_for_minutes: 240
duplicate_policy:
  exact_active_duplicates: "Block"
  overlapping_non_identical: "Manual Review"
closure_policy:
  closure_required: false
change_reason: "Enable early leave workflow."
client_request_id: "request-type-config-unique-to-caller"
```

## Workflow Template Configuration

```yaml
template_name: "Early Leave Standard Approval"
steps:
  - step_key: "guardian-consent"
    step_type: "Guardian Consent"
    assigned_role: "Guardian"
    required_permission: "requests.guardian_consent.act"
    reason_required: false
    expiry_duration_minutes: 120
    escalation_target: "Queue:RequestReview"
  - step_key: "staff-approval"
    step_type: "Approval"
    assigned_role: "Request Manager"
    required_permission: "requests.decisions.act"
    reason_required: true
    expiry_duration_minutes: 180
    escalation_target: "Queue:RequestReview"
change_reason: "Initial early leave approval workflow."
client_request_id: "workflow-template-unique-to-caller"
```

## Acceptance Rules

- Activation requires tenant access, enabled `requests.configuration`, required
  management permission, and audit evidence.
- Request type activation is rejected when required fields are missing, no
  valid approver path exists, workflow steps are circular, escalation timing is
  impossible, consent settings are invalid, dependent features are disabled, or
  star rules cannot be evaluated under enabled capabilities.
- Workflow step expiry defaults to manual review or configured escalation while
  keeping the request pending.
- Guardian consent defaults to one authorized guardian unless the request type
  configures stricter consent.
- Request type and workflow template updates create new versions instead of
  mutating historical request evidence.
- List responses are paginated and tenant-scoped.
