# Wallet UI Review

- School finance routes expose `/wallet`, `/wallet/top-ups`, `/wallet/pos`, `/wallet/limits`, `/wallet/review`, `/wallet/settings`, and `/wallet/reconciliation`.
- Guardian routes expose `/guardian/wallet`, `/guardian/wallet/top-ups`, `/guardian/wallet/transactions`, and `/guardian/wallet/limits`.
- The dashboard uses compact metrics, tab navigation, ledger/top-up/POS/reconciliation panels, and clear safety-boundary copy.
- The route move from `(guardian)/wallet` to `(guardian)/guardian/wallet` was required because Next.js route groups do not contribute URL segments; keeping both school and guardian pages at `(group)/wallet` fails production build.
