# Identity Access Traceability

| Requirement | Runtime coverage | Test coverage |
|-------------|------------------|---------------|
| FR-001 | Student profile domain, service, controller, admin student page | StudentProfileTests, StudentProfilesContractTests, student-profile.spec.ts |
| FR-002 | DuplicateStudentProfileDetector, create-profile duplicate guard | DuplicateStudentProfileDetectorTests |
| FR-003 | StudentProfileAuditAdapter, AuditWriter | AuditAccessDecisionAndPermissionGuardTests |
| FR-004 | GuardianRecord, GuardianLink, GuardianService, GuardianLinkService | GuardianRecordTests, GuardianLinkingContractTests |
| FR-005 | GuardianLink access scope, validity window, lifecycle status | GuardianLinkTests |
| FR-006 | GuardianVisibilityService | GuardianLinkingIntegrationTests |
| FR-007 | IdentityCredential, NfcCardCredential, CredentialLifecycleService | IdentityCredentialTests, CredentialLifecycleContractTests |
| FR-008 | NFC credential reference index and duplicate active reference check | CredentialLifecycleIntegrationTests |
| FR-009 | QrFallbackCredentialService, QR fallback route and feature key | QrFallbackCredentialTests, credential-lifecycle.spec.ts |
| FR-010 | QR validity window, rotation sequence, status reason | QrFallbackCredentialTests |
| FR-011 | PermissionCatalog, Role, Permission, ActorRoleAssignment | PermissionEnforcementContractTests |
| FR-012 | PermissionGuard, PermissionEvaluator | PermissionEvaluatorTests, PermissionEnforcementIntegrationTests |
| FR-013 | AccessDecision, AccessDecisionWriter | AuditAccessDecisionAndPermissionGuardTests |
| FR-014 | IdentityAccessCapabilities, FeatureGateService | FeatureGateServiceTests |
| FR-015 | TenantResolutionMiddleware, tenant-owned model base | TenantResolutionMiddlewareTests |
| FR-016 | AuditEvent, review controllers, audit timeline UI | AuditAccessDecisionAndPermissionGuardTests, access-control.spec.ts |
| FR-017 | CredentialStatusSnapshotService, Flutter status repository/cache/screen | credential_status_snapshot_test.dart, credential_status_cache_test.dart |
| FR-018 | Explicit exclusions in docs, tests, and route names | identity-access.e2e.spec.ts |
| SC-001 | Admin student form/table and student profile API route | student-profile.spec.ts |
| SC-002 | PermissionGuard, PermissionEvaluator, AccessDecisionWriter | PermissionEnforcementIntegrationTests |
| SC-003 | Guardian link lifecycle states and access scope | GuardianLinkTests, guardian-linking.spec.ts |
| SC-004 | Credential table, status snapshot API, mobile screen | credential-lifecycle.spec.ts, credential_status_snapshot_test.dart |
| SC-005 | AuditWriter and review timeline | AuditAccessDecisionAndPermissionGuardTests |
| SC-006 | DuplicateStudentProfileDetector | DuplicateStudentProfileDetectorTests |
| SC-007 | CredentialStatusSnapshotService and mobile cache | CredentialLifecycleIntegrationTests, credential_status_cache_test.dart |
