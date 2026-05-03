# Contract: Credential Lifecycle

This contract defines NFC card and QR fallback credential issue, status,
replacement, suspension, revocation, rotation, and status snapshot behavior.

## Capabilities and Permissions

- Required capabilities:
  - `identity.nfc_credentials`
  - `identity.qr_fallback`
- Common permissions:
  - `identity.credentials.read`
  - `identity.credentials.issue`
  - `identity.credentials.suspend`
  - `identity.credentials.restore`
  - `identity.credentials.replace`
  - `identity.credentials.revoke`
  - `identity.credentials.rotate_qr`
  - `identity.credentials.read_history`

Credential commands must be retry-safe and must not create duplicate active
credentials when a caller retries the same request.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/identity/students/{studentProfileId}/credentials/nfc` | Issue an NFC card credential |
| POST | `/api/v1/schools/{schoolAccountId}/identity/credentials/{credentialId}/suspend` | Suspend a credential |
| POST | `/api/v1/schools/{schoolAccountId}/identity/credentials/{credentialId}/restore` | Restore a suspended credential |
| POST | `/api/v1/schools/{schoolAccountId}/identity/credentials/{credentialId}/replace` | Replace a credential |
| POST | `/api/v1/schools/{schoolAccountId}/identity/credentials/{credentialId}/revoke` | Revoke a credential |
| POST | `/api/v1/schools/{schoolAccountId}/identity/students/{studentProfileId}/credentials/qr` | Create a QR fallback credential |
| POST | `/api/v1/schools/{schoolAccountId}/identity/credentials/{credentialId}/rotate-qr` | Rotate a QR fallback credential |
| GET | `/api/v1/schools/{schoolAccountId}/identity/students/{studentProfileId}/credentials` | List credentials for a student |
| GET | `/api/v1/schools/{schoolAccountId}/identity/credentials/{credentialId}/history` | Read credential audit history |
| GET | `/api/v1/schools/{schoolAccountId}/identity/credentials/status-snapshot` | Get credential status evidence for later scan flows |

## Issue NFC Credential Request

```yaml
card_reference: "non-secret-card-reference"
card_label: "Blue card 124"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_until: null
status_reason: "Initial card issue."
client_request_id: "request-unique-to-caller"
```

## Create QR Fallback Credential Request

```yaml
qr_reference: "non-secret-qr-reference"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_until: "YYYY-MM-DDTHH:MM:SSZ"
rotation_reason: "Initial QR fallback issue."
client_request_id: "request-unique-to-caller"
```

## Credential Response

```yaml
identity_credential_id: "credential-reference"
school_account_id: "school-account-reference"
student_profile_id: "student-profile-reference"
credential_type: "NFC Card"
credential_reference: "non-secret-card-reference"
credential_status: "Active"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_until: null
issued_at: "YYYY-MM-DDTHH:MM:SSZ"
status_reason: "Initial card issue."
created_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Credential Status Snapshot Response

```yaml
items:
  - credential_status_snapshot_id: "snapshot-reference"
    school_account_id: "school-account-reference"
    student_profile_id: "student-profile-reference"
    identity_credential_id: "credential-reference"
    credential_type: "NFC Card"
    credential_status: "Active"
    valid_from: "YYYY-MM-DDTHH:MM:SSZ"
    valid_until: null
    revoked_at: null
    snapshot_generated_at: "YYYY-MM-DDTHH:MM:SSZ"
    snapshot_expires_at: "YYYY-MM-DDTHH:MM:SSZ"
page:
  size: 100
  next_cursor: "cursor-or-null"
```

## Acceptance Rules

- NFC commands require `identity.nfc_credentials` to be enabled.
- QR commands require `identity.qr_fallback` to be enabled.
- Credentials can be issued only to verified active student profiles in the
  same school account.
- One active NFC card reference can belong to only one active student identity
  within a school account at a time.
- QR fallback credentials require a validity window and rotation evidence.
- Suspended, replaced, expired, or revoked credentials must not appear as
  current identity evidence in status snapshots.
- Replacing a credential must link the old and new credential records.
- Lifecycle commands must preserve status reason, actor, tenant, target, and
  audit evidence.
- Status snapshots are identity evidence only and must not define attendance,
  campus access, or transport outcomes.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny with feature capability reason and record access decision |
| Missing credential permission | Deny and record access decision |
| Student is not active | Reject issue request with student status reason |
| Duplicate active NFC card reference | Reject issue request and identify conflict for review |
| QR fallback missing validity window | Reject request with validation details |
| Credential already revoked | Treat retry as same final state when client request identity matches; otherwise reject invalid transition |
| Tenant mismatch | Deny without exposing cross-tenant credential existence |
