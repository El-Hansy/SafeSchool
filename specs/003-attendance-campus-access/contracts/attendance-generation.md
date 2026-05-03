# Contract: Attendance Generation

This contract defines attendance sessions, scan-based generation, attendance
records, manual corrections, and attendance summaries for Phase 2.

## Capabilities and Permissions

- Required capabilities:
  - `attendance_access.attendance_generation`
- Common permissions:
  - `attendance_access.attendance.read`
  - `attendance_access.attendance.generate`
  - `attendance_access.attendance.correct`
  - `attendance_access.scans.read`
  - `attendance_access.audit.read`

Attendance generation must be retry-safe for the same attendance session and
rule version. Re-running generation must update existing records predictably
without creating duplicate current attendance records.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/attendance-sessions` | List attendance sessions |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/attendance-sessions` | Create an attendance session |
| PATCH | `/api/v1/schools/{schoolAccountId}/attendance-access/attendance-sessions/{attendanceSessionId}` | Update session details or status |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/attendance-sessions/{attendanceSessionId}/generate` | Generate attendance from accepted scan evidence |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/attendance-records` | Review attendance records with filters |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/attendance-records/{attendanceRecordId}` | Read attendance record detail |
| POST | `/api/v1/schools/{schoolAccountId}/attendance-access/attendance-records/{attendanceRecordId}/correct` | Correct an attendance record |
| GET | `/api/v1/schools/{schoolAccountId}/attendance-access/attendance-summary` | Read attendance summaries by day, group, gate, or anomaly status |

## Create Attendance Session Request

```yaml
session_name: "Morning Attendance"
attendance_date: "YYYY-MM-DD"
campus_reference: "north-campus"
student_group_reference: "grade-05"
entry_window_start: "YYYY-MM-DDTHH:MM:SSZ"
entry_window_end: "YYYY-MM-DDTHH:MM:SSZ"
late_after: "YYYY-MM-DDTHH:MM:SSZ"
early_exit_before: "YYYY-MM-DDTHH:MM:SSZ"
expected_population_rule: "grade-05-active-students"
generation_status: "Active"
client_request_id: "request-unique-to-caller"
```

## Generate Attendance Request

```yaml
generation_rule_version: "attendance-rules-2026-05"
include_delayed_offline_scans: true
requested_reason: "Daily morning attendance generation."
client_request_id: "request-unique-to-caller"
```

## Attendance Record Response

```yaml
attendance_record_id: "attendance-record-reference"
school_account_id: "school-account-reference"
attendance_session_id: "attendance-session-reference"
student_profile_id: "student-profile-reference"
current_status: "Present"
entry_scan_event_id: "entry-scan-reference"
exit_scan_event_id: null
generation_source: "Scan Generated"
generation_rule_version: "attendance-rules-2026-05"
generated_at: "YYYY-MM-DDTHH:MM:SSZ"
review_status: "Not Required"
correction_reason: null
updated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Correction Request

```yaml
new_status: "Excused"
correction_reason: "Approved absence documentation reviewed."
related_scan_event_id: null
client_request_id: "request-unique-to-caller"
```

## Attendance Summary Response

```yaml
school_account_id: "school-account-reference"
attendance_date: "YYYY-MM-DD"
filters:
  campus_reference: "north-campus"
  student_group_reference: "grade-05"
summary:
  present: 120
  late: 8
  absent: 5
  early_exit: 2
  needs_review: 3
  excused: 1
generated_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Attendance generation requires `attendance_access.attendance_generation` to be
  enabled.
- Generation requires an active attendance session, expected student
  population, school account time rules, and accepted gate scan evidence.
- Normal Present, Late, and Early Exit statuses require accepted scan evidence
  unless explicitly corrected by an authorized reviewer.
- Absent status requires an expected student with no accepted entry evidence,
  unless school rules route the case to Needs Review.
- One current attendance record must exist per school account, session, and
  student.
- Re-running generation for the same session must not duplicate current
  attendance records.
- Corrections require `attendance_access.attendance.correct`, a reason, and
  audit evidence preserving original scan-based status.
- Attendance record list responses must be paginated and support filtering by
  session, student, group, status, review status, scan evidence, and updated
  time.

## Error Outcomes

| Condition | Outcome |
|-----------|---------|
| Capability disabled | Deny attendance workflow and record feature capability reason |
| Actor lacks generate permission | Deny generation and record required permission |
| Session inactive or invalid | Reject generation with validation details |
| Expected population includes another tenant | Deny without exposing cross-tenant data |
| Accepted scan evidence missing | Mark Absent or Needs Review according to school rules |
| Scan evidence denied or unresolved | Exclude from normal attendance and create or update anomaly where needed |
| Duplicate generation request | Return existing generation outcome when request identity matches |
| Correction lacks reason | Reject correction with validation details |
| Audit write fails for correction | Reject correction rather than allowing unaudited change |
