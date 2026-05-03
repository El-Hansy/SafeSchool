# Tasks: Phase 4 Wallet & Payments

**Input**: Design documents from `/specs/005-wallet-payments/`
**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/), [quickstart.md](./quickstart.md)

**Tests**: Included because the constitution and Phase 4 plan require unit,
integration, contract, authorization, tenant-isolation, audit, offline sync,
mobile/POS, retention, reconciliation, and critical UI journey coverage. Write
test tasks before implementation tasks in each user-story phase and confirm
they fail for the missing behavior before completing implementation.

**Executor guidance for lower-cost models**: Follow tasks in ID order unless a
task is marked `[P]`. Do not implement tuition invoicing, payroll, staff
expense management, external accounting replacement, banking account
management, credit/lending, debt collection, cryptocurrency, broad marketplace,
general messaging, broad admin dashboards, attendance generation, campus
entry/exit decisions, transport outcomes, or physical cash drawer hardware.
Every sensitive action must resolve tenant context, check the required Phase 4
capability, enforce permission, prevent cross-school visibility, record denied
access decisions, use minor-unit money values, avoid full external payment
credentials, and emit audit evidence.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no
  dependency on incomplete tasks in the same phase.
- **[Story]**: User story label required only for user-story phases.
- Every task includes exact target file paths.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create or extend the runtime project skeleton and test harnesses
described in [plan.md](./plan.md).

- [ ] T001 Create or update API solution and Wallet source/test root folders in apps/api/SafeSchool.sln and apps/api/src/SafeSchool.Api/Features/Wallet/
- [ ] T002 [P] Create or update API project manifest with ASP.NET Core, EF Core, Npgsql, authentication, validation, OpenAPI, logging, and background worker dependencies in apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj
- [ ] T003 [P] Create or update API test project manifest with xUnit, FluentAssertions, WebApplicationFactory, EF test helpers, time provider fakes, and coverage dependencies in apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj
- [ ] T004 [P] Create or update admin web package and TypeScript manifests with Next.js, React, TanStack Query, lint, route tests, and component test dependencies in apps/admin-web/package.json and apps/admin-web/tsconfig.json
- [ ] T005 [P] Create or update mobile package manifest with Flutter test, SQLite local storage, NFC integration, QR scanning, HTTP client, and permission dependencies in apps/mobile/pubspec.yaml
- [ ] T006 [P] Create repository coding defaults and generated-file ignores for API, web, mobile, coverage, local SQLite, provider fixture, and POS fixture artifacts in .editorconfig and .gitignore
- [ ] T007 Create API bootstrap with versioned routing, authentication, authorization, validation, OpenAPI, DbContext registration, background worker registration, and Wallet endpoint registration placeholders in apps/api/src/SafeSchool.Api/Program.cs
- [ ] T008 [P] Create API configuration placeholders for connection strings, JWT, logging, audit, feature flags, payment provider adapter keys, wallet top-up limits, POS offline reserve limits, retention, and duplicate retry limits in apps/api/src/SafeSchool.Api/appsettings.json and apps/api/src/SafeSchool.Api/appsettings.Development.json
- [ ] T009 [P] Create admin web school wallet shell route, POS route placeholder, settings route placeholder, review route placeholder, and feature export in apps/admin-web/src/app/(school)/wallet/page.tsx, apps/admin-web/src/app/(school)/wallet/pos/page.tsx, apps/admin-web/src/app/(school)/wallet/settings/page.tsx, apps/admin-web/src/app/(school)/wallet/review/page.tsx, and apps/admin-web/src/features/wallet/index.ts
- [ ] T010 [P] Create guardian wallet shell route, top-up route placeholder, transaction route placeholder, and feature export in apps/admin-web/src/app/(guardian)/wallet/page.tsx, apps/admin-web/src/app/(guardian)/wallet/top-ups/page.tsx, apps/admin-web/src/app/(guardian)/wallet/transactions/page.tsx, and apps/admin-web/src/features/guardian-wallet/index.ts
- [ ] T011 [P] Create mobile Wallet module export shell in apps/mobile/lib/features/wallet/wallet.dart
- [ ] T012 [P] Create contract test documentation index linking the seven Phase 4 contracts in tests/contracts/wallet/README.md
- [ ] T013 [P] Create implementation README linking spec, plan, contracts, quickstart, and this task list in docs/wallet/README.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build shared tenant, feature flag, authorization, identity evidence,
guardian scope, money, idempotency, persistence, audit, retention, API, web,
and mobile foundations required by every Phase 4 user story.

**Critical**: No user-story implementation should start until this phase is
complete.

- [ ] T014 Create shared Wallet tenant-owned entity base, result types, validation error type, paged response type, time provider abstraction, source metadata type, and review reason type in apps/api/src/SafeSchool.Api/Features/Wallet/Common/TenantOwnedEntity.cs and apps/api/src/SafeSchool.Api/Features/Wallet/Common/OperationResults.cs
- [ ] T015 [P] Create shared Wallet enums for wallet, ledger, top-up, payment confirmation, merchant, terminal, purchase, offline sync, spending limit, correction, settlement, anomaly, review, rule setting, feature, and retention states from data-model.md in apps/api/src/SafeSchool.Api/Features/Wallet/Common/WalletEnums.cs
- [ ] T016 Create or extend SafeSchoolDbContext registration for Wallet entities in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [ ] T017 Create Wallet model-builder extension for tenant metadata, timestamps, money precision, idempotency indexes, student wallet lookup, guardian visibility lookup, POS lookup, reconciliation lookup, review lookup, retention lookup, and foreign key relationships in apps/api/src/SafeSchool.Api/Features/Wallet/WalletDbContextModelBuilderExtensions.cs
- [ ] T018 Create Phase 4 capability constants for wallet.ledger, wallet.top_up, wallet.payment_processing, wallet.spending_limits, wallet.transaction_history, and wallet.canteen_pos in apps/api/src/SafeSchool.Api/Infrastructure/FeatureFlags/WalletCapabilities.cs
- [ ] T019 Create Wallet permission catalog entries for wallets, ledger, top-ups, cashier top-ups, payment confirmations, purchases, offline sync, limits, history, guardian history, corrections, chargebacks, reconciliation, anomalies, rules, audit, and review summaries in apps/api/src/SafeSchool.Api/Features/Wallet/Common/WalletPermissionCatalog.cs
- [ ] T020 Create Wallet permission guard that wraps tenant context, feature gate, permission evaluation, guardian link scope, POS device scope, access decision writing, and audit denial behavior in apps/api/src/SafeSchool.Api/Features/Wallet/Common/WalletPermissionGuard.cs
- [ ] T021 Create Wallet feature gate service that enforces Phase 4 capability keys server-side and returns typed disabled-capability errors in apps/api/src/SafeSchool.Api/Features/Wallet/Common/WalletFeatureGate.cs
- [ ] T022 Create IdentityAccess student profile adapter interface and test fake for active, inactive, missing, and cross-tenant student profiles in apps/api/src/SafeSchool.Api/Features/Wallet/Common/Identity/StudentProfileEvidenceProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Fixtures/FakeStudentProfileEvidenceProvider.cs
- [ ] T023 Create IdentityAccess guardian link adapter interface and test fake for approved, pending, suspended, expired, removed, rejected, and out-of-scope guardian links in apps/api/src/SafeSchool.Api/Features/Wallet/Common/Identity/WalletGuardianLinkProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Fixtures/FakeWalletGuardianLinkProvider.cs
- [ ] T024 Create IdentityAccess credential snapshot adapter interface and test fake for active, expired, suspended, revoked, replaced, unknown, duplicated, and cross-tenant NFC/QR credentials in apps/api/src/SafeSchool.Api/Features/Wallet/Common/Identity/WalletCredentialEvidenceProvider.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Fixtures/FakeWalletCredentialEvidenceProvider.cs
- [ ] T025 Create AttendanceAccess boundary adapter interface that exposes campus separation checks only and blocks attendance/campus-access mutation from Wallet in apps/api/src/SafeSchool.Api/Features/Wallet/Common/Boundaries/AttendanceAccessBoundary.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Fixtures/FakeAttendanceAccessBoundary.cs
- [ ] T026 Create Transport boundary adapter interface that blocks transport outcome mutation from Wallet purchase scans in apps/api/src/SafeSchool.Api/Features/Wallet/Common/Boundaries/TransportOutcomeBoundary.cs and apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Fixtures/FakeTransportOutcomeBoundary.cs
- [ ] T027 Create Wallet money value object and rounding validator for school-account currency, minor-unit precision, signed ledger amounts, source amount evidence, and invalid currency errors in apps/api/src/SafeSchool.Api/Features/Wallet/Common/Money/WalletMoney.cs and apps/api/src/SafeSchool.Api/Features/Wallet/Common/Money/WalletMoneyValidator.cs
- [ ] T028 Create Wallet idempotency service for client_request_id, provider_event_id, client_purchase_id, client_batch_id, ledger entry source keys, correction requests, reconciliation runs, rule settings, and review summary retries in apps/api/src/SafeSchool.Api/Features/Wallet/Common/Idempotency/WalletIdempotencyService.cs
- [ ] T029 Create Wallet audit event entity and audit writer adapter for wallet, ledger, top-up, payment, chargeback, purchase, offline sync, limit, history, correction, reconciliation, anomaly, review, rule, retention, feature, and access events in apps/api/src/SafeSchool.Api/Features/Wallet/Audit/WalletAuditEvent.cs and apps/api/src/SafeSchool.Api/Features/Wallet/Audit/WalletAuditWriter.cs
- [ ] T030 Create Wallet trace reference type for wallet, ledger, top-up, payment confirmation, purchase, limit, correction, settlement, anomaly, manual review, rule setting, review summary, and audit links in apps/api/src/SafeSchool.Api/Features/Wallet/Common/Trace/WalletTraceReference.cs
- [ ] T031 Create Wallet rule setting model, rule setting DTOs, and validation helper for currency, top-up min/max, cashier threshold, offline reserves, strictest-limit policy, refund windows, chargeback handling, anomaly policy, duplicate retry, reconciliation threshold, and school-configured retention in apps/api/src/SafeSchool.Api/Features/Wallet/Rules/WalletRuleSetting.cs, apps/api/src/SafeSchool.Api/Features/Wallet/Rules/WalletRuleSettingDtos.cs, and apps/api/src/SafeSchool.Api/Features/Wallet/Rules/WalletRuleSettingValidator.cs
- [ ] T032 Create Wallet rule setting service for draft create, update, activate, suspend, supersede, feature-scoped reads, idempotency, audit evidence, and active-hold retention validation in apps/api/src/SafeSchool.Api/Features/Wallet/Rules/WalletRuleSettingService.cs
- [ ] T033 Create Wallet route group registration and route prefix constants for /api/v1/schools/{schoolAccountId}/wallet and guardian /api/v1/guardians/me/students/{studentProfileId}/wallet routes in apps/api/src/SafeSchool.Api/Features/Wallet/WalletEndpointRegistration.cs
- [ ] T034 Create Wallet OpenAPI tag registration and shared response/error examples for tenant mismatch, disabled capability, missing permission, invalid money, guardian scope mismatch, duplicate request, audit failure, and cross-school reference in apps/api/src/SafeSchool.Api/Features/Wallet/WalletOpenApiExamples.cs
- [ ] T035 Create API test fixture for tenants, capabilities, permissions, students, guardians, credentials, wallets, money, idempotency, rule settings, POS sources, payment confirmations, reviews, retention, and audit assertions in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Fixtures/WalletTestFixture.cs
- [ ] T036 [P] Create shared API test data builders for tenants, capabilities, permissions, students, guardians, credentials, wallets, ledger entries, top-ups, payments, merchants, terminals, purchases, limits, corrections, settlements, anomalies, reviews, rule settings, and review summaries in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Fixtures/WalletTestData.cs
- [ ] T037 [P] Create unit tests for Phase 4 capability decisions and disabled-workflow denial in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Foundational/WalletFeatureGateTests.cs
- [ ] T038 [P] Create unit tests for WalletPermissionGuard tenant mismatch, guardian scope mismatch, POS device mismatch, missing permission, disabled capability, allowed access, denied access decision, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Foundational/WalletPermissionGuardTests.cs
- [ ] T039 [P] Create unit tests for WalletIdempotencyService retry, conflict, duplicate provider event, duplicate purchase, duplicate offline batch, duplicate correction, duplicate reconciliation run, and duplicate rule setting behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Foundational/WalletIdempotencyServiceTests.cs
- [ ] T040 [P] Create unit tests for WalletMoneyValidator currency mismatch, minor-unit precision, signed ledger amount, rounding evidence, negative amount denial, and source amount preservation in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Foundational/WalletMoneyValidatorTests.cs
- [ ] T041 [P] Create unit tests for WalletRuleSetting validation including top-up limits, cashier threshold, offline reserve pair requirement, chargeback restrict-and-review policy, anomaly policy, duplicate retry policy, retention without platform minimum, active hold preservation, activation, supersede, permission denial, and audit requirement in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Foundational/WalletRuleSettingTests.cs
- [ ] T042 [P] Create unit tests for WalletAuditWriter sensitive mutation failure behavior and denied access evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Foundational/WalletAuditWriterTests.cs
- [ ] T043 Create initial EF migration for Wallet audit, idempotency, feature setting references, rule settings, and retained review-summary foundations in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040012_WalletFoundation.cs
- [ ] T044 [P] Create admin web Wallet API client with tenant context, typed errors, pagination, rule-setting endpoints, review-summary endpoints, feature-disabled handling, money formatting, and guardian-scope error mapping in apps/admin-web/src/features/wallet/api/client.ts
- [ ] T045 [P] Create guardian Wallet API client with linked-student scope, typed errors, pagination, safe payment reference display, and feature-disabled handling in apps/admin-web/src/features/guardian-wallet/api/client.ts
- [ ] T046 [P] Create mobile Wallet API client shell with tenant context, auth headers, retry metadata, source metadata, POS device metadata, money encoding, and offline error mapping in apps/mobile/lib/features/wallet/wallet_api.dart
- [ ] T047 [P] Create mobile SQLite database wrapper and migration registry for wallet POS offline purchase queue, local rule snapshot cache, and local terminal cache tables in apps/mobile/lib/features/wallet/local/wallet_database.dart
- [ ] T048 [P] Create shared web test data builders for wallet, top-up, payment confirmation, merchant, terminal, purchase, limit, history, correction, settlement, anomaly, and review-summary states in apps/admin-web/tests/wallet/walletTestData.ts
- [ ] T049 [P] Create shared mobile test data builders for POS terminal, credential evidence, purchase request, offline batch, sync response, money values, and wallet rule snapshot states in apps/mobile/test/features/wallet/wallet_test_data.dart
- [ ] T050 Create Wallet rule settings endpoints for current read, draft create, update, activate, and suspend from contracts/wallet-rules-and-review-summary.md in apps/api/src/SafeSchool.Api/Features/Wallet/Rules/WalletRuleSettingsController.cs
- [ ] T051 Create admin web Wallet settings API hooks and settings page for rule setting read, draft update, activate, suspend, offline reserve validation, chargeback policy, and retention policy in apps/admin-web/src/features/wallet/rules/walletRulesApi.ts and apps/admin-web/src/app/(school)/wallet/settings/page.tsx
- [ ] T052 Create OpenAPI examples for wallet rule setting create, activate, suspend, offline reserve validation, chargeback policy validation, retention active-hold validation, disabled capability, missing permission, duplicate request, and audit failure in apps/api/src/SafeSchool.Api/Features/Wallet/Rules/WalletRulesOpenApiExamples.cs

**Checkpoint**: Foundation ready. User-story work can start after T014-T052 are complete.

---

## Phase 3: User Story 1 - Maintain Student Wallet Ledgers (Priority: P1) MVP

**Goal**: Finance managers can create, activate, suspend, close, restrict,
restore, review, list, and trace tenant-scoped student wallets with trustworthy
append-only ledger balances.

**Independent Test**: Create active wallets for students in one school account,
post an approved credit and debit, verify available/pending/held/settled
balances, verify entries are tenant-scoped and reviewable, and confirm
corrections use reversal entries instead of editing originals.

### Tests for User Story 1

- [ ] T053 [P] [US1] Create StudentWallet lifecycle unit tests for Draft, Active, Restricted, Suspended, Closed, one active wallet per student, inactive student denial, cross-school student denial, unresolved hold close denial, and restore review requirement in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Wallets/StudentWalletDomainTests.cs
- [ ] T054 [P] [US1] Create WalletLedgerEntry unit tests for append-only posting, credit, debit, hold, release, refund, reversal, manual adjustment, balance projection, negative available balance denial, original entry preservation, and idempotent source key behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Ledger/WalletLedgerEntryDomainTests.cs
- [ ] T055 [P] [US1] Create WalletLedgerPostingService unit tests for money precision, tenant mismatch, disabled wallet.ledger capability, missing wallet.ledger.post permission, duplicate source identity, audit failure, and reversal entry behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Ledger/WalletLedgerPostingServiceTests.cs
- [ ] T056 [P] [US1] Create contract tests for student-wallet create, bulk activate, list, detail, restrict, restore, suspend, close, ledger-entry list, ledger-entry detail, and wallet trace routes from contracts/wallet-ledger.md in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Wallets/WalletLedgerContractTests.cs
- [ ] T057 [P] [US1] Create integration tests for tenant isolation, wallet.ledger disabled capability, missing wallet.wallets.manage permission, inactive student, cross-school student, duplicate wallet activation, direct balance mutation rejection, unresolved hold close denial, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Wallets/WalletLedgerIntegrationTests.cs
- [ ] T058 [P] [US1] Create admin web wallet management journey tests for bulk activation, wallet list filters, wallet detail, ledger trace, wallet restriction, wallet suspension, close denial, disabled capability state, and access-denied state in apps/admin-web/tests/wallet/wallet-ledger.spec.ts
- [ ] T059 [P] [US1] Create API performance test for activating 30 active student wallets within 10 minutes using the wallet test fixture in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Wallets/WalletBulkActivationPerformanceTests.cs

### Implementation for User Story 1

- [ ] T060 [P] [US1] Create StudentWallet domain model with status transitions, one-active-wallet guard helpers, balance fields, pending recovery fields, tenant ownership, and student eligibility helpers in apps/api/src/SafeSchool.Api/Features/Wallet/Wallets/StudentWallet.cs
- [ ] T061 [P] [US1] Create WalletLedgerEntry domain model with append-only state, entry types, balance-after fields, source references, idempotency key, original entry link, and reversal helpers in apps/api/src/SafeSchool.Api/Features/Wallet/Ledger/WalletLedgerEntry.cs
- [ ] T062 [P] [US1] Create wallet lifecycle, wallet list, bulk activation, ledger entry, wallet trace, restriction, restore, suspend, close, and error DTOs matching contracts/wallet-ledger.md in apps/api/src/SafeSchool.Api/Features/Wallet/Wallets/WalletLedgerDtos.cs
- [ ] T063 [US1] Create EF configurations and migration for student wallets, ledger entries, tenant indexes, student wallet uniqueness, ledger source idempotency, wallet status indexes, pending recovery indexes, and ledger trace indexes in apps/api/src/SafeSchool.Api/Features/Wallet/Wallets/WalletLedgerEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040013_WalletLedger.cs
- [ ] T064 [US1] Implement StudentWalletService for create, bulk activate, view, list filters, restrict, restore, suspend, close, inactive student denial, duplicate wallet handling, unresolved hold close denial, tenant checks, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Wallets/StudentWalletService.cs
- [ ] T065 [US1] Implement WalletLedgerPostingService for approved credit, debit, hold, release, refund, reversal, chargeback, recovery, manual adjustment, balance projection, negative balance guard, source identity idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Ledger/WalletLedgerPostingService.cs
- [ ] T066 [US1] Implement WalletBalanceProjectionService for available, pending, held, settled, and pending recovery balances derived from approved ledger entries in apps/api/src/SafeSchool.Api/Features/Wallet/Ledger/WalletBalanceProjectionService.cs
- [ ] T067 [US1] Implement WalletLedgerQueryService for paginated ledger-entry list, ledger-entry detail, tenant-scoped filters, guardian-safe projection handoff, and read permission checks in apps/api/src/SafeSchool.Api/Features/Wallet/Ledger/WalletLedgerQueryService.cs
- [ ] T068 [US1] Implement WalletTraceService for wallet to ledger, top-up, payment, purchase, correction, settlement, anomaly, manual review, rule, and audit references when authorized in apps/api/src/SafeSchool.Api/Features/Wallet/Wallets/WalletTraceService.cs
- [ ] T069 [US1] Implement StudentWalletsController and WalletLedgerEntriesController routes from contracts/wallet-ledger.md in apps/api/src/SafeSchool.Api/Features/Wallet/Wallets/StudentWalletsController.cs and apps/api/src/SafeSchool.Api/Features/Wallet/Ledger/WalletLedgerEntriesController.cs
- [ ] T070 [US1] Wire wallet create, bulk activate, restrict, restore, suspend, close, ledger post, reversal, direct mutation rejection, trace read, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Wallets/WalletLedgerAuditAdapter.cs
- [ ] T071 [P] [US1] Create admin web wallet API hooks for wallet list, create, bulk activate, detail, restrict, restore, suspend, close, ledger entries, ledger detail, and trace in apps/admin-web/src/features/wallet/wallets/walletsApi.ts
- [ ] T072 [P] [US1] Create admin web wallet list table, balance summary, status badge, bulk activation dialog, lifecycle action menu, ledger table, and trace panel in apps/admin-web/src/features/wallet/wallets/WalletListTable.tsx and apps/admin-web/src/features/wallet/wallets/WalletTracePanel.tsx
- [ ] T073 [US1] Create admin web wallet management route with student filters, wallet filters, bulk activation, wallet detail drawer, ledger entries, lifecycle actions, disabled capability state, access-denied state, and trace links in apps/admin-web/src/app/(school)/wallet/page.tsx
- [ ] T074 [P] [US1] Create mobile wallet summary models for POS and review surfaces with safe balance fields, wallet status, and typed API errors in apps/mobile/lib/features/wallet/wallet_summary.dart
- [ ] T075 [US1] Add OpenAPI examples for wallet create, bulk activate, wallet list, wallet detail, restrict, restore, suspend, close, ledger list, ledger detail, wallet trace, inactive student denial, cross-school denial, duplicate request, direct balance mutation rejection, and audit failure in apps/api/src/SafeSchool.Api/Features/Wallet/Wallets/WalletLedgerOpenApiExamples.cs
- [ ] T076 [US1] Create wallet ledger test data builder for active wallet, inactive student, cross-school student, duplicate wallet, approved credit, approved debit, hold, release, reversal, pending recovery, and unresolved hold close-denial cases in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Wallets/WalletLedgerTestData.cs
- [ ] T077 [US1] Update Wallet endpoint registration to map StudentWalletsController and WalletLedgerEntriesController routes in apps/api/src/SafeSchool.Api/Features/Wallet/WalletEndpointRegistration.cs
- [ ] T078 [US1] Update SafeSchoolDbContext model builder to include StudentWallet and WalletLedgerEntry sets and configurations in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [ ] T079 [US1] Create contract documentation fixtures for wallet-ledger success, denial, and trace examples in tests/contracts/wallet/wallet-ledger-fixtures.md
- [ ] T080 [US1] Update docs/wallet/README.md with Wallet Ledger implementation notes, invariants, endpoints, test commands, and quickstart scenario mapping in docs/wallet/README.md

**Checkpoint**: User Story 1 can be demonstrated independently after T053-T080 pass.

---

## Phase 4: User Story 2 - Top Up Student Wallets (Priority: P1)

**Goal**: Guardians and authorized cashiers can add funds to an active linked
student wallet, with provider confirmations and cashier credits creating one
spendable ledger credit only after valid evidence.

**Independent Test**: Add funds to an active linked student wallet, confirm the
funding result, verify spendable balance increases once, and confirm failed,
cancelled, duplicate, disputed, and out-of-scope top-ups do not create
duplicate credits.

### Tests for User Story 2

- [ ] T081 [P] [US2] Create WalletTopUp domain tests for Draft, Initiated, Awaiting Confirmation, Confirmed, Credited, Failed, Cancelled, Expired, Disputed, Charged Back, Needs Review, guardian source, cashier source, and amount rule validation in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/TopUps/WalletTopUpDomainTests.cs
- [ ] T082 [P] [US2] Create PaymentConfirmation domain tests for safe provider reference, provider_event_id idempotency, success, failed, cancelled, expired, disputed, reversed, duplicate, amount mismatch, currency mismatch, and no full payment credential storage in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Payments/PaymentConfirmationDomainTests.cs
- [ ] T083 [P] [US2] Create GuardianTopUpService unit tests for active guardian link, pending guardian link denial, suspended guardian link denial, cross-school link denial, wallet.top_up disabled, wallet.payment_processing disabled, amount min/max, active wallet, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/TopUps/GuardianTopUpServiceTests.cs
- [ ] T084 [P] [US2] Create CashierTopUpService unit tests for authorized cashier, missing cashier permission, cashier threshold review, source evidence, inactive wallet denial, restricted wallet review, cross-school wallet denial, duplicate client_request_id, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/TopUps/CashierTopUpServiceTests.cs
- [ ] T085 [P] [US2] Create PaymentConfirmationService unit tests for successful confirmation crediting once, delayed confirmation, duplicate provider event, failed confirmation, cancelled confirmation, expired confirmation, disputed confirmation, cross-school top-up denial, amount mismatch hold, and audit failure in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Payments/PaymentConfirmationServiceTests.cs
- [ ] T086 [P] [US2] Create contract tests for guardian top-up initiate/list, cashier top-up, top-up list/detail, payment confirmation receive/detail, mark disputed, chargeback review placeholder, and top-up trace routes from contracts/wallet-top-up-payment.md in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/TopUps/WalletTopUpPaymentContractTests.cs
- [ ] T087 [P] [US2] Create integration tests for guardian online top-up, provider success credit, failed provider no-credit, duplicate confirmation no-duplicate-credit, cashier top-up credit, unauthorized cashier denial, out-of-scope guardian denial, safe payment display, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/TopUps/WalletTopUpPaymentIntegrationTests.cs
- [ ] T088 [P] [US2] Create guardian web top-up journey tests for linked-student wallet view, top-up initiation, awaiting confirmation, confirmed credit display, failed status display, duplicate status handling, and unlinked student denial in apps/admin-web/tests/wallet/guardian-top-up.spec.ts
- [ ] T089 [P] [US2] Create admin web cashier top-up journey tests for cashier top-up create, cashier evidence capture, threshold review message, failed validation, top-up list filters, and trace link display in apps/admin-web/tests/wallet/cashier-top-up.spec.ts
- [ ] T090 [P] [US2] Create API performance test verifying 95 percent of confirmed guardian top-ups become spendable credits within 2 minutes of confirmation availability in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/TopUps/TopUpConfirmationPerformanceTests.cs

### Implementation for User Story 2

- [ ] T091 [P] [US2] Create WalletTopUp domain model with source, status transitions, guardian link reference, cashier reference, amount fields, provider reference, idempotency key, and review reason helpers in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/WalletTopUp.cs
- [ ] T092 [P] [US2] Create PaymentConfirmation domain model with provider reference, provider_event_id, safe payment method summary, normalized payload reference, confirmation status, idempotency key, and credential-redaction helpers in apps/api/src/SafeSchool.Api/Features/Wallet/Payments/PaymentConfirmation.cs
- [ ] T093 [P] [US2] Create top-up, cashier top-up, payment confirmation, chargeback review, top-up trace, guardian top-up, safe payment reference, and error DTOs matching contracts/wallet-top-up-payment.md in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/WalletTopUpPaymentDtos.cs
- [ ] T094 [US2] Create EF configurations and migration for wallet top-ups, payment confirmations, top-up source indexes, guardian visibility indexes, provider_event_id uniqueness, safe provider references, status indexes, chargeback status fields, and ledger reference relationships in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/WalletTopUpPaymentEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040014_WalletTopUpsAndPayments.cs
- [ ] T095 [US2] Create payment provider adapter interface that exposes normalized safe payment confirmation data and rejects full external payment credential payloads in apps/api/src/SafeSchool.Api/Features/Wallet/Payments/PaymentProviderAdapter.cs
- [ ] T096 [US2] Implement GuardianTopUpService for linked-student validation, wallet status validation, top-up rule validation, approved provider request creation, awaiting confirmation state, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/GuardianTopUpService.cs
- [ ] T097 [US2] Implement CashierTopUpService for cashier permission, source evidence, cashier threshold review, immediate approved ledger credit through WalletLedgerPostingService, duplicate request handling, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/CashierTopUpService.cs
- [ ] T098 [US2] Implement PaymentConfirmationService for normalized provider event handling, amount and currency match, successful credit posting once, failed/cancelled/expired/disputed state changes, duplicate event return, safe payment references, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Payments/PaymentConfirmationService.cs
- [ ] T099 [US2] Implement TopUpQueryService for school top-up list/detail, guardian top-up list, safe payment detail projection, status filters, provider filters, cashier filters, and tenant/guardian scope enforcement in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/TopUpQueryService.cs
- [ ] T100 [US2] Implement TopUpTraceService for top-up to payment confirmation, ledger entry, chargeback state, anomaly, review, reconciliation, and audit references in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/TopUpTraceService.cs
- [ ] T101 [US2] Implement GuardianTopUpsController, SchoolTopUpsController, and PaymentConfirmationsController routes from contracts/wallet-top-up-payment.md in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/WalletTopUpPaymentControllers.cs
- [ ] T102 [US2] Wire top-up initiation, cashier credit, payment confirmation received, payment success, payment failure, duplicate confirmation, disputed top-up, no-credit denial, safe credential rejection, trace read, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/WalletTopUpPaymentAuditAdapter.cs
- [ ] T103 [P] [US2] Create guardian web top-up API hooks for linked-student wallet summary, initiate top-up, list top-ups, safe payment status, and typed denial errors in apps/admin-web/src/features/guardian-wallet/topups/guardianTopUpsApi.ts
- [ ] T104 [P] [US2] Create guardian web top-up form, top-up status list, safe payment reference display, linked-student selector, and failed/duplicate state display in apps/admin-web/src/features/guardian-wallet/topups/GuardianTopUpForm.tsx and apps/admin-web/src/features/guardian-wallet/topups/GuardianTopUpStatusList.tsx
- [ ] T105 [US2] Create guardian wallet top-up route with linked-student-only access, top-up form, status history, confirmation state display, no-full-credential display, and access-denied state in apps/admin-web/src/app/(guardian)/wallet/top-ups/page.tsx
- [ ] T106 [P] [US2] Create admin web top-up API hooks for cashier top-up, top-up list, payment confirmation detail, disputed mark, chargeback review placeholder, and trace in apps/admin-web/src/features/wallet/topups/topUpsApi.ts
- [ ] T107 [P] [US2] Create admin web cashier top-up form, top-up table, payment status badge, safe payment detail panel, cashier evidence field, and top-up trace panel in apps/admin-web/src/features/wallet/topups/CashierTopUpForm.tsx and apps/admin-web/src/features/wallet/topups/TopUpTracePanel.tsx
- [ ] T108 [US2] Create admin web top-up route with cashier top-up workflow, top-up filters, payment confirmation detail, disputed status, duplicate outcome visibility, trace links, and disabled capability state in apps/admin-web/src/app/(school)/wallet/top-ups/page.tsx
- [ ] T109 [US2] Add OpenAPI examples for guardian top-up, cashier top-up, payment confirmation success, failed confirmation, duplicate provider event, amount mismatch hold, safe payment detail, disputed top-up, chargeback review placeholder, unlinked guardian denial, and audit failure in apps/api/src/SafeSchool.Api/Features/Wallet/TopUps/WalletTopUpPaymentOpenApiExamples.cs
- [ ] T110 [US2] Create top-up and payment test data builder for linked guardian, unlinked guardian, active wallet, restricted wallet, cashier actor, provider success, provider failure, duplicate provider event, amount mismatch, dispute, and cross-school top-up cases in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/TopUps/WalletTopUpPaymentTestData.cs
- [ ] T111 [US2] Update Wallet endpoint registration to map GuardianTopUpsController, SchoolTopUpsController, and PaymentConfirmationsController routes in apps/api/src/SafeSchool.Api/Features/Wallet/WalletEndpointRegistration.cs
- [ ] T112 [US2] Update SafeSchoolDbContext model builder to include WalletTopUp and PaymentConfirmation sets and configurations in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [ ] T113 [US2] Create contract documentation fixtures for wallet-top-up-payment success, denial, duplicate, safe-payment, and trace examples in tests/contracts/wallet/wallet-top-up-payment-fixtures.md

**Checkpoint**: User Story 2 can be demonstrated independently after T081-T113 pass.

---

## Phase 5: User Story 3 - Process Canteen POS Purchases (Priority: P1)

**Goal**: Canteen POS operators can charge an active student wallet using NFC/QR
identity evidence, with approved purchases posting one debit and denied or
unsafe purchases preserving reviewable reasons without normal debits.

**Independent Test**: Use an active student credential at an authorized canteen
POS, purchase an allowed item within wallet balance and active rules, verify
one debit is posted, and confirm insufficient funds, invalid credentials,
disabled POS access, offline reserve conflicts, and cross-school wallets are
denied or held without a normal debit.

### Tests for User Story 3

- [ ] T114 [P] [US3] Create CanteenMerchant and POSTerminal domain tests for Draft, Active, Offline Allowed, Suspended, Retired, unique merchant_code, unique terminal_code, active merchant requirement, offline reserve requirement, device reference, and operator validation in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Pos/CanteenMerchantTerminalDomainTests.cs
- [ ] T115 [P] [US3] Create CanteenPurchaseTransaction domain tests for Submitted, Approved, Denied, Held for Review, Duplicate, Refunded, Reversed, online mode, offline mode, item summary, category codes, rule snapshot, reserve snapshot, and ledger debit reference in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Pos/CanteenPurchaseTransactionDomainTests.cs
- [ ] T116 [P] [US3] Create OnlinePurchaseAuthorizationService unit tests for active wallet, restricted wallet denial, active credential, invalid credential denial, active merchant, suspended terminal, sufficient funds, insufficient funds, POS operator permission, device authorization, duplicate client_purchase_id, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Pos/OnlinePurchaseAuthorizationServiceTests.cs
- [ ] T117 [P] [US3] Create OfflinePosSyncService unit tests for client_batch_id idempotency, client_purchase_id idempotency, local time preservation, received time preservation, per-student reserve, per-terminal reserve, stale rule snapshot, duplicate purchase, out-of-order purchase, disabled offline POS, and review routing in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Sync/OfflinePosSyncServiceTests.cs
- [ ] T118 [P] [US3] Create POS credential validation unit tests for NFC active credential, QR active credential, expired credential, suspended credential, revoked credential, replaced credential, unknown credential, duplicated credential, cross-school credential, and denial without normal debit in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Pos/WalletPosCredentialValidationTests.cs
- [ ] T119 [P] [US3] Create contract tests for merchant create/patch, terminal create/patch, online purchase, offline sync, purchase list/detail, purchase trace, and held purchase review placeholder routes from contracts/canteen-pos-purchase.md in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Pos/CanteenPosPurchaseContractTests.cs
- [ ] T120 [P] [US3] Create integration tests for approved online purchase debit, insufficient funds denial, invalid credential denial, unauthorized merchant denial, suspended terminal denial, spending limit port denial, cross-school wallet denial, offline sync accepted, offline reserve exceeded held, duplicate purchase no-duplicate-debit, no attendance outcome, no transport outcome, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Pos/CanteenPosPurchaseIntegrationTests.cs
- [ ] T121 [P] [US3] Create mobile offline POS queue tests for enqueue purchase, pending list, mark synced, mark held, mark rejected, duplicate client_purchase_id, local rule snapshot storage, reserve snapshot storage, and local terminal cache use in apps/mobile/test/features/wallet/pos/offline_pos_purchase_queue_test.dart
- [ ] T122 [P] [US3] Create mobile NFC/QR POS purchase tests for active terminal, NFC read success, QR read success, offline fallback, retry sync, insufficient funds display, invalid credential display, reserve exceeded display, and safe evidence display in apps/mobile/test/features/wallet/pos/wallet_pos_purchase_test.dart
- [ ] T123 [P] [US3] Create admin web POS journey tests for merchant create, terminal create, terminal suspend, purchase review filters, denied purchase detail, offline batch detail, trace link display, and disabled capability state in apps/admin-web/tests/wallet/canteen-pos.spec.ts
- [ ] T124 [P] [US3] Create API performance test verifying approved wallet purchases complete under 8 seconds during normal operating conditions in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Pos/CanteenPurchasePerformanceTests.cs

### Implementation for User Story 3

- [ ] T125 [P] [US3] Create CanteenMerchant domain model with merchant code uniqueness helpers, status transitions, allowed category codes, settlement policy, and tenant ownership in apps/api/src/SafeSchool.Api/Features/Wallet/Canteen/CanteenMerchant.cs
- [ ] T126 [P] [US3] Create POSTerminal domain model with terminal code, merchant reference, device reference, operator reference, offline_enabled, per-terminal reserve, last sync, status transitions, and offline readiness helpers in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/POSTerminal.cs
- [ ] T127 [P] [US3] Create OfflinePosSyncBatch and CanteenPurchaseTransaction domain models with idempotency, identity evidence, decision, local/received time, rule snapshot, reserve snapshot, review status, and ledger debit reference helpers in apps/api/src/SafeSchool.Api/Features/Wallet/Sync/OfflinePosSyncBatch.cs and apps/api/src/SafeSchool.Api/Features/Wallet/Pos/CanteenPurchaseTransaction.cs
- [ ] T128 [P] [US3] Create merchant, terminal, online purchase, offline sync, purchase list, purchase detail, purchase trace, review outcome placeholder, and error DTOs matching contracts/canteen-pos-purchase.md in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/CanteenPosPurchaseDtos.cs
- [ ] T129 [US3] Create EF configurations and migration for canteen merchants, POS terminals, offline POS sync batches, canteen purchase transactions, merchant indexes, terminal indexes, purchase idempotency indexes, credential indexes, offline reserve fields, review status fields, and ledger debit relationships in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/CanteenPosPurchaseEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040015_WalletCanteenPos.cs
- [ ] T130 [US3] Implement CanteenMerchantService for create, update, status changes, duplicate merchant code validation, tenant filters, capability enforcement, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Canteen/CanteenMerchantService.cs
- [ ] T131 [US3] Implement POSTerminalService for create, update, active/offline/suspend/retire state changes, active merchant validation, device authorization metadata, offline reserve validation, tenant filters, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/POSTerminalService.cs
- [ ] T132 [US3] Create WalletSpendingRuleEvaluationPort interface with a no-active-limits implementation used until US4 replaces it with strictest-rule evaluation in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/WalletSpendingRuleEvaluationPort.cs
- [ ] T133 [US3] Implement WalletPosCredentialValidationService for NFC/QR evidence, active credential lookup, invalid credential denial, duplicated credential denial, cross-school credential denial, non-secret credential references, and audit-safe decision reasons in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/WalletPosCredentialValidationService.cs
- [ ] T134 [US3] Implement OnlinePurchaseAuthorizationService for tenant, capability, permission, wallet status, credential status, merchant status, terminal status, operator/device authorization, available balance, spending rule port, idempotency, ledger debit posting, denial evidence, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/OnlinePurchaseAuthorizationService.cs
- [ ] T135 [US3] Implement OfflinePosReserveService for school rule setting lookup, per-student reserve snapshot, per-terminal reserve snapshot, disabled offline POS denial, stale snapshot detection, and reserve-exceeded review reasons in apps/api/src/SafeSchool.Api/Features/Wallet/Sync/OfflinePosReserveService.cs
- [ ] T136 [US3] Implement OfflinePosSyncService for batch idempotency, purchase idempotency, accepted offline purchase debit, held review routing, duplicate detection, local time preservation, received time preservation, stale rule snapshot handling, out-of-order handling, cross-school denial, and audit outcomes in apps/api/src/SafeSchool.Api/Features/Wallet/Sync/OfflinePosSyncService.cs
- [ ] T137 [US3] Implement CanteenPurchaseQueryService for purchase list/detail, filters by student, wallet, credential, merchant, terminal, operator, mode, amount, category, decision, sync status, anomaly status, review status, and time in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/CanteenPurchaseQueryService.cs
- [ ] T138 [US3] Implement CanteenPurchaseTraceService for purchase to wallet, credential, ledger, limit snapshot, anomaly, manual review, reconciliation, and audit references in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/CanteenPurchaseTraceService.cs
- [ ] T139 [US3] Implement CanteenMerchantsController, POSTerminalsController, CanteenPurchasesController, OfflinePosSyncController, and CanteenPurchaseTraceController routes from contracts/canteen-pos-purchase.md in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/CanteenPosPurchaseControllers.cs
- [ ] T140 [US3] Wire merchant create/update, terminal create/update, purchase approved, purchase denied, purchase held, offline sync, duplicate purchase, reserve exceeded, invalid credential, no attendance outcome, no transport outcome, trace read, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/CanteenPosPurchaseAuditAdapter.cs
- [ ] T141 [P] [US3] Create admin web POS API hooks for merchant list/create/update, terminal list/create/update, purchase list/detail, offline sync batch detail, purchase trace, and typed denial errors in apps/admin-web/src/features/wallet/pos/posApi.ts
- [ ] T142 [P] [US3] Create admin web merchant form, terminal form, terminal status badge, purchase table, purchase decision badge, offline sync summary, denied reason panel, and purchase trace panel in apps/admin-web/src/features/wallet/pos/MerchantTerminalForms.tsx and apps/admin-web/src/features/wallet/pos/PurchaseTracePanel.tsx
- [ ] T143 [US3] Create admin web POS route with merchant management, terminal management, purchase filters, offline sync review, denial detail, trace links, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/wallet/pos/page.tsx
- [ ] T144 [US3] Implement mobile POS purchase models, terminal cache model, offline batch model, sync response model, and typed error model in apps/mobile/lib/features/wallet/pos/wallet_pos_models.dart
- [ ] T145 [US3] Implement SQLite-backed offline POS purchase queue with enqueue, pending list, mark synced, mark held, mark rejected, duplicate client_purchase_id detection, rule snapshot storage, reserve snapshot storage, and migration in apps/mobile/lib/features/wallet/pos/offline_pos_purchase_queue.dart
- [ ] T146 [US3] Implement mobile NFC and QR wallet credential reader adapter interfaces with fakeable implementations for tests in apps/mobile/lib/features/wallet/pos/wallet_pos_credential_reader_adapter.dart
- [ ] T147 [US3] Implement mobile WalletPosRepository for terminal bootstrap, online purchase submission, offline fallback, batch sync, retry, typed errors, and safe display values in apps/mobile/lib/features/wallet/pos/wallet_pos_repository.dart
- [ ] T148 [US3] Implement mobile Wallet POS screen with terminal status, merchant label, NFC/QR mode, amount entry, item/category summary, online/offline indicator, reserve warning, retry action, and audit-safe result display in apps/mobile/lib/features/wallet/pos/wallet_pos_screen.dart
- [ ] T149 [US3] Add OpenAPI examples for merchant create, terminal create, online purchase approved, online purchase denied, offline sync accepted, offline sync held, invalid credential, insufficient funds, duplicate purchase, reserve exceeded, purchase trace, disabled capability, and audit failure in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/CanteenPosPurchaseOpenApiExamples.cs
- [ ] T150 [US3] Create POS purchase test data builder for active merchant, suspended merchant, active terminal, offline terminal, suspended terminal, active credential, invalid credential, sufficient wallet, insufficient wallet, duplicate purchase, offline batch, reserve exceeded, and cross-school wallet cases in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Pos/CanteenPosPurchaseTestData.cs
- [ ] T151 [US3] Update Wallet endpoint registration to map canteen merchant, POS terminal, canteen purchase, offline sync, and purchase trace routes in apps/api/src/SafeSchool.Api/Features/Wallet/WalletEndpointRegistration.cs
- [ ] T152 [US3] Update SafeSchoolDbContext model builder to include CanteenMerchant, POSTerminal, OfflinePosSyncBatch, and CanteenPurchaseTransaction sets and configurations in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [ ] T153 [US3] Create contract documentation fixtures for canteen-pos-purchase success, denial, offline, duplicate, reserve, and trace examples in tests/contracts/wallet/canteen-pos-purchase-fixtures.md

**Checkpoint**: User Story 3 can be demonstrated independently after T114-T153 pass.

---

## Phase 6: User Story 4 - Manage Spending Limits (Priority: P2)

**Goal**: Guardians and school administrators can configure wallet spending
limits, and purchases enforce the strictest active applicable rule with
versioned traceability.

**Independent Test**: Configure a daily limit, per-purchase limit, and canteen
category restriction for a linked student, verify allowed purchases pass, verify
disallowed purchases are denied with a clear reason, and confirm historical
purchases retain the rule version used.

### Tests for User Story 4

- [ ] T154 [P] [US4] Create SpendingLimit domain tests for Draft, Active, Suspended, Superseded, Expired, school owner, guardian owner, finance review owner, daily amount, weekly amount, per-purchase amount, merchant, category, time window, active date range, and wallet restriction types in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Limits/SpendingLimitDomainTests.cs
- [ ] T155 [P] [US4] Create SpendingLimitEvaluationService unit tests for strictest applicable rule, guardian limit cannot loosen school limit, review restriction wins, daily remaining amount, weekly remaining amount, category denial, merchant denial, time-window denial, active-date expiry, and rule version snapshot output in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Limits/SpendingLimitEvaluationServiceTests.cs
- [ ] T156 [P] [US4] Create SpendingLimitManagementService unit tests for school limit create, guardian limit create, active guardian link requirement, missing limit permission, invalid amount, invalid currency, invalid merchant, invalid category, duplicate client_request_id, activation, suspend, supersede, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Limits/SpendingLimitManagementServiceTests.cs
- [ ] T157 [P] [US4] Create contract tests for school spending limit create/list/detail/patch/activate/suspend, guardian limit create/list, effective rules, and limit trace routes from contracts/spending-limits.md in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Limits/SpendingLimitsContractTests.cs
- [ ] T158 [P] [US4] Create integration tests for school limit enforcement in POS purchase, guardian category restriction denial, strictest rule with school and guardian limits, future purchase enforcement within 1 minute, rule version trace, unlinked guardian denial, disabled capability denial, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Limits/SpendingLimitsIntegrationTests.cs
- [ ] T159 [P] [US4] Create guardian web spending limit journey tests for linked-student limit create, category restriction, per-purchase amount, active limits list, invalid limit message, and unlinked student denial in apps/admin-web/tests/wallet/guardian-spending-limits.spec.ts
- [ ] T160 [P] [US4] Create admin web spending limit journey tests for school daily limit, merchant limit, time-window limit, activation, suspension, effective rule read, trace link display, and disabled capability state in apps/admin-web/tests/wallet/spending-limits.spec.ts

### Implementation for User Story 4

- [ ] T161 [P] [US4] Create SpendingLimit domain model with owner type, guardian link reference, limit type, amount, merchant, category, time window, active date range, precedence rank, status, rule_version, and strictness helpers in apps/api/src/SafeSchool.Api/Features/Wallet/Limits/SpendingLimit.cs
- [ ] T162 [P] [US4] Create spending limit, effective rule, strictest rule, guardian limit, limit trace, purchase decision snapshot, and error DTOs matching contracts/spending-limits.md in apps/api/src/SafeSchool.Api/Features/Wallet/Limits/SpendingLimitDtos.cs
- [ ] T163 [US4] Create EF configurations and migration for spending limits, owner indexes, wallet indexes, guardian link indexes, merchant/category indexes, active date indexes, rule version indexes, and purchase rule snapshot relationship fields in apps/api/src/SafeSchool.Api/Features/Wallet/Limits/SpendingLimitEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040016_WalletSpendingLimits.cs
- [ ] T164 [US4] Implement SpendingLimitManagementService for school limit create/update/activate/suspend, guardian limit create/update/activate/suspend, active guardian link validation, value validation, idempotency, supersede behavior, list filters, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Limits/SpendingLimitManagementService.cs
- [ ] T165 [US4] Implement SpendingLimitEvaluationService for strictest active applicable rule, amount period aggregation, per-purchase amount, category restriction, merchant restriction, time-window restriction, active-date filtering, guardian/school/review precedence, denial reason, and rule version snapshot in apps/api/src/SafeSchool.Api/Features/Wallet/Limits/SpendingLimitEvaluationService.cs
- [ ] T166 [US4] Replace WalletSpendingRuleEvaluationPort no-active-limits implementation with SpendingLimitEvaluationService adapter used by OnlinePurchaseAuthorizationService and OfflinePosReserveService in apps/api/src/SafeSchool.Api/Features/Wallet/Pos/WalletSpendingRuleEvaluationPort.cs
- [ ] T167 [US4] Implement SpendingLimitTraceService for limit to purchases, denials, rule versions, reviews, and audit references in apps/api/src/SafeSchool.Api/Features/Wallet/Limits/SpendingLimitTraceService.cs
- [ ] T168 [US4] Implement SchoolSpendingLimitsController and GuardianSpendingLimitsController routes from contracts/spending-limits.md in apps/api/src/SafeSchool.Api/Features/Wallet/Limits/SpendingLimitsController.cs
- [ ] T169 [US4] Wire limit create, activate, suspend, supersede, effective rule read, purchase denial by limit, guardian scope denial, trace read, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Limits/SpendingLimitAuditAdapter.cs
- [ ] T170 [P] [US4] Create admin web spending limit API hooks for school limit create/list/detail/patch/activate/suspend, effective rules, trace, and typed errors in apps/admin-web/src/features/wallet/limits/spendingLimitsApi.ts
- [ ] T171 [P] [US4] Create admin web spending limit form, limit table, effective rule panel, strictest rule badge, rule version badge, and limit trace panel in apps/admin-web/src/features/wallet/limits/SpendingLimitForm.tsx and apps/admin-web/src/features/wallet/limits/SpendingLimitTracePanel.tsx
- [ ] T172 [US4] Create admin web spending limits route with student filters, limit filters, school limit creation, activation, suspension, effective rule display, trace links, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/wallet/limits/page.tsx
- [ ] T173 [P] [US4] Create guardian web spending limit API hooks and components for linked-student limit create/list, category restriction, amount limit, active-date limit, and typed guardian denial errors in apps/admin-web/src/features/guardian-wallet/limits/guardianSpendingLimitsApi.ts and apps/admin-web/src/features/guardian-wallet/limits/GuardianSpendingLimitForm.tsx
- [ ] T174 [US4] Create guardian spending limits route with linked-student selector, active limits list, create/suspend actions, strictest rule explanation, and unlinked student denial state in apps/admin-web/src/app/(guardian)/wallet/limits/page.tsx
- [ ] T175 [US4] Add OpenAPI examples for school limit create, guardian limit create, effective rules, strictest-rule denial, activate, suspend, trace, unlinked guardian denial, invalid amount, invalid date range, disabled capability, and audit failure in apps/api/src/SafeSchool.Api/Features/Wallet/Limits/SpendingLimitsOpenApiExamples.cs
- [ ] T176 [US4] Create spending limit test data builder for school daily limit, guardian category restriction, per-purchase limit, merchant limit, time-window limit, expired limit, review restriction, strictest rule, and cross-school wallet cases in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Limits/SpendingLimitTestData.cs
- [ ] T177 [US4] Update Wallet endpoint registration to map school and guardian spending limit routes in apps/api/src/SafeSchool.Api/Features/Wallet/WalletEndpointRegistration.cs
- [ ] T178 [US4] Update SafeSchoolDbContext model builder to include SpendingLimit set and configuration in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [ ] T179 [US4] Create contract documentation fixtures for spending-limits success, denial, strictest-rule, guardian, effective-rule, and trace examples in tests/contracts/wallet/spending-limits-fixtures.md
- [ ] T180 [US4] Update POS purchase documentation to state spending limits are enforced by SpendingLimitEvaluationService and historical purchase records keep rule snapshots in docs/wallet/README.md

**Checkpoint**: User Story 4 can be demonstrated independently after T154-T180 pass.

---

## Phase 7: User Story 5 - Review Transaction History and Corrections (Priority: P2)

**Goal**: Guardians, finance managers, and reviewers can view permission-scoped
wallet history and apply refunds, reversals, holds, releases, restrictions,
chargeback recovery, and manual corrections without hiding original events.

**Independent Test**: Review a wallet history, filter by top-up, purchase,
refund, and denial, issue an authorized refund or reversal with a reason, verify
original and corrective events are visible, verify guardian history is
privacy-limited, and verify chargeback after spend restricts the wallet with
pending recovery until financial review.

### Tests for User Story 5

- [ ] T181 [P] [US5] Create RefundOrReversal domain tests for Requested, Approved, Rejected, Posted, Needs Review, refund, void, reversal, hold, release, chargeback recovery, manual adjustment, original source reference, amount validation, and resulting ledger entry link in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Corrections/RefundReversalDomainTests.cs
- [ ] T182 [P] [US5] Create ManualWalletReview domain tests for Requested, In Review, Applied, Rejected, Closed, assign, resolve, dismiss, correct, refund, reverse, hold, release, restrict, record recovery, escalate, close, reason requirement, and original evidence preservation in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reviews/ManualWalletReviewDomainTests.cs
- [ ] T183 [P] [US5] Create WalletAnomaly domain tests for duplicate confirmation, duplicate purchase, negative available balance, offline overspend, invalid credential purchase, spending limit bypass, unmatched settlement, chargeback after spend, suspicious repeated attempt, manual-review-required, assign, resolve, dismiss, and reopen states in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Anomalies/WalletAnomalyDomainTests.cs
- [ ] T184 [P] [US5] Create TransactionHistoryQueryService unit tests for school history filters, wallet history filters, guardian linked-student filter, guardian privacy-limited fields, unlinked guardian denial, staff-only field hiding, pagination, tenant isolation, disabled transaction_history capability, and audit denial evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/History/TransactionHistoryQueryServiceTests.cs
- [ ] T185 [P] [US5] Create WalletCorrectionService unit tests for refund, void, reversal, hold, release, manual adjustment, correction after closed reconciliation, amount exceeds refundable denial, attempt to edit original event denial, idempotent client_request_id, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Corrections/WalletCorrectionServiceTests.cs
- [ ] T186 [P] [US5] Create ChargebackRecoveryService unit tests for chargeback after spend, wallet restriction, pending recovery amount, no further discretionary spending, original funding preservation, original spending preservation, reviewer restore, duplicate chargeback event, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Payments/ChargebackRecoveryServiceTests.cs
- [ ] T187 [P] [US5] Create WalletAnomalyDetectionService unit tests for duplicate top-up confirmation, duplicate POS purchase, negative available balance, offline overspend, invalid credential purchase, spending limit bypass, unmatched settlement, chargeback after spend, suspicious repeated attempts, and manual review required events in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Anomalies/WalletAnomalyDetectionServiceTests.cs
- [ ] T188 [P] [US5] Create contract tests for school transaction history, wallet transaction history, guardian transaction history, transaction detail, manual reviews, refunds-reversals, correction detail, and transaction trace routes from contracts/transaction-history-review.md in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/History/TransactionHistoryReviewContractTests.cs
- [ ] T189 [P] [US5] Create integration tests for history filters, guardian privacy-limited history, unlinked guardian denial, refund posting, reversal posting, hold/release posting, chargeback after spend restriction, pending recovery review, correction after closed reconciliation review item, anomaly creation, tenant isolation, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/History/TransactionHistoryReviewIntegrationTests.cs
- [ ] T190 [P] [US5] Create guardian web transaction history journey tests for amount, merchant, item/category summary, status, corrections, linked-student-only access, staff-only field hiding, and 90-day transaction search in apps/admin-web/tests/wallet/guardian-transaction-history.spec.ts
- [ ] T191 [P] [US5] Create admin web transaction review journey tests for wallet history filters, refund, reversal, hold, release, chargeback recovery review, anomaly assignment, manual review close, trace links, and access-denied state in apps/admin-web/tests/wallet/transaction-history-review.spec.ts
- [ ] T192 [P] [US5] Create API performance test verifying authorized users can find wallet transactions from the last 90 days in under 30 seconds and trace a sampled wallet balance in under 60 seconds in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/History/TransactionHistoryPerformanceTests.cs

### Implementation for User Story 5

- [ ] T193 [P] [US5] Create RefundOrReversal domain model with correction type, original source, amount, reviewer actor, ledger entry reference, client_request_id, reason, and state transition helpers in apps/api/src/SafeSchool.Api/Features/Wallet/Corrections/RefundOrReversal.cs
- [ ] T194 [P] [US5] Create ManualWalletReview domain model with review scope, scope reference, review action, review status, reason, reviewer actor, resulting ledger entry reference, client_request_id, and close helpers in apps/api/src/SafeSchool.Api/Features/Wallet/Reviews/ManualWalletReview.cs
- [ ] T195 [P] [US5] Create WalletAnomaly domain model with related source, anomaly type, severity, status, reviewer assignment, resolution reason, resolution history, detected_at, and reopen helpers in apps/api/src/SafeSchool.Api/Features/Wallet/Anomalies/WalletAnomaly.cs
- [ ] T196 [P] [US5] Create transaction history, guardian history, manual review, refund/reversal, correction response, anomaly, trace, and error DTOs matching contracts/transaction-history-review.md in apps/api/src/SafeSchool.Api/Features/Wallet/History/TransactionHistoryReviewDtos.cs
- [ ] T197 [US5] Create EF configurations and migration for refund/reversal records, manual wallet reviews, wallet anomalies, transaction history projection indexes, guardian history indexes, correction idempotency indexes, anomaly status indexes, and wallet restriction recovery fields in apps/api/src/SafeSchool.Api/Features/Wallet/History/TransactionHistoryReviewEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040017_WalletHistoryCorrectionsAndAnomalies.cs
- [ ] T198 [US5] Implement TransactionHistoryQueryService for school history, wallet history, guardian history, filters, privacy-limited guardian projection, staff-only field hiding, pagination, tenant scope, linked-student scope, and audit denial behavior in apps/api/src/SafeSchool.Api/Features/Wallet/History/TransactionHistoryQueryService.cs
- [ ] T199 [US5] Implement WalletCorrectionService for refund, void, reversal, hold, release, manual adjustment, original evidence lookup, amount validation, append-only ledger correction posting, closed reconciliation review item creation, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Corrections/WalletCorrectionService.cs
- [ ] T200 [US5] Implement ChargebackRecoveryService for provider chargeback/dispute after spend, wallet restriction, pending recovery ledger/review evidence, duplicate chargeback idempotency, reviewer restore behavior, and discretionary spending block in apps/api/src/SafeSchool.Api/Features/Wallet/Payments/ChargebackRecoveryService.cs
- [ ] T201 [US5] Implement ManualWalletReviewService for create/apply review action, assign, resolve, dismiss, correct, refund, reverse, hold, release, restrict, record recovery, escalate, close, reason requirement, permission checks, idempotency, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Reviews/ManualWalletReviewService.cs
- [ ] T202 [US5] Implement WalletAnomalyDetectionService for duplicate confirmation, duplicate purchase, negative balance, offline overspend, invalid credential purchase, spending limit bypass, unmatched settlement, chargeback after spend, suspicious repeated attempt, and manual review required detection in apps/api/src/SafeSchool.Api/Features/Wallet/Anomalies/WalletAnomalyDetectionService.cs
- [ ] T203 [US5] Implement WalletAnomalyReviewService for anomaly list, detail, assign, resolve, dismiss, reopen, resolution history, correction linkage, and audit evidence in apps/api/src/SafeSchool.Api/Features/Wallet/Anomalies/WalletAnomalyReviewService.cs
- [ ] T204 [US5] Implement TransactionTraceService for normalized transaction to original event, corrective event, ledger, anomaly, manual review, reconciliation, and audit evidence in apps/api/src/SafeSchool.Api/Features/Wallet/History/TransactionTraceService.cs
- [ ] T205 [US5] Implement TransactionHistoryController, GuardianTransactionHistoryController, ManualWalletReviewsController, RefundsReversalsController, WalletAnomaliesController, and TransactionTraceController routes from contracts/transaction-history-review.md in apps/api/src/SafeSchool.Api/Features/Wallet/History/TransactionHistoryReviewControllers.cs
- [ ] T206 [US5] Wire history read, guardian history denial, refund, reversal, hold, release, manual adjustment, chargeback recovery, wallet restriction, wallet restore, anomaly creation, anomaly resolution, trace read, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Wallet/History/TransactionHistoryReviewAuditAdapter.cs
- [ ] T207 [P] [US5] Create admin web transaction history API hooks for history list, transaction detail, review create/apply, refund/reversal create, correction detail, anomaly list/detail, anomaly review, and transaction trace in apps/admin-web/src/features/wallet/history/transactionHistoryApi.ts
- [ ] T208 [P] [US5] Create admin web transaction history table, transaction filters, transaction detail drawer, correction dialog, chargeback recovery panel, anomaly table, manual review dialog, and transaction trace panel in apps/admin-web/src/features/wallet/history/TransactionHistoryTable.tsx and apps/admin-web/src/features/wallet/history/TransactionReviewPanel.tsx
- [ ] T209 [US5] Create admin web wallet review route with transaction history filters, refund/reversal actions, hold/release actions, chargeback recovery review, anomaly workflow, trace links, disabled capability state, and access-denied state in apps/admin-web/src/app/(school)/wallet/review/page.tsx
- [ ] T210 [P] [US5] Create guardian web transaction history API hooks and components for linked-student history, amount, merchant, item/category summary, status, corrections, pagination, and typed denial errors in apps/admin-web/src/features/guardian-wallet/history/guardianTransactionHistoryApi.ts and apps/admin-web/src/features/guardian-wallet/history/GuardianTransactionHistoryTable.tsx
- [ ] T211 [US5] Create guardian transaction history route with linked-student selector, transaction filters, privacy-limited columns, correction summary, unlinked student denial, disabled capability state, and no staff-only field display in apps/admin-web/src/app/(guardian)/wallet/transactions/page.tsx
- [ ] T212 [US5] Add OpenAPI examples for school history, guardian history, unlinked guardian denial, refund, reversal, hold, release, chargeback recovery, manual review, anomaly resolution, transaction trace, closed reconciliation correction, disabled capability, and audit failure in apps/api/src/SafeSchool.Api/Features/Wallet/History/TransactionHistoryReviewOpenApiExamples.cs
- [ ] T213 [US5] Create transaction history and correction test data builder for top-up, purchase, refund, reversal, hold, release, chargeback, pending recovery, manual adjustment, guardian-visible transaction, staff-only field, anomaly, manual review, and closed reconciliation cases in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/History/TransactionHistoryReviewTestData.cs
- [ ] T214 [US5] Update Wallet endpoint registration to map transaction history, guardian history, manual review, refund/reversal, anomaly, and transaction trace routes in apps/api/src/SafeSchool.Api/Features/Wallet/WalletEndpointRegistration.cs
- [ ] T215 [US5] Update SafeSchoolDbContext model builder to include RefundOrReversal, ManualWalletReview, WalletAnomaly, and transaction history configuration in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs

**Checkpoint**: User Story 5 can be demonstrated independently after T181-T215 pass.

---

## Phase 8: User Story 6 - Reconcile Payments and POS Activity (Priority: P3)

**Goal**: Finance managers can reconcile wallet ledger totals against top-ups,
payment confirmations, POS batches, purchases, refunds, reversals, chargebacks,
and settlement references, closing matched totals and routing mismatches to
review without silently changing balances.

**Independent Test**: Select a school day, compare wallet ledger totals to
top-up confirmations, POS batches, refunds, reversals, and settlement
references, confirm matching totals close, and confirm mismatches become
reviewable exceptions with preserved source evidence.

### Tests for User Story 6

- [ ] T216 [P] [US6] Create SettlementReference domain tests for Draft, Matched, Mismatched, In Review, Closed, Reopened, payment provider scope, merchant scope, POS terminal scope, date range scope, funding source scope, total comparison, and review reason in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/SettlementReferenceDomainTests.cs
- [ ] T217 [P] [US6] Create WalletReviewSummary tests for student wallet, guardian, merchant, POS terminal, date range, settlement, anomaly scopes, permission-scoped counts, guardian detail suppression, latest evidence time, and trace references in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/WalletReviewSummaryTests.cs
- [ ] T218 [P] [US6] Create WalletReconciliationService unit tests for ledger totals, confirmed top-ups, POS batches, purchases, refunds, reversals, chargebacks, settlement references, matched close, mismatch creation, duplicate confirmation anomaly, duplicate purchase anomaly, offline overspend anomaly, unmatched settlement anomaly, and no silent balance mutation in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/WalletReconciliationServiceTests.cs
- [ ] T219 [P] [US6] Create WalletRetentionService unit tests for school-configured retention, no platform-wide minimum, active review hold preservation, active dispute preservation, active recovery preservation, reconciliation hold preservation, audit evidence retention, and detail reduction behavior in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/WalletRetentionServiceTests.cs
- [ ] T220 [P] [US6] Create contract tests for reconciliation run create/list/detail, mismatches, close, reopen, settlement reference list/detail, reconciliation trace, rule setting review summary routes, and guardian review summary route from contracts/wallet-reconciliation.md and contracts/wallet-rules-and-review-summary.md in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/WalletReconciliationContractTests.cs
- [ ] T221 [P] [US6] Create integration tests for daily reconciliation matched totals, mismatched settlement, unmatched provider confirmation, duplicate POS purchase, offline overspend, chargeback after spend, close, reopen after later correction, review summary filters, guardian summary privacy, tenant isolation, and audit evidence in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/WalletReconciliationIntegrationTests.cs
- [ ] T222 [P] [US6] Create admin web reconciliation journey tests for reconciliation run create, summary totals, mismatch list, anomaly link, close, reopen, settlement detail, review summary filters, retention status, and disabled permission state in apps/admin-web/tests/wallet/wallet-reconciliation.spec.ts
- [ ] T223 [P] [US6] Create guardian wallet summary journey tests for available balance, recent transaction counts, open visible issue count, linked-student-only access, and staff-only detail suppression in apps/admin-web/tests/wallet/guardian-wallet-summary.spec.ts
- [ ] T224 [P] [US6] Create API performance test verifying daily reconciliation matches sampled wallet ledger totals with no unexplained difference and review summaries load within expected query bounds in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/WalletReconciliationPerformanceTests.cs

### Implementation for User Story 6

- [ ] T225 [P] [US6] Create SettlementReference domain model with settlement scope, expected total, ledger total, difference, status, closed_by, closed_at, review reason, reopen helpers, and source evidence links in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/SettlementReference.cs
- [ ] T226 [P] [US6] Create WalletReviewSummary read model with summary scope, counts by wallet, transaction, spending limit, settlement, anomaly, review, latest evidence time, and trace references in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletReviewSummary.cs
- [ ] T227 [P] [US6] Create reconciliation run, mismatch, settlement reference, close, reopen, review summary, guardian review summary, retention, trace, and error DTOs matching contracts/wallet-reconciliation.md and contracts/wallet-rules-and-review-summary.md in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletReconciliationDtos.cs
- [ ] T228 [US6] Create EF configurations and migration for settlement references, reconciliation run persisted state, mismatch records, review summary read model, settlement indexes, source evidence indexes, close/reopen fields, retention hold fields, and trace indexes in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletReconciliationEntityTypeConfiguration.cs and apps/api/src/SafeSchool.Api/Infrastructure/Persistence/Migrations/202605040018_WalletReconciliation.cs
- [ ] T229 [US6] Implement WalletReconciliationService for run creation, ledger total aggregation, source total aggregation, top-up/payment/POS/refund/reversal/chargeback/settlement matching, mismatch creation, anomaly linkage, idempotency, and no silent balance mutation in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletReconciliationService.cs
- [ ] T230 [US6] Implement SettlementReferenceService for settlement list/detail, provider scope, merchant scope, POS terminal scope, funding source scope, date range scope, close, reopen, later chargeback handling, and audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/SettlementReferenceService.cs
- [ ] T231 [US6] Implement WalletReviewSummaryService for student wallet, guardian, merchant, POS terminal, date range, settlement, and anomaly summaries with permission-scoped counts and guardian detail suppression in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletReviewSummaryService.cs
- [ ] T232 [US6] Implement WalletRetentionService for school-configured detailed financial retention, active review/dispute/recovery/reconciliation hold preservation, audit evidence preservation, scheduled execution entrypoint, and retention audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletRetentionService.cs
- [ ] T233 [US6] Implement WalletReconciliationTraceService for reconciliation to ledger, payments, POS batches, corrections, anomalies, reviews, retention, and audit references in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletReconciliationTraceService.cs
- [ ] T234 [US6] Implement WalletReconciliationController, SettlementReferencesController, WalletReviewSummariesController, and GuardianWalletReviewSummaryController routes from contracts/wallet-reconciliation.md and contracts/wallet-rules-and-review-summary.md in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletReconciliationControllers.cs
- [ ] T235 [US6] Wire reconciliation run, mismatch detected, reconciliation close, reconciliation reopen, settlement reference read, review summary read, retention applied, trace read, and access denial audit events in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletReconciliationAuditAdapter.cs
- [ ] T236 [P] [US6] Create admin web reconciliation API hooks for run create/list/detail, mismatches, close, reopen, settlement references, review summaries, retention state, and trace in apps/admin-web/src/features/wallet/reconciliation/reconciliationApi.ts
- [ ] T237 [P] [US6] Create admin web reconciliation summary table, mismatch table, settlement detail panel, close dialog, reopen dialog, review summary filters, retention status badge, and reconciliation trace panel in apps/admin-web/src/features/wallet/reconciliation/ReconciliationSummary.tsx and apps/admin-web/src/features/wallet/reconciliation/ReconciliationTracePanel.tsx
- [ ] T238 [US6] Create admin web reconciliation route with date range, merchant, terminal, provider, funding source filters, run action, mismatch review, close/reopen actions, settlement links, summary filters, trace links, disabled permission state, and audit-safe displays in apps/admin-web/src/app/(school)/wallet/reconciliation/page.tsx
- [ ] T239 [P] [US6] Create guardian web wallet summary API hooks and component for available balance, recent transaction counts, visible issue count, linked-student-only scope, and staff-only detail suppression in apps/admin-web/src/features/guardian-wallet/summary/guardianWalletSummaryApi.ts and apps/admin-web/src/features/guardian-wallet/summary/GuardianWalletSummary.tsx
- [ ] T240 [US6] Create guardian wallet landing route with linked-student wallet summaries, recent visible transaction counts, visible issue count, safe balance display, and unlinked student denial state in apps/admin-web/src/app/(guardian)/wallet/page.tsx
- [ ] T241 [US6] Add OpenAPI examples for reconciliation run, matched close, mismatch list, reopen after chargeback, settlement reference detail, wallet review summary, guardian review summary, retention state, disabled permission, cross-school scope, and audit failure in apps/api/src/SafeSchool.Api/Features/Wallet/Reconciliation/WalletReconciliationOpenApiExamples.cs
- [ ] T242 [US6] Create reconciliation test data builder for matched day, mismatched settlement, unmatched provider confirmation, duplicate POS purchase, offline overspend, chargeback after spend, closed run, reopened run, guardian summary, merchant summary, and retention hold cases in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/WalletReconciliationTestData.cs
- [ ] T243 [US6] Update Wallet endpoint registration to map reconciliation, settlement reference, review summary, guardian review summary, and retention routes in apps/api/src/SafeSchool.Api/Features/Wallet/WalletEndpointRegistration.cs
- [ ] T244 [US6] Update SafeSchoolDbContext model builder to include SettlementReference, WalletReviewSummary, and reconciliation configurations in apps/api/src/SafeSchool.Api/Infrastructure/Persistence/SafeSchoolDbContext.cs
- [ ] T245 [US6] Create contract documentation fixtures for wallet-reconciliation and wallet-rules-review-summary success, mismatch, guardian summary, retention, close, reopen, and trace examples in tests/contracts/wallet/wallet-reconciliation-fixtures.md

**Checkpoint**: User Story 6 can be demonstrated independently after T216-T245 pass.

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Validate the full Phase 4 package, close quality gaps, and ensure
scope boundaries remain intact.

- [ ] T246 [P] Update final endpoint inventory, feature capability map, permission map, tenant isolation notes, and known Phase 4 exclusions in docs/wallet/README.md
- [ ] T247 [P] Update all implemented route groups, request/response fixture links, denial fixture links, and trace fixture links in tests/contracts/wallet/README.md
- [ ] T248 Run API wallet unit, integration, contract, authorization, tenant-isolation, audit, retention, reconciliation, and performance tests and record command results in docs/wallet/verification.md
- [ ] T249 Run admin web wallet school, guardian, POS, top-up, history, limits, review, and reconciliation journey tests and record command results in docs/wallet/verification.md
- [ ] T250 Run mobile wallet POS NFC/QR, offline queue, retry sync, reserve, and safe display tests and record command results in docs/wallet/verification.md
- [ ] T251 Perform security review for tenant isolation, guardian scope, POS device scope, payment credential redaction, correction permissions, chargeback recovery, audit failure behavior, and cross-school denial paths in docs/wallet/security-review.md
- [ ] T252 Perform observability review for structured logs, audit events, metrics, payment confirmation events, POS authorization events, offline sync events, anomaly events, reconciliation events, retention events, and error reports in docs/wallet/observability-review.md
- [ ] T253 Perform data review for indexes, EF migration order, minor-unit money precision, idempotency keys, retention holds, ledger append-only invariants, and reconciliation query performance in docs/wallet/data-review.md
- [ ] T254 Perform accessibility and responsive UI review for school wallet, guardian wallet, POS, top-up, history, limits, review, and reconciliation screens in docs/wallet/ui-review.md
- [ ] T255 Verify quickstart scenarios from specs/005-wallet-payments/quickstart.md and record pass/fail evidence in docs/wallet/quickstart-verification.md
- [ ] T256 Verify Phase 4 exclusions are not implemented by searching API, web, and mobile wallet code for tuition, payroll, accounting replacement, banking, credit, lending, debt collection, cryptocurrency, marketplace, messaging, attendance, access, transport outcome, and cash drawer behavior in docs/wallet/scope-boundary-review.md
- [ ] T257 Run OpenAPI generation and ensure wallet routes expose no full payment credentials, no cross-school identifiers, and no staff-only guardian fields in docs/wallet/openapi-review.md
- [ ] T258 Create final Phase 4 implementation summary with completed stories, validation commands, residual risks, and follow-up recommendations in docs/wallet/implementation-summary.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies. Can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion. Blocks all user
  stories.
- **User Stories (Phase 3+)**: All depend on Foundational completion.
- **Polish (Phase 9)**: Depends on all desired user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Starts after Foundational. Required MVP and required
  by every later story because all money movement depends on wallets and ledger
  posting.
- **User Story 2 (P1)**: Starts after US1 because top-ups post wallet ledger
  credits.
- **User Story 3 (P1)**: Starts after US1 because POS purchases post wallet
  ledger debits. Can run in parallel with US2 after US1.
- **User Story 4 (P2)**: Starts after US3 because spending limits are enforced
  during POS purchase authorization.
- **User Story 5 (P2)**: Starts after US2 and US3 because history and
  corrections need top-up and purchase evidence.
- **User Story 6 (P3)**: Starts after US2, US3, and US5 because reconciliation
  needs source financial events and correction evidence.

### Within Each User Story

- Tests required by the constitution must be written and fail before
  implementation.
- Models before EF configuration and migrations.
- DTOs before controllers.
- Services before endpoints.
- Tenant and feature flag enforcement before UI exposure.
- Idempotency and audit behavior before sensitive financial mutation is exposed.
- Story complete before moving to the next priority unless work is explicitly
  parallelized.

## Parallel Opportunities

- Setup tasks T002-T006 and T009-T013 can run in parallel after T001 creates the
  intended roots.
- Foundational tasks marked `[P]` can run in parallel after T014, T016, and
  T017 are understood.
- US1 tests T053-T059 can run in parallel; implementation tasks T060-T062,
  T071-T072, and T074 can run in parallel after DTO/model conventions are
  agreed.
- US2 tests T081-T090 can run in parallel; implementation tasks T091-T093,
  T103-T104, T106-T107 can run in parallel after US1 ledger services exist.
- US3 tests T114-T124 can run in parallel; implementation tasks T125-T128,
  T141-T142, and T144-T146 can run in parallel after US1 ledger services exist.
- US4 tests T154-T160 can run in parallel; implementation tasks T161-T162,
  T170-T171, and T173 can run in parallel after US3 purchase authorization is
  available.
- US5 tests T181-T192 can run in parallel; implementation tasks T193-T196,
  T207-T208, and T210 can run in parallel after US2 and US3 source records
  exist.
- US6 tests T216-T224 can run in parallel; implementation tasks T225-T227,
  T236-T237, and T239 can run in parallel after reconciliation source entities
  exist.

## Parallel Examples

### User Story 1

```text
Task: T053 StudentWallet lifecycle unit tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Wallets/StudentWalletDomainTests.cs
Task: T054 WalletLedgerEntry unit tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Ledger/WalletLedgerEntryDomainTests.cs
Task: T058 Admin web wallet journey tests in apps/admin-web/tests/wallet/wallet-ledger.spec.ts
```

### User Story 2

```text
Task: T081 WalletTopUp domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/TopUps/WalletTopUpDomainTests.cs
Task: T082 PaymentConfirmation domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Payments/PaymentConfirmationDomainTests.cs
Task: T088 Guardian web top-up journey tests in apps/admin-web/tests/wallet/guardian-top-up.spec.ts
```

### User Story 3

```text
Task: T114 Merchant and terminal domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Pos/CanteenMerchantTerminalDomainTests.cs
Task: T117 Offline POS sync unit tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Sync/OfflinePosSyncServiceTests.cs
Task: T121 Mobile offline POS queue tests in apps/mobile/test/features/wallet/pos/offline_pos_purchase_queue_test.dart
```

### User Story 4

```text
Task: T154 SpendingLimit domain tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Limits/SpendingLimitDomainTests.cs
Task: T155 SpendingLimitEvaluationService tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Limits/SpendingLimitEvaluationServiceTests.cs
Task: T159 Guardian web spending limit journey tests in apps/admin-web/tests/wallet/guardian-spending-limits.spec.ts
```

### User Story 5

```text
Task: T184 TransactionHistoryQueryService tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/History/TransactionHistoryQueryServiceTests.cs
Task: T186 ChargebackRecoveryService tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Payments/ChargebackRecoveryServiceTests.cs
Task: T190 Guardian transaction history journey tests in apps/admin-web/tests/wallet/guardian-transaction-history.spec.ts
```

### User Story 6

```text
Task: T218 WalletReconciliationService tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/WalletReconciliationServiceTests.cs
Task: T219 WalletRetentionService tests in apps/api/tests/SafeSchool.Api.Tests/Features/Wallet/Reconciliation/WalletRetentionServiceTests.cs
Task: T222 Admin web reconciliation journey tests in apps/admin-web/tests/wallet/wallet-reconciliation.spec.ts
```

## Implementation Strategy

### MVP First

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. Stop and validate wallet ledger independently against the US1 independent
   test.
5. Demo wallet creation, ledger posting, reversal correction, tenant isolation,
   and audit evidence before adding money movement workflows.

### Incremental Delivery

1. Setup plus Foundational creates tenant, permission, money, idempotency,
   rules, audit, and client foundations.
2. US1 delivers wallet ledger source of truth.
3. US2 adds funding into the ledger.
4. US3 adds spending from the ledger through POS.
5. US4 adds spending limits and strictest-rule enforcement.
6. US5 adds transparent history, correction, anomaly, and chargeback recovery.
7. US6 adds reconciliation, retention, and review summaries.

### Parallel Team Strategy

1. Team completes Setup and Foundational together.
2. One implementer takes US2 after US1 ledger posting exists.
3. Another implementer takes US3 after US1 ledger posting exists.
4. US4 starts after US3 purchase authorization exists.
5. US5 starts after top-up and purchase source evidence exists.
6. US6 starts after source events and correction evidence exist.

## Notes

- `[P]` tasks touch different files and can run in parallel when their phase
  prerequisites are satisfied.
- `[US1]` through `[US6]` map directly to the six user stories in [spec.md](./spec.md).
- Every route must preserve `/api/v1/` versioning and DTO-based responses.
- Every tenant-owned table must include `tenant_id`, `created_at`, and
  `updated_at`.
- Every sensitive financial mutation must fail if its required audit event
  cannot be written.
- Never store or display full external payment credentials.
- Commit after each task or small logical group when implementation work begins.
