# Feature Specification: Phase 4 Wallet & Payments

**Feature Branch**: `005-wallet-payments`
**Created**: 2026-05-04
**Status**: Draft
**Input**: User description: "Read PLAN.md and create a specification for phase 4: Wallet & Payments ONLY."

## Clarifications

### Session 2026-05-04

- Q: Which top-up funding sources are in Phase 4 scope? → A: Guardian online top-ups through an approved external payment provider plus authorized cashier top-ups.
- Q: If a guardian top-up is later disputed or charged back after the student already spent the credited funds, how should Phase 4 handle the wallet? → A: Restrict wallet, record pending recovery, and require financial review before further discretionary spending.
- Q: How should Phase 4 handle canteen POS purchases when the POS is offline? → A: Allow offline purchases only within school-configured per-student and per-terminal reserve limits.
- Q: How long should Phase 4 retain detailed wallet financial records? → A: Each school configures retention with no platform-wide minimum.
- Q: What transaction details should guardians see for linked students? → A: Guardians see amount, merchant, item/category summary, status, and corrections for linked students only.

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 4: Wallet & Payments
- **Feature Module(s)**: Wallet Ledger, Wallet Top-Up, Payment Processing, Spending Limits, Transaction History, Canteen POS Integration
- **Tenant Scope**: All student wallets, ledger entries, top-up records, payment attempts, canteen merchant records, POS terminals, purchase transactions, spending limits, refunds, reversals, chargebacks, settlement references, reconciliation summaries, wallet anomaly records, review actions, and wallet rule settings belong to one school account and must not be visible or actionable outside that school account unless an explicit platform-level financial review role permits it.
- **Feature Flag(s)**: Wallet ledger, wallet top-up, payment processing, spending limits, transaction history, and canteen POS integration must respect each school account's enabled capabilities before users can access or automate the related workflow.
- **Security/Roles**: Platform owners, school administrators, finance managers, cashiers, canteen managers, POS operators, guardians, students, auditors, and reviewers must have explicit permissions for each Phase 4 action. Guardians can see and fund only wallets for students linked to them through an approved active guardian relationship. Students can only spend from their own wallet through approved school account rules.
- **Offline/NFC Impact**: Canteen POS purchases may use NFC or QR identity evidence to identify the student wallet. Offline POS capture is allowed only when the school account enables bounded per-student and per-terminal offline reserve limits and preserves identity evidence, merchant or terminal source, local time, later received time, spending limit snapshot, decision, and reconciliation outcome. If the wallet, limit, terminal reserve, student reserve, or credential state cannot be trusted under the enabled offline rules, the purchase must be declined or held for review rather than creating an unbounded debit.
- **Observability**: The system must emit reviewable evidence for wallet creation, wallet suspension, top-up initiation, payment confirmation, payment failure, chargeback, purchase authorization, purchase denial, offline POS sync, spending limit changes, refund, reversal, settlement reconciliation, anomaly detection, manual review, correction, and access denial.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Maintain Student Wallet Ledgers (Priority: P1)

As a finance manager, I need student wallets with trustworthy balances and reviewable ledger entries so school cashless spending can be controlled without losing financial traceability.

**Why this priority**: The wallet ledger is the financial source of truth for top-ups, canteen purchases, refunds, reversals, limits, history, and reconciliation.

**Independent Test**: Create active wallets for students in one school account, post an approved credit and debit, verify the available balance, and confirm ledger entries are tenant-scoped, reviewable, and corrected only through reversal entries.

**Acceptance Scenarios**:

1. **Given** wallet ledger is enabled and a finance manager is authorized, **When** they create or activate wallets for active students in a school account, **Then** each wallet is available only within that school account and has reviewable balance and status history.
2. **Given** a wallet has approved ledger activity, **When** a correction is required, **Then** the system records a reversal or adjustment entry with a reason and preserves the original entry.
3. **Given** a student is inactive, missing, or belongs to another school account, **When** a user attempts to create or activate a wallet, **Then** the system blocks the action and records the reason.

---

### User Story 2 - Top Up Student Wallets (Priority: P1)

As a guardian or authorized cashier, I need to add funds to a linked student's wallet so the student can make approved cashless purchases at school.

**Why this priority**: Top-ups make wallets useful and connect guardian funding, school cashier handling, payment confirmation, and ledger crediting.

**Independent Test**: Add funds to an active linked student's wallet, confirm the funding result, verify the spendable balance increases once, and confirm failed, cancelled, duplicate, or out-of-scope top-ups do not create duplicate credits.

**Acceptance Scenarios**:

1. **Given** wallet top-up and payment processing are enabled and a guardian has an approved active link to the student, **When** the guardian completes an online top-up through an approved external payment provider that is confirmed as successful, **Then** the wallet receives one approved credit and the guardian can see the resulting transaction record.
2. **Given** a cashier is authorized to record school-approved top-ups, **When** they add funds for an active student wallet, **Then** the credit is recorded with cashier source evidence, reason, and audit history.
3. **Given** a top-up is failed, cancelled, expired, duplicated, disputed, or linked to another school account, **When** confirmation is received or retried, **Then** the wallet balance is not credited more than once and the reason remains reviewable.

---

### User Story 3 - Process Canteen POS Purchases (Priority: P1)

As a canteen POS operator, I need to charge a student's wallet using approved identity evidence so purchases are fast, cashless, and blocked when funds or rules do not allow the sale.

**Why this priority**: Canteen POS integration is the primary Phase 4 spending workflow and must protect balances, spending limits, and student identity.

**Independent Test**: Use an active student credential at an authorized canteen POS, purchase an allowed item within wallet balance and limits, verify one debit is posted, and confirm insufficient funds, invalid credentials, disabled POS access, and cross-school wallets are denied without a debit.

**Acceptance Scenarios**:

1. **Given** canteen POS integration is enabled, the POS operator is authorized, the student credential is active, and the wallet has sufficient available funds, **When** the operator submits an approved purchase, **Then** the wallet is debited once and the purchase record shows item, amount, merchant, terminal, student, decision, and review status.
2. **Given** the wallet balance is insufficient, the credential is invalid, the merchant is not authorized, the item violates spending rules, or the wallet belongs to another school account, **When** a purchase is attempted, **Then** the system denies the purchase and records the reason without creating a normal debit.
3. **Given** POS connectivity is unavailable and offline POS is enabled with bounded per-student and per-terminal reserves, **When** an authorized POS operator records a purchase within the offline rules, **Then** the transaction is captured for later reconciliation and cannot exceed either offline exposure limit.

---

### User Story 4 - Manage Spending Limits (Priority: P2)

As a guardian or school administrator, I need spending limits and restrictions so wallet funds are used only within approved daily, category, merchant, and per-purchase boundaries.

**Why this priority**: Limits reduce misuse and give guardians and schools control after core ledger, top-up, and purchase flows work.

**Independent Test**: Configure a daily limit, a per-purchase limit, and a canteen category restriction for a linked student, then verify allowed purchases pass and disallowed purchases are denied with a clear reason.

**Acceptance Scenarios**:

1. **Given** spending limits are enabled and the user is authorized, **When** they configure daily, weekly, per-purchase, category, merchant, or time-window limits, **Then** future wallet purchases enforce the active rule within the school account.
2. **Given** both guardian and school limits apply to a student wallet, **When** a purchase is evaluated, **Then** the strictest active applicable rule is enforced and the decision remains reviewable.
3. **Given** a limit is changed, suspended, or superseded, **When** wallet transactions are reviewed later, **Then** the rule version used for each purchase remains traceable.

---

### User Story 5 - Review Transaction History and Corrections (Priority: P2)

As a guardian, finance manager, or reviewer, I need transaction history, refunds, reversals, and dispute visibility so wallet activity can be explained and corrected without hiding the original financial event.

**Why this priority**: Financial trust depends on transparent history and controlled correction workflows after money can move.

**Independent Test**: Review a student's wallet history, filter by top-up, purchase, refund, and denied transaction, issue an authorized refund or reversal with a reason, and confirm the original transaction and correction are both visible to authorized users.

**Acceptance Scenarios**:

1. **Given** transaction history is enabled and the user is authorized, **When** they view a wallet's activity, **Then** they can see tenant-scoped top-ups, purchases, refunds, reversals, holds, denials, chargebacks, and pending items allowed by their role or guardian link.
2. **Given** a guardian has an approved active link to a student, **When** the guardian views that student's transaction history, **Then** the guardian can see amount, merchant, item or category summary, status, and corrections without staff-only terminal, operator, or settlement details.
3. **Given** a purchase, top-up, chargeback, or settlement event needs correction, **When** an authorized reviewer records a refund, reversal, hold, pending recovery, or dispute outcome, **Then** the system preserves the original event, records the reason, and updates wallet availability according to the reviewed outcome.
4. **Given** a guardian is not linked to a student wallet, **When** they request transaction history, **Then** the system prevents access and records the denial without exposing whether unrelated wallet records exist.

---

### User Story 6 - Reconcile Payments and POS Activity (Priority: P3)

As a finance manager, I need reconciliation summaries for top-ups, canteen purchases, refunds, and settlement references so daily wallet activity can be matched and exceptions can be reviewed.

**Why this priority**: Reconciliation is essential for operations and audit, but it depends on completed ledger, top-up, POS, and correction records.

**Independent Test**: Select a school day, compare wallet ledger totals to top-up confirmations, POS batches, refunds, reversals, and settlement references, then confirm matching totals close and mismatches become reviewable exceptions.

**Acceptance Scenarios**:

1. **Given** reconciliation is enabled and the finance manager is authorized, **When** they review a date range, merchant, POS terminal, or funding source, **Then** the system summarizes totals by status and identifies unmatched or suspicious records.
2. **Given** a settlement reference, POS batch, or payment confirmation does not match wallet ledger totals, **When** reconciliation runs, **Then** the mismatch is flagged for review with source evidence and does not silently alter balances.
3. **Given** reconciliation has been reviewed and closed, **When** a later chargeback, refund, or correction changes the financial position, **Then** the system records a new review item and preserves the prior reconciliation evidence.

---

### Edge Cases

- A guardian tries to top up a wallet for a student whose guardian link is pending, suspended, expired, removed, rejected, or outside the school account.
- A payment confirmation is delayed, retried, duplicated, partially failed, cancelled, disputed, or reversed after funds were made available.
- A cashier records a top-up for the wrong student or wrong school account and requests correction.
- A student credential is expired, suspended, revoked, replaced, unknown, duplicated, or belongs to another school account.
- The same credential is tapped repeatedly at the POS within a short period or a POS retries the same purchase after a timeout.
- The wallet balance changes between item selection and purchase confirmation.
- A spending limit changes while the student is at the POS checkout.
- A POS terminal is offline, has stale wallet or limit data, exceeds the student's offline reserve, exceeds the terminal's offline reserve, or submits transactions after the school account has disabled offline POS spending.
- An offline POS transaction arrives after the wallet has insufficient funds or after a daily limit has already been reached.
- A refund or void is attempted after a reconciliation period has been closed.
- A top-up chargeback or dispute arrives after the student has already spent the credited funds, requiring wallet restriction, pending recovery tracking, and financial review before further discretionary spending.
- A canteen item price, item category, or merchant status changes after a purchase has been captured.
- A school account disables one Phase 4 capability while another Phase 4 capability remains enabled.
- A financial reviewer corrects a top-up, purchase, refund, or settlement outcome after guardians have already viewed the prior status.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST allow authorized users to create, view, activate, suspend, close, and review student wallets within a school account when wallet ledger is enabled.
- **FR-002**: The system MUST validate student profile status, guardian link status, identity credential status, wallet status, merchant or POS status, school account scope, actor permission, spending rules, and feature availability before creating wallets, crediting top-ups, authorizing purchases, showing history, or creating corrections.
- **FR-003**: Wallet balance changes MUST be represented as reviewable ledger entries for credits, debits, holds, releases, refunds, reversals, chargebacks, and manual adjustments, and approved entries MUST NOT be edited or deleted directly.
- **FR-004**: Wallets MUST show available, pending, held, and settled balance states so purchases cannot spend unconfirmed or restricted funds unless enabled per-student and per-terminal offline POS reserve limits explicitly allow a bounded pending debit.
- **FR-005**: The system MUST allow eligible guardians to initiate online top-ups through an approved external payment provider and authorized cashiers to record school-approved top-ups for active student wallets with amount, currency, source, actor, linked student, payment status, and review evidence.
- **FR-006**: Payment confirmations, retries, cancellations, failures, expirations, disputes, and chargebacks MUST be idempotent so the same funding event cannot create duplicate wallet credits, duplicate wallet restrictions, or duplicate recovery outcomes.
- **FR-007**: A top-up MUST become spendable only after it reaches an approved confirmed state, and failed, pending, cancelled, disputed, or cross-school top-ups MUST remain non-spendable with a visible review reason.
- **FR-008**: The system MUST allow authorized users to create, view, update, deactivate, and review school canteen merchants, POS terminals, item categories, and purchase eligibility rules when canteen POS integration is enabled.
- **FR-009**: Each canteen purchase attempt MUST capture the school account, student wallet, identity evidence, credential type, merchant, POS terminal, operator or device source, item or category summary, amount, currency, time evidence, decision, and review status.
- **FR-010**: The system MUST approve a canteen purchase only when the wallet has sufficient available funds, the student credential is valid, the merchant and POS terminal are authorized, the purchase satisfies active spending limits, and the actor or device is permitted within the school account.
- **FR-011**: The system MUST deny or hold purchases from invalid, expired, suspended, revoked, replaced, unknown, duplicated, or cross-school credentials without creating a normal wallet debit.
- **FR-012**: Offline POS purchase capture MUST be allowed only when the school account enables offline wallet spending rules with per-student and per-terminal reserve limits, and it MUST preserve source evidence, local time, received time, spending rule snapshot, reserve snapshot, duplicate identity, reconciliation status, and review outcome.
- **FR-013**: Offline POS reconciliation MUST prevent duplicate debits for the same physical purchase and MUST route overspend, stale-limit, per-student reserve, per-terminal reserve, duplicate, out-of-order, or cross-school conflicts to review rather than silently changing balances.
- **FR-014**: The system MUST allow authorized guardians and school users to configure spending limits by student wallet, amount, time period, transaction amount, merchant, item category, and active date range, with the strictest applicable active rule enforced.
- **FR-015**: Spending limit records MUST preserve rule versions so later transaction history can show which rule allowed, denied, or held a purchase.
- **FR-016**: The system MUST provide transaction history by student wallet, guardian link, date range, transaction type, merchant, POS terminal, funding source, status, amount, and review state without exposing records outside the user's authorized school account or guardian link scope. Guardian-facing transaction history MUST show amount, merchant, item or category summary, status, and corrections for linked students only, and MUST hide staff-only POS terminal, operator, settlement, and internal review assignment details.
- **FR-017**: Authorized reviewers MUST be able to refund, void, reverse, hold, release, restrict, correct, recover, or dispute wallet activity with a reason while preserving both the original financial event and the corrective event. When a top-up dispute or chargeback arrives after credited funds have been spent, the system MUST restrict the affected wallet from further discretionary spending, record the pending recovery amount, require authorized financial review before restoring normal spending, and preserve both the original funding and spending evidence.
- **FR-018**: The system MUST provide reconciliation summaries for top-ups, payment confirmations, canteen POS batches, purchases, refunds, reversals, chargebacks, settlement references, mismatches, and review actions within the authorized school account.
- **FR-019**: The system MUST detect wallet anomalies, including duplicate top-up confirmation, duplicate POS purchase, negative available balance, offline overspend, invalid credential purchase, spending limit bypass, unmatched settlement, chargeback after spend, suspicious repeated purchase attempts, and manual-review-required events.
- **FR-020**: Each wallet anomaly MUST include the affected student wallet when applicable, school account, related top-up or purchase, source evidence, anomaly type, severity, status, reviewer assignment when applicable, resolution reason, and resolution history.
- **FR-021**: The system MUST allow each school account to configure Phase 4 wallet rule settings for top-up minimums and maximums, cashier adjustment thresholds, offline POS per-student and per-terminal reserve limits, spending limit precedence, refund windows, chargeback handling, anomaly detection, duplicate retry handling, reconciliation review thresholds, and detailed financial record retention, with tenant scope, permissions, and audit evidence.
- **FR-022**: The system MUST respect school account feature configuration independently for wallet ledger, wallet top-up, payment processing, spending limits, transaction history, and canteen POS integration.
- **FR-023**: The system MUST keep all Phase 4 records scoped to the school account and prevent cross-school visibility or action unless an explicit platform-level financial review role permits it.
- **FR-024**: The system MUST prevent storage or display of full external payment credentials, and it MUST show only safe payment references, status, and review evidence needed for wallet support and reconciliation.
- **FR-025**: The system MUST record audit evidence for wallet creation, wallet status changes, top-up initiation, payment confirmation, payment failure, chargeback, purchase authorization, purchase denial, offline POS sync, spending limit changes, refund, reversal, settlement reconciliation, anomaly creation, manual review, correction, and access denial.
- **FR-026**: The system MUST provide wallet review summaries by student wallet, guardian, merchant, POS terminal, transaction type, transaction status, spending limit status, settlement status, anomaly status, and review status without exposing records outside the authorized scope.
- **FR-027**: The system MUST use the school account's configured currency for wallet amounts and MUST preserve minor-unit precision, rounding decisions, and source amount evidence for every financial entry.
- **FR-028**: The system MUST allow each school account to configure how long detailed wallet financial records remain available, with no Phase 4 platform-wide minimum retention period, while preserving audit evidence and records still under active review, dispute, recovery, or reconciliation hold.
- **FR-029**: The system MUST explicitly exclude tuition invoicing, payroll, staff expense management, external accounting system replacement, banking account management, credit or lending, debt collection, cryptocurrency, broad marketplace commerce, general messaging, broad admin dashboards, attendance generation, campus entry or exit decisions, transport outcomes, and physical cash drawer hardware from Phase 4 deliverable scope.

### Key Entities *(include if feature involves data)*

- **Student Wallet**: A tenant-owned stored value account for one active student, including balance states, wallet status, configured currency, and review history.
- **Wallet Ledger Entry**: A reviewable financial entry that changes or explains wallet balance through credit, debit, hold, release, refund, reversal, chargeback, or adjustment activity.
- **Wallet Top-Up**: A guardian, cashier, or school-approved funding event for a student wallet, including amount, source, status, confirmation evidence, and duplicate prevention identity.
- **Payment Confirmation**: Evidence that a funding source was confirmed, failed, cancelled, expired, disputed, or reversed.
- **Canteen Merchant**: A school-authorized canteen operator or outlet that can accept wallet purchases under the school account.
- **POS Terminal**: A school-authorized purchase source associated with a canteen merchant, operator permissions, and optional per-terminal offline spending reserve rules.
- **Canteen Purchase Transaction**: A purchase attempt or approved wallet debit tied to a student wallet, identity evidence, merchant, POS terminal, items or categories, amount, decision, and review status.
- **Spending Limit**: A school or guardian rule that restricts wallet purchases by amount, period, merchant, category, time window, active dates, and precedence.
- **Refund or Reversal**: A corrective financial event that references an original top-up, purchase, or adjustment and records a reason, actor, and resulting wallet impact.
- **Settlement Reference**: A reconciliation record that groups payment confirmations, POS batches, purchases, refunds, reversals, and mismatches for financial review.
- **Wallet Anomaly**: A reviewable issue involving duplicate, conflicting, suspicious, invalid, overspent, disputed, unmatched, or manual-review-required wallet evidence.
- **Manual Wallet Review**: A reviewer action that resolves, dismisses, corrects, refunds, reverses, holds, releases, restricts, records recovery, or escalates wallet activity with a reason and history.
- **Wallet Rule Setting**: A school account configuration record for top-up limits, adjustment thresholds, spending rule precedence, per-student and per-terminal offline POS reserves, refund windows, chargeback handling, anomaly detection, duplicate retry handling, reconciliation thresholds, and detailed financial record retention.
- **Wallet Review Summary**: A permission-scoped view of wallet status, transaction status, merchant activity, settlement status, anomalies, and review outcomes.
- **School Account Feature Setting**: A school account capability setting that determines whether Phase 4 workflows are available.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Authorized finance users can create or activate wallets for 30 active students in under 10 minutes during review testing.
- **SC-002**: 100% of sampled inactive students, unauthorized actors, out-of-scope guardians, and cross-school student combinations are prevented from creating, funding, viewing, or spending from wallets.
- **SC-003**: 95% of confirmed guardian top-ups are reflected as spendable wallet credits within 2 minutes of confirmation availability.
- **SC-004**: 100% of sampled failed, cancelled, expired, duplicated, disputed, and cross-school top-up confirmations do not create duplicate or unauthorized wallet credits.
- **SC-005**: Authorized canteen POS operators can complete an approved wallet purchase in under 8 seconds during normal operating conditions.
- **SC-006**: 100% of sampled insufficient balance, invalid credential, unauthorized merchant, disabled POS capability, spending limit violation, and cross-school wallet purchases are denied or held without creating a normal debit.
- **SC-007**: 100% of sampled duplicate, retried, delayed, per-student reserve-exceeding, and per-terminal reserve-exceeding offline POS purchase submissions reconcile without creating duplicate or unauthorized wallet debits.
- **SC-008**: Spending limit changes are enforced for eligible future purchases within 1 minute of becoming active during review testing.
- **SC-009**: Authorized guardians and finance users can find a wallet transaction from the last 90 days by student, date range, type, amount, status, or merchant in under 30 seconds during review testing, and 100% of sampled guardian transaction history views for linked students show amount, merchant, item or category summary, status, and corrections while hiding staff-only terminal, operator, settlement, and internal review assignment details.
- **SC-010**: 100% of sampled refunds, reversals, chargebacks, pending recoveries, wallet restrictions, and manual adjustments preserve the original financial event and show the corrective event with actor, reason, and resulting wallet impact.
- **SC-011**: Daily reconciliation for a sampled school account matches wallet ledger totals to confirmed top-ups, POS purchases, refunds, reversals, chargebacks, and settlement references with no unexplained difference.
- **SC-012**: 100% of sampled Phase 4 records are visible only within the authorized school account scope or approved guardian link scope unless an explicit platform-level financial review role permits access.
- **SC-013**: Reviewers can trace a sampled wallet balance to top-ups, purchases, refunds, reversals, chargebacks, spending rules, reconciliation summaries, anomalies, and correction history in under 60 seconds.
- **SC-014**: 100% of sampled wallet records expose no full external payment credential while still showing safe payment references needed for support and reconciliation.
- **SC-015**: 100% of sampled detailed wallet financial records follow the school account's configured retention setting and preserve audit evidence for records under active review, dispute, recovery, or reconciliation hold.

## Assumptions

- Phase 4 builds on Phase 0 foundation rules, tenant configuration, feature flags, audit, and observability capabilities, and Phase 1 identity, guardian link, credential, role, and permission capabilities.
- Student wallets are the default wallet type for Phase 4; guardian and cashier actions fund or adjust student wallets rather than creating separate guardian stored-value wallets.
- Each school account uses one configured currency for Phase 4 wallet activity unless a later specification explicitly adds multi-currency support.
- External funding confirmation is provided by a school-approved external payment provider for guardian online top-ups or by an authorized school cashier workflow for cashier top-ups; unconfirmed funding is not spendable.
- Canteen POS integration in Phase 4 is limited to school-authorized canteen merchants, POS terminals, item or category summaries, and wallet purchase records.
- Offline POS spending is disabled by default and may be enabled only with school account per-student and per-terminal reserve limits, local evidence capture, duplicate prevention, and reconciliation review.
- General messaging and broadcast delivery belong to the later communication phase; Phase 4 only makes wallet transaction records, statuses, and review evidence available to authorized users.
- Tax, fee, and settlement details are recorded when supplied by the funding or merchant process, but Phase 4 does not replace an external accounting or tax compliance system.
- Detailed wallet financial record retention is configured by each school account with no platform-wide minimum in Phase 4.
- Manual wallet correction is allowed only for authorized financial reviewers and must preserve the original financial evidence.
- Reference frames under `docs/references/frames/` are contextual inspiration only and do not define Phase 4 requirements.
