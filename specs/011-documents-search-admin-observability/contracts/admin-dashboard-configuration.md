# Contract: Admin Dashboard and Tenant Configuration

This contract defines admin dashboard summaries, tenant feature settings,
configuration changes, dependency validation, review routing, versioning, and
audit behavior for Phase 11.

## Capabilities and Permissions

- Required capabilities:
  - `admin.dashboard`
  - `admin.tenant_configuration`
  - `admin.configuration_review` when approval is required
- Common permissions:
  - `admin.dashboard.read`
  - `admin.configuration.read`
  - `admin.configuration.manage`
  - `admin.configuration.approve`
  - `admin.audit.read`

Dashboard summaries and configuration changes are tenant-scoped,
permission-scoped, versioned, dependency-validated, and auditable. Platform
scope requires explicit platform authority.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/admin/dashboard` | Read permission-scoped school admin dashboard |
| GET | `/api/v1/schools/{schoolAccountId}/admin/features` | List tenant feature settings |
| PUT | `/api/v1/schools/{schoolAccountId}/admin/features/{featureKey}` | Update one tenant feature setting |
| GET | `/api/v1/schools/{schoolAccountId}/admin/features/history` | Search configuration change history |
| POST | `/api/v1/schools/{schoolAccountId}/admin/features/{changeId}/approve` | Approve pending configuration change |
| POST | `/api/v1/schools/{schoolAccountId}/admin/features/{changeId}/reject` | Reject pending configuration change |
| GET | `/api/v1/platform/admin/dashboard` | Read platform dashboard where authorized |

## Dashboard Query

```yaml
module_key: "Documents"
summary_scope: "School"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
include_open_exceptions: true
include_pending_reviews: true
```

## Dashboard Response

```yaml
summary_scope: "School"
enabled_feature_count: 8
disabled_feature_count: 2
pending_review_count: 5
open_exception_count: 3
document_active_count: 240
certificate_issued_count: 42
search_stale_count: 1
audit_activity_count: 1200
open_alert_count: 2
open_incident_count: 1
generated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Feature Setting Request

```yaml
feature_key: "documents.global_search"
setting_state: "Enabled"
setting_value:
  guardian_search_enabled: true
  student_search_enabled: false
  max_page_size: 25
effective_from: "YYYY-MM-DDTHH:MM:SSZ"
change_reason: "Enable guardian access to permitted document and certificate search."
client_request_id: "feature-setting-unique-to-admin"
```

## Configuration Change Response

```yaml
configuration_change_id: "configuration-change-reference"
feature_key: "documents.global_search"
prior_setting_state: "Disabled"
new_setting_state: "Enabled"
dependency_decision: "Valid"
approval_state: "Approved"
effective_from: "YYYY-MM-DDTHH:MM:SSZ"
version: 4
```

## Acceptance Rules

- Dashboard reads require tenant access, enabled dashboard capability, actor
  permission, scoped summaries, and audit evidence.
- Dashboard counts and summaries cannot reveal restricted records, hidden
  search results, cross-school data, or platform operations details beyond the
  actor's authority.
- Configuration reads and changes require tenant access, enabled
  configuration capability, administrator authority, valid feature key,
  dependency validation, reason capture, and audit evidence.
- Configuration changes preserve prior setting, new setting, actor, reason,
  time, effective window, dependency decision, approval state, and version
  history.
- Invalid, unsafe, cross-school, dependency-breaking, or mandatory-safety
  changes are rejected or routed to review without applying partial effects.
- Configuration behavior must not create side effects in excluded source
  domains.
