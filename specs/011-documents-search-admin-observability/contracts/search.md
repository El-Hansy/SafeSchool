# Contract: Global Search

This contract defines search indexing, search queries, result listing,
result-open access decisions, freshness states, denied result handling,
restricted-detail minimization, search exports, and audit behavior for Phase
10 and Phase 11.

## Capabilities and Permissions

- Required capabilities:
  - `documents.search_indexing`
  - `documents.global_search`
  - `documents.exports` when search result export is requested
- Common permissions:
  - `search.query`
  - `search.open_result`
  - `search.admin.read`
  - `search.reindex`
  - `search.export`
  - `search.audit.read`

Search entries are tenant-scoped source-record references. Search result
visibility is revalidated when results are listed and when a result is opened.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/search` | Search authorized records |
| GET | `/api/v1/schools/{schoolAccountId}/search/results/{entryId}` | Open a search result after access revalidation |
| GET | `/api/v1/schools/{schoolAccountId}/search/index-state` | Review search indexing freshness and failures |
| POST | `/api/v1/schools/{schoolAccountId}/search/reindex` | Request reindex for eligible records |
| POST | `/api/v1/schools/{schoolAccountId}/search/exports` | Request controlled export of permitted search results |
| GET | `/api/v1/guardians/me/search` | Guardian search within approved scope |
| GET | `/api/v1/students/me/search` | Student search within enabled self-scope |

## Search Query

```yaml
query: "attendance certificate"
record_type: "Certificate"
source_module: "Attendance"
student_profile_id: "student-reference"
category: "Certificate"
status: "Issued"
date_from: "YYYY-MM-DD"
date_to: "YYYY-MM-DD"
page: 1
page_size: 25
```

## Search Response

```yaml
query_log_id: "search-query-reference"
freshness_state: "Fresh"
results:
  - search_index_entry_id: "search-entry-reference"
    record_type: "Certificate"
    source_module: "Attendance"
    title: "Attendance certificate"
    summary: "Term 1 attendance certificate."
    status: "Issued"
    visible_fields:
      - "title"
      - "summary"
      - "status"
    access_decision: "Listed"
    action_hint: "Open"
page: 1
page_size: 25
total_visible_results: 1
```

## Reindex Request

```yaml
record_type: "Document"
source_module: "Documents"
source_record_reference: "document-reference"
reindex_reason: "Document metadata changed."
client_request_id: "search-reindex-unique-to-reviewer"
```

## Search Export Request

```yaml
query_log_id: "search-query-reference"
export_reason: "Compliance review of permitted document results."
included_fields:
  - "title"
  - "summary"
  - "status"
client_request_id: "search-export-unique-to-reviewer"
```

## Acceptance Rules

- Search requires tenant access, enabled search capability, actor permission,
  scoped filters, and audit evidence.
- Indexed records must be marked searchable for at least one authorized
  audience by the source module or document/certificate policy.
- Search listing and result opening must revalidate current tenant,
  guardian-link, student ownership, staff assignment, source-module,
  restricted-detail, feature configuration, and review authority.
- Results cannot reveal restricted title, summary, count, snippet, preview,
  facet, or source existence to unauthorized actors.
- Stale, failed, pending, suppressed, and review-required indexing states are
  visible to authorized administrators and reviewers.
- Broad, suspicious, stale, failed, or excessive searches may create
  operational exceptions.
- Search behavior must not create side effects in excluded source domains.
