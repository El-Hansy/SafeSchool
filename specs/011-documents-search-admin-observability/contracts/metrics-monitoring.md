# Contract: Metrics, Alerts, and Incidents

This contract defines metric observation review, alert rule configuration,
alert creation, alert acknowledgement, incident lifecycle, operational
exception review, dashboard integration, and audit behavior for Phase 11.

## Capabilities and Permissions

- Required capabilities:
  - `admin.metrics_monitoring`
  - `admin.alerting`
  - `admin.incidents`
  - `admin.operational_exceptions`
- Common permissions:
  - `monitoring.metrics.read`
  - `monitoring.alert_rules.manage`
  - `monitoring.alerts.read`
  - `monitoring.alerts.manage`
  - `monitoring.incidents.read`
  - `monitoring.incidents.manage`
  - `monitoring.audit.read`

Metrics, alerts, incidents, and operational exceptions are tenant-scoped or
explicitly platform-scoped, permission-scoped, lifecycle-tracked, and
auditable. Platform-scope views require explicit platform authority.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/monitoring/metrics` | Search metric observations |
| GET | `/api/v1/schools/{schoolAccountId}/monitoring/alert-rules` | List alert rules |
| POST | `/api/v1/schools/{schoolAccountId}/monitoring/alert-rules` | Create alert rule draft |
| POST | `/api/v1/schools/{schoolAccountId}/monitoring/alert-rules/{ruleId}/activate` | Activate alert rule version |
| GET | `/api/v1/schools/{schoolAccountId}/monitoring/alerts` | Search alerts |
| POST | `/api/v1/schools/{schoolAccountId}/monitoring/alerts/{alertId}/acknowledge` | Acknowledge alert |
| POST | `/api/v1/schools/{schoolAccountId}/monitoring/alerts/{alertId}/resolve` | Resolve alert |
| GET | `/api/v1/schools/{schoolAccountId}/monitoring/incidents` | Search incidents |
| POST | `/api/v1/schools/{schoolAccountId}/monitoring/incidents` | Create incident |
| POST | `/api/v1/schools/{schoolAccountId}/monitoring/incidents/{incidentId}/update` | Update incident lifecycle |
| GET | `/api/v1/schools/{schoolAccountId}/monitoring/exceptions` | Search operational exceptions |
| POST | `/api/v1/schools/{schoolAccountId}/monitoring/exceptions/{exceptionId}/review` | Resolve, dismiss, or escalate exception |
| GET | `/api/v1/platform/monitoring/alerts` | Search platform alerts where authorized |
| GET | `/api/v1/platform/monitoring/incidents` | Search platform incidents where authorized |

## Metric Query

```yaml
metric_name: "documents.upload.failure_count"
metric_source: "Documents"
metric_scope: "School"
data_quality_state: "Complete"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
page: 1
page_size: 50
```

## Alert Rule Request

```yaml
rule_name: "Document upload failures"
metric_name: "documents.upload.failure_count"
condition: "Greater Than 5 In 10 Minutes"
severity: "Elevated"
owner_queue: "Operations Review"
suppression_policy: "Suppress Duplicate"
notification_eligible: true
client_request_id: "alert-rule-unique-to-admin"
```

## Alert Action Request

```yaml
action_reason: "Operations reviewer acknowledged investigation."
assign_to_queue: "Document Operations"
client_request_id: "alert-action-unique-to-reviewer"
```

## Incident Create Request

```yaml
incident_title: "Search indexing delays"
incident_summary: "Document and certificate indexing delayed for one school."
severity: "Elevated"
linked_alert_ids:
  - "alert-reference"
linked_record_references:
  - "search-index-state-reference"
client_request_id: "incident-create-unique-to-reviewer"
```

## Incident Update Request

```yaml
incident_status: "Resolved"
severity: "Normal"
resolution_summary: "Delayed index jobs were replayed and freshness recovered."
update_reason: "Search freshness returned to normal."
client_request_id: "incident-update-unique-to-reviewer"
```

## Acceptance Rules

- Metric, alert, incident, and exception routes require tenant access or
  explicit platform scope, enabled capability, actor permission, scoped
  filters, and audit evidence.
- Alert rules require valid metric source, condition, severity, owner,
  suppression behavior, review behavior, and dependent capabilities.
- Alert actions preserve actor, time, reason, owner, current state, and audit
  evidence.
- Incident lifecycle changes require operations authority and preserve status,
  severity, owner, linked evidence, resolution, reason, and audit history.
- Missing, delayed, duplicated, noisy, or inconsistent metrics create
  data-quality or review-required evidence instead of being silently ignored
  when they affect alert evaluation.
- Alert and incident behavior must not create side effects in excluded source
  domains.
