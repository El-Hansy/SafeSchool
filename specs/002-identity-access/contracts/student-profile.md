# Contract: Student Profile

This contract defines the `/api/v1/` behavior for creating, reviewing, listing,
updating, and deactivating student identity profiles.

## Capability and Permissions

- Required capability: `identity.student_profiles`
- Common permissions:
  - `identity.student_profiles.read`
  - `identity.student_profiles.create`
  - `identity.student_profiles.update`
  - `identity.student_profiles.deactivate`
  - `identity.student_profiles.review_history`

Every operation must resolve tenant context, check capability availability, and
enforce permissions before changing or returning tenant-owned data.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/identity/students` | Create a draft or active student profile |
| GET | `/api/v1/schools/{schoolAccountId}/identity/students` | List student profiles with pagination and filters |
| GET | `/api/v1/schools/{schoolAccountId}/identity/students/{studentProfileId}` | Read one student profile |
| PATCH | `/api/v1/schools/{schoolAccountId}/identity/students/{studentProfileId}` | Update controlled profile fields |
| POST | `/api/v1/schools/{schoolAccountId}/identity/students/{studentProfileId}/deactivate` | Deactivate a profile |
| GET | `/api/v1/schools/{schoolAccountId}/identity/students/{studentProfileId}/history` | Read profile audit history |
| POST | `/api/v1/schools/{schoolAccountId}/identity/students/duplicate-check` | Check whether identifiers collide before activation |

## Create Student Profile Request

```yaml
school_student_number: "S-10024"
external_identity_references:
  - reference_type: "school-import"
    reference_value: "legacy-234"
legal_name: "Student Legal Name"
preferred_name: "Student Preferred Name"
date_of_birth: "YYYY-MM-DD"
grade_level: "Grade 6"
campus_or_division: "Main Campus"
enrollment_status: "Enrolled"
profile_status: "Active"
review_reason: "Initial profile creation after school verification."
client_request_id: "request-unique-to-caller"
```

## Student Profile Response

```yaml
student_profile_id: "student-profile-reference"
school_account_id: "school-account-reference"
school_student_number: "S-10024"
external_identity_references:
  - reference_type: "school-import"
    reference_value: "legacy-234"
legal_name: "Student Legal Name"
preferred_name: "Student Preferred Name"
date_of_birth: "YYYY-MM-DD"
grade_level: "Grade 6"
campus_or_division: "Main Campus"
enrollment_status: "Enrolled"
profile_status: "Active"
duplicate_review_status: "Clear"
created_at: "YYYY-MM-DDTHH:MM:SSZ"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- `schoolAccountId` must match the resolved tenant context unless a
  platform-level review role explicitly permits inspection.
- Create and update operations must reject disabled or suspended
  `identity.student_profiles` capability.
- Active student profiles must be unique by school account plus configured
  active identity identifiers.
- Ambiguous duplicates must return a review-required result rather than
  activating a conflicting profile.
- Deactivation must not delete profile history, guardian link history,
  credential history, or audit evidence.
- List responses must be paginated and filterable by status, grade, campus or
  division, and duplicate review state.
- Sensitive create, update, duplicate conflict, and deactivate operations must
  produce Audit Events.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny with feature capability reason and record access decision |
| Missing permission | Deny with permission reason and record access decision |
| Tenant mismatch | Deny with tenant boundary reason and record access decision |
| Duplicate active identifiers | Reject activation and return duplicate review context |
| Invalid lifecycle transition | Reject with validation details |
| Profile not found in tenant | Return not found without exposing cross-tenant existence |
