# Contract: Mobile Role Access

## Purpose

Defines the user-facing and API contract for resolving one mobile app into the
correct tenant, role workspace, permissions, language, and visible actions.

## Actors

- Guardian
- Student
- Transport driver
- Gate/access staff
- Canteen cashier
- Teacher
- Medical staff
- Complaint handler
- Communication sender
- Document administrator
- School administrator
- Platform support user

## Endpoints

### GET `/api/v1/mobile/profile`

Returns the signed-in user's mobile profile and available tenant contexts.

**Query**:
- `tenantId` optional when the user has one tenant; required when ambiguous.

**Response**:
- `userId`
- `activeTenantId`
- `availableTenants[]`
- `availableRoles[]`
- `linkedStudents[]`
- `languagePreference`
- `mobileAccessStatus`
- `deniedReason` when blocked

**Rules**:
- Must not return tenant data for tenants outside the user's access.
- Guardian-linked students must be approved and current.
- Student scope returns only the signed-in student's permitted information.

### POST `/api/v1/mobile/context`

Selects the active tenant, role, and language context.

**Request**:
- `tenantId`
- `roleCode`
- `languageCode`: `ar` or `en`
- `deviceId`

**Response**:
- `activeTenantId`
- `activeRoleCode`
- `languageCode`
- `textDirection`
- `workspaceSummary`

**Errors**:
- `TENANT_ACCESS_DENIED`
- `ROLE_ACCESS_DENIED`
- `MOBILE_FEATURE_DISABLED`
- `UNSUPPORTED_LANGUAGE`
- `DEVICE_SESSION_BLOCKED`

### GET `/api/v1/mobile/workspaces`

Returns role workspaces visible in the active tenant context.

**Response**:
- `workspaces[]`
  - `workspaceCode`
  - `displayName`
  - `roleCode`
  - `summaryCards[]`
  - `actions[]`
  - `featureFlags[]`
  - `offlineCapabilities[]`

**Rules**:
- Hidden features are omitted by default.
- A blocked deep link must produce a denied-access response with a reason code.

### GET `/api/v1/mobile/workspaces/{workspaceCode}`

Returns the detail model for one workspace.

**Response**:
- `workspaceCode`
- `roleContext`
- `studentContext` when applicable
- `summarySections[]`
- `primaryActions[]`
- `secondaryActions[]`
- `notificationPreview`
- `emptyStates[]`
- `languageCode`
- `textDirection`

**Rules**:
- Each action must include source feature ownership and permission state.
- No action may bypass source-domain validation.

### POST `/api/v1/mobile/actions/{actionCode}`

Starts a permitted mobile action through the owning source-domain workflow.

**Request**:
- `workspaceCode`
- `sourceFeatureCode`
- `targetId`
- `clientActionId`
- `payload`
- `localOccurredAt`

**Response**:
- `actionResult`
- `sourceReference`
- `auditEventId`
- `queued` when offline queue is accepted
- `userMessage`

**Errors**:
- `ACTION_NOT_PERMITTED`
- `FEATURE_DISABLED`
- `SOURCE_DOMAIN_REJECTED`
- `OFFLINE_NOT_SUPPORTED`
- `IDEMPOTENCY_CONFLICT`
- `VALIDATION_FAILED`

## UI Contract

- The app must show one active tenant and one active role context before
  sensitive role actions.
- Arabic uses right-to-left layout and Arabic text.
- English uses left-to-right layout and English text.
- Hidden features are not shown in navigation.
- Denied deep links show a plain-language reason and do not reveal restricted
  record details.
- Shared-device sign-out clears user-visible tenant data.

## Acceptance Coverage

- Guardian sees only approved linked students.
- Student sees only self-scope information.
- Staff users see only assigned operational duties.
- Platform support users see support evidence only within support scope.
- Every listed role has at least one critical journey contract test.
