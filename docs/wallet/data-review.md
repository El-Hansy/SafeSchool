# Wallet Data Review

- Every wallet-owned entity inherits tenant id, created at, and updated at metadata.
- Money is represented in minor units with currency validation in `WalletMoneyValidator`.
- Idempotency indexes cover ledger keys, top-ups, payment provider events, POS purchases, and offline POS batches.
- EF mapping covers wallet lookup, ledger trace, guardian visibility, POS terminal/merchant lookup, spending limits, corrections, anomalies, reconciliation, retention/review summaries, and audit events.
- Migration marker files document the Phase 4 persistence order from foundation through reconciliation.
