# Identity Access API

Base path: `/api/v1/schools/{schoolAccountId}/identity`

Every route resolves the school account from the route plus `X-School-Account-Id`, accepts `X-Actor-Reference` for local development and tests, and returns tenant-scoped records only.

## Student Profiles

- `POST /students`: create a student profile with duplicate review.
- `GET /students`: list student profiles for one school account.
- `GET /students/{studentProfileId}`: read a profile without cross-tenant disclosure.
- `PATCH /students/{studentProfileId}`: update controlled profile fields.
- `POST /students/{studentProfileId}/deactivate`: deactivate a profile with review reason.
- `POST /students/duplicate-check`: evaluate active identifier collisions.
- `GET /students/{studentProfileId}/history`: review profile audit history.

## Permission Enforcement

- `GET /roles`
- `POST /roles`
- `PATCH /roles/{roleId}`
- `GET /permissions`
- `PUT /roles/{roleId}/permissions`
- `POST /role-assignments`
- `PATCH /role-assignments/{assignmentId}`
- `GET /access-decisions`
- `GET /audit-events`

## Guardian Linking

- `POST /guardians`
- `GET /guardians`
- `GET /guardians/{guardianId}`
- `PATCH /guardians/{guardianId}`
- `GET /guardians/{guardianId}/students`
- `POST /students/{studentProfileId}/guardian-links`
- `PATCH /guardian-links/{guardianLinkId}`
- `POST /guardian-links/{guardianLinkId}/approve`
- `POST /guardian-links/{guardianLinkId}/suspend`
- `POST /guardian-links/{guardianLinkId}/remove`
- `POST /guardian-links/{guardianLinkId}/expire`
- `GET /guardian-links/{guardianLinkId}/history`

## Credential Lifecycle

- `POST /students/{studentProfileId}/credentials/nfc`
- `POST /students/{studentProfileId}/credentials/qr`
- `POST /credentials/{credentialId}/suspend`
- `POST /credentials/{credentialId}/restore`
- `POST /credentials/{credentialId}/replace`
- `POST /credentials/{credentialId}/revoke`
- `POST /credentials/{credentialId}/rotate-qr`
- `GET /students/{studentProfileId}/credentials`
- `GET /credentials/{credentialId}/history`
- `GET /credentials/status-snapshot`

## Common Errors

- `tenant_mismatch`: route tenant and active tenant context do not match.
- `feature_disabled`: required school capability is disabled.
- `missing_permission`: actor lacks the required permission.
- `duplicate_active_identity`: profile activation conflicts with active identifiers.
- `not_found`: target is absent or not visible in this tenant.
