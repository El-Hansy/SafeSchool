# Contract: Mobile Sync And Notifications

## Purpose

Defines offline queue, sync outcome, and unified mobile notification behavior
for the role-based app.

## Endpoints

### POST `/api/v1/mobile/offline-actions`

Submits queued mobile actions for approved offline workflows.

**Request**:
- `tenantId`
- `deviceId`
- `userId`
- `activeRoleCode`
- `sourceFeatureCode`
- `sourceActionCode`
- `clientActionId`
- `localOccurredAt`
- `payload`

**Response**:
- `clientActionId`
- `syncStatus`: `accepted`, `rejected`, `duplicate`, or `conflict`
- `sourceReference`
- `conflictReason`
- `auditEventId`
- `userMessage`

**Rules**:
- Only source features with approved offline behavior can accept queued actions.
- `clientActionId` is idempotent for tenant, device, source feature, and action.
- Conflicts use source-domain timestamp and validation rules.

### GET `/api/v1/mobile/offline-actions`

Returns sync state for queued actions.

**Query**:
- `deviceId`
- `sourceFeatureCode` optional
- `syncStatus` optional
- `from`, `to` optional

**Response**:
- `items[]`
  - `clientActionId`
  - `sourceFeatureCode`
  - `sourceActionCode`
  - `syncStatus`
  - `conflictReason`
  - `lastUpdatedAt`

### GET `/api/v1/mobile/notifications`

Returns notifications visible to the active mobile role context.

**Query**:
- `tenantId`
- `roleCode`
- `studentId` optional for guardian/student views
- `cursor` optional
- `limit`

**Response**:
- `items[]`
  - `notificationId`
  - `sourceFeatureCode`
  - `title`
  - `body`
  - `createdAt`
  - `readState`
  - `actionLink`
- `nextCursor`

**Rules**:
- Notifications must honor active tenant, role, feature flags, guardian links,
  student self-scope, and source-domain visibility.
- Restricted notifications cannot reveal hidden record details in preview text.
- Arabic and English notification content must match the user's language
  preference when available.

### POST `/api/v1/mobile/notifications/{notificationId}/read`

Marks a notification as read in the current user context.

**Response**:
- `notificationId`
- `readState`
- `auditEventId`

## Acceptance Coverage

- Offline queue accepts only approved source workflows.
- Duplicate offline actions are deduplicated.
- Online-only workflows fail safely while offline.
- Notifications do not leak hidden features or unrelated student data.
- Arabic and English notification rendering is validated.
