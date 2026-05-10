# Contract: Transport Rules and Review Summary

This contract defines school-account transport rule setting management and
review-summary access for Phase 3 transport workflows.

## Capabilities and Permissions

- Required capabilities:
  - None beyond tenant access and permissions for rule management.
  - Related workflow capabilities apply when rule values are enforced or when
    summary filters expose workflow-specific data.
- Common permissions:
  - `transport.rules.read`
  - `transport.rules.manage`
  - `transport.review.read`
  - `transport.audit.read`

Rule settings are tenant-owned and versioned. Review summaries are read models
computed from tenant-owned transport evidence and must not expose records outside
the caller's authorized school account or guardian link scope.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/transport/rule-settings/current` | Read active Phase 3 transport rule settings |
| POST | `/api/v1/schools/{schoolAccountId}/transport/rule-settings` | Create a draft transport rule setting |
| PATCH | `/api/v1/schools/{schoolAccountId}/transport/rule-settings/{ruleSettingId}` | Update a draft or suspended rule setting |
| POST | `/api/v1/schools/{schoolAccountId}/transport/rule-settings/{ruleSettingId}/activate` | Activate a rule setting and supersede the prior active version |
| POST | `/api/v1/schools/{schoolAccountId}/transport/rule-settings/{ruleSettingId}/suspend` | Suspend an active rule setting with a reason |
| GET | `/api/v1/schools/{schoolAccountId}/transport/review-summaries` | Read paginated transport review summaries by scope and status filters |
| GET | `/api/v1/schools/{schoolAccountId}/transport/review-summaries/{summaryScope}/{scopeReference}` | Read one transport review summary with trace links |

## Rule Setting Request

```yaml
assignment_eligibility_policy: "ActiveStudentAndActiveRouteOnly"
pickup_window: "PT20M"
drop_window: "PT20M"
route_deviation_threshold: "500m"
location_staleness_threshold: "PT2M"
eta_change_threshold: "PT5M"
notification_eligibility_policy: "AcceptedOrReviewedEventsOnly"
anomaly_detection_policy:
  enabled_types:
    - "Wrong Route"
    - "Wrong Stop"
    - "Duplicate Scan"
scan_clock_drift_tolerance: "PT3M"
retry_handling_policy: "IdempotentClientIdentifiers"
location_detail_retention_days: 30
change_reason: "Start of term transport policy."
client_request_id: "request-unique-to-caller"
```

## Review Summary Response

```yaml
summary_scope: "Trip"
scope_reference: "trip-reference"
school_account_id: "school-account-reference"
transport_route_id: "route-reference"
transport_vehicle_id: "vehicle-reference"
transport_trip_id: "trip-reference"
scan_status_counts:
  accepted: 38
  needs_review: 2
location_status_counts:
  current: 1
  stale: 0
eta_state_counts:
  fresh: 5
  unavailable: 1
notification_status_counts:
  visible: 34
  suppressed: 3
anomaly_status_counts:
  open: 2
  resolved: 1
latest_evidence_at: "YYYY-MM-DDTHH:MM:SSZ"
trace_reference:
  route_trace: "route-reference"
  trip_trace: "trip-reference"
```

## Acceptance Rules

- Rule reads require tenant access and `transport.rules.read` or
  `transport.rules.manage`.
- Rule mutation requires tenant access, `transport.rules.manage`, validation of
  every policy value, idempotent `client_request_id` handling, and audit
  evidence.
- Activating a rule setting supersedes the prior active version without changing
  historical event evidence evaluated under the prior version.
- Detailed location retention remains 30 days unless an approved review hold is
  recorded under school account policy.
- Review-summary reads require tenant access and `transport.review.read`.
- Review-summary list responses must be paginated and support filters for
  student, route, bus, trip, stop, scan status, location status, ETA state,
  notification status, anomaly status, and latest evidence time.
- Guardian-scoped summaries must be limited to linked-student transport
  visibility and must not include unrelated student, route, bus, trip, or stop
  identifiers.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Actor lacks tenant access | Deny without exposing school account data |
| Actor lacks rule permission | Deny rule read or mutation and record required permission |
| Actor lacks review permission | Deny summary read and record required permission |
| Invalid rule value | Reject with field-level validation details |
| Retention days differ from 30 without approved hold | Reject activation and record retention policy reason |
| Summary filter references another tenant | Deny without exposing cross-tenant existence |
| Guardian requests unrelated summary | Suppress summary and record guardian scope reason |
| Audit write fails for rule mutation | Reject mutation rather than allowing unaudited change |
