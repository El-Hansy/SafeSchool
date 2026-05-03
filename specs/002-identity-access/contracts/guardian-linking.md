# Contract: Guardian Linking

This contract defines guardian records, guardian-student links, guardian
visibility, and link lifecycle review behavior.

## Capability and Permissions

- Required capability: `identity.guardian_linking`
- Common permissions:
  - `identity.guardians.read`
  - `identity.guardians.create`
  - `identity.guardians.update`
  - `identity.guardian_links.create`
  - `identity.guardian_links.approve`
  - `identity.guardian_links.suspend`
  - `identity.guardian_links.remove`
  - `identity.guardian_links.read_history`

Guardian visibility also depends on an Approved active Guardian Link and its
access scope.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/identity/guardians` | Create a guardian record |
| GET | `/api/v1/schools/{schoolAccountId}/identity/guardians` | List guardian records with pagination |
| GET | `/api/v1/schools/{schoolAccountId}/identity/guardians/{guardianId}` | Read a guardian record |
| PATCH | `/api/v1/schools/{schoolAccountId}/identity/guardians/{guardianId}` | Update guardian record details |
| POST | `/api/v1/schools/{schoolAccountId}/identity/students/{studentProfileId}/guardian-links` | Create a guardian link |
| PATCH | `/api/v1/schools/{schoolAccountId}/identity/guardian-links/{guardianLinkId}` | Update link access scope or review state |
| POST | `/api/v1/schools/{schoolAccountId}/identity/guardian-links/{guardianLinkId}/suspend` | Suspend a disputed or temporarily invalid link |
| POST | `/api/v1/schools/{schoolAccountId}/identity/guardian-links/{guardianLinkId}/remove` | Remove a link that is no longer valid |
| GET | `/api/v1/schools/{schoolAccountId}/identity/guardian-links/{guardianLinkId}/history` | Read link audit history |
| GET | `/api/v1/schools/{schoolAccountId}/identity/guardians/{guardianId}/students` | List students visible to the guardian |

## Create Guardian Link Request

```yaml
guardian_id: "guardian-reference"
relationship_type: "Legal Guardian"
access_scope:
  student_profile: "read"
  attendance: "none"
  transport: "none"
  wallet: "none"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_until: null
link_status: "Approved"
review_reason: "Relationship verified by school administration."
client_request_id: "request-unique-to-caller"
```

## Guardian Link Response

```yaml
guardian_link_id: "guardian-link-reference"
school_account_id: "school-account-reference"
student_profile_id: "student-profile-reference"
guardian_id: "guardian-reference"
relationship_type: "Legal Guardian"
access_scope:
  student_profile: "read"
  attendance: "none"
  transport: "none"
  wallet: "none"
link_status: "Approved"
valid_from: "YYYY-MM-DDTHH:MM:SSZ"
valid_until: null
review_reason: "Relationship verified by school administration."
created_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Guardian records and student profiles must belong to the same school account.
- A Guardian Record alone grants no student visibility.
- Only Approved links within their validity window grant access.
- Pending, suspended, expired, rejected, or removed links deny guardian access.
- Access scope must be explicit for each visible information category.
- Later-phase scopes such as attendance, transport, wallet, and learning may be
  present as disabled or `none`, but Phase 1 must not implement those outcomes.
- Link lifecycle changes must preserve review reason and Audit Events.
- Guardian-visible student lists must be calculated per guardian, per link, and
  per school account.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny with feature capability reason and record access decision |
| Guardian or student belongs to another tenant | Deny with tenant boundary reason |
| Link status does not permit access | Deny with link state reason |
| Missing access scope | Deny student visibility by default |
| Missing permission for link changes | Deny and record access decision |
| Invalid lifecycle transition | Reject with validation details |
