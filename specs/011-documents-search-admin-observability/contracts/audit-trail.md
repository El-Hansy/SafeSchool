# Contract: Audit Trail

This contract defines audit event review, audit filtering, sensitive payload
minimization, audit export, export evidence, append-only correction behavior,
and audit access control for Phase 11.

## Capabilities and Permissions

- Required capabilities:
  - `admin.audit_trail`
  - `admin.audit_exports` when exports are requested
- Common permissions:
  - `audit.events.read`
  - `audit.events.read.restricted`
  - `audit.exports.create`
  - `audit.exports.read`
  - `audit.platform.read`

Audit events are tenant-scoped or explicitly platform-scoped, append-only from
a reviewer perspective, permission-scoped, export-controlled, and auditable.
Audit trail access and exports create audit evidence.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/audit/events` | Search permitted audit events |
| GET | `/api/v1/schools/{schoolAccountId}/audit/events/{auditEventId}` | Read audit event detail |
| POST | `/api/v1/schools/{schoolAccountId}/audit/exports` | Request controlled audit export |
| GET | `/api/v1/schools/{schoolAccountId}/audit/exports` | List audit exports |
| GET | `/api/v1/schools/{schoolAccountId}/audit/exports/{exportId}` | Read audit export status |
| GET | `/api/v1/platform/audit/events` | Search platform audit events where authorized |

## Audit Query

```yaml
actor_id: "actor-reference"
actor_role: "SchoolAdministrator"
student_profile_id: "student-reference"
source_module: "Documents"
action_name: "DocumentDownloaded"
target_record_type: "Document"
target_record_reference: "document-reference"
result: "Success"
risk_level: "Normal"
correlation_reference: "operation-reference"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
page: 1
page_size: 50
```

## Audit Event Response

```yaml
audit_event_id: "audit-reference"
source_module: "Documents"
action_name: "DocumentDownloaded"
actor_id: "actor-reference"
actor_role: "DocumentManager"
target_record_type: "Document"
target_record_reference: "document-reference"
result: "Success"
reason: "Authorized download"
risk_level: "Normal"
payload_visibility: "Minimized"
occurred_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Audit Export Request

```yaml
export_scope: "Source Module"
source_module: "Documents"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
included_fields:
  - "actor"
  - "action"
  - "target"
  - "result"
  - "occurred_at"
export_reason: "Compliance review for document activity."
client_request_id: "audit-export-unique-to-reviewer"
```

## Acceptance Rules

- Audit search, event reads, and exports require tenant access or explicit
  platform scope, enabled audit capability, actor permission, scoped filters,
  and audit evidence.
- Audit entries preserve action, actor, actor role, school account, target
  record, source module, result, time, reason where required, correlation
  reference, risk level, and restricted payload visibility rules.
- Restricted audit payloads are minimized or withheld unless the reviewer has
  explicit restricted audit authority.
- Audit events and audit exports are append-only from a reviewer perspective.
  Corrections or redactions create new evidence and do not erase original
  events except through a separate authorized privacy masking process.
- Audit exports require export capability, export permission, validated scope,
  permitted fields, reason capture, retention policy, and audit evidence.
- Audit trail behavior must not create side effects in excluded source domains.
