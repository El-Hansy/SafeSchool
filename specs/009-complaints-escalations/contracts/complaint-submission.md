# Contract: Complaint Submission

This contract defines guardian, student, and staff complaint intake, complaint
tracking, status visibility, withdrawal, duplicate handling, evidence
references, and audit behavior for Phase 8.

## Capabilities and Permissions

- Required capabilities:
  - `complaints.submission`
  - `complaints.history` when complaint history is read
- Common permissions:
  - `complaints.submit.guardian`
  - `complaints.submit.student`
  - `complaints.submit.staff`
  - `complaints.read.own`
  - `complaints.read.assigned`
  - `complaints.withdraw.own`
  - `complaints.audit.read`

Complaint submission commands are tenant-scoped, permission-scoped, idempotent
where they mutate state, and auditable. Authenticated intake is the baseline
for Phase 8.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/complaints` | Submit staff or school-admin complaint |
| GET | `/api/v1/schools/{schoolAccountId}/complaints` | List complaints visible to the current school actor |
| GET | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}` | Read complaint detail within authorized scope |
| POST | `/api/v1/schools/{schoolAccountId}/complaints/{complaintId}/withdraw` | Withdraw eligible complaint with reason |
| POST | `/api/v1/guardians/me/students/{studentProfileId}/complaints` | Submit guardian complaint for linked student |
| GET | `/api/v1/guardians/me/complaints` | List guardian-visible complaints |
| GET | `/api/v1/guardians/me/complaints/{complaintId}` | Read guardian-visible complaint detail |
| POST | `/api/v1/students/me/complaints` | Submit student complaint where enabled |
| GET | `/api/v1/students/me/complaints` | List student-visible own complaints |
| GET | `/api/v1/students/me/complaints/{complaintId}` | Read student-visible own complaint detail |

## Complaint Submission Request

```yaml
student_profile_id: "student-reference"
category_id: "complaint-category-reference"
subcategory: "Transport Pickup"
priority_hint: "Normal"
additional_participants:
  - participant_type: "Student"
    participant_reference: "second-student-reference"
    participant_role: "Affected Student"
  - participant_type: "SchoolUnit"
    participant_reference: "transport-office"
    participant_role: "Involved Party"
description: "Complaint statement from submitter."
event_window_start: "YYYY-MM-DDTHH:MM:SSZ"
event_window_end: "YYYY-MM-DDTHH:MM:SSZ"
requested_outcome: "Requested outcome from complainant."
evidence_references:
  - source_module: "Transport"
    source_record_reference: "route-or-trip-reference"
client_request_id: "complaint-submission-unique-to-caller"
```

## Complaint Submission Response

```yaml
complaint_id: "complaint-reference"
tracking_reference: "CMP-2026-0001"
complaint_status: "Submitted"
category_id: "complaint-category-reference"
priority: "Normal"
confidentiality_level: "Standard"
target_response_at: "YYYY-MM-DDTHH:MM:SSZ"
already_processed: false
visible_to_complainant: true
submitted_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Withdrawal Request

```yaml
withdrawal_reason: "Issue resolved before school review."
client_request_id: "complaint-withdrawal-unique-to-caller"
```

## Acceptance Rules

- Every route requires tenant access, enabled capability, role permission, and
  backend feature enforcement.
- Guardian routes require an approved active guardian link unless school rules
  permit a restricted personal-submitter view.
- Student routes require student self-scope and enabled student complaint
  submission for the selected category.
- Staff routes require school-account staff authority for the complaint
  context.
- Submission requires category availability, required fields, source identity,
  primary affected student or context where applicable, additional participant
  validation where provided, visibility rules, and audit evidence.
- Multi-participant complaints must create participant records for every
  involved student, guardian, staff member, or school unit, and must evaluate
  visibility independently for each participant.
- Evidence references require permission to reference the source record and
  must not copy restricted source details into the complaint.
- Exact active duplicates return the existing complaint reference or are
  blocked; overlapping non-identical complaints are linked, grouped, or routed
  to review.
- Complaint submission must not create attendance, gate, scan, transport,
  wallet, learning reward, request approval, medical, emergency, document,
  search, or broad messaging outcomes.
