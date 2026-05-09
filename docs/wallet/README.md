# Wallet & Payments Implementation

Phase 4 adds a tenant-scoped Wallet feature area for student wallets, append-only ledger entries, guardian and cashier top-ups, safe payment confirmation handling, canteen POS purchases, bounded offline POS sync, spending limits, transaction history, refunds/reversals, chargeback recovery, reconciliation, wallet rule settings, review summaries, audit evidence, school web views, guardian web views, and Flutter POS offline queue support.

## Runtime Boundaries

- School routes are exposed under `/wallet` for finance, canteen, reviewer, rule-setting, and reconciliation workflows.
- Guardian routes are exposed under `/guardian/wallet` to avoid Next.js route-group collisions with school `/wallet` pages while preserving guardian linked-student scope in API routes.
- Backend routes are versioned under `/api/v1/schools/{schoolAccountId}/wallet` and `/api/v1/guardians/me/students/{studentProfileId}/wallet`.
- Wallet NFC/QR purchase scans do not create attendance, campus access, or transport outcomes.
- Payment confirmations keep provider references and safe payment summaries only; full external payment credentials are rejected by the provider adapter boundary.

## Capability Map

- `wallet.ledger`: wallets, balances, ledger trace, wallet status lifecycle.
- `wallet.top_up`: guardian online top-up and authorized cashier top-up.
- `wallet.payment_processing`: normalized payment confirmation and chargeback recovery.
- `wallet.canteen_pos`: merchants, item categories, terminals, purchases, offline POS sync.
- `wallet.spending_limits`: school and guardian limits with strictest-rule evidence.
- `wallet.transaction_history`: school and privacy-limited guardian transaction history.
- `wallet.reconciliation`: runs, mismatches, settlement references, retention and review summaries.

## Permission Map

Wallet permissions are declared in `WalletPermissionCatalog` for wallet management, ledger read/post, top-up initiation, cashier top-up, payment confirmation, POS record/sync/read, limits, history, guardian history, corrections, chargeback review, reconciliation, anomalies, rules, and audit read.

## Exclusions

Phase 4 does not implement tuition invoicing, payroll, staff expense management, external accounting replacement, banking account management, credit/lending, debt collection, cryptocurrency, broad marketplace commerce, general messaging, broad admin dashboards, attendance generation, campus entry/exit decisions, transport outcomes, or physical cash drawer hardware.
