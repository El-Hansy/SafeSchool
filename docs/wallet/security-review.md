# Wallet Security Review

- Tenant isolation is enforced by route-scoped `schoolAccountId`, `ITenantContext`, Wallet DbSet tenant indexes, and `WalletPermissionGuard` denial recording.
- Guardian access uses linked-student API routes and guardian-specific web routes; staff-only settlement, terminal, operator, and internal review fields are suppressed in guardian summary/history projections.
- POS device scope is represented in terminal metadata and permission guard device checks.
- Full external payment credentials are not accepted by `DemoPaymentProviderAdapter`; only safe references and normalized provider event ids are stored.
- Ledger balance mutation goes through append-only `WalletLedgerPostingService`; corrections create refund/reversal/chargeback entries instead of editing original records.
- Chargebacks restrict wallet spending and record pending recovery for finance review.
- Sensitive wallet denial paths write access-decision evidence through the shared identity access writer.
