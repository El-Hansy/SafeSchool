# Quickstart: Phase 4 Wallet & Payments

Use this quickstart to validate that the Phase 4 planning package is complete
before generating tasks or starting implementation.

## Prerequisites

- Read [spec.md](./spec.md) for Phase 4 scope, clarifications, and user
  stories.
- Read [plan.md](./plan.md) for technical context and constitution checks.
- Read Phase 0 artifacts under `specs/001-platform-foundations/` for tenant,
  feature capability, audit, observability, money-safe API, and offline sync
  foundations.
- Read Phase 1 artifacts under `specs/002-identity-access/` for student
  profiles, guardian links, credentials, roles, permissions, and credential
  status snapshots.
- Read Phase 2 artifacts under `specs/003-attendance-campus-access/` only to
  preserve attendance and campus access boundaries; Phase 4 wallet scans must
  not generate attendance or campus entry/exit outcomes.
- Read Phase 3 artifacts under `specs/004-transport-bus-tracking/` only to
  preserve transport boundaries; Phase 4 wallet workflows must not generate
  transport outcomes.
- Confirm `.specify/feature.json` points to
  `specs/005-wallet-payments`.

## Artifact Review

1. Confirm [research.md](./research.md) resolves all planning decisions without
   unresolved clarification markers.
2. Confirm [data-model.md](./data-model.md) includes student wallets, ledger
   entries, top-ups, payment confirmations, canteen merchants, canteen item
   categories, purchase eligibility rules, POS terminals, offline POS sync
   batches, purchase transactions, spending limits, refunds or reversals,
   settlement references, anomalies, manual reviews, rule settings, feature
   settings, and review summaries.
3. Confirm [contracts/wallet-ledger.md](./contracts/wallet-ledger.md) covers
   wallet lifecycle, append-only ledger behavior, wallet restrictions, balance
   traceability, and audit expectations.
4. Confirm [contracts/wallet-top-up-payment.md](./contracts/wallet-top-up-payment.md)
   covers guardian online top-ups, cashier top-ups, provider confirmation
   idempotency, failed states, safe payment references, disputes, chargebacks,
   and pending recovery.
5. Confirm [contracts/canteen-pos-purchase.md](./contracts/canteen-pos-purchase.md)
   covers merchants, terminals, online purchases, offline POS sync, NFC/QR
   evidence, spending rule snapshots, reserve snapshots, duplicate prevention,
   denial behavior, and purchase traceability.
6. Confirm [contracts/spending-limits.md](./contracts/spending-limits.md)
   covers school and guardian limits, strictest-rule enforcement, versioning,
   future purchase impact, and denial traceability.
7. Confirm [contracts/transaction-history-review.md](./contracts/transaction-history-review.md)
   covers school history, guardian privacy-limited history, refunds, reversals,
   holds, releases, chargeback recovery, manual reviews, and correction
   traceability.
8. Confirm [contracts/wallet-reconciliation.md](./contracts/wallet-reconciliation.md)
   covers ledger, top-up, payment confirmation, POS batch, purchase, refund,
   reversal, chargeback, settlement, mismatch, anomaly, close, reopen, and
   trace behavior.
9. Confirm [contracts/wallet-rules-and-review-summary.md](./contracts/wallet-rules-and-review-summary.md)
   covers wallet rule settings, offline reserve policy, chargeback policy,
   retention policy, review summaries, and guardian-scoped summaries.

## Implementation Order for Later Tasks

1. Establish shared tenant, capability, permission, Phase 1 identity and
   guardian-link, credential status, money precision, idempotency, retention,
   and audit guards for Wallet workflows.
2. Create Student Wallet and Wallet Ledger Entry models, migrations,
   append-only posting rules, wallet status transitions, balance projection,
   contracts, and school finance workflows.
3. Create guardian online top-up initiation through approved provider adapter
   boundaries, including safe payment references and pending confirmation
   state.
4. Create idempotent payment confirmation handling for success, failure,
   cancellation, expiration, dispute, reversal, duplicate, and chargeback
   outcomes.
5. Create authorized cashier top-up behavior with cashier source evidence,
   amount threshold rules, ledger credit posting, and audit history.
6. Create canteen merchant, item category, purchase eligibility rule, and POS
   terminal management with terminal/device authorization and optional offline
   reserve configuration.
7. Create online POS purchase authorization for active wallet, active
   credential, active merchant, active terminal, sufficient funds, active
   spending limits, and duplicate purchase prevention.
8. Create mobile or POS NFC/QR purchase capture with offline queue storage,
   source metadata, local time evidence, reserve snapshots, and retry-safe sync.
9. Create offline POS reconciliation for delayed, duplicate, stale-limit,
   per-student reserve, per-terminal reserve, cross-school, and out-of-order
   conflicts, routing unsafe cases to review.
10. Create school and guardian spending limit behavior with strictest-rule
    evaluation, rule version snapshots, future-purchase enforcement, and denial
    reasons.
11. Create transaction history for finance users and guardian-visible linked
    students, preserving privacy-limited guardian detail.
12. Create refund, void, reversal, hold, release, restriction, pending recovery,
    and manual correction workflows that preserve original events and post
    append-only corrective ledger entries.
13. Create chargeback-after-spend handling that restricts wallet spending,
    records pending recovery, creates review evidence, and requires authorized
    financial review before discretionary spending is restored.
14. Create reconciliation summaries for top-ups, payment confirmations, POS
    batches, purchases, refunds, reversals, chargebacks, settlement references,
    mismatches, and review actions.
15. Create wallet anomaly detection and reviewer workflow for duplicates,
    invalid credentials, negative balances, offline overspend, spending limit
    bypass, unmatched settlement, chargeback after spend, repeated suspicious
    attempts, and manual-review-required cases.
16. Create wallet rule settings and review summaries, including top-up limits,
    cashier thresholds, offline reserve limits, chargeback policy, anomaly
    policy, duplicate retry policy, reconciliation threshold, and
    school-configured financial record retention.
17. Complete unit, integration, contract, authorization, tenant-isolation,
    audit, mobile offline/POS, retention, reconciliation, and critical UI
    journey tests.

## Validation Scenarios

### Wallet Ledger

- Create or activate wallets for 30 active students in one school account and
  confirm completion in under 10 minutes during review testing.
- Attempt wallet creation for inactive students, missing students, unauthorized
  actors, and cross-school students and confirm each is prevented without
  exposing cross-tenant existence.
- Post an approved credit and debit through allowed workflows and confirm
  available, pending, held, and settled balances are derived from ledger
  entries.
- Attempt to edit or delete an approved ledger entry and confirm the system
  rejects direct mutation and requires a reversal or adjustment entry.
- Restrict, suspend, restore, and close a wallet and confirm each state change
  requires reason, permission, and audit evidence.

### Guardian Online Top-Up

- Initiate an online top-up for a linked student through an approved external
  payment provider and confirm the top-up remains non-spendable until provider
  success evidence is accepted.
- Submit a successful payment confirmation and confirm exactly one spendable
  ledger credit posts within 2 minutes of confirmation availability.
- Submit failed, cancelled, expired, duplicate, disputed, and cross-school
  confirmations and confirm they do not create duplicate or unauthorized
  credits.
- Confirm guardian-visible top-up history shows safe payment reference, amount,
  status, and resulting transaction record without full external payment
  credentials.

### Cashier Top-Up

- Record a cashier top-up for an active student wallet using an authorized
  cashier and source evidence.
- Confirm the cashier top-up posts one approved credit with cashier reference,
  reason, actor, and audit evidence.
- Attempt cashier top-ups for inactive wallets, restricted wallets,
  cross-school wallets, unauthorized cashiers, invalid amounts, and threshold
  violations and confirm each is rejected or routed to review.

### Canteen POS Purchase

- Create an active merchant and active POS terminal for one school account.
- Record an online NFC or QR purchase for an active student credential with
  sufficient funds and valid limits, and confirm one debit posts in under 8
  seconds using a seeded active wallet, active credential, active merchant,
  active POS terminal, warm database, local API test environment, and no
  external payment-provider call in the purchase path.
- Attempt purchases with insufficient funds, invalid credentials, disabled POS
  capability, unauthorized merchant, suspended terminal, spending limit
  violation, and cross-school wallet references and confirm each is denied or
  held without creating a normal debit.
- Submit the same purchase request twice and confirm the retry returns the same
  outcome without a duplicate debit.

### Offline POS Reserve Sync

- Enable offline POS with school-configured per-student and per-terminal reserve
  limits.
- Capture offline purchases within both reserves and confirm the later sync
  posts or matches one debit per physical purchase.
- Capture offline purchases that exceed the student reserve, exceed the
  terminal reserve, use stale spending limits, duplicate the same physical
  purchase, arrive out of order, or reference another school account and confirm
  unsafe cases are routed to review rather than silently changing balances.
- Disable offline POS and confirm terminals can no longer capture normal
  offline purchases.

### Spending Limits

- Create daily, weekly, per-purchase, category, merchant, time-window, and
  active-date limits for a student wallet.
- Create both school and guardian limits and confirm purchases enforce the
  strictest active applicable rule.
- Change a limit and confirm eligible future purchases enforce the new rule
  within 1 minute during review testing.
- Confirm each purchase stores the rule version used for allow, deny, or hold
  decisions.

### Transaction History and Privacy

- View a finance user's wallet history by student, wallet, date range,
  transaction type, merchant, POS terminal, funding source, status, amount, and
  review state.
- View guardian history for a linked student and confirm it shows amount,
  merchant, item or category summary, status, and corrections only.
- Confirm guardian history hides staff-only terminal, operator, settlement, and
  internal review assignment details.
- Attempt guardian history access for an unlinked student and confirm denial
  without exposing unrelated wallet records.

### Corrections and Chargebacks

- Refund or reverse an approved purchase with authorized review and confirm the
  original purchase remains visible and a corrective ledger entry posts.
- Apply a hold and release to a wallet with review reason and audit evidence.
- Submit a chargeback after credited funds have already been spent and confirm
  the wallet becomes restricted, pending recovery is recorded, and further
  discretionary spending is blocked until authorized financial review restores
  the wallet.
- Attempt corrections after a closed reconciliation period and confirm a new
  review item is created while prior reconciliation evidence remains preserved.

### Reconciliation

- Run daily reconciliation for one school account and compare wallet ledger
  totals to confirmed top-ups, payment confirmations, POS batches, purchases,
  refunds, reversals, chargebacks, and settlement references.
- Confirm matching totals can be closed with finance review evidence.
- Create mismatched settlement, unmatched provider confirmation, duplicate POS
  purchase, offline overspend, and chargeback-after-spend examples and confirm
  each becomes a reviewable exception without silently altering balances.
- Reopen a closed reconciliation after a later chargeback or correction and
  confirm prior close evidence remains traceable.

### Rules, Retention, and Audit

- Configure wallet rule settings for top-up minimums and maximums, cashier
  thresholds, offline reserves, spending limit precedence, refund windows,
  chargeback handling, anomaly detection, duplicate retry handling,
  reconciliation thresholds, and detailed financial record retention.
- Confirm detailed wallet financial records follow the school account's
  configured retention setting with no Phase 4 platform-wide minimum.
- Confirm records under active review, dispute, recovery, or reconciliation
  hold are preserved despite retention settings.
- Confirm wallet creation, status changes, top-ups, payment confirmations,
  failures, chargebacks, purchase approvals, purchase denials, offline sync,
  limit changes, refunds, reversals, settlement reconciliation, anomaly
  creation, manual reviews, corrections, and access denials emit audit
  evidence.
- Confirm a failed audit write prevents sensitive financial mutation rather
  than allowing an unaudited change.

### Scope Boundaries

- Confirm Phase 4 does not implement tuition invoicing, payroll, staff expense
  management, external accounting replacement, banking account management,
  credit/lending, debt collection, cryptocurrency, broad marketplace commerce,
  general messaging, broad admin dashboards, attendance generation, campus
  entry or exit decisions, transport outcomes, or physical cash drawer hardware.

## Expected Verification Commands

The implementation repository should provide equivalent commands once runtime
manifests exist:

```bash
dotnet test apps/api/tests/SafeSchool.Api.Tests
npm test --prefix apps/admin-web
flutter test apps/mobile
```

Contract and end-to-end validation should cover:

```bash
dotnet test apps/api/tests/SafeSchool.Api.Tests --filter Wallet
npm test --prefix apps/admin-web -- wallet
flutter test apps/mobile/test/features/wallet
```

## Readiness Criteria

- Every Phase 4 user story can be implemented independently.
- Every sensitive action has tenant, capability, role, permission,
  guardian-link, money precision, idempotency, retention, and audit
  expectations.
- Every public route in the contracts has denial behavior for tenant mismatch,
  disabled capability, missing permission, invalid guardian link, invalid
  credential, insufficient funds, spending limit violation, duplicate
  submission, unsafe offline reserve, and audit failure where applicable.
- Append-only ledger behavior, corrective entries, safe payment references, and
  chargeback pending recovery are planned before task generation.
- Offline POS continuity, duplicate prevention, delayed sync, local time
  evidence, per-student reserve, per-terminal reserve, and reconciliation
  behavior are planned before task generation.
- Guardian transaction history remains limited to linked-student amount,
  merchant, item or category summary, status, and corrections.
- Wallet retention remains school-configured with no Phase 4 platform-wide
  minimum while active review, dispute, recovery, and reconciliation holds are
  preserved.
- No Phase 2 attendance/access, Phase 3 transport, Phase 5 learning, Phase 6
  request, Phase 9 messaging, Phase 10 document/search, Phase 11 dashboard, or
  out-of-scope financial system behavior is implemented as part of Phase 4.
