# Contract: Permission Enforcement

This contract defines role, permission, assignment, feature-gate, access
decision, and audit behavior for Phase 1 identity and access workflows.

## Capabilities and Permissions

- Required capabilities:
  - `identity.role_administration`
  - `identity.permission_enforcement`
- Common permissions:
  - `identity.roles.read`
  - `identity.roles.manage`
  - `identity.permissions.read`
  - `identity.permissions.manage`
  - `identity.role_assignments.manage`
  - `identity.access_decisions.read`
  - `identity.audit.read`

Permission enforcement is mandatory for every Phase 1 workflow, even when role
administration screens are disabled for a school account.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/identity/roles` | List tenant roles |
| POST | `/api/v1/schools/{schoolAccountId}/identity/roles` | Create a tenant role |
| PATCH | `/api/v1/schools/{schoolAccountId}/identity/roles/{roleId}` | Update role details or status |
| GET | `/api/v1/schools/{schoolAccountId}/identity/permissions` | List available permissions |
| PUT | `/api/v1/schools/{schoolAccountId}/identity/roles/{roleId}/permissions` | Replace role permission set |
| POST | `/api/v1/schools/{schoolAccountId}/identity/role-assignments` | Assign a role to an actor |
| PATCH | `/api/v1/schools/{schoolAccountId}/identity/role-assignments/{assignmentId}` | Change assignment state or validity |
| GET | `/api/v1/schools/{schoolAccountId}/identity/access-decisions` | Review allow/deny decisions with filters |
| GET | `/api/v1/schools/{schoolAccountId}/identity/audit-events` | Review Phase 1 audit events |

## Role Assignment Request

```yaml
actor_reference: "user-or-service-reference"
role_id: "role-reference"
assignment_status: "Active"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_until: null
review_reason: "Authorized staff member for identity workflows."
client_request_id: "request-unique-to-caller"
```

## Access Decision Record

```yaml
access_decision_id: "access-decision-reference"
school_account_id: "school-account-reference"
actor_reference: "user-or-service-reference"
attempted_action: "identity.guardian_links.create"
target_type: "Guardian Link"
target_reference: "guardian-link-reference"
decision: "Denied"
decision_reason: "Actor lacks identity.guardian_links.create in this school account."
role_sources:
  - "Staff Viewer"
feature_capability_key: "identity.guardian_linking"
decided_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Audit Event Record

```yaml
audit_event_id: "audit-event-reference"
school_account_id: "school-account-reference"
event_category: "Access Decision"
event_type: "identity.guardian_links.create.denied"
actor_reference: "user-or-service-reference"
subject_type: "Guardian Link"
subject_reference: "guardian-link-reference"
previous_value_summary: null
new_value_summary: null
reason: "Actor lacks permission."
access_decision_id: "access-decision-reference"
event_time: "YYYY-MM-DDTHH:MM:SSZ"
review_status: "Recorded"
```

## Acceptance Rules

- Every sensitive Phase 1 action must check tenant access, feature capability,
  role assignment state, permission availability, and target ownership before
  business logic runs.
- Missing tenant access, disabled capability, inactive role assignment, missing
  permission, or invalid target state must deny the action.
- Denied sensitive actions must create an Access Decision and Audit Event.
- Allowed sensitive changes must create an Audit Event with previous and new
  value summaries when applicable.
- A user with roles in multiple school accounts must be evaluated only against
  the active school account unless a platform-level review role is used.
- Role and permission administration changes require their own permission
  checks and audit evidence.
- UI feature gates may hide unavailable workflows but cannot be the only
  enforcement point.

## Default Permission Families

| Family | Examples |
|--------|----------|
| Student profile | read, create, update, deactivate, review history |
| Guardian | read, create, update, manage links, review history |
| Credential | read, issue, suspend, restore, replace, revoke, rotate QR, review history |
| Role administration | read roles, manage roles, assign roles |
| Permission administration | read permissions, manage permission sets |
| Audit and review | read audit events, read access decisions |

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Feature capability disabled | Deny action, record capability reason, and emit audit event |
| Actor lacks tenant access | Deny action without exposing tenant data |
| Actor lacks permission | Deny action and record required permission |
| Role assignment inactive or expired | Deny action and record assignment state |
| Target belongs to another tenant | Deny action without exposing cross-tenant existence |
| Audit write fails for sensitive change | Reject sensitive change rather than allowing unaudited mutation |
