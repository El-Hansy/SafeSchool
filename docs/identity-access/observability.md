# Identity Access Observability

## Audit Events

Identity Access records audit evidence for profile, guardian, and credential lifecycle changes through `IAuditWriter`. Audit events include tenant, category, event type, actor, subject, reason, and event time.

Recommended event categories:

- `Student Profile`
- `Guardian Link`
- `Credential`
- `Role Administration`
- `Permission Administration`
- `Access Review`

## Access Decisions

Denied and allowed sensitive actions are represented by `AccessDecision`. Reviewer dashboards should group by tenant, actor, feature capability, attempted action, and denial reason.

## Logs And Metrics

Recommended operational signals:

- Count of denied sensitive actions by capability and permission.
- Count of disabled-capability attempts by school account.
- Student duplicate-check outcomes by status.
- Guardian link state changes by relationship type and school account.
- Credential lifecycle changes by credential type and status.
- Status snapshot generation latency and stale snapshot count.

## Error Reporting

`ApiErrorMiddleware` normalizes validation, tenant, feature, permission, and not-found errors so API clients can display stable reviewable reasons.
