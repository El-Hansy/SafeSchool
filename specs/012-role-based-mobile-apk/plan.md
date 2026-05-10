# Implementation Plan: Phase 12 Role-Based Mobile App & APK Release

**Branch**: `012-role-based-mobile-apk` | **Date**: 2026-05-10 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/012-role-based-mobile-apk/spec.md`

**Note**: This plan is filled in by the `/speckit.plan` workflow and stops after
planning artifacts. Implementation tasks are generated later by
`/speckit.tasks`.

## Summary

Phase 12 adds one production-complete role-based mobile app and controlled APK
release capability for the SafeSchool platform. The app serves guardians,
students, transport drivers, gate/access staff, canteen cashiers, teachers,
medical staff, complaint handlers, communication senders, document
administrators, school administrators, and platform support users through
tenant-scoped roles and permissions rather than separate apps. The planning
approach keeps the existing modular monolith, adds a Mobile module for app
shell, role workspaces, device sessions, language preference, offline action
queues, release metadata, install evidence, and mobile audit events, and
exposes production-ready Flutter mobile experiences backed by the existing
ASP.NET Core APIs. Arabic and English, including right-to-left Arabic layouts,
are in scope for every role-critical journey. APK distribution is controlled by
tenant, audience, version, release status, integrity evidence, and support
review.

## Technical Context

**Language/Version**: Constitution baseline: latest supported .NET and ASP.NET
Core for backend services, Next.js/React/TypeScript for school administration
and support web controls, and Flutter/Dart for the role-based Android mobile
app. Android native packaging uses the project mobile Android Gradle/Kotlin
tooling where required by Flutter.
**Primary Dependencies**: ASP.NET Core Web API, EF Core, PostgreSQL,
S3-compatible object storage for APK artifacts and release evidence where
needed, JWT-based authentication, existing IdentityAccess tenant/user/role/
permission foundations, tenant feature configuration, prior phase feature APIs,
SQLite-backed mobile offline queues for approved offline workflows, Flutter
localization, app version metadata, audit/event logging, metrics/error
reporting foundations, Next.js App Router admin/support surfaces, and Spec Kit
planning artifacts.
**Storage**: Single PostgreSQL database for mobile user profiles, role
workspace visibility records, mobile permission grants, tenant mobile feature
availability, mobile language preferences, device sessions, APK release
records, release audiences, install and upgrade events, offline action queue
metadata, version check evidence, support diagnostics, and mobile audit events.
S3-compatible object storage stores APK files and optional release evidence
objects. Mobile devices use SQLite only for cached tenant/role state and
offline queues for approved offline workflows. Every tenant-owned table
includes `tenant_id`, `created_at`, and `updated_at`, with indexes for tenant,
user, role, permission, student link, device, app version, release status,
release audience, sync state, language, and audit correlation lookups.
**Testing**: Backend unit tests for role workspace resolution, permission
visibility, tenant feature availability, device session lifecycle, release
status transitions, audience matching, version blocking, language preference,
offline queue metadata, and audit events; integration and contract tests for
`/api/v1/` mobile profile, workspace, permissions, device, release, install,
version, sync, notification, and support routes; mobile widget/journey tests
for every listed role in English and Arabic RTL; APK release smoke tests;
authorization, tenant-isolation, feature-flag, revocation, offline sync,
shared-device privacy, and critical UI journey tests.
**Target Platform**: Multi-tenant SaaS platform with backend APIs, school
administration and support web controls, and one Android mobile app delivered
as a controlled APK for production or pilot use. Public app-store release and
iOS release remain outside this phase unless separately specified.
**Project Type**: Modular monolith SaaS with backend API, admin/support web
controls, and Flutter mobile app organized by feature modules.
**Performance Goals**: Satisfy spec success criteria: 95% of pilot users can
install and reach sign-in within 10 minutes; 95% of successful sign-ins show
the correct role workspace within 5 seconds after tenant and role resolution;
95% of revocations are reflected within 5 minutes; support can identify user
version/install/sign-in/denied-access/sync evidence within 2 minutes; all
listed role-critical journeys pass permission and localization validation.
**Constraints**: Phase 12 only. One mobile app must cover all listed roles
through tenant-scoped role and permission enforcement. Guardian access is a
role/permission, not a separate parent app. Arabic and English with RTL support
are required. APK release is controlled distribution, not app-store
publication. Mobile actions must route through owning source-domain workflows
and must not mutate attendance, access, transport, wallet, learning, request,
medical, complaint, communication, document, or admin outcomes outside those
features' permissions. Offline queues are allowed only where the approved
source journey supports offline continuity. No new microservice, separate
identity system, separate mobile database service, or third-party release
management platform is introduced.
**Scale/Scope**: Phase 12 covers Mobile App Shell, Role-Based Mobile
Experience, Mobile Permission Visibility, Device Sessions, APK Release
Management, Mobile Localization, Support Diagnostics, and cross-feature mobile
access to enabled prior modules for guardian, student, transport driver,
gate/access staff, canteen cashier, teacher, medical staff, complaint handler,
communication sender, document administrator, school administrator, and
platform support roles.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first delivery**: PASS. The plan references [spec.md](./spec.md),
  which defines user stories, acceptance scenarios, requirements, entities,
  edge cases, measurable success criteria, assumptions, and clarification
  decisions for production-complete role coverage and Arabic/English RTL.
- **Phase and module mapping**: PASS. The feature is assigned to Phase 12:
  Role-Based Mobile App & APK Release and maps to Mobile App Shell,
  Role-Based Mobile Experience, Mobile Permission Visibility, Device Sessions,
  APK Release Management, Mobile Localization, Support Diagnostics, and
  cross-feature mobile access to enabled prior modules.
- **Multi-tenancy and feature flags**: PASS. Tenant resolution, active tenant
  context, tenant-owned data, tenant mobile feature availability, APK release
  audience, inherited domain feature flags, and UI feature gates are required
  before mobile data or actions are exposed.
- **Security and authorization**: PASS. JWT authentication, tenant access,
  active role context, permission checks, approved guardian links, student
  self-scope, staff duty assignment, release audience, support scope, denied
  access evidence, and backend validation are planned for every sensitive
  mobile view and action.
- **Data and API contracts**: PASS. PostgreSQL metadata storage, EF Core
  migrations, DTO-based `/api/v1/` contracts, tenant indexes, paginated support
  review lists, device/release/version contracts, idempotent install and sync
  commands, and S3-compatible APK object references are planned.
- **Offline NFC integrity**: PASS. Phase 12 supports offline queues only for
  approved source workflows such as attendance/access scans, transport scans,
  wallet POS, and emergency capture. Mobile sync contracts remain idempotent
  and include actor, tenant, source feature, local timestamp, conflict outcome,
  and audit events. Phase 12 does not create source-domain outcomes directly.
- **Observability and testing**: PASS. Mobile sign-in, role selection, denied
  access, device registration, APK install, version checks, offline queue
  activity, sync outcomes, localization validation, support diagnostics, and
  sensitive actions emit structured logs, audit events, metrics, and error
  reports with unit, contract, integration, mobile journey, permission,
  revocation, tenant-isolation, and localization coverage.
- **Simplicity and cost**: PASS. The modular monolith, single PostgreSQL
  database, Flutter mobile app, existing admin web, and S3-compatible artifact
  storage remain the default. No microservice, queue platform, app-store
  automation system, separate identity provider, or separate mobile backend is
  introduced.

**Post-design re-check**: PASS. `research.md`, `data-model.md`, `contracts/`,
and `quickstart.md` preserve the one-app role model, production-complete role
coverage, Arabic/English RTL, tenant/permission enforcement, source-domain
authority boundaries, controlled APK release, offline queue limits, and support
observability without introducing constitution violations.

## Project Structure

### Documentation (this feature)

```text
specs/012-role-based-mobile-apk/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── apk-release-management.md
│   ├── mobile-role-access.md
│   ├── mobile-sync-notifications.md
│   └── support-observability.md
├── checklists/
│   └── requirements.md
└── tasks.md             # Future output from /speckit.tasks
```

### Source Code (repository root)

```text
apps/
├── api/
│   ├── src/SafeSchool.Api/Features/Mobile/
│   │   ├── Common/
│   │   ├── Profiles/
│   │   ├── RoleWorkspaces/
│   │   ├── Permissions/
│   │   ├── DeviceSessions/
│   │   ├── Releases/
│   │   ├── Localization/
│   │   ├── OfflineSync/
│   │   ├── Notifications/
│   │   ├── Support/
│   │   └── Seed/
│   └── tests/SafeSchool.Api.Tests/Features/Mobile/
├── admin-web/
│   ├── src/app/(school)/mobile/
│   ├── src/app/(school)/mobile/releases/
│   ├── src/app/(school)/mobile/roles/
│   ├── src/app/(school)/mobile/support/
│   ├── src/features/mobile/
│   └── tests/mobile/
└── mobile/
    ├── lib/app/
    ├── lib/core/auth/
    ├── lib/core/localization/
    ├── lib/core/offline/
    ├── lib/core/release/
    ├── lib/features/mobile_shell/
    ├── lib/features/role_workspaces/
    ├── lib/features/guardian/
    ├── lib/features/student/
    ├── lib/features/transport_driver/
    ├── lib/features/gate_access/
    ├── lib/features/canteen_cashier/
    ├── lib/features/teacher/
    ├── lib/features/medical_staff/
    ├── lib/features/complaint_handler/
    ├── lib/features/communication_sender/
    ├── lib/features/document_admin/
    ├── lib/features/school_admin/
    ├── lib/features/platform_support/
    ├── test/
    └── integration_test/

tests/
├── contracts/mobile/
└── e2e/mobile/
```

**Structure Decision**: Use the existing modular monolith plus mobile app
structure. Backend mobile orchestration and release metadata live under a new
`Features/Mobile` module while source-domain actions continue to be owned by
their prior modules. Admin web adds school/support controls for mobile
permissions, releases, and diagnostics. Flutter mobile adds a core app shell
and separate role feature folders so each listed role can be planned, tested,
and released independently inside one APK.

## Complexity Tracking

No constitution violations are introduced. No added complexity requires an
exception.
