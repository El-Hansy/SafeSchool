# Identity Access Security Review

## Tenant Boundaries

- All tenant-owned entities inherit `TenantOwnedEntity` with `TenantId`, `CreatedAt`, and `UpdatedAt`.
- API routes are versioned under `/api/v1/schools/{schoolAccountId}/identity`.
- `TenantResolutionMiddleware` rejects route/header tenant mismatch unless platform review scope is explicit.
- Tests cover tenant resolution and cross-tenant permission denial.

## Feature Gates

- Phase 1 capability keys cover student profiles, guardian linking, NFC credentials, QR fallback, role administration, and permission enforcement.
- `PermissionGuard` and `PermissionEvaluator` check feature availability before allowing sensitive actions.

## Permissions

- `PermissionCatalog` seeds school administrator permissions for student, guardian, credential, role, permission, audit, and access decision workflows.
- Access decisions capture denied action, target, actor, reason, feature capability, and decision time.

## Credential Exposure

- NFC and QR domain models store credential references only.
- UI and audit examples do not expose raw NFC or QR secrets.
- Mobile status snapshots include credential type, status, validity, and revocation evidence only.

## Remaining Hardening

- Replace local `X-Actor-Reference` development behavior with authenticated JWT claims in deployed environments.
- Add idempotency persistence for `clientRequestId` before production credential lifecycle use.
- Replace static route examples with generated OpenAPI once the API host is part of CI.
