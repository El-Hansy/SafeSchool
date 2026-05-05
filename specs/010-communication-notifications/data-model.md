# Data Model: Phase 9 Communication & Notifications

This model defines runtime business entities for notification source events,
notification records, direct messages, broadcasts, announcements, delivery
tracking, read states, acknowledgements, preferences, moderation, history,
reviews, summaries, lifecycle events, and audit evidence. All tenant-owned
entities include `tenant_id`, `created_at`, and `updated_at`.

## Conversation

**Purpose**: A tenant-owned direct message thread between authorized school,
guardian, student, role, reviewer, or system participants.

**Fields**:
- `conversation_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `conversation_reference`: Human-friendly reference.
- `conversation_type`: Staff Guardian, Staff Student, Staff Staff, Role Group,
  System Related, Review.
- `subject`: Conversation subject.
- `student_profile_id`: Student context when applicable.
- `source_module`: Source module when conversation is opened from a prior
  workflow.
- `source_record_reference`: Source-domain record reference when applicable.
- `priority`: Low, Normal, High, Urgent.
- `visibility_level`: Recipient Visible, Internal, Restricted, Reviewer Only.
- `conversation_status`: Draft, Active, Pending Moderation, Closed, Archived,
  Review Required.
- `reply_policy`: Open, Participants Only, Staff Only, No Reply, Closed.
- `latest_message_at`: Latest visible activity time.
- `created_by_actor_id`: Actor that opened the thread.
- `closed_by_actor_id`: Actor that closed the thread when applicable.
- `closed_reason`: Reason when closed.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Relationships**:
- Belongs to one School Account.
- May reference one Student Profile and one source record.
- Has many Conversation Participants, Messages, Recipient Snapshots, Delivery
  Attempts, Acknowledgement Records, Communication Exceptions, Moderation
  Reviews, Communication Lifecycle Events, and Audit Events.

**Validation rules**:
- Create, read, reply, close, archive, and review operations require tenant
  access, enabled capability, actor permission, recipient eligibility,
  visibility authority, and audit evidence.
- Conversations with student context require student visibility or approved
  guardian-link visibility where applicable.
- Closed, no-reply, or review-required conversations reject replies unless an
  authorized reviewer records a permitted action.
- Conversation actions must not create outcomes in attendance, campus access,
  scans, transport, wallet, learning rewards, request approvals, medical,
  emergency, complaint resolution, document storage, global search, or broad
  dashboards.

## Conversation Participant

**Purpose**: A user, role, guardian, student, staff member, reviewer, or system
participant associated with a conversation.

**Fields**:
- `conversation_participant_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `conversation_id`: Parent conversation.
- `actor_id`: Platform actor when applicable.
- `participant_type`: Guardian, Student, Staff, Role, Reviewer, System.
- `participant_reference`: Actor, role, guardian link, student, or system
  reference.
- `student_profile_id`: Student visibility context when applicable.
- `guardian_link_id`: Guardian relationship evidence when applicable.
- `participant_role`: Sender, Recipient, Observer, Moderator, Reviewer,
  System Sender.
- `send_scope`: Can Send, Can Reply, Read Only, No Access.
- `visibility_scope`: Hidden, Status Only, Visible Details, Internal Details,
  Restricted Review.
- `participant_status`: Active, Removed, Restricted, Superseded, Ineligible.
- `inclusion_reason`: Why the participant was included.
- `exclusion_reason`: Why a candidate participant was excluded when preserved.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Participant visibility cannot exceed tenant, guardian-link, student
  ownership, role, assignment, moderation, or review authority.
- Ineligible participants cannot receive new messages, read restricted
  details, or reply unless reviewer rules allow a limited action.

## Message

**Purpose**: A preserved communication entry in a conversation.

**Fields**:
- `message_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `conversation_id`: Parent conversation.
- `sender_actor_id`: Actor that authored the message.
- `sender_participant_id`: Conversation participant that sent the message.
- `message_sequence`: Monotonic order within the conversation.
- `message_subject`: Optional message-specific subject.
- `message_body`: Message text.
- `message_language`: Language metadata.
- `priority`: Low, Normal, High, Urgent.
- `visibility_level`: Recipient Visible, Internal, Restricted, Reviewer Only.
- `message_status`: Draft, Pending Moderation, Sent, Corrected, Withdrawn,
  Rejected, Failed.
- `correction_of_message_id`: Prior message when this is a correction.
- `withdrawal_reason`: Reason when withdrawn.
- `moderation_state`: Not Required, Pending, Approved, Rejected, Corrected.
- `acknowledgement_required`: Whether recipient acknowledgement is required.
- `client_request_id`: Idempotency key for mutation commands.
- `sent_at`: Send time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Sending requires active conversation, valid sender participant, permitted
  recipients, feature availability, visibility checks, moderation decision
  where required, and audit evidence.
- Corrections and withdrawals preserve original message content and require a
  reason.
- Message content must not include restricted source details beyond recipient
  authority.

## Broadcast Announcement

**Purpose**: A one-to-many school communication such as a broadcast or
announcement.

**Fields**:
- `broadcast_announcement_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `communication_type`: Broadcast, Announcement.
- `title`: Communication title.
- `body`: Communication text.
- `language`: Language metadata.
- `priority`: Low, Normal, High, Urgent.
- `category`: School-defined or system category.
- `audience_rule_id`: Audience rule used to resolve recipients.
- `template_id`: Template used where applicable.
- `template_version`: Template version used.
- `effective_from`: Start of visible window.
- `effective_to`: End of visible window.
- `scheduled_publish_at`: Scheduled publication time.
- `publication_status`: Draft, Pending Approval, Scheduled, Published,
  Corrected, Withdrawn, Expired, Rejected, Review Required.
- `approval_required`: Whether approval is required.
- `approved_by_actor_id`: Approver when applicable.
- `acknowledgement_required`: Whether recipient acknowledgement is required.
- `withdrawal_reason`: Reason when withdrawn.
- `correction_of_id`: Prior announcement when corrected.
- `client_request_id`: Idempotency key for mutation commands.
- `created_by_actor_id`: Author actor.
- `published_at`: Publication time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Publication requires tenant access, enabled broadcast or announcement
  capability, publisher authority, valid audience, restricted-detail checks,
  duplicate checks, approval where required, and audit evidence.
- Recipient snapshots are preserved at publication time.
- Corrections and withdrawals append new evidence and cannot delete original
  publication history.

## Audience Rule

**Purpose**: A school-account rule or recipient selection that resolves
permitted recipients.

**Fields**:
- `audience_rule_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `rule_name`: Display name.
- `audience_type`: Whole School, Guardians, Students, Staff, Grade, Class,
  Route, Activity Group, Role Group, Source Event Recipients, Manual Selection.
- `audience_criteria`: Structured audience criteria.
- `allowed_recipient_types`: Guardian, Student, Staff, Role, System.
- `restricted_detail_policy`: Full, Summary Only, Withhold, Review Required.
- `requires_count_confirmation`: Whether large audience confirmation is
  required.
- `requires_moderation`: Whether moderation is required.
- `rule_status`: Draft, Active, Suspended, Archived.
- `version`: Rule version.
- `effective_from`: Start of rule validity.
- `effective_to`: End of rule validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Activation requires tenant scope, valid criteria, valid recipient types,
  restricted-detail policy, and enabled dependent capabilities.
- Audience rules must not resolve recipients outside the school account or
  outside approved guardian, student, role, assignment, or review scope.

## Recipient Snapshot

**Purpose**: The resolved recipient set for a message, notification, broadcast,
or announcement at send or publication time.

**Fields**:
- `recipient_snapshot_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `communication_kind`: Message, Notification, Broadcast, Announcement.
- `communication_reference`: Parent communication reference.
- `recipient_actor_id`: Recipient actor where applicable.
- `recipient_type`: Guardian, Student, Staff, Role, System.
- `recipient_reference`: Actor, role, guardian link, student, or system
  reference.
- `student_profile_id`: Student context when applicable.
- `guardian_link_id`: Guardian relationship evidence when applicable.
- `inclusion_reason`: Why recipient was included.
- `exclusion_reason`: Why candidate recipient was excluded.
- `recipient_visibility_level`: Detail level allowed for this recipient.
- `delivery_eligible`: Whether delivery should be attempted.
- `snapshot_status`: Included, Excluded, Suppressed, Ineligible, Review
  Required.
- `resolved_at`: Resolution time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Snapshots preserve the recipient decision at send or publication time.
- Duplicate recipient selections for the same communication intent are
  suppressed or merged with evidence.

## Notification Source Event

**Purpose**: A tenant-scoped event from another phase that is eligible to
generate a notification.

**Fields**:
- `notification_source_event_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `source_module`: Attendance, Campus Access, Transport, Wallet, Learning,
  Requests, Medical, Emergency, Complaints, Other.
- `source_record_reference`: Source-domain record identifier.
- `source_event_type`: Source-domain event type.
- `source_event_version`: Source event version or revision.
- `student_profile_id`: Student context when applicable.
- `event_priority`: Low, Normal, High, Urgent.
- `event_summary`: Minimum permitted source summary.
- `restricted_detail_level`: Public Summary, Recipient Summary, Restricted,
  Reviewer Only.
- `communication_eligible`: Whether the source module marked it eligible.
- `dedupe_key`: Source event duplicate suppression key.
- `source_event_status`: Accepted, Suppressed, Rejected, Superseded, Review
  Required.
- `received_at`: Time Phase 9 accepted the source event.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Source events must be tenant-scoped, feature-enabled, permission-safe, and
  marked communication eligible.
- Phase 9 must not mutate the originating source record.
- Duplicate or superseded source events preserve handling evidence.

## Notification Record

**Purpose**: A recipient-visible or staff-visible notification item.

**Fields**:
- `notification_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `notification_source_event_id`: Source event when applicable.
- `recipient_snapshot_id`: Recipient snapshot used for this notification.
- `recipient_actor_id`: Recipient actor where applicable.
- `student_profile_id`: Student context when applicable.
- `category`: Notification category.
- `priority`: Low, Normal, High, Urgent.
- `title`: Notification title.
- `summary`: Recipient-visible summary.
- `source_module`: Source module.
- `source_record_reference`: Source-domain reference when applicable.
- `allowed_action`: Optional recipient action.
- `reply_policy`: No Reply, Support Route, Conversation Reply, Staff Review.
- `support_action`: Configured support route, conversation target, or review
  queue action exposed when direct replies are blocked or routed.
- `detail_visibility_level`: Recipient-visible detail level.
- `notification_status`: New, Queued, Delivered, Read, Acknowledged, Failed,
  Suppressed, Expired, Corrected, Withdrawn, Review Required.
- `correction_of_notification_id`: Prior notification when this record corrects
  a notification already visible to recipients.
- `correction_reason`: Reason for the correction.
- `corrected_title`: Corrected notification title when applicable.
- `corrected_summary`: Corrected recipient-visible summary when applicable.
- `withdrawal_reason`: Reason when withdrawn.
- `corrected_by_actor_id`: Actor who corrected the notification.
- `withdrawn_by_actor_id`: Actor who withdrew the notification.
- `corrected_at`: Correction time.
- `withdrawn_at`: Withdrawal time.
- `read_state`: Unread, Read.
- `acknowledgement_required`: Whether acknowledgement is required.
- `acknowledgement_state`: Not Required, Pending, Acknowledged, Overdue,
  Waived, Failed.
- `exception_state`: None, Needs Review, Under Review, Resolved.
- `client_request_id`: Idempotency key for mutation commands.
- `generated_at`: Generation time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Notification reads require tenant access, recipient eligibility, visibility
  authority, feature availability, and audit evidence.
- Restricted source details are minimized, withheld, or routed to review based
  on recipient authority.
- Notification corrections and withdrawals require tenant scope, notification
  management permission, reason capture, recipient impact tracking, audit
  evidence, and lifecycle events while preserving the original title, summary,
  source reference, and recipient snapshot.
- No Reply notifications reject replies; Support Route notifications expose only
  the configured support action without mutating the originating source record.

## Communication Template

**Purpose**: A versioned school-account template for notifications, messages,
broadcasts, or announcements.

**Fields**:
- `communication_template_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `template_code`: School-unique template code.
- `template_name`: Display name.
- `communication_kind`: Notification, Message, Broadcast, Announcement.
- `category`: Communication category.
- `language`: Template language.
- `title_template`: Title text with variables.
- `body_template`: Body text with variables.
- `required_variables`: Variables required for rendering.
- `default_priority`: Low, Normal, High, Urgent.
- `default_audience_rule_id`: Default audience rule.
- `restricted_detail_policy`: Full, Summary Only, Withhold, Review Required.
- `acknowledgement_behavior`: None, Optional, Required, Required By Deadline.
- `quiet_hour_behavior`: Delay Optional, Send Immediately, Review Required.
- `channel_behavior`: Enabled channels and defaults.
- `moderation_required`: Whether template use requires review.
- `template_status`: Draft, Active, Suspended, Archived.
- `version`: Template version.
- `effective_from`: Start of validity.
- `effective_to`: End of validity.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Activation requires title, body, valid variables, valid language behavior,
  valid audience, restricted-detail policy, channel behavior, and enabled
  dependent capabilities.
- Historical communications preserve the template version used at generation
  or publication time.

## Delivery Attempt

**Purpose**: A per-recipient, per-channel attempt to deliver a communication.

**Fields**:
- `delivery_attempt_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `communication_kind`: Message, Notification, Broadcast, Announcement.
- `communication_reference`: Parent communication reference.
- `recipient_snapshot_id`: Recipient snapshot.
- `recipient_actor_id`: Recipient actor where applicable.
- `channel`: In App, Push, Email, SMS, Other.
- `attempt_number`: Attempt sequence.
- `delivery_status`: Queued, Sent, Delivered, Failed, Retry Scheduled,
  Suppressed, Excluded, Cancelled.
- `failure_reason`: Failure reason when applicable.
- `exclusion_reason`: Exclusion reason when applicable.
- `provider_reference`: External channel reference when available.
- `next_retry_at`: Retry time when applicable.
- `final_state`: Whether no more attempts are expected.
- `attempted_at`: Attempt time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Delivery attempts require enabled channel, recipient eligibility, duplicate
  suppression, preference policy, quiet-hour policy, and audit evidence.
- Failed or excluded attempts cannot be marked delivered without a new
  successful attempt or reviewer correction.

## Communication Preference

**Purpose**: User or school-account preference rules for optional categories
and delivery channels.

**Fields**:
- `communication_preference_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `actor_id`: Actor preference owner when user-specific.
- `recipient_type`: Guardian, Student, Staff, School Default.
- `category`: Communication category.
- `channel`: In App, Push, Email, SMS, Other.
- `preference_state`: Enabled, Disabled, Mandatory, School Managed.
- `quiet_hours_start`: Optional quiet-hour start.
- `quiet_hours_end`: Optional quiet-hour end.
- `mandatory_override_allowed`: Whether school policy can override preference.
- `preference_status`: Active, Suspended, Superseded.
- `version`: Preference policy version.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Optional preferences are respected for newly generated optional
  communications.
- Mandatory categories remain governed by school policy and cannot be fully
  opted out where policy requires delivery.

## Acknowledgement Record

**Purpose**: Per-recipient evidence that a required communication was read or
acknowledged.

**Fields**:
- `acknowledgement_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `communication_kind`: Message, Notification, Broadcast, Announcement.
- `communication_reference`: Parent communication reference.
- `recipient_snapshot_id`: Recipient snapshot.
- `recipient_actor_id`: Recipient actor.
- `acknowledgement_state`: Not Required, Pending, Read, Acknowledged, Overdue,
  Waived, Failed.
- `acknowledgement_deadline`: Deadline when required.
- `read_at`: Read time.
- `acknowledged_at`: Acknowledgement time.
- `acknowledged_by_actor_id`: Actor that acknowledged.
- `reminder_state`: Not Eligible, Eligible, Sent, Suppressed, Failed.
- `overdue_at`: Time it became overdue.
- `waived_by_actor_id`: Reviewer that waived when applicable.
- `waiver_reason`: Reason for waiver.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Read and acknowledgement state changes are per recipient and require tenant
  access, recipient eligibility, and audit evidence.
- Overdue acknowledgements route to review or reminder eligibility according to
  school configuration.

## Communication Exception

**Purpose**: A reviewable issue involving communication generation, recipient
resolution, delivery, acknowledgement, moderation, or visibility.

**Fields**:
- `communication_exception_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `communication_kind`: Source Event, Notification, Message, Broadcast,
  Announcement, Template, Delivery, Acknowledgement, Preference.
- `communication_reference`: Affected communication when applicable.
- `student_profile_id`: Student context when applicable.
- `recipient_actor_id`: Affected recipient when applicable.
- `exception_type`: Invalid Recipient, Invalid Guardian Link, Inactive
  Student, Disabled Feature, Disabled Channel, Missing Template, Missing
  Audience, Moderation Required, Cross-School Recipient, Restricted Detail
  Exposure Risk, Duplicate Communication, Delivery Failure, Acknowledgement
  Overdue, Manual Review Required.
- `severity`: Low, Medium, High, Critical.
- `source_evidence`: Evidence that created the exception.
- `exception_status`: Open, Under Review, Resolved, Dismissed, Escalated.
- `reviewer_assignment`: Reviewer or queue when assigned.
- `resolution_reason`: Reason after resolution.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Exceptions preserve source evidence and cannot be deleted to hide an issue.
- Critical exceptions route to configured review where available.

## Moderation Review

**Purpose**: A reviewer action that approves, rejects, corrects, withdraws, or
documents a message, broadcast, template, or communication exception.

**Fields**:
- `moderation_review_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `communication_kind`: Message, Broadcast, Announcement, Template,
  Exception.
- `communication_reference`: Reviewed record.
- `review_action`: Approve, Reject, Correct, Withdraw, Republish, Resolve,
  Escalate, Document.
- `review_reason`: Required reviewer reason.
- `status_before`: State before review.
- `status_after`: State after review.
- `reviewer_actor_id`: Reviewer actor.
- `visibility_change`: Visibility change when applicable.
- `content_change_summary`: Summary of content correction when applicable.
- `reviewed_at`: Review time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Moderation requires reviewer authority, enabled moderation capability,
  tenant scope, reason, and audit evidence.
- Review actions preserve original communication content and prior states.

## Communication Review Summary

**Purpose**: Permission-scoped communication counts, aging, delivery,
acknowledgement, moderation, and exception outcomes.

**Fields**:
- `communication_review_summary_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `summary_scope`: Category, Audience, Source Module, Sender, Recipient,
  Delivery State, Acknowledgement State, Moderation State, Exception State,
  Date Range.
- `filters`: Serialized permitted filters used to generate summary.
- `sent_count`: Sent communications count.
- `delivered_count`: Delivered count.
- `failed_count`: Failed count.
- `read_count`: Read count.
- `acknowledged_count`: Acknowledged count.
- `overdue_count`: Overdue acknowledgement count.
- `moderated_count`: Moderated count.
- `withdrawn_count`: Withdrawn count.
- `corrected_count`: Corrected count.
- `exception_count`: Exception count.
- `pending_count`: Pending count.
- `generated_by_actor_id`: Actor that generated summary.
- `generated_at`: Generation time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Summary reads require tenant access, enabled review summary capability,
  actor permission, scoped filters, and audit evidence.
- Summaries must not expose counts or details outside the actor's authorized
  school, guardian-link, student, role, assignment, moderation, or review
  scope.

## Communication Feature Setting

**Purpose**: Tenant capability and policy settings for Phase 9 workflows.

**Fields**:
- `communication_feature_setting_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `direct_messaging_enabled`: Whether direct messaging is enabled.
- `staff_guardian_messaging_enabled`: Whether staff-to-guardian messaging is
  enabled.
- `student_messaging_enabled`: Whether student messaging is enabled.
- `broadcasts_enabled`: Whether broadcasts are enabled.
- `announcements_enabled`: Whether announcements are enabled.
- `notification_center_enabled`: Whether notification center is enabled.
- `external_delivery_enabled`: Whether external channels may be used.
- `templates_enabled`: Whether templates are enabled.
- `delivery_tracking_enabled`: Whether delivery tracking is enabled.
- `acknowledgements_enabled`: Whether acknowledgements are enabled.
- `preferences_enabled`: Whether preferences are enabled.
- `history_enabled`: Whether communication history is enabled.
- `moderation_enabled`: Whether moderation is enabled.
- `review_summaries_enabled`: Whether review summaries are enabled.
- `default_quiet_hours`: School default quiet-hour policy.
- `mandatory_categories`: Categories that override optional preferences.
- `bulk_recipient_limit`: Recipient count threshold for extra confirmation.
- `setting_status`: Active, Suspended.
- `version`: Setting version.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Settings changes require administrator authority, tenant scope, valid
  dependencies, and audit evidence.
- Disabling a capability blocks future actions but preserves historical
  records and review visibility.

## Communication Lifecycle Event

**Purpose**: Tenant-scoped evidence that a communication lifecycle state
changed and may be used for review or later operational monitoring.

**Fields**:
- `communication_lifecycle_event_id`: Stable identifier.
- `tenant_id`: Owning school account.
- `communication_kind`: Source Event, Notification, Message, Broadcast,
  Announcement, Delivery, Acknowledgement, Moderation, Exception,
  Configuration.
- `communication_reference`: Related communication reference.
- `event_type`: Created, Queued, Sent, Delivered, Failed, Read, Acknowledged,
  Overdue, Moderated, Withdrawn, Corrected, Suppressed, Access Denied.
- `event_summary`: Minimum permitted summary.
- `actor_id`: Actor or system process that caused the event.
- `student_profile_id`: Student context when applicable.
- `recipient_actor_id`: Recipient context when applicable.
- `visibility_level`: Internal, Recipient Visible, Reviewer Only, Restricted.
- `event_status`: Active, Superseded, Restricted.
- `event_time`: Event time.
- `created_at`: Creation time.
- `updated_at`: Last update time.

**Validation rules**:
- Lifecycle events preserve review evidence and must not expose restricted
  details outside permitted scope.
- Lifecycle events do not create source-domain outcomes in excluded phases.
