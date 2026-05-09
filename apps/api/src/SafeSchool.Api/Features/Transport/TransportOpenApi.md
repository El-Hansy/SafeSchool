# Transport OpenAPI Summary

Phase 3 Transport endpoints are versioned under `/api/v1/schools/{schoolAccountId}/transport` with guardian-scoped reads under `/api/v1/guardians/me/students/{studentProfileId}/transport`.

Covered route groups: routes, stops, vehicles, assignments, scan context trips, scan events, live tracking, ETA, notification records, anomaly runs, manual reviews, rule settings, and review summaries.

Every sensitive route resolves tenant scope, checks a Phase 3 feature capability or rule permission, enforces actor permissions, records denied access decisions, and emits transport audit evidence for mutations.
