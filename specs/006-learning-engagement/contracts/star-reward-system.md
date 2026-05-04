# Contract: Star and Reward System

This contract defines star rule management, append-only star ledger behavior,
balance snapshots, reservations, consumption, releases, reward catalog,
redemption, fulfillment, Phase 6 star evidence export, and audit behavior for
Phase 5.

## Capabilities and Permissions

- Required capabilities:
  - `learning.stars_rewards`
  - `learning.rewards`
  - `learning.progress_history` when star history is read
- Common permissions:
  - `learning.stars.read`
  - `learning.stars.manage`
  - `learning.rewards.read`
  - `learning.rewards.manage`
  - `learning.rewards.redeem`
  - `learning.reviews.manage`
  - `learning.audit.read`

Phase 5 is the source of truth for star balances, reservations, consumption,
release, correction, reward redemption, and star evidence consumed by later
Phase 6 permission rules.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/learning/star-rules` | Create draft star rule |
| POST | `/api/v1/schools/{schoolAccountId}/learning/star-rules/{starRuleId}/activate` | Activate star rule version |
| POST | `/api/v1/schools/{schoolAccountId}/learning/stars/manual-awards` | Record authorized manual star award or correction |
| GET | `/api/v1/schools/{schoolAccountId}/learning/students/{studentProfileId}/stars` | Read student star balance and ledger |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/learning/stars` | Read guardian-visible linked-student stars |
| POST | `/api/v1/schools/{schoolAccountId}/learning/rewards` | Create reward catalog item |
| POST | `/api/v1/students/me/learning/rewards/{rewardId}/redeem` | Redeem reward as eligible student |
| POST | `/api/v1/schools/{schoolAccountId}/learning/rewards/redemptions/{redemptionId}/fulfill` | Fulfill or cancel reward redemption |
| GET | `/api/v1/schools/{schoolAccountId}/learning/students/{studentProfileId}/star-evidence` | Export Phase 6-readable star evidence |
| GET | `/api/v1/schools/{schoolAccountId}/learning/stars/{ledgerEntryId}/trace` | Trace star ledger entry to source, reward, review, and audit evidence |

## Star Rule Request

```yaml
rule_name: "Quiz excellence stars"
source_type: "Quiz"
eligible_student_scope: "Learning Group"
star_amount: 10
award_cap: 50
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_to: null
review_behavior: "Auto Award"
client_request_id: "star-rule-unique-to-caller"
```

## Manual Star Award Request

```yaml
student_profile_id: "student-reference"
source_type: "Manual Award"
source_id: "manual-source-reference"
ledger_direction: "Credit"
star_amount: 5
reason: "Teacher commendation"
client_request_id: "manual-star-unique-to-caller"
```

## Reward Request

```yaml
reward_name: "Library privilege"
description: "Redeem stars for a library privilege."
eligible_student_scope: "Learning Group"
star_cost: 20
available_from: "YYYY-MM-DDTHH:MM:SSZ"
available_to: null
inventory_limit: 30
redemption_limit: 1
fulfillment_policy: "Staff Fulfilled"
client_request_id: "reward-unique-to-caller"
```

## Reward Redemption Response

```yaml
reward_redemption_id: "redemption-reference"
reward_catalog_item_id: "reward-reference"
student_profile_id: "student-reference"
redemption_status: "Approved"
star_cost_snapshot: 20
star_ledger_entry_id: "ledger-reference"
fulfillment_status: "Pending"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Phase 6 Star Evidence Response

```yaml
student_profile_id: "student-reference"
available_stars: 42
reserved_stars: 5
latest_ledger_entry_id: "ledger-reference"
latest_evidence_at: "YYYY-MM-DDTHH:MM:SSZ"
evidence_status: "Available"
```

## Acceptance Rules

- Star and reward routes require tenant access, enabled capability, role
  permission, valid student status, source evidence, idempotent client identity,
  and audit evidence.
- Star ledger entries are append-only and cannot be edited or deleted directly.
- Star balances must be reconstructable from ledger entries.
- Exact duplicate source awards, reservations, consumption, releases, and
  reward redemptions return the existing result or are rejected without
  duplicate star impact.
- Reward redemption validates eligible student scope, active reward status,
  available stars, redemption limits, and fulfillment policy before stars are
  reserved or consumed.
- Reward workflows must not create wallet credits, payments, refunds, canteen
  purchases, or financial records.
- Phase 6 star evidence export is read-only and must not approve requests or
  mutate Phase 5 star balances.
