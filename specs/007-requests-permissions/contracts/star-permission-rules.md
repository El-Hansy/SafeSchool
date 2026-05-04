# Contract: Star-Based Permission Rules

This contract defines star-based request eligibility, rule configuration,
Phase 5 evidence use, star reservation at submission, consumption on approval,
release on denial/withdrawal/expiry, and star outcome traceability for Phase 6.

## Capabilities and Permissions

- Required capabilities:
  - `requests.star_rules`
  - `requests.approval_workflow` when star rules are attached to workflowed requests
  - Phase 5 star/reward evidence capability for automated evaluation
- Common permissions:
  - `requests.star_rules.read`
  - `requests.star_rules.manage`
  - `requests.requests.create`
  - `requests.reviews.manage`
  - `requests.audit.read`

Phase 6 records rule snapshots and star outcome references. Phase 5 remains the
source of truth for star balances, reservations, consumption, and releases.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/requests/star-rules` | Create a draft star permission rule |
| GET | `/api/v1/schools/{schoolAccountId}/requests/star-rules` | List star permission rules |
| GET | `/api/v1/schools/{schoolAccountId}/requests/star-rules/{starRuleId}` | Read star permission rule detail |
| PATCH | `/api/v1/schools/{schoolAccountId}/requests/star-rules/{starRuleId}` | Update a draft or suspended rule |
| POST | `/api/v1/schools/{schoolAccountId}/requests/star-rules/{starRuleId}/activate` | Activate a rule version |
| POST | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/star-evaluation/retry` | Retry a failed or pending star evaluation |
| GET | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/star-evaluation` | Read star rule evaluation outcome |
| GET | `/api/v1/schools/{schoolAccountId}/requests/{requestId}/star-evaluation/trace` | Trace star rule, Phase 5 evidence references, reservation, consumption, release, review, and audit evidence |

## Star Rule Request

```yaml
request_type_id: "request-type-reference"
rule_name: "Weekend outing star rule"
required_star_threshold: 20
star_cost: 5
eligible_student_group: "Middle School"
manual_review_behavior: "Route To Review"
unavailable_evidence_behavior: "Route To Review"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_to: null
change_reason: "Students must have enough stars for privilege requests."
client_request_id: "star-rule-request-unique-to-caller"
```

## Star Evaluation Response

```yaml
star_rule_evaluation_id: "star-evaluation-reference"
permission_request_id: "request-reference"
star_permission_rule_id: "star-rule-reference"
rule_version: "star-rule-version-reference"
phase5_evidence_reference: "phase5-star-evidence-reference"
available_star_count: 24
required_star_threshold: 20
star_cost: 5
evaluation_status: "Reserved"
reservation_reference: "phase5-reservation-reference"
consumption_reference: null
release_reference: null
review_reason: null
evaluated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Star Outcome Callback or Sync Result

```yaml
permission_request_id: "request-reference"
star_rule_evaluation_id: "star-evaluation-reference"
phase5_outcome_reference: "phase5-outcome-reference"
outcome_type: "Consumed"
outcome_status: "Succeeded"
occurred_at: "YYYY-MM-DDTHH:MM:SSZ"
client_request_id: "phase5-outcome-unique-reference"
```

## Acceptance Rules

- Rule management requires tenant access, enabled `requests.star_rules`, rule
  management permission, valid request type, and audit evidence.
- Automated evaluation requires available Phase 5 star or reward evidence; if
  evidence is missing, stale, unavailable, or disabled, Phase 6 follows the
  configured unavailable-evidence behavior.
- A star-gated request with a star cost reserves stars at submission, consumes
  reserved stars only on final approval, and releases reservations on denial,
  withdrawal, or expiry.
- Star reservation, consumption, and release commands are idempotent by request
  and star evaluation reference.
- Phase 6 must not invent, silently modify, or own star balances.
- Historical requests preserve the star rule version and evaluation outcome
  used at submission time.
