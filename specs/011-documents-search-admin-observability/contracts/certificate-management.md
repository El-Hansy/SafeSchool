# Contract: Certificate Management

This contract defines certificate type configuration, certificate issuance,
verification, correction, revocation, expiration, supersession, reissue,
controlled export, restricted-detail minimization, and audit behavior for
Phase 10.

## Capabilities and Permissions

- Required capabilities:
  - `documents.certificates`
  - `documents.storage` when a rendered certificate document is attached
  - `documents.exports` when exports are requested
- Common permissions:
  - `certificates.issue`
  - `certificates.read`
  - `certificates.verify`
  - `certificates.correct`
  - `certificates.revoke`
  - `certificates.reissue`
  - `certificates.export`
  - `certificates.audit.read`

Certificates are tenant-scoped, type-scoped, source-evidence-scoped,
versioned, verification-aware, and auditable. Certificates summarize or
reference source evidence without mutating the source-domain record.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/certificates` | Search certificates within authorized scope |
| POST | `/api/v1/schools/{schoolAccountId}/certificates` | Issue a certificate |
| GET | `/api/v1/schools/{schoolAccountId}/certificates/{certificateId}` | Read certificate detail |
| POST | `/api/v1/schools/{schoolAccountId}/certificates/{certificateId}/correct` | Correct an issued certificate |
| POST | `/api/v1/schools/{schoolAccountId}/certificates/{certificateId}/revoke` | Revoke a certificate |
| POST | `/api/v1/schools/{schoolAccountId}/certificates/{certificateId}/reissue` | Reissue or supersede a certificate |
| POST | `/api/v1/schools/{schoolAccountId}/certificates/{certificateId}/verify` | Verify certificate status for authorized actor |
| POST | `/api/v1/schools/{schoolAccountId}/certificates/{certificateId}/exports` | Request controlled certificate export |
| GET | `/api/v1/schools/{schoolAccountId}/certificate-types` | List certificate types |
| POST | `/api/v1/schools/{schoolAccountId}/certificate-types` | Create certificate type draft |
| POST | `/api/v1/schools/{schoolAccountId}/certificate-types/{typeId}/activate` | Activate certificate type version |
| GET | `/api/v1/guardians/me/certificates` | List guardian-visible certificates |
| GET | `/api/v1/students/me/certificates` | List student-visible certificates where enabled |

## Certificate Issue Request

```yaml
certificate_type_id: "attendance-summary"
subject_type: "Student"
subject_reference: "student-reference"
recipient_actor_id: "guardian-actor-reference"
source_module: "Attendance"
source_record_reference: "attendance-summary-reference"
source_summary: "Attendance summary for term 1."
language: "en"
valid_from: "YYYY-MM-DD"
valid_to: "YYYY-MM-DD"
visibility_level: "Recipient Visible"
client_request_id: "certificate-issue-unique-to-issuer"
```

## Certificate Correction Request

```yaml
corrected_source_summary: "Corrected attendance summary for term 1."
correction_reason: "Source attendance calculation was corrected."
recipient_impact_note: "Prior certificate remains visible as superseded."
client_request_id: "certificate-correction-unique-to-issuer"
```

## Certificate Revocation Request

```yaml
revocation_reason: "Issued against stale source evidence."
recipient_impact_note: "Certificate verification will show revoked status."
client_request_id: "certificate-revocation-unique-to-issuer"
```

## Verification Response

```yaml
certificate_id: "certificate-reference"
certificate_status: "Issued"
verification_state: "Valid"
visible_status: "Valid"
subject_type: "Student"
subject_reference: "student-reference"
issued_at: "YYYY-MM-DDTHH:MM:SSZ"
valid_from: "YYYY-MM-DD"
valid_to: "YYYY-MM-DD"
```

## Acceptance Rules

- Certificate issue, read, correction, revocation, reissue, verification, and
  export require tenant access, enabled capability, actor permission, source
  evidence visibility, and audit evidence.
- Certificate type activation requires valid issuer roles, subject rules,
  source evidence rules, validity behavior, visibility, duplicate active
  certificate policy, and dependent capabilities.
- Issuance rejects cross-school subjects, stale or unauthorized source
  evidence, disabled certificate types, missing required fields, duplicate
  active certificate conflicts, and conflicted issuers unless routed to review.
- Corrections, revocations, expirations, supersessions, and reissues preserve
  original issuance evidence, actor, time, reason, recipient impact,
  verification impact, and audit evidence.
- Verification responses expose only permitted status and summary for the
  requesting actor.
- Certificate behavior must not create side effects in excluded source domains.
