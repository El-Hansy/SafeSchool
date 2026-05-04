# Contract: Course Content Delivery

This contract defines course and learning group setup, content publication,
student and guardian visibility, progress recording, duplicate handling, and
traceability for Phase 5.

## Capabilities and Permissions

- Required capabilities:
  - `learning.content_delivery`
  - `learning.progress_history` when progress views are read
- Common permissions:
  - `learning.courses.manage`
  - `learning.content.publish`
  - `learning.content.read`
  - `learning.progress.read`
  - `learning.student.access`
  - `learning.guardian_history.read`
  - `learning.audit.read`

Content and progress commands are tenant-scoped, permission-scoped, idempotent
where they mutate state, and auditable. Content resource references are
learning evidence only and do not create broad document storage workflows.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/learning/courses` | Create a course |
| GET | `/api/v1/schools/{schoolAccountId}/learning/courses` | List courses |
| POST | `/api/v1/schools/{schoolAccountId}/learning/groups` | Create or update a learning group |
| POST | `/api/v1/schools/{schoolAccountId}/learning/content` | Create draft content |
| POST | `/api/v1/schools/{schoolAccountId}/learning/content/{contentId}/publish` | Publish content to target learners |
| POST | `/api/v1/schools/{schoolAccountId}/learning/content/{contentId}/withdraw` | Withdraw published content with reason |
| GET | `/api/v1/students/me/learning/content` | List student-visible content |
| GET | `/api/v1/guardians/me/students/{studentProfileId}/learning/content` | List guardian-visible linked-student content |
| POST | `/api/v1/students/me/learning/content/{contentId}/progress` | Record student progress |
| GET | `/api/v1/schools/{schoolAccountId}/learning/content/{contentId}/trace` | Trace content to progress, exceptions, stars, reviews, and audit evidence |

## Content Publication Request

```yaml
course_id: "course-reference"
learning_group_id: "group-reference"
title: "Fractions practice"
description: "Practice equivalent fractions."
resource_reference: "learning-resource-reference"
release_at: "YYYY-MM-DDTHH:MM:SSZ"
expires_at: "YYYY-MM-DDTHH:MM:SSZ"
completion_policy: "Completion Required"
student_visibility_policy: "Full"
guardian_visibility_policy: "Progress Summary"
client_request_id: "content-publication-unique-to-caller"
```

## Progress Request

```yaml
progress_status: "Completed"
progress_percent: 100
source_event_reference: "student-progress-unique-reference"
occurred_at: "YYYY-MM-DDTHH:MM:SSZ"
client_request_id: "progress-unique-to-caller"
```

## Content Response

```yaml
learning_content_item_id: "content-reference"
school_account_id: "school-reference"
course_id: "course-reference"
learning_group_id: "group-reference"
content_status: "Published"
release_at: "YYYY-MM-DDTHH:MM:SSZ"
expires_at: "YYYY-MM-DDTHH:MM:SSZ"
visible_to_student: true
visible_to_guardian: true
progress_status: "In Progress"
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Every route requires tenant access, enabled capability, role permission, and
  backend feature enforcement.
- Staff routes require school-account permission plus active course, group, or
  coordinator assignment unless the actor has explicit administrator or review
  authority.
- Student routes require student self-scope and active learning group
  eligibility.
- Guardian routes require an approved active guardian link and school-approved
  guardian visibility.
- Published content requires title, target learners, release window, resource
  reference, and active course or group scope.
- Draft, withdrawn, expired, unassigned, disabled-feature, and cross-school
  content is blocked or hidden according to role and visibility rules.
- Progress updates are idempotent by source event and client request identity.
- Content and progress reads must not create attendance, transport, wallet,
  request, document, search, or notification delivery side effects.
