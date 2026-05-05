# Contract: Direct Messaging

This contract defines scoped conversation creation, participant resolution,
message send, replies, read state, correction, withdrawal, moderation routing,
idempotency, and audit behavior for Phase 9.

## Capabilities and Permissions

- Required capabilities:
  - `communications.direct_messaging`
  - `communications.staff_guardian_messaging` for staff-to-guardian threads
  - `communications.student_messaging` for student participation
  - `communications.moderation` when moderation is required
- Common permissions:
  - `communications.conversations.create`
  - `communications.conversations.read.own`
  - `communications.messages.send`
  - `communications.messages.reply`
  - `communications.messages.withdraw`
  - `communications.moderation.review`
  - `communications.audit.read`

Conversations and messages are tenant-scoped, participant-scoped, idempotent
where they mutate state, and auditable. Conversation participants determine
send, reply, read, archive, and visibility authority.

## Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/v1/schools/{schoolAccountId}/communications/conversations` | Create a school-side conversation |
| GET | `/api/v1/schools/{schoolAccountId}/communications/conversations` | List school-visible conversations |
| GET | `/api/v1/schools/{schoolAccountId}/communications/conversations/{conversationId}` | Read conversation detail |
| POST | `/api/v1/schools/{schoolAccountId}/communications/conversations/{conversationId}/messages` | Send a message or reply |
| POST | `/api/v1/schools/{schoolAccountId}/communications/messages/{messageId}/withdraw` | Withdraw eligible message with reason |
| POST | `/api/v1/schools/{schoolAccountId}/communications/conversations/{conversationId}/close` | Close a conversation |
| GET | `/api/v1/guardians/me/conversations` | List guardian-visible conversations |
| GET | `/api/v1/guardians/me/conversations/{conversationId}` | Read guardian-visible conversation |
| POST | `/api/v1/guardians/me/conversations/{conversationId}/messages` | Send guardian reply where allowed |
| GET | `/api/v1/students/me/conversations` | List student-visible conversations where enabled |
| GET | `/api/v1/students/me/conversations/{conversationId}` | Read student-visible conversation |
| POST | `/api/v1/students/me/conversations/{conversationId}/messages` | Send student reply where allowed |

## Conversation Create Request

```yaml
conversation_type: "Staff Guardian"
subject: "Upcoming trip reminder"
student_profile_id: "student-reference"
source_module: "Requests"
source_record_reference: "request-reference"
priority: "Normal"
visibility_level: "Recipient Visible"
reply_policy: "Participants Only"
participants:
  - participant_type: "Guardian"
    participant_reference: "guardian-link-reference"
    participant_role: "Recipient"
  - participant_type: "Staff"
    participant_reference: "staff-actor-reference"
    participant_role: "Sender"
initial_message:
  message_body: "Please review the upcoming trip details."
  acknowledgement_required: false
client_request_id: "conversation-create-unique-to-caller"
```

## Message Send Request

```yaml
message_body: "Thank you, reviewed."
priority: "Normal"
visibility_level: "Recipient Visible"
acknowledgement_required: false
client_request_id: "message-send-unique-to-caller"
```

## Message Response

```yaml
message_id: "message-reference"
conversation_id: "conversation-reference"
message_sequence: 3
sender_actor_id: "actor-reference"
message_status: "Sent"
moderation_state: "Not Required"
delivery_state: "Queued"
already_processed: false
sent_at: "YYYY-MM-DDTHH:MM:SSZ"
```

## Acceptance Rules

- Conversation creation requires enabled messaging capability, tenant scope,
  sender authority, eligible recipients, visibility validation, and audit
  evidence.
- Guardian participants require approved active guardian links for
  student-context conversations.
- Student participants require student messaging capability and school-enabled
  student communication rules.
- Replies require active conversation state, participant send scope, reply
  policy, and restricted-detail validation.
- Closed, no-reply, or moderation-required threads block delivery until the
  required action is resolved.
- Corrections and withdrawals preserve original message content, reason,
  actor, time, and recipient impact.
- Messaging behavior must not create side effects in excluded domains.
