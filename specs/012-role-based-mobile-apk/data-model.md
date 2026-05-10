# Data Model: Phase 12 Role-Based Mobile App & APK Release

## Overview

Phase 12 adds mobile access, role workspace, APK release, localization, support,
and audit records while preserving source-domain ownership for business
outcomes. Tenant-owned records include `tenant_id`, `created_at`, and
`updated_at`. Sensitive records also include actor, correlation, and audit
metadata.

## Entities

### Mobile User Profile

Represents the mobile view of an authenticated user in one active tenant.

**Fields**:
- `id`: Unique profile identifier.
- `tenant_id`: Owning tenant.
- `user_id`: Authenticated platform user.
- `active_tenant_id`: Tenant selected for the current mobile context.
- `active_role_code`: Role selected for the current mobile context.
- `available_tenant_ids`: Tenants where the user has mobile access.
- `available_role_codes`: Roles available in the active tenant.
- `linked_student_ids`: Students visible to the user through approved links or self-scope.
- `mobile_access_status`: `active`, `blocked`, `revoked`, or `pending`.
- `last_resolved_at`: When role/permission visibility was last evaluated.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Has many Role Workspace Visibility records.
- Has many Device Sessions.
- References IdentityAccess users, tenants, roles, and guardian/student links.

**Validation rules**:
- A profile cannot expose tenant data until `active_tenant_id` is resolved.
- Guardian links must be approved and current before `linked_student_ids` are visible.
- A revoked or blocked profile cannot perform sensitive mobile actions.

### Role Workspace

Defines one mobile workspace available for a role in a tenant.

**Fields**:
- `id`: Unique workspace identifier.
- `tenant_id`: Owning tenant.
- `workspace_code`: Canonical workspace such as `guardian`, `student`, `transport_driver`, `gate_access`, `canteen_cashier`, `teacher`, `medical_staff`, `complaint_handler`, `communication_sender`, `document_administrator`, `school_administrator`, or `platform_support`.
- `display_name_key`: Localization key for workspace label.
- `required_role_code`: Role required to see the workspace.
- `required_permission_codes`: Permission set required for visible actions.
- `enabled_feature_codes`: Source-domain features shown in the workspace.
- `workspace_status`: `active`, `disabled`, or `retired`.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Has many Role Workspace Actions.
- Has many Mobile Permission Grants.
- References tenant feature availability.

**Validation rules**:
- `workspace_code` must be unique per tenant.
- A workspace cannot be active if its required tenant feature is disabled.
- Platform support workspace requires support-scoped access and cannot reveal
  tenant data outside assigned support scope.

### Role Workspace Action

Defines a view or action exposed inside a role workspace.

**Fields**:
- `id`: Unique action identifier.
- `tenant_id`: Owning tenant.
- `workspace_id`: Parent Role Workspace.
- `action_code`: Canonical action, such as `view_transport_eta` or `submit_complaint`.
- `source_feature_code`: Owning source-domain feature.
- `required_permission_code`: Permission required to perform the action.
- `offline_policy`: `not_supported`, `queue_allowed`, or `capture_only`.
- `requires_online_confirmation`: Whether current server confirmation is required.
- `audit_level`: `standard`, `sensitive`, or `restricted`.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Belongs to Role Workspace.
- References the owning source-domain workflow.

**Validation rules**:
- `offline_policy` must match the owning source feature's approved offline behavior.
- Sensitive or restricted actions require audit evidence.

### Mobile Permission Grant

Tenant-scoped assignment that makes a mobile workspace or action visible.

**Fields**:
- `id`: Unique grant identifier.
- `tenant_id`: Owning tenant.
- `grant_type`: `role`, `user`, `group`, or `student_link`.
- `subject_id`: Role, user, group, or link identifier.
- `workspace_code`: Workspace affected by the grant.
- `permission_code`: Permission granted or denied.
- `grant_status`: `active`, `revoked`, `expired`, or `pending_review`.
- `effective_from`: Start timestamp.
- `effective_until`: Optional end timestamp.
- `created_by_user_id`: Actor who created the grant.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- References Role Workspace and source identity/role records.
- Produces Mobile Audit Events for create, update, revoke, and expiry.

**Validation rules**:
- Active grants require an active tenant feature.
- Revoked grants must block future sensitive actions within 5 minutes.
- Direct user grants cannot bypass tenant or source-domain authorization.

### Tenant Mobile Feature Availability

Tenant configuration for mobile app and module visibility.

**Fields**:
- `id`: Unique configuration identifier.
- `tenant_id`: Owning tenant.
- `mobile_enabled`: Enables the mobile app for the tenant.
- `apk_release_enabled`: Enables controlled APK release.
- `enabled_feature_codes`: Prior module features visible in mobile.
- `enabled_workspace_codes`: Role workspaces enabled for the tenant.
- `configuration_status`: `draft`, `active`, `disabled`, or `archived`.
- `last_reviewed_by_user_id`: Actor who reviewed configuration.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Controls Role Workspaces, Release Audiences, and mobile navigation.
- References prior tenant feature configuration.

**Validation rules**:
- Mobile workspaces cannot be enabled unless `mobile_enabled` is true.
- A source-domain feature cannot be visible if its source feature flag is off.

### Mobile Language Preference

Stores the user's mobile language and text direction context.

**Fields**:
- `id`: Unique preference identifier.
- `tenant_id`: Owning tenant.
- `user_id`: User who owns the preference.
- `language_code`: `ar` or `en`.
- `text_direction`: `rtl` for Arabic, `ltr` for English.
- `source`: `user_selected`, `tenant_default`, or `device_default`.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Used by Mobile User Profile and Device Session.

**Validation rules**:
- Phase 12 supports only Arabic and English.
- Arabic preference must use right-to-left direction.

### Device Session

Represents a signed-in mobile device context.

**Fields**:
- `id`: Unique session identifier.
- `tenant_id`: Active tenant.
- `user_id`: Signed-in user.
- `device_id`: Stable app-generated device identifier.
- `device_label`: User-visible device label when available.
- `app_version`: Installed app version.
- `release_id`: APK release associated with the session.
- `active_role_code`: Role currently selected.
- `language_code`: Current language.
- `session_status`: `active`, `signed_out`, `revoked`, `expired`, or `blocked_version`.
- `signed_in_at`: Sign-in timestamp.
- `last_seen_at`: Last app contact.
- `signed_out_at`: Optional sign-out timestamp.
- `revoked_at`: Optional revocation timestamp.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Belongs to Mobile User Profile.
- References APK Release.
- Has many Install or Upgrade Events, Version Check Events, and Mobile Audit Events.

**Validation rules**:
- A session cannot remain active if the user, tenant, role, permission, or app
  version is revoked.
- Shared-device sign-out must clear user-visible cached tenant data.

### APK Release

Approved APK version that can be distributed to an audience.

**Fields**:
- `id`: Unique release identifier.
- `version_name`: User-visible version.
- `version_code`: Monotonic Android version code.
- `environment`: `demo`, `pilot`, or `production`.
- `release_status`: `draft`, `approved`, `active`, `revoked`, `superseded`, or `retired`.
- `artifact_uri`: Object storage reference for APK file.
- `artifact_checksum`: Integrity checksum.
- `release_notes_key`: Localization key or release notes reference.
- `minimum_supported_version_code`: Oldest allowed version.
- `support_contact`: Support contact displayed to users.
- `approved_by_user_id`: Release approver.
- `approved_at`: Approval timestamp.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Has many Release Audiences.
- Has many Install or Upgrade Events.
- Referenced by Device Sessions.

**Validation rules**:
- Only approved or active releases can be distributed.
- Revoked releases must block app access or direct users to an approved
  replacement.
- `version_code` must increase for replacement releases.

### Release Audience

Defines who can install or use an APK Release.

**Fields**:
- `id`: Unique audience identifier.
- `release_id`: Parent APK Release.
- `tenant_id`: Tenant audience.
- `audience_type`: `tenant`, `role`, `group`, `user`, or `pilot`.
- `audience_ref`: Identifier for role, group, user, or pilot group when applicable.
- `audience_status`: `active`, `paused`, or `revoked`.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Belongs to APK Release.
- References tenant, role, group, or user records.

**Validation rules**:
- A release cannot be installed by a user outside an active audience.
- Audience revocation must apply to new installs and version checks.

### Install or Upgrade Event

Evidence of APK install, app launch, upgrade, block, or obsolete-version flow.

**Fields**:
- `id`: Unique event identifier.
- `tenant_id`: Tenant context when known.
- `user_id`: User when known.
- `device_id`: Mobile device identifier.
- `release_id`: Related APK Release.
- `version_name`: Reported app version.
- `version_code`: Reported version code.
- `event_type`: `install`, `launch`, `upgrade`, `blocked`, `obsolete`, or `revoked_version`.
- `event_result`: `allowed`, `blocked`, `update_required`, or `support_required`.
- `occurred_at`: Event timestamp.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Belongs to Device Session where available.
- References APK Release.

**Validation rules**:
- Duplicate install evidence from the same device/release can be deduplicated
  by event id or device/release/time window.
- Blocked and obsolete events require a user-facing reason.

### Offline Action Queue

Mobile queue metadata for workflows that support offline continuity.

**Fields**:
- `id`: Unique queue record identifier.
- `tenant_id`: Owning tenant.
- `user_id`: Actor.
- `device_id`: Device that captured the action.
- `source_feature_code`: Owning feature, such as attendance, transport, wallet, or medical.
- `source_action_code`: Source-domain action.
- `client_action_id`: Client-generated idempotency key.
- `local_occurred_at`: Device timestamp.
- `server_received_at`: Server receipt timestamp when synced.
- `sync_status`: `queued`, `syncing`, `accepted`, `rejected`, `duplicate`, or `conflict`.
- `conflict_reason`: Optional conflict explanation.
- `audit_event_id`: Related Mobile Audit Event.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- Belongs to Device Session.
- References source-domain sync outcome.

**Validation rules**:
- Only approved offline source actions can be queued.
- `client_action_id` must be unique within tenant/device/source feature.
- Sync outcomes must be auditable and idempotent.

### Mobile Audit Event

Reviewable evidence for mobile security, release, sync, and support behavior.

**Fields**:
- `id`: Unique audit event identifier.
- `tenant_id`: Tenant context.
- `actor_user_id`: Actor who caused the event.
- `device_id`: Device involved where applicable.
- `event_type`: Mobile event category.
- `target_type`: Target record type.
- `target_id`: Target record identifier.
- `role_code`: Active role at event time.
- `permission_code`: Permission involved where applicable.
- `language_code`: Language context where applicable.
- `result`: `allowed`, `blocked`, `created`, `updated`, `revoked`, `accepted`, or `failed`.
- `reason_code`: Reason for denied or failed events.
- `correlation_id`: Correlation reference for support review.
- `occurred_at`: Event timestamp.
- `created_at`, `updated_at`: Audit timestamps.

**Relationships**:
- References Device Session, APK Release, Permission Grant, Offline Action
  Queue, or source-domain event where applicable.

**Validation rules**:
- Sensitive mobile actions must create audit events.
- Denied access events must include reason and active context.

## State Transitions

### Mobile Permission Grant

```text
pending_review -> active
active -> revoked
active -> expired
revoked -> active      # requires a new authorized grant action
expired -> active      # requires a new authorized grant action
```

### Device Session

```text
active -> signed_out
active -> revoked
active -> expired
active -> blocked_version
blocked_version -> active      # only after approved upgrade/version check
revoked -> active              # only after new sign-in and permission resolution
```

### APK Release

```text
draft -> approved
approved -> active
active -> superseded
active -> revoked
superseded -> retired
revoked -> retired
```

### Offline Action Queue

```text
queued -> syncing
syncing -> accepted
syncing -> rejected
syncing -> duplicate
syncing -> conflict
conflict -> accepted           # only after source-domain conflict handling
```

## Indexing And Uniqueness

- Mobile User Profile: unique `tenant_id + user_id`.
- Role Workspace: unique `tenant_id + workspace_code`.
- Mobile Permission Grant: index `tenant_id + subject_id + workspace_code + permission_code + grant_status`.
- Device Session: index `tenant_id + user_id + device_id + session_status`.
- APK Release: unique `version_code`; index `release_status + environment`.
- Release Audience: index `release_id + tenant_id + audience_type + audience_ref`.
- Install or Upgrade Event: index `tenant_id + user_id + device_id + version_code + occurred_at`.
- Offline Action Queue: unique `tenant_id + device_id + source_feature_code + client_action_id`.
- Mobile Audit Event: index `tenant_id + actor_user_id + event_type + target_type + target_id + occurred_at`.

## Data Boundaries

- Phase 12 owns mobile shell, release, device, role visibility, localization,
  support, and mobile audit records.
- Source-domain records remain owned by their original modules.
- Mobile cached records cannot become the source of truth for domain outcomes.
- Demo seed data must be clearly marked and must not expose real tenant data.
