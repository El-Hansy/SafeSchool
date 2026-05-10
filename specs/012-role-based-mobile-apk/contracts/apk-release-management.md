# Contract: APK Release Management

## Purpose

Defines controlled Android APK release, audience, version check, install,
upgrade, obsolete-version, and support review behavior.

## Endpoints

### GET `/api/v1/mobile/releases/current`

Returns the current release available to the signed-in or install-link user.

**Query**:
- `tenantId`
- `roleCode` optional when known
- `deviceId` optional before sign-in
- `versionCode` optional for version checks

**Response**:
- `releaseId`
- `versionName`
- `versionCode`
- `environment`
- `releaseStatus`
- `downloadAllowed`
- `updateRequired`
- `releaseNotes`
- `supportContact`
- `checksum`
- `expiresAt` when applicable

**Errors**:
- `NO_RELEASE_FOR_AUDIENCE`
- `RELEASE_REVOKED`
- `VERSION_OBSOLETE`
- `MOBILE_RELEASE_DISABLED`

### POST `/api/v1/mobile/releases`

Creates a draft APK release record for authorized release operators.

**Request**:
- `versionName`
- `versionCode`
- `environment`
- `artifactUri`
- `artifactChecksum`
- `releaseNotes`
- `supportContact`
- `minimumSupportedVersionCode`

**Response**:
- `releaseId`
- `releaseStatus`
- `auditEventId`

**Rules**:
- `versionCode` must be unique and higher than replaced active releases.
- Artifact integrity evidence is required before approval.

### POST `/api/v1/mobile/releases/{releaseId}/approve`

Approves a draft release for controlled distribution.

**Request**:
- `approvalNote`
- `audiences[]`

**Response**:
- `releaseId`
- `releaseStatus`
- `audienceSummary`
- `auditEventId`

### POST `/api/v1/mobile/releases/{releaseId}/revoke`

Revokes a release and defines the replacement behavior.

**Request**:
- `revocationReason`
- `replacementReleaseId` optional
- `blockImmediately`

**Response**:
- `releaseStatus`
- `blockedVersionPolicy`
- `auditEventId`

### POST `/api/v1/mobile/install-events`

Records install, launch, upgrade, blocked, obsolete, or revoked-version events.

**Request**:
- `deviceId`
- `tenantId` optional before sign-in
- `userId` optional before sign-in
- `releaseId`
- `versionName`
- `versionCode`
- `eventType`
- `eventResult`
- `occurredAt`

**Response**:
- `eventId`
- `accepted`
- `nextAction`
- `userMessage`

**Rules**:
- Duplicate install evidence from the same device and release is idempotent.
- Blocked versions must return clear upgrade or support guidance.

## Admin Web Contract

- Authorized school administrators can view releases assigned to their tenant.
- Platform release operators can create, approve, revoke, and supersede APK
  releases.
- Release notes must be available in Arabic and English before approval.
- Audience changes are audited.
- Revoked releases are not installable.

## Acceptance Coverage

- Current release lookup honors tenant and audience.
- Revoked and obsolete versions are blocked or directed to upgrade.
- Install and launch evidence is reviewable by support.
- Release notes render correctly in Arabic RTL and English LTR.
