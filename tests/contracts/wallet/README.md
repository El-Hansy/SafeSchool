# Wallet Contract Fixtures

This directory indexes Phase 4 contract fixtures for:

- Wallet ledger lifecycle and append-only traceability.
- Guardian online top-up, cashier top-up, payment confirmation, disputes, and chargebacks.
- Canteen POS purchases, offline POS sync, denial reasons, and trace evidence.
- Spending limits and strictest-rule enforcement.
- Transaction history, privacy-limited guardian history, refunds, reversals, reviews, and anomalies.
- Reconciliation, settlement references, review summaries, retention, close, reopen, and trace examples.

Backend routes are implemented under `/api/v1/schools/{schoolAccountId}/wallet` and guardian routes under `/api/v1/guardians/me/students/{studentProfileId}/wallet`.
