# Contract: Document Storage

This contract defines document upload, classification, metadata updates,
versioning, access decisions, archive, restore, legal hold, retention,
controlled export, restricted-detail minimization, and audit behavior for
Phase 10.

## Capabilities and Permissions

- Required capabilities:
  - `documents.storage`
  - `documents.retention`
  - `documents.legal_hold` when holds are used
  - `documents.exports` when exports are requested
- Common permissions:
  - `documents.create`
  - `documents.read`
  - `documents.update`
  - `documents.version`
  - `documents.archive`
  - `documents.restore`
  - `documents.hold`
  - `documents.export`
  - `documents.audit.read`

Documents are tenant-scoped, category-scoped, permission-scoped, versioned,
retention-aware, legal-hold-aware, and auditable. Binary content is referenced
through managed object metadata; authorization is always based on platform
metadata and current access decisions.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/documents` | Search document metadata within authorized scope |
| POST | `/api/v1/schools/{schoolAccountId}/documents` | Create document metadata and upload intent |
| GET | `/api/v1/schools/{schoolAccountId}/documents/{documentId}` | Read document detail |
| PUT | `/api/v1/schools/{schoolAccountId}/documents/{documentId}` | Update editable document metadata |
| GET | `/api/v1/schools/{schoolAccountId}/documents/{documentId}/download` | Request authorized document download |
| POST | `/api/v1/schools/{schoolAccountId}/documents/{documentId}/versions` | Add a new document version |
| GET | `/api/v1/schools/{schoolAccountId}/documents/{documentId}/versions` | List document versions |
| POST | `/api/v1/schools/{schoolAccountId}/documents/{documentId}/archive` | Archive eligible document |
| POST | `/api/v1/schools/{schoolAccountId}/documents/{documentId}/restore` | Restore eligible archived document |
| POST | `/api/v1/schools/{schoolAccountId}/documents/{documentId}/legal-holds` | Place legal hold on a document |
| POST | `/api/v1/schools/{schoolAccountId}/documents/{documentId}/exports` | Request controlled document export |
| GET | `/api/v1/schools/{schoolAccountId}/document-categories` | List document categories |
| POST | `/api/v1/schools/{schoolAccountId}/document-categories` | Create document category draft |
| POST | `/api/v1/schools/{schoolAccountId}/document-categories/{categoryId}/activate` | Activate document category version |

## Document Create Request

```yaml
document_category_id: "student-record-category"
title: "Student ID proof"
description: "Guardian-submitted identity document."
subject_type: "Student"
subject_reference: "student-reference"
source_module: "IdentityAccess"
source_record_reference: "guardian-link-reference"
visibility_level: "Restricted"
retention_policy_id: "student-document-retention"
file_name: "student-id.pdf"
file_type: "application/pdf"
file_size_bytes: 120000
content_hash: "sha256-content-hash"
client_request_id: "document-create-unique-to-caller"
```

## Document Metadata Update Request

```yaml
title: "Updated student ID proof"
description: "Corrected metadata after reviewer check."
visibility_level: "Reviewer Only"
update_reason: "Restricted document should not be guardian-visible."
client_request_id: "document-update-unique-to-caller"
```

## Document Version Request

```yaml
file_name: "student-id-corrected.pdf"
file_type: "application/pdf"
file_size_bytes: 125000
content_hash: "sha256-corrected-content-hash"
version_reason: "Replacement after incorrect first upload."
client_request_id: "document-version-unique-to-caller"
```

## Legal Hold Request

```yaml
hold_reason: "Required for active complaint review."
client_request_id: "document-hold-unique-to-reviewer"
```

## Controlled Export Request

```yaml
export_reason: "Compliance review for student file."
included_fields:
  - "metadata"
  - "active_version"
  - "audit_summary"
client_request_id: "document-export-unique-to-reviewer"
```

## Document Response

```yaml
document_id: "document-reference"
document_status: "Active"
document_category_id: "student-record-category"
title: "Student ID proof"
subject_type: "Student"
subject_reference: "student-reference"
visibility_level: "Restricted"
active_version_id: "document-version-reference"
legal_hold_state: "None"
retention_state: "Protected"
export_allowed: true
created_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Every route requires tenant access, enabled capability, actor permission, and
  backend feature enforcement.
- Upload and version creation require valid category, subject, source
  reference visibility, required metadata, validation state, retention rule,
  and audit evidence.
- Document visibility cannot exceed school account, approved guardian link,
  student ownership, staff assignment, source-module, ownership, audit,
  operations, review, or platform review authority.
- Legal hold and retention protection block deletion and permanent hiding while
  protection applies.
- Controlled exports require export capability, export permission, validated
  scope, included fields, reason capture, retention policy, and audit evidence.
- Document behavior must not create side effects in excluded source domains.
