# Implementation Plan: Phase 4 Wallet & Payments

**Branch**: `005-wallet-payments` | **Date**: 2026-05-04 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/005-wallet-payments/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 4 implements secure student wallet and school canteen payment workflows
for the School NFC platform. The implementation approach adds a Wallet feature
area for student wallets, append-only ledger entries, guardian online top-ups,
authorized cashier top-ups, external payment confirmation handling, canteen
merchant and POS purchase authorization, bounded offline POS capture, spending
limits, transaction history, refunds, reversals, chargeback recovery,
reconciliation summaries, anomaly review, wallet rule settings, and review
summaries. All behavior is scoped to a school account, gated by tenant feature
configuration, protected by RBAC and permission checks, backed by PostgreSQL
persistence, and observable through audit evidence.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration
and guardian web surfaces, and Flutter/Dart for POS-facing NFC/QR wallet
purchase capture, optional offline POS queues, and mobile transaction review
support. Exact package versions are pinned when runtime manifests are created.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL, JWT-based
authentication, IdentityAccess student profiles, approved guardian links,
credential status snapshots, tenant feature configuration, audit/event
logging, external payment provider adapter boundaries for guardian online
top-ups, Next.js App Router, TanStack Query or typed server-driven data access,
Flutter NFC/QR platform integrations, mobile SQLite offline queues, and Spec
Kit planning artifacts.
**Storage**: Single PostgreSQL database for tenant-owned student wallets,
ledger entries, top-ups, payment confirmations, canteen merchants, canteen item
categories, purchase eligibility rules, POS terminals, purchase transactions,
offline POS sync batches, spending limits, refunds, reversals, settlement
references, reconciliation summaries, wallet anomalies, manual wallet reviews,
wallet rule settings, feature settings, and audit evidence. Every tenant-owned
table includes `tenant_id`, `created_at`, and `updated_at`, with indexes for
tenant boundaries, student wallet lookup, guardian visibility, top-up
idempotency, payment confirmation idempotency, purchase idempotency, POS
terminal and merchant lookup, item category lookup, purchase eligibility lookup,
offline reserve reconciliation, spending limit evaluation, settlement matching,
anomaly status, review state, retention jobs, and audit traceability.
**Testing**: Backend unit tests for wallet lifecycle, ledger posting,
idempotent top-up confirmation, chargeback recovery, POS purchase validation,
offline POS reconciliation, spending limit precedence, transaction visibility,
refund and reversal state transitions, anomaly detection, retention, and
reconciliation; integration and contract tests for `/api/v1/` wallet routes;
authorization, tenant-isolation, feature-flag, and audit tests; mobile POS
offline queue and NFC/QR identity tests; and web/guardian journey tests for
wallet management, top-up, transaction history, limits, review, and
reconciliation flows.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
finance and canteen administration web app, guardian-facing wallet top-up and
history views, and POS-capable mobile or web clients for NFC/QR wallet
purchases and optional offline capture.
**Project Type**: Modular monolith SaaS with web, backend, and mobile clients
organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: create or activate
wallets for 30 active students in under 10 minutes, reflect 95% of confirmed
guardian top-ups as spendable credits within 2 minutes of confirmation
availability, complete approved canteen POS purchases in under 8 seconds during
review testing with a seeded active wallet, active credential, active merchant,
active POS terminal, warm database, local API test environment, and no external
payment-provider call in the purchase path, enforce spending limit changes for
eligible future purchases within 1 minute, find wallet transactions from the
last 90 days in under 30 seconds, trace a sampled wallet balance in under 60
seconds, and reconcile duplicate/retried/offline POS submissions without
duplicate or unauthorized wallet debits.
**Constraints**: Phase 4 only. Guardian online top-ups use an approved external
payment provider and authorized cashier top-ups use school-approved workflows.
The platform must not store or display full external payment credentials.
Offline POS is disabled by default and, when enabled, is limited by
school-configured per-student and per-terminal reserve limits. Chargebacks
after spend restrict the wallet, record pending recovery, and require financial
review before further discretionary spending. Detailed wallet financial record
retention is configured per school account with no Phase 4 platform-wide
minimum while preserving records under active review, dispute, recovery, or
reconciliation hold. Phase 4 excludes tuition invoicing, payroll, staff expense
management, external accounting replacement, banking account management,
credit/lending, debt collection, cryptocurrency, broad marketplace commerce,
general messaging, broad admin dashboards, attendance generation, campus
entry/exit decisions, transport outcomes, and physical cash drawer hardware.
**Scale/Scope**: Six Phase 4 modules from `PLAN.md`: Wallet Ledger, Wallet
Top-Up, Payment Processing, Spending Limits, Transaction History, and Canteen
POS Integration. Supporting anomaly review, manual correction, rule settings,
reconciliation, audit, and retention behavior are included only where required
by the Phase 4 spec. Reconciliation is exposed as its own workflow capability
when finance users run, close, reopen, or review reconciliation results.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md), which
  defines user stories, acceptance scenarios, requirements, entities, edge
  cases, clarifications, success criteria, and assumptions.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 4:
  Wallet & Payments and maps to Wallet Ledger, Wallet Top-Up, Payment
  Processing, Spending Limits, Transaction History, and Canteen POS
  Integration.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, tenant-owned
  wallet records, workflow capability keys including reconciliation, backend
  feature enforcement, and web/mobile feature gates are required before Phase 4
  workflows execute.
- **Security and authorization**: PASS. JWT authentication, tenant access, role
  assignments, permission checks, guardian-link funding and visibility checks,
  cashier authorization, finance review authorization, POS operator or device
  authorization, feature availability checks, and auditable access decisions are
  scoped for sensitive financial actions.
- **Data and API contracts**: PASS. PostgreSQL storage, EF Core migrations,
  DTO-based `/api/v1/` contracts, paginated list responses, tenant and lookup
  indexes, minor-unit money handling, and idempotent top-up, payment
  confirmation, purchase, offline sync, refund, reversal, review, and
  reconciliation commands are planned where applicable.
- **Offline NFC integrity**: PASS. Phase 4 defines POS NFC/QR identity
  evidence, optional mobile SQLite offline purchase queues, source evidence,
  local and received times, per-student and per-terminal reserve snapshots,
  timestamp conflict handling, idempotent sync APIs, reconciliation outcomes,
  and audit events.
- **Observability and testing**: PASS. Structured logs, audit events, wallet
  ledger metrics, payment confirmation metrics, POS authorization metrics,
  offline sync metrics, anomaly metrics, error reporting, backend tests,
  contract tests, authorization tests, tenant-isolation tests, mobile offline
  tests, and critical UI journey tests are scoped.
- **Simplicity and cost**: PASS. The modular monolith and single PostgreSQL
  database remain the default. No microservices, message queues, external
  accounting platform replacement, separate ledger database, banking subsystem,
  or broad CQRS patterns are introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve Phase 4 scope, keep tenant and permission
enforcement explicit, define financial idempotency and bounded offline POS
behavior, and introduce no constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/005-wallet-payments/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── canteen-pos-purchase.md
│   ├── spending-limits.md
│   ├── transaction-history-review.md
│   ├── wallet-ledger.md
│   ├── wallet-reconciliation.md
│   ├── wallet-rules-and-review-summary.md
│   └── wallet-top-up-payment.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/Wallet/
│   │   ├── Wallets/
│   │   ├── Ledger/
│   │   ├── TopUps/
│   │   ├── Payments/
│   │   ├── Canteen/
│   │   ├── Pos/
│   │   ├── Limits/
│   │   ├── History/
│   │   ├── Reconciliation/
│   │   ├── Anomalies/
│   │   ├── Reviews/
│   │   ├── Rules/
│   │   ├── Sync/
│   │   └── Audit/
│   └── tests/SafeSchool.Api.Tests/Features/Wallet/
├── admin-web/
│   ├── src/app/(school)/wallet/
│   ├── src/app/(guardian)/wallet/
│   ├── src/features/wallet/
│   └── tests/wallet/
└── mobile/
    ├── lib/features/wallet/
    └── test/features/wallet/

tests/
├── contracts/wallet/
└── e2e/wallet/
```

**Structure Decision**: Use the constitution's modular monolith baseline.
Backend feature folders own wallet lifecycle, ledger posting, top-up and
payment confirmation idempotency, POS purchase validation, offline sync,
spending limit evaluation, transaction history, refund and reversal workflows,
chargeback recovery, reconciliation, anomaly review, retention, persistence,
and audit events. Web code owns school finance, canteen manager, cashier,
reviewer, and guardian-facing wallet workflows. Mobile code is limited to POS
NFC/QR identity capture, offline purchase storage, retry-safe sync, and mobile
wallet transaction review where needed. The current repository contains
planning artifacts only, so these source paths are the implementation target
for the later `/speckit.tasks` output.

## Complexity Tracking

No constitution violations. No complexity exceptions are required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

## Phase 0: Research Output

Generated [research.md](./research.md) to resolve runtime scope, module
boundaries, Phase 0 and Phase 1 dependency use, tenant and feature capability
enforcement, wallet permissions, append-only ledger behavior, top-up funding
sources, payment confirmation idempotency, chargeback recovery, POS purchase
authorization, bounded offline POS handling, spending limit precedence,
guardian transaction visibility, reconciliation, anomaly review, retention,
observability, testing strategy, and implementation structure.

## Phase 1: Design & Contracts Output

Generated [data-model.md](./data-model.md), [quickstart.md](./quickstart.md),
and contracts under [contracts/](./contracts/) for wallet lifecycle and ledger
traceability, top-up and payment confirmation, canteen POS purchase capture and
offline sync, spending limit management, transaction history and correction
review, reconciliation, wallet rule settings, and review summaries. These
artifacts define entities, state transitions, validation rules, `/api/v1/`
interface behavior, idempotency requirements, guardian visibility boundaries,
retention behavior, audit evidence, and verification steps.

## Planning Readiness

The feature is ready for `/speckit.tasks`. Tasks must be grouped by the six
independently testable user stories in [spec.md](./spec.md), and each story must
include tenant resolution, feature flag checks, permission enforcement,
idempotency or offline behavior where applicable, audit evidence, and validation
coverage where it touches those concerns.
