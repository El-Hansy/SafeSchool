# Phase 4 Research: Wallet & Payments

## Decision: Implement Phase 4 as runtime product behavior

**Rationale**: The Phase 4 spec defines operational wallet ledgers, student
stored-value balances, top-ups, payment confirmations, canteen POS purchases,
spending limits, transaction history, refunds, reversals, chargebacks,
reconciliation, anomaly review, and retention. These are production financial
workflows with measurable correctness, speed, privacy, and audit outcomes.

**Alternatives considered**:
- Treat Phase 4 as documentation only: rejected because the spec requires
  student wallet balances, spend decisions, guardian top-ups, and financial
  review evidence.
- Limit Phase 4 to a canteen report: rejected because wallet ledger, top-up,
  payment processing, limits, and history are explicit Phase 4 modules.

## Decision: Use the constitution runtime baseline without adding new platforms

**Rationale**: The constitution already defines ASP.NET Core Web API for
backend, PostgreSQL for storage, Next.js/React/TypeScript for web, and
Flutter/Dart for mobile where native NFC/QR and offline capture are required.
Phase 4 does not justify a new microservice, message broker, separate ledger
store, banking subsystem, or external accounting replacement.

**Alternatives considered**:
- Create a wallet microservice: rejected because the modular monolith is the
  default and no measured scale pressure exists.
- Add a separate ledger database: rejected because tenant-scoped wallet
  records, append-only entries, indexes, and reconciliation fit the single
  PostgreSQL baseline for this planning phase.
- Replace school accounting: rejected because external accounting replacement
  is explicitly outside Phase 4 scope.

## Decision: Organize implementation around one Wallet feature area

**Rationale**: Wallets, ledger entries, top-ups, payment confirmations, POS
purchases, limits, history, corrections, reconciliation, anomalies, rules, and
audit evidence share tenant, feature, permission, student identity, guardian
visibility, and money-handling rules. A single Wallet feature area with internal
modules keeps ownership cohesive while avoiding one oversized service.

**Alternatives considered**:
- Split payments, POS, and history into unrelated roots: rejected because
  spendability, reversals, chargebacks, and reconciliation all depend on the
  same wallet ledger and review evidence.
- Put all wallet behavior into one service: rejected because wallet lifecycle,
  ledger posting, top-up confirmation, POS authorization, offline sync, limits,
  reconciliation, and anomaly review have separate responsibilities.

## Decision: Reuse Phase 0 tenant, feature, audit, and offline sync foundations

**Rationale**: Phase 0 owns school account tenant resolution, feature
configuration, audit/event logging, offline sync conventions, observability,
and shared platform rules. Phase 4 should consume those foundations rather than
redefining tenant or audit behavior.

**Alternatives considered**:
- Create wallet-specific tenant resolution: rejected because it risks divergent
  authorization and cross-school exposure.
- Create unaudited convenience corrections: rejected because financial changes
  must remain traceable and reviewable.

## Decision: Reuse Phase 1 identity, credential, guardian, role, and permission evidence

**Rationale**: Phase 1 owns student profiles, guardian links, NFC credentials,
QR fallback credentials, credential status, roles, and permissions. Phase 4
uses that evidence for wallet eligibility, guardian funding, guardian history,
student spend identity, cashier access, POS operator access, and reviewer
authorization.

**Alternatives considered**:
- Duplicate student or guardian records inside Wallet: rejected because it
  creates inconsistent identity ownership.
- Allow POS purchases without active credential evidence: rejected because
  expired, suspended, revoked, replaced, unknown, duplicated, and cross-school
  credentials must be denied or held.

## Decision: Keep Phase 2 and Phase 3 outcomes out of wallet behavior

**Rationale**: Wallet purchases may use NFC/QR identity evidence, but Phase 4
must not create campus attendance, campus entry/exit decisions, or transport
outcomes. It must also not depend on transport route, trip, or bus state.

**Alternatives considered**:
- Treat canteen POS scans as campus entry events: rejected because Phase 4
  covers spending, not attendance or access.
- Link purchase approval to bus or transport status: rejected because transport
  outcomes are outside Phase 4 scope.

## Decision: Treat school account as the tenant boundary for all financial records

**Rationale**: Phase 0 established School Account as the tenant boundary.
Wallets, ledger entries, top-ups, confirmations, purchases, merchants, POS
terminals, limits, reviews, and reconciliation evidence belong to one school
account. Guardians see only linked-student wallet records allowed by active
guardian link scope.

**Alternatives considered**:
- Use merchant or POS terminal as a tenant boundary: rejected because finance
  users must reconcile multiple merchants and terminals within one school
  account.
- Allow global student wallet lookup: rejected because it risks exposing
  cross-school student or wallet existence.

## Decision: Enforce feature availability with explicit Phase 4 capability keys

**Rationale**: The constitution requires backend feature flag enforcement and UI
feature gates. Phase 4 workflows can be enabled independently because a school
may start with ledger and history before online top-up, POS spending, or
offline POS.

**Capability keys**:
- `wallet.ledger`
- `wallet.top_up`
- `wallet.payment_processing`
- `wallet.spending_limits`
- `wallet.transaction_history`
- `wallet.canteen_pos`

Supporting review, reconciliation, anomaly, and rule-setting behavior is
governed by the relevant wallet workflow capabilities and permissions.

**Alternatives considered**:
- One `wallet` flag for every workflow: rejected because schools may phase in
  ledger, top-up, payment processing, limits, history, and POS separately.
- UI-only gating: rejected because backend enforcement must protect tenant data
  and sensitive financial actions.

## Decision: Use explicit Phase 4 permissions with inherited RBAC

**Rationale**: Phase 1 establishes role and permission enforcement. Phase 4
must add concrete permission keys for school administrators, finance managers,
cashiers, canteen managers, POS operators, guardians, auditors, reviewers, and
platform financial reviewers while evaluating permissions inside the active
school account.

**Common permission families**:
- `wallet.wallets.read`
- `wallet.wallets.manage`
- `wallet.ledger.read`
- `wallet.ledger.post`
- `wallet.topups.initiate`
- `wallet.topups.cashier`
- `wallet.payments.confirm`
- `wallet.purchases.record`
- `wallet.purchases.sync`
- `wallet.purchases.read`
- `wallet.limits.read`
- `wallet.limits.manage`
- `wallet.history.read`
- `wallet.guardian_history.read`
- `wallet.corrections.manage`
- `wallet.chargebacks.review`
- `wallet.reconciliation.read`
- `wallet.reconciliation.manage`
- `wallet.anomalies.read`
- `wallet.anomalies.resolve`
- `wallet.rules.read`
- `wallet.rules.manage`
- `wallet.audit.read`

**Alternatives considered**:
- Let all school staff adjust balances: rejected because wallet corrections are
  financial actions requiring explicit review authorization.
- Allow guardians to view all wallet records for a school: rejected because
  guardian access is limited to approved active linked students.

## Decision: Use an append-only wallet ledger with reversal and correction entries

**Rationale**: Financial traceability requires approved credits, debits, holds,
releases, refunds, reversals, chargebacks, and manual adjustments to remain
reviewable. Approved entries are not edited or deleted directly. Corrections
post new reversal or adjustment entries that reference the original financial
event and preserve the reason, actor, and resulting wallet impact.

**Alternatives considered**:
- Update wallet balances in place without ledger entries: rejected because
  reviewers and guardians could not trace how a balance was produced.
- Edit original entries during correction: rejected because it hides the
  original financial event.

## Decision: Represent money in configured currency minor units

**Rationale**: Wallet amounts must use the school account's configured currency
and preserve minor-unit precision, rounding decisions, source amount evidence,
and ledger impact for every financial event. This avoids floating-point balance
errors and keeps reconciliation deterministic.

**Alternatives considered**:
- Store decimal strings without normalized minor units: rejected because it
  makes indexing and total comparison harder.
- Allow arbitrary multi-currency wallet balances in Phase 4: rejected because
  the spec assumes one configured school-account currency unless a later spec
  adds multi-currency support.

## Decision: Support guardian online top-ups and authorized cashier top-ups

**Rationale**: The clarification selected guardian online top-ups through an
approved external payment provider plus authorized cashier top-ups. Guardian
top-ups become spendable only after confirmed successful payment evidence.
Cashier top-ups require cashier source evidence, reason, permission, and audit
history.

**Alternatives considered**:
- Guardian online top-up only: rejected because authorized cashier top-ups are
  explicitly in scope.
- Cashier top-up only: rejected because guardian online funding is explicitly
  in scope.
- Allow unconfirmed online top-ups to be spendable: rejected because failed,
  pending, cancelled, disputed, or cross-school top-ups must remain
  non-spendable.

## Decision: Keep external payment provider integration behind a safe adapter boundary

**Rationale**: Phase 4 needs payment status, safe payment references, provider
event identity, and review evidence, but must not store or display full
external payment credentials. Idempotent confirmation handling prevents a
retried provider event from creating duplicate credits or duplicate recovery
outcomes.

**Alternatives considered**:
- Store full card or payment credentials: rejected because the spec explicitly
  forbids full external payment credentials.
- Build provider-specific logic into core wallet ledger code: rejected because
  core ledger rules should depend on normalized confirmation outcomes, not
  provider-specific payloads.

## Decision: Restrict wallets and require review after chargeback following spend

**Rationale**: The clarification selected restricting the wallet, recording a
pending recovery, and requiring financial review before further discretionary
spending. This protects financial integrity without silently reversing student
purchases that already occurred.

**Alternatives considered**:
- Allow negative balance and continue normal spending: rejected because it
  permits ongoing discretionary spend against disputed funds.
- Automatically reverse recent purchases: rejected because it can hide valid
  canteen purchases and create unclear guardian, merchant, and audit outcomes.
- School absorbs the chargeback without restriction: rejected because recovery
  and review would be lost.

## Decision: Authorize canteen POS purchases against active wallet, credential, merchant, terminal, limits, and balance

**Rationale**: A wallet debit is allowed only when the wallet is active, the
student credential is valid, the merchant and POS terminal are authorized, the
purchase satisfies active spending limits, the actor or device is permitted,
and sufficient available funds exist. Denied attempts preserve reason evidence
without creating a normal debit.

**Alternatives considered**:
- Debit based only on card tap and amount: rejected because it ignores tenant,
  credential, merchant, terminal, limit, and balance rules.
- Deny without recording failed attempts: rejected because invalid credentials,
  retries, and repeated suspicious attempts are operational evidence.

## Decision: Allow offline POS only within configured per-student and per-terminal reserves

**Rationale**: The clarification selected bounded offline purchases. Offline
POS is disabled by default and may be enabled only with school-configured
per-student and per-terminal reserve limits. Each offline purchase preserves
identity evidence, merchant, terminal, local time, received time, spending rule
snapshot, reserve snapshot, duplicate identity, reconciliation status, and
review outcome.

**Alternatives considered**:
- Disallow all offline POS: rejected because the clarification allows bounded
  offline purchases.
- Allow unlimited offline POS: rejected because stale data could create
  unbounded negative exposure.
- Trust offline data without reconciliation: rejected because delayed,
  duplicate, out-of-order, stale-limit, cross-school, and reserve-exceeding
  conflicts need review.

## Decision: Use caller-stable purchase and batch identity for idempotent sync

**Rationale**: POS terminals can retry purchase commands and offline batches.
Each purchase and sync batch must carry caller-stable identity and source
metadata so retries are treated as the same physical purchase, duplicates remain
reviewable, and duplicate wallet debits are prevented.

**Alternatives considered**:
- Deduplicate only by student, terminal, amount, and timestamp: rejected
  because repeated similar purchases and clock drift can produce false matches.
- Let reviewers manually correct duplicate debits later: rejected because
  common retries should resolve deterministically before corrupting balances.

## Decision: Enforce the strictest applicable spending limit

**Rationale**: Guardian and school limits may overlap. The spec requires the
strictest active applicable rule to govern daily, weekly, per-purchase,
category, merchant, time-window, and active-date restrictions. Each purchase
stores the rule version used so later history can explain why the purchase was
allowed, denied, or held.

**Alternatives considered**:
- Let guardian rules always override school rules: rejected because school
  safety and merchant policy rules may need to be stricter.
- Let school rules always override guardian rules: rejected because guardian
  funding controls are part of Phase 4.
- Recompute historical decisions from current rules: rejected because rule
  changes would make old purchase decisions appear inconsistent.

## Decision: Guardian transaction history exposes a privacy-limited detail set

**Rationale**: The clarification selected amount, merchant, item or category
summary, status, and corrections for linked students only. Guardian-facing
history must hide staff-only POS terminal, operator, settlement, internal
review assignment, and unrelated student details.

**Alternatives considered**:
- Show only total balance: rejected because guardians need transaction
  explainability.
- Show full staff financial detail: rejected because terminal, operator,
  settlement, and internal review details exceed guardian scope.

## Decision: Reconcile wallet ledger, payment confirmations, POS batches, refunds, reversals, chargebacks, and settlements

**Rationale**: Finance users need daily and period review that compares wallet
ledger totals to source evidence. Matching totals can be closed. Mismatches,
unmatched settlement references, duplicate confirmations, offline overspend,
and chargebacks after spend become reviewable exceptions without silently
changing balances.

**Alternatives considered**:
- Omit reconciliation from Phase 4: rejected because it is required by the spec
  for financial tracking.
- Let reconciliation alter balances automatically: rejected because corrections
  must be explicit ledger entries with reasons and review evidence.

## Decision: Detect wallet anomalies as first-class review records

**Rationale**: Duplicate confirmations, duplicate POS purchases, negative
available balance, offline overspend, invalid credential purchases, spending
limit bypass, unmatched settlement, chargeback after spend, repeated suspicious
attempts, and manual-review-required events need consistent review, assignment,
resolution, dismissal, reopening, and audit evidence.

**Alternatives considered**:
- Expose anomalies only as logs: rejected because reviewers need workflow state
  and resolution history.
- Block every anomaly source without persistence: rejected because denied and
  suspicious attempts are operational evidence.

## Decision: Make detailed wallet financial record retention school-configured

**Rationale**: The clarification selected school-configured retention with no
Phase 4 platform-wide minimum. The system must apply each school account's
retention setting while preserving audit evidence and records under active
review, dispute, recovery, or reconciliation hold.

**Alternatives considered**:
- Enforce a fixed platform-wide minimum: rejected because the clarification
  says no platform-wide minimum in Phase 4.
- Delete records under active review when retention expires: rejected because
  active financial evidence must remain available until the hold is resolved.

## Decision: Keep Phase 4 implementation tasking aligned to six user stories

**Rationale**: The spec defines six independently testable stories: wallet
ledger, top-up, canteen POS purchase, spending limits, transaction history and
corrections, and reconciliation. Tasks should preserve that order while
including shared foundations for tenant, capability, permission, money,
idempotency, audit, and testing concerns.

**Alternatives considered**:
- Organize tasks purely by technical layer: rejected because it obscures the
  independent product outcomes required by the Spec Kit workflow.
- Implement reconciliation before ledger, top-up, and POS: rejected because it
  depends on source financial evidence.
