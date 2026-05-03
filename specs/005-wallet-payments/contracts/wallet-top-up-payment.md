# Contract: Wallet Top-Up and Payment Confirmation

This contract defines guardian online top-ups, authorized cashier top-ups,
external payment confirmation handling, duplicate prevention, failed payment
handling, chargebacks, and safe payment references for Phase 4.

## Capabilities and Permissions

- Required capabilities:
  - `wallet.top_up`
  - `wallet.payment_processing` for guardian online payment flows and provider
    confirmations
- Common permissions:
  - `wallet.topups.initiate`
  - `wallet.topups.cashier`
  - `wallet.payments.confirm`
  - `wallet.ledger.read`
  - `wallet.chargebacks.review`
  - `wallet.audit.read`

Guardian online top-ups must be confirmed by an approved external payment
provider before funds become spendable. Cashier top-ups require authorized
cashier evidence. No route may store or return full external payment
credentials.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/guardians/me/students/{studentProfileId}/wallet/top-ups` | Initiate guardian online top-up for a linked student |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/wallet/top-ups` | List guardian-visible top-ups for a linked student |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/top-ups/cashier` | Record an authorized cashier top-up |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/top-ups` | List top-ups with finance filters |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/top-ups/{topUpId}` | Read top-up detail |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/payment-confirmations` | Receive normalized payment confirmation from approved provider adapter |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/payment-confirmations/{paymentConfirmationId}` | Read safe payment confirmation detail |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/top-ups/{topUpId}/mark-disputed` | Mark top-up disputed when provider or review evidence requires it |
| POST | `/api/v1/schools/{schoolAccountId}/wallet/top-ups/{topUpId}/chargeback-review` | Apply chargeback handling and pending recovery review |
| GET | `/api/v1/schools/{schoolAccountId}/wallet/top-ups/{topUpId}/trace` | Trace top-up to payment, ledger, chargeback, anomaly, review, reconciliation, and audit evidence |

## Guardian Top-Up Request

```yaml
student_wallet_id: "wallet-reference"
amount_minor: 5000
currency_code: "SAR"
payment_provider: "approved-provider-key"
return_reference: "safe-client-return-reference"
client_request_id: "request-unique-to-caller"
```

## Cashier Top-Up Request

```yaml
student_wallet_id: "wallet-reference"
amount_minor: 5000
currency_code: "SAR"
cashier_reference: "cashier-session-or-receipt-reference"
top_up_reason: "Cashier top-up approved by finance desk."
client_request_id: "request-unique-to-caller"
```

## Top-Up Response

```yaml
wallet_top_up_id: "top-up-reference"
school_account_id: "school-account-reference"
student_wallet_id: "wallet-reference"
student_profile_id: "student-profile-reference"
top_up_source: "Guardian Online Provider"
amount_minor: 5000
currency_code: "SAR"
fee_minor: 0
net_credit_minor: 5000
top_up_status: "Awaiting Confirmation"
payment_provider_reference: "safe-provider-reference"
safe_payment_method_summary: "Card ending 1234"
initiated_at: "YYYY-MM-DDTHH:MM:SSZ"
confirmed_at: null
credited_at: null
review_reason: null
```

## Payment Confirmation Request

```yaml
provider_reference: "safe-provider-payment-reference"
provider_event_id: "provider-event-unique-id"
top_up_reference: "top-up-reference"
confirmation_status: "Successful"
amount_minor: 5000
currency_code: "SAR"
safe_payment_method_summary: "Card ending 1234"
provider_event_time: "YYYY-MM-DDTHH:MM:SSZ"
normalized_payload_reference: "secure-normalized-evidence-reference"
client_request_id: "provider-event-unique-id"
```

## Payment Confirmation Response

```yaml
payment_confirmation_id: "payment-confirmation-reference"
wallet_top_up_id: "top-up-reference"
confirmation_status: "Successful"
confirmation_outcome: "Credited"
wallet_ledger_entry_id: "ledger-entry-reference"
duplicate: false
review_reason: null
received_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Chargeback Review Request

```yaml
payment_confirmation_id: "payment-confirmation-reference"
chargeback_amount_minor: 5000
currency_code: "SAR"
provider_event_id: "provider-chargeback-event-id"
chargeback_reason: "Provider reported dispute after settlement."
client_request_id: "provider-chargeback-event-id"
```

## Acceptance Rules

- Guardian top-up initiation requires an approved active guardian link to the
  student, active wallet, enabled `wallet.top_up`, enabled
  `wallet.payment_processing`, linked student scope, `wallet.topups.initiate`,
  amount rule validation, idempotent request identity, and audit evidence.
- Cashier top-up requires tenant access, active wallet, enabled
  `wallet.top_up`, `wallet.topups.cashier`, cashier source evidence, amount
  rule validation, idempotent request identity, and audit evidence.
- A top-up becomes spendable only when successful confirmation evidence is
  accepted and one approved credit ledger entry posts.
- Failed, cancelled, expired, pending, disputed, duplicate, and cross-school
  confirmations must not create duplicate or unauthorized wallet credits.
- Payment confirmation handling requires safe provider references, provider
  event identity uniqueness, top-up match validation, amount and currency match,
  and no storage or display of full external payment credentials.
- Duplicate confirmation submissions return the existing outcome when the
  provider event identity and content match.
- Chargeback after credited funds are spent restricts the wallet, records
  pending recovery, creates review evidence, and prevents further discretionary
  spending until authorized financial review resolves it.
- Top-up and payment list responses must be paginated and support filters for
  student, guardian, cashier, source, provider reference, amount, status,
  disputed state, chargeback state, date range, and review state.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Top-up capability disabled | Deny top-up and record feature capability reason |
| Payment processing disabled | Deny guardian online payment and provider confirmation workflow |
| Actor lacks tenant access | Deny without exposing school account data |
| Guardian not actively linked | Deny without exposing unrelated student wallet existence |
| Cashier lacks permission | Deny cashier top-up and record required permission |
| Payment confirmation lacks permission or adapter trust | Reject confirmation and record source reason |
| Wallet inactive, restricted, or cross-school | Reject or route to review according to wallet status |
| Amount or currency mismatch | Hold confirmation for review without wallet credit |
| Duplicate provider event | Return existing outcome without duplicate ledger entry |
| Chargeback after spend | Restrict wallet, record pending recovery, and create manual review item |
| Full payment credential supplied | Reject sensitive payload and record policy violation |
| Audit write fails for sensitive outcome | Reject mutation rather than allowing unaudited financial change |
