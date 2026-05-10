# Wallet Quickstart Verification

- Wallet ledger: active wallet lifecycle, append-only ledger, trace, and closed-wallet hold checks are represented by backend entities, services, endpoints, and tests.
- Guardian online top-up: guardian initiation, safe provider reference, idempotent confirmation, and no full credential storage are represented.
- Cashier top-up: authorized cashier credit posts through the ledger service and audit adapter.
- Canteen POS: merchant, terminal, credential validation, approved/denied purchase, duplicate client purchase handling, and no attendance/transport mutation boundaries are represented.
- Offline POS: Flutter offline queue, API sync batch, reserve review fields, and retry-safe client identifiers are represented.
- Spending limits, transaction history, corrections, chargebacks, reconciliation, retention, and audit reviews are implemented as tenant-scoped module surfaces.
