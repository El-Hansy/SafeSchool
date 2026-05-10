# Contract: Support Observability

## Purpose

Defines support and audit visibility for mobile sign-in, role selection,
denied access, device sessions, version checks, installs, sync outcomes,
language context, and sensitive mobile actions.

## Endpoints

### GET `/api/v1/mobile/support/device-sessions`

Returns device sessions visible to an authorized support user.

**Query**:
- `tenantId`
- `userId` optional
- `deviceId` optional
- `appVersion` optional
- `sessionStatus` optional
- `from`, `to` optional
- `cursor` optional
- `limit`

**Response**:
- `items[]`
  - `sessionId`
  - `tenantId`
  - `userId`
  - `deviceId`
  - `appVersion`
  - `activeRoleCode`
  - `languageCode`
  - `sessionStatus`
  - `lastSeenAt`
- `nextCursor`

**Rules**:
- Support users can only see tenants and users inside their support scope.
- Sensitive personal details are minimized unless permission allows full view.

### GET `/api/v1/mobile/support/audit-events`

Returns mobile audit events for review.

**Query**:
- `tenantId`
- `actorUserId` optional
- `deviceId` optional
- `eventType` optional
- `result` optional
- `correlationId` optional
- `from`, `to` optional
- `cursor` optional
- `limit`

**Response**:
- `items[]`
  - `auditEventId`
  - `eventType`
  - `actorUserId`
  - `deviceId`
  - `roleCode`
  - `permissionCode`
  - `languageCode`
  - `result`
  - `reasonCode`
  - `targetType`
  - `targetId`
  - `occurredAt`
- `nextCursor`

### GET `/api/v1/mobile/support/install-events`

Returns APK install, launch, upgrade, blocked, obsolete, and revoked-version
evidence.

**Query**:
- `tenantId`
- `releaseId` optional
- `userId` optional
- `deviceId` optional
- `versionCode` optional
- `eventType` optional
- `from`, `to` optional
- `cursor` optional
- `limit`

**Response**:
- `items[]`
  - `installEventId`
  - `releaseId`
  - `versionName`
  - `versionCode`
  - `eventType`
  - `eventResult`
  - `occurredAt`
  - `userMessage`
- `nextCursor`

### GET `/api/v1/mobile/support/diagnostics/{correlationId}`

Returns a support-safe timeline for one issue.

**Response**:
- `correlationId`
- `tenantContext`
- `userContext`
- `deviceContext`
- `versionContext`
- `events[]`
- `mostLikelyReason`
- `recommendedSupportAction`

## Admin Web Contract

- Support screens must show tenant/user/device/version filters.
- Denied access must show reason code without exposing hidden record details.
- Audit exports require explicit permission and scope.
- Metrics must include sign-in success, denied access, blocked version, sync
  success/failure, obsolete version, install success, and language coverage.

## Acceptance Coverage

- Support can identify version, install, sign-in, denied-access, and sync
  evidence within 2 minutes.
- Support scope blocks cross-tenant evidence.
- Sensitive mobile actions create audit events.
- Arabic/English language context is visible in support evidence where useful.
