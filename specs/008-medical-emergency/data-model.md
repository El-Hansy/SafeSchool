# Data Model: Phase 7 Medical & Emergency

This model defines runtime business entities for medical records, emergency
access, medical incidents, medical notifications, history, reviews, summaries,
status events, and audit evidence. All tenant-owned entities include
`tenant_id`, `created_at`, and `updated_at`.

## Student Medical Profile

**Purpose**: A school-account record containing a student's medical summary,
visibility rules, active care context, guardian visibility, review state, and
audit history.

**Fields**:
- `student_medical_profile_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Student from Phase 1.
- `medical_summary`: School-held summary for authorized staff.
- `critical_summary`: Minimum-necessary summary for emergency access.
- `profile_status`: Draft, Active, Pending Review, Superseded, Archived,
  Disputed.
- `guardian_visibility_policy`: Hidden, Summary, Full Approved Details.
- `student_visibility_policy`: Hidden, Summary.
- `verified_by_actor_id`: Medical staff actor that verified current evidence.
- `verified_at`: Verification time.
- `effective_from`: Start of active medical profile validity.
- `effective_to`: End of active medical profile validity.
- `review_state`: None, Needs Review, Under Review, Corrected, Closed.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account and one Student Profile.
- Has many Medical Condition Records, Allergy Records, Medication
  Instructions, Care Plans, Emergency Contacts, Medical Consent Records,
  Guardian Medical Update Submissions, Medical Incidents, Emergency Access
  Sessions, Medical Exceptions, Manual Medical Reviews, Medical Review
  Summaries, Status Events, and Audit Events.

**Validation rules**:
- Profile create, update, archive, view, and review operations require tenant
  access, enabled medical records capability, active student status, actor
  permission, role authority, and audit evidence.
- One active school-verified profile is allowed per student and tenant.
- Archived, superseded, disputed, or restricted details are hidden from
  unauthorized users.

## Medical Condition Record

**Purpose**: A school-held condition label, externally supplied condition
reference, health concern, or care-relevant status recorded by authorized
sources.

**Fields**:
- `medical_condition_record_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_medical_profile_id`: Parent profile.
- `student_profile_id`: Student.
- `condition_name`: Display name or school-held label.
- `condition_category`: Chronic, Temporary, Injury, Care Concern, Other.
- `severity`: Low, Medium, High, Critical.
- `emergency_relevant`: Whether it appears in emergency profile views.
- `guardian_visible`: Whether approved guardians may see it.
- `source_type`: Medical Staff, Guardian Submission, Manual Review, Import.
- `source_reference`: Reviewable source evidence reference.
- `condition_status`: Pending Review, Active, Superseded, Archived, Disputed.
- `effective_from`: Start of condition validity.
- `effective_to`: End of condition validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Guardian-submitted condition changes remain Pending Review until medical
  staff verify them.
- Emergency-relevant conditions must have severity and active effective dates.
- Superseding a condition preserves the older record.

## Allergy Record

**Purpose**: A care-critical allergy or sensitivity record.

**Fields**:
- `allergy_record_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_medical_profile_id`: Parent profile.
- `student_profile_id`: Student.
- `allergen`: Allergy or sensitivity label.
- `reaction_notes`: Reaction detail.
- `severity`: Low, Medium, High, Critical.
- `exposure_guidance`: School-approved avoidance or response guidance.
- `emergency_relevant`: Whether it appears in emergency profile views.
- `guardian_visible`: Whether approved guardians may see it.
- `allergy_status`: Pending Review, Active, Superseded, Archived, Disputed.
- `effective_from`: Start of allergy validity.
- `effective_to`: End of allergy validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Active critical allergies must appear in minimum-necessary emergency views.
- Allergy changes from guardians are pending review until verified.
- Expired allergies are shown only in authorized history.

## Medication Instruction

**Purpose**: A school-held medication or administration instruction.

**Fields**:
- `medication_instruction_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_medical_profile_id`: Parent profile.
- `student_profile_id`: Student.
- `medication_name`: Medication label.
- `dosage_instruction`: School-held dosage evidence.
- `schedule_instruction`: Timing or condition for administration.
- `administration_route`: Oral, Topical, Inhaled, Injectable, Other.
- `authorization_state`: Pending Review, Authorized, Expired, Suspended,
  Superseded, Disputed.
- `guardian_consent_record_id`: Consent evidence when required.
- `emergency_use_allowed`: Whether emergency use is allowed by school policy.
- `override_allowed`: Whether emergency override with reason is allowed.
- `effective_from`: Start of instruction validity.
- `effective_to`: End of instruction validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- May be referenced by Care Actions and Medical Incidents.

**Validation rules**:
- Medication administration evidence requires an active instruction or
  authorized override reason.
- Medication instructions cannot create diagnosis, prescription, pharmacy,
  wallet, or payment outcomes.
- Expired instructions cannot be used without review or override reason.

## Care Plan

**Purpose**: A school-approved plan for care instructions, restrictions, and
emergency steps.

**Fields**:
- `care_plan_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_medical_profile_id`: Parent profile.
- `student_profile_id`: Student.
- `plan_title`: Display title.
- `care_instructions`: Staff care instructions.
- `emergency_steps`: Emergency response guidance.
- `restriction_notes`: Activity, transport, food, or other restrictions.
- `guardian_visible_summary`: Guardian-approved summary.
- `staff_only_notes`: Restricted staff-only notes.
- `care_plan_status`: Pending Review, Active, Superseded, Archived, Disputed.
- `effective_from`: Start of plan validity.
- `effective_to`: End of plan validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Emergency-relevant care plans appear in critical emergency views.
- Staff-only notes are hidden from guardians and students unless explicitly
  permitted by school rules.
- Superseded plans remain visible in authorized history.

## Emergency Contact

**Purpose**: A guardian, emergency contact, or authorized contact route.

**Fields**:
- `emergency_contact_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_medical_profile_id`: Parent profile.
- `student_profile_id`: Student.
- `guardian_link_id`: Guardian link when the contact is a guardian.
- `contact_name`: Contact display name.
- `relationship`: Relationship to student.
- `contact_priority`: Ordered contact priority.
- `contact_route_category`: Phone, SMS, Email, In Person, Other.
- `contact_reference`: Contact reference or masked route.
- `medical_detail_allowed`: Whether recipient may receive medical details.
- `contact_status`: Active, Suspended, Expired, Removed, Needs Review.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Contact priority is unique per active student profile where configured.
- Contacts not authorized for medical details receive minimized summaries.
- Invalid or expired contacts are skipped or routed to review.

## Medical Consent Record

**Purpose**: Evidence of consent for medical visibility, medication
administration, emergency care steps, or information sharing.

**Fields**:
- `medical_consent_record_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_medical_profile_id`: Parent profile.
- `student_profile_id`: Student.
- `guardian_link_id`: Guardian that provided consent when applicable.
- `consent_area`: Medical Visibility, Medication Administration, Emergency
  Care, Information Sharing.
- `consent_state`: Pending, Granted, Denied, Revoked, Expired, Needs Review.
- `consent_source`: Guardian, School Policy, Manual Review.
- `consent_reason`: Reason or scope note.
- `effective_from`: Start of consent validity.
- `effective_to`: End of consent validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Consent must be active before medication administration unless an authorized
  emergency override reason is recorded.
- Revoked or expired consent blocks or routes actions to review.

## Guardian Medical Update Submission

**Purpose**: Guardian-provided medical information that requires school medical
review before becoming verified evidence.

**Fields**:
- `guardian_medical_update_submission_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Student.
- `guardian_link_id`: Approved guardian relationship.
- `submitted_area`: Profile, Condition, Allergy, Medication, Care Plan,
  Emergency Contact, Consent.
- `submitted_payload_reference`: Structured update payload reference.
- `submission_status`: Submitted, Pending Review, Accepted, Rejected,
  Partially Accepted, Needs Information, Withdrawn.
- `reviewer_actor_id`: Medical staff reviewer.
- `review_reason`: Required review reason.
- `submitted_at`: Submission time.
- `reviewed_at`: Review time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- All guardian submissions enter Pending Review before school-verified use.
- Exact duplicate active submissions return existing result.
- Conflicting non-identical submissions route to manual review.

## Emergency Access Session

**Purpose**: A 30-minute access event that exposes critical medical data during
an emergency.

**Fields**:
- `emergency_access_session_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Student.
- `actor_id`: Actor opening emergency view.
- `access_mode`: Emergency Authorized, Break Glass, Offline Cache.
- `access_reason`: Required emergency reason.
- `access_context`: Location, activity, trip, gate, bus, or incident context.
- `viewed_data_categories`: Minimum-necessary categories viewed.
- `started_at`: Session start.
- `expires_at`: Session expiry, 30 minutes after start or re-confirmation.
- `reconfirmed_at`: Latest re-confirmation time when applicable.
- `session_status`: Active, Expired, Reconfirmed, Closed, Denied, Needs Review.
- `review_state`: Not Required, Mandatory Review, Under Review, Resolved,
  Dismissed.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one Student Medical Profile.
- May reference Break-Glass Access Events, Medical Incidents, Medical
  Exceptions, Manual Medical Reviews, Status Events, and Audit Events.

**Validation rules**:
- Access requires tenant scope, enabled emergency access, eligible student,
  actor authority or break-glass authority, emergency reason, and audit
  evidence.
- Session access expires after 30 minutes and requires re-confirmation to
  continue.
- Critical views expose only minimum-necessary data.

**State transitions**:
- Active -> Reconfirmed when the actor confirms continued emergency need before
  or at expiry.
- Active or Reconfirmed -> Expired when the 30-minute window passes.
- Active or Reconfirmed -> Closed when the actor ends the session.
- Denied or Needs Review are final for failed attempts unless reopened through
  manual review.

## Break-Glass Access Event

**Purpose**: Emergency access granted only to school-configured
pre-authorized emergency roles when ordinary medical record permission is not
available.

**Fields**:
- `break_glass_access_event_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `emergency_access_session_id`: Related session.
- `student_profile_id`: Student.
- `actor_id`: Actor invoking break-glass.
- `pre_authorized_emergency_role`: Role that permits break-glass.
- `confirmation_text`: Emergency confirmation evidence.
- `reason`: Required emergency reason.
- `mandatory_review_due_at`: Review due time.
- `review_status`: Pending Review, Reviewed, Dismissed, Escalated.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Break-glass is denied for actors outside school-configured pre-authorized
  emergency roles.
- Every break-glass event routes to mandatory review.

## Emergency Offline Cache Access

**Purpose**: Syncable evidence for accessing cached critical medical data
during connectivity loss.

**Fields**:
- `emergency_offline_cache_access_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Student.
- `actor_id`: Actor accessing cache.
- `cached_profile_version`: Profile version included in cache.
- `last_synced_at`: Cache freshness timestamp.
- `cache_age_hours`: Cache age at access time.
- `cache_status`: Fresh, Stale, Unavailable.
- `stale_warning_acknowledged`: Whether actor acknowledged stale warning.
- `reason`: Required reason for stale-cache access.
- `sync_status`: Pending Sync, Synced, Sync Failed, Needs Review.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Cached critical data is usable only when synced within the last 24 hours.
- Older cached data requires warning, reason, and review routing.
- Offline cache exposes only emergency essentials and cannot browse unrelated
  records.

## Medical Incident

**Purpose**: A health, injury, medication, emergency, or care event logged for
a student.

**Fields**:
- `medical_incident_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Student.
- `student_medical_profile_id`: Medical profile at incident time.
- `reported_by_actor_id`: Actor that logged the incident.
- `incident_type`: Illness, Injury, Medication, Allergy, Emergency, Other.
- `severity`: Low, Medium, High, Critical.
- `location_context`: Clinic, Classroom, Gate, Bus, Trip, Playground, Other.
- `observed_details`: Observation summary.
- `incident_status`: Open, In Care, Escalated, Waiting Contact,
  Follow-Up Required, Closed, Corrected, Disputed, Needs Review.
- `notification_required`: Whether contact notification is required.
- `follow_up_required`: Whether follow-up is required.
- `source_event_reference`: Idempotency/source reference.
- `occurred_at`: Business event time.
- `closed_at`: Closure time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Has many Care Actions, Medical Notification Requests, Medical Contact
  Attempts, Medical Exceptions, Manual Medical Reviews, Status Events, and
  Audit Events.

**Validation rules**:
- Incident create/update requires tenant access, enabled incident logging,
  active student, actor authority, required severity and observation, and audit
  evidence.
- Exact duplicate active incidents return existing result.
- Conflicting non-identical incidents route to review.

**State transitions**:
- Open -> In Care when care action begins.
- Open or In Care -> Escalated when severity or handoff requires escalation.
- Open, In Care, or Escalated -> Waiting Contact when contact is required.
- Waiting Contact or In Care -> Follow-Up Required when follow-up is needed.
- Any active state -> Closed with closure reason.
- Any active state -> Corrected, Disputed, or Needs Review through review.

## Care Action

**Purpose**: A recorded action taken during or after a medical incident.

**Fields**:
- `care_action_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `medical_incident_id`: Parent incident.
- `student_profile_id`: Student.
- `actor_id`: Actor recording the action.
- `action_type`: Observation, First Aid, Medication Administration,
  Emergency Services Handoff, Guardian Contact, Follow-Up, Other.
- `action_summary`: Action summary.
- `medication_instruction_id`: Related instruction when applicable.
- `medication_evidence_reference`: Evidence reference when medication was
  administered.
- `override_reason`: Required when active instruction or consent is missing
  and school policy allows emergency override.
- `follow_up_state`: Not Required, Required, Scheduled, Completed, Needs Review.
- `correction_state`: None, Corrected, Disputed.
- `performed_at`: Action time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Medication administration requires active medication instruction or
  authorized override reason.
- Care actions are append-only; corrections add review evidence.
- Care actions never create diagnosis, prescription, pharmacy, wallet, or
  payment outcomes.

## Medical Notification Request

**Purpose**: A request to notify guardians, emergency contacts, assigned nurse
or clinic staff, the school emergency coordinator, or required staff about a
medical incident or emergency access event.

**Fields**:
- `medical_notification_request_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `source_type`: Medical Incident, Emergency Access, Manual Review.
- `source_id`: Source record.
- `student_profile_id`: Student.
- `urgency`: Low, Normal, High, Critical.
- `default_audience_applied`: Whether high-severity default audience was used.
- `audience_scope`: Approved Guardians, Emergency Contacts, Nurse Clinic Staff,
  Emergency Coordinator, Required Staff.
- `privacy_summary`: Minimized content summary.
- `acknowledgement_required`: Whether acknowledgement is required.
- `notification_status`: Draft, Pending, Contacting, Partially Acknowledged,
  Acknowledged, Failed, Cancelled, Needs Review.
- `client_request_id`: Idempotency key.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Has many Medical Contact Attempts.
- May create Medical Exceptions, Manual Medical Reviews, Status Events, and
  Audit Events.

**Validation rules**:
- High-severity medical incidents default audience to approved guardians,
  emergency contacts, assigned nurse or clinic staff, and the school emergency
  coordinator.
- Notification requests minimize sensitive details and do not deliver broad
  messages or broadcasts.
- Duplicate active requests are idempotent.

## Medical Contact Attempt

**Purpose**: Evidence that a recipient was contacted or contact was attempted.

**Fields**:
- `medical_contact_attempt_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `medical_notification_request_id`: Parent notification request.
- `student_profile_id`: Student.
- `recipient_actor_id`: Platform actor when applicable.
- `emergency_contact_id`: Emergency contact when applicable.
- `recipient_role`: Guardian, Emergency Contact, Nurse Clinic Staff,
  Emergency Coordinator, Required Staff.
- `contact_route_category`: Phone, SMS, Email, In Person, Other.
- `attempt_outcome`: Pending, Successful, Failed, Skipped, Needs Review.
- `acknowledgement_state`: Not Required, Pending, Acknowledged, Declined,
  Timed Out.
- `failure_reason`: Reason when failed.
- `recorded_by_actor_id`: Actor when manually recorded.
- `attempted_at`: Attempt time.
- `acknowledged_at`: Acknowledgement time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Invalid, expired, or unauthorized contacts are skipped or routed to review.
- Manual attempts require actor, time, and outcome evidence.
- Contact attempts do not create Phase 9 delivery management records.

## Medical Exception

**Purpose**: A reviewable issue involving invalid, conflicting, stale,
duplicate, unauthorized, or manual-review-required medical evidence.

**Fields**:
- `medical_exception_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Affected student when applicable.
- `source_type`: Profile, Guardian Update, Consent, Emergency Access,
  Offline Cache, Incident, Care Action, Notification, Contact, Configuration,
  Access.
- `source_id`: Related source record.
- `exception_type`: Invalid Student, Inactive Student, Expired Instruction,
  Conflicting Medical Record, Missing Consent, Missing Emergency Reason,
  Duplicate Incident, Duplicate Notification, Failed Contact, Stale Emergency
  Cache, Disabled Feature, Cross-School Access, Manual Review Required.
- `severity`: Info, Warning, High, Critical.
- `exception_status`: Open, Assigned, Resolved, Dismissed, Escalated,
  Reopened.
- `source_evidence_reference`: Source evidence.
- `reviewer_assignment`: Role, actor, or queue.
- `resolution_reason`: Required on resolution or dismissal.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Exceptions cannot be deleted to hide evidence.
- Resolution requires reviewer authority, reason, and audit evidence.

## Manual Medical Review

**Purpose**: Reviewer action that corrects, reopens, resolves, dismisses,
escalates, or documents medical records, incidents, notifications, access
events, or exceptions.

**Fields**:
- `manual_medical_review_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `medical_exception_id`: Related exception when applicable.
- `source_type`: Reviewed source type.
- `source_id`: Reviewed source record.
- `reviewer_actor_id`: Reviewer actor.
- `review_action`: Correct, Reopen, Close, Resolve, Dismiss, Escalate,
  Migrate Rule Version.
- `review_reason`: Required reason.
- `original_status`: Status before review.
- `resulting_status`: Status after review.
- `reviewed_at`: Review time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Reviews require explicit reviewer permission and tenant access.
- Original evidence remains preserved.

## Medical Rule Setting

**Purpose**: School-account configuration for medical visibility, emergency
access, break-glass, medication evidence, incident severity, notification
audience, acknowledgement, and review routing.

**Fields**:
- `medical_rule_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `rule_area`: Visibility, Emergency Access, Break Glass, Consent,
  Medication Evidence, Incident Severity, Notification Audience,
  Acknowledgement, Review.
- `rule_payload`: Structured rule configuration.
- `rule_status`: Draft, Active, Suspended, Retired.
- `rule_version`: Version number.
- `valid_from`: Start of validity.
- `valid_to`: End of validity.
- `change_reason`: Required on activation or suspension.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Activation requires enabled medical configuration capability and valid
  dependent capabilities.
- Historical records preserve the active rule version at event time.

## Medical Review Summary

**Purpose**: Permission-scoped summary of medical records, incident status,
emergency access, notification acknowledgement, exceptions, and reviews.

**Fields**:
- `medical_review_summary_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `summary_scope`: Student, Medical Profile, Incident, Emergency Access,
  Notification, Reviewer.
- `scope_reference`: Identifier for summary scope.
- `student_profile_id`: Student when applicable.
- `condition_counts`: Summary of active/reviewed conditions.
- `incident_counts`: Summary by severity and status.
- `emergency_access_counts`: Summary of access and break-glass events.
- `notification_counts`: Summary of pending/acknowledged/failed contacts.
- `exception_counts`: Summary of open/resolved exceptions.
- `latest_evidence_at`: Most recent evidence time.
- `visibility_scope`: Staff, Guardian, Student, Reviewer, Platform Reviewer.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Summaries enforce the same visibility boundaries as underlying records.
- Guardian and student summaries hide staff-only details.

## Medical Status Event

**Purpose**: Exportable evidence that medical or emergency workflow status
changed and may be consumed by later communication or notification
capabilities.

**Fields**:
- `medical_status_event_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `student_profile_id`: Optional student reference.
- `source_type`: Profile, Guardian Update, Emergency Access, Incident,
  Care Action, Notification, Contact, Exception, Review, Configuration.
- `source_id`: Source record.
- `event_type`: Profile Updated, Guardian Update Submitted, Emergency Accessed,
  Break Glass Used, Incident Logged, Care Action Added, Notification Requested,
  Contact Attempted, Acknowledgement Changed, Exception Opened,
  Review Completed, Configuration Changed.
- `event_payload_reference`: Minimal trace payload.
- `eligible_for_notification`: Whether Phase 9 may consume it.
- `occurred_at`: Business event time.
- `created_at`: Creation time.

**Validation rules**:
- Events are append-only and tenant-scoped.
- Events do not deliver messages or create broad notification workflows.

## School Account Feature Setting

**Purpose**: Tenant capability setting that determines whether Phase 7
workflows are available.

**Fields**:
- `school_account_feature_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `capability_key`: Medical capability key.
- `enabled`: Whether the capability is enabled.
- `effective_from`: Start time.
- `effective_to`: End time.
- `changed_by`: Actor that changed the setting.
- `change_reason`: Reason for change.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Backend operations must check feature availability before mutating or
  exposing Phase 7 workflow data.
- UI gates are usability controls only and do not replace backend checks.
