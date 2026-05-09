# Wallet OpenAPI Review

The wallet endpoint registration exposes DTO-based `/api/v1/` route groups for student wallets, ledger entries, rule settings, top-ups, payment confirmations, canteen POS, offline sync, spending limits, transaction history, reviews, corrections, anomalies, reconciliation, settlement references, and review summaries.

OpenAPI example marker files cover common denial outcomes and safe financial payload examples. No route or DTO exposes full external payment credentials; guardian APIs use linked-student route scope and privacy-limited response shapes.
