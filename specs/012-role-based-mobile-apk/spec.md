# Feature Specification: Phase 12 Role-Based Mobile App & APK Release

**Feature Branch**: `012-role-based-mobile-apk`  
**Created**: 2026-05-10  
**Status**: Draft  
**Input**: User description: "phase 12 Role-Based Mobile App & APK Release"

## Clarifications

### Session 2026-05-10

- Q: What should Phase 12 deliver for mobile role coverage? → A: Production-complete mobile coverage for every role listed in the spec.
- Q: What language and layout support must Phase 12 include? → A: Arabic and English with RTL support.

## Constitution Alignment *(mandatory)*

- **Implementation Phase**: Phase 12: Role-Based Mobile App & APK Release, a planned extension after Phase 11 to make prior platform capabilities production-ready and usable from one mobile app.
- **Feature Module(s)**: Mobile App Shell, Role-Based Mobile Experience, Mobile Permission Visibility, Device Sessions, APK Release Management, and cross-feature mobile access to enabled prior modules.
- **Tenant Scope**: Mobile access, device sessions, role workspaces, permission visibility, APK release audience, install records, and mobile audit evidence are tenant-scoped. A user with access to more than one tenant must choose or be assigned an active tenant before seeing tenant-owned data.
- **Feature Flag(s)**: Requires tenant-level mobile app enablement, APK release enablement, and inherited feature flags for each module exposed in the app. A mobile screen or action must remain hidden or blocked when the tenant or role does not have the relevant capability.
- **Security/Roles**: One mobile app must serve multiple roles through role and permission enforcement, not through separate apps. Guardian, student, driver, gate/access staff, canteen cashier, teacher, medical staff, complaint handler, communication sender, document administrator, school administrator, and platform support views are available only when the authenticated user has the matching tenant role and permission.
- **Offline/NFC Impact**: The app may support offline scan or queued action continuity only for workflows already designed for offline use, such as NFC/QR attendance, transport scans, wallet POS, and emergency capture. Phase 12 must not create new attendance, access, wallet, transport, medical, request, complaint, communication, or document outcomes outside those source-domain permissions.
- **Observability**: Mobile sign-in, role selection, denied access, device registration, app version checks, APK release install events, offline queue events, sync outcomes, support diagnostics, and sensitive mobile actions must produce audit evidence, operational metrics, and reviewable error records.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Open One App With the Right Role Workspace (Priority: P1)

A user installs one school mobile app, signs in, and sees only the workspace, student links, and actions allowed by their current tenant role and permissions.

**Why this priority**: This is the core product gap. The platform needs production-complete mobile role access without creating separate apps for parents, staff, and school operators.

**Independent Test**: Can be fully tested by signing in as users with guardian-only, student-only, staff-only, and multi-role accounts and verifying that each account sees the correct mobile workspace and no unauthorized actions.

**Acceptance Scenarios**:

1. **Given** a guardian has an approved link to one or more students and the tenant has mobile access enabled, **When** the guardian signs in, **Then** the app shows guardian-allowed student cards, notifications, transport, wallet, requests, complaints, documents, and certificates only where each feature is enabled.
2. **Given** a user has both guardian and staff roles in the same tenant, **When** the user opens the app, **Then** the app requires an active role context or shows a clear role switcher before role-specific actions are available.
3. **Given** a user has no active mobile permission in the tenant, **When** the user signs in, **Then** the app blocks all tenant data and explains that mobile access is not assigned.
4. **Given** a user chooses Arabic or English, **When** the user opens any role workspace, **Then** the workspace uses the selected language and correct text direction without hiding role-critical actions.

---

### User Story 2 - Guardian and Student Complete Mobile Journeys (Priority: P2)

Guardians and students can use the app to review school information and submit allowed actions without needing the school admin web portal.

**Why this priority**: Guardians and students need complete mobile access to their assigned workflows, including mobile access to the work already completed in earlier phases.

**Independent Test**: Can be tested by using guardian and student demo accounts to complete enabled read and submit journeys for linked students while confirming that hidden features remain inaccessible.

**Acceptance Scenarios**:

1. **Given** a guardian is linked to a student, **When** the guardian opens the student profile, **Then** the guardian can see enabled summaries for attendance, campus access, transport, wallet, learning, requests, complaints, communications, documents, and certificates according to tenant configuration.
2. **Given** a guardian has permission to submit requests or complaints, **When** the guardian submits a mobile form, **Then** the submission is tied to the selected student, tenant, actor, timestamp, and audit trail.
3. **Given** a student has student mobile access, **When** the student signs in, **Then** the student sees only student-appropriate learning, communications, complaints, documents, certificates, and profile information.

---

### User Story 3 - Staff Use Operational Mobile Duties (Priority: P2)

School staff can perform mobile duties assigned to their roles, such as driver transport actions, gate scans, canteen POS, learning updates, medical capture, communication sending, and document review.

**Why this priority**: The mobile app is not only for parents. The same role-based app must support operational roles where a mobile device is the natural working surface.

**Independent Test**: Can be tested by signing in as each configured staff role and completing one assigned workflow while verifying that unrelated role actions are blocked.

**Acceptance Scenarios**:

1. **Given** a driver is assigned to an active route and trip, **When** the driver opens the transport workspace, **Then** the driver can view assigned trip details and perform permitted boarding, drop, and location actions.
2. **Given** a canteen cashier has wallet POS permission, **When** the cashier scans or enters a student credential for a purchase, **Then** the app allows only permitted purchase actions and records the decision for review.
3. **Given** a teacher, nurse, gate staff member, or document administrator signs in, **When** they open the app, **Then** the app shows only the workflows assigned to that staff role and tenant.

---

### User Story 4 - Release and Support an Installable APK (Priority: P3)

A platform release operator or authorized school administrator can identify the current APK release, distribute it to pilot users, verify installs, and support rollback or upgrade decisions.

**Why this priority**: Production rollout needs an installable artifact with version, audience, release notes, and support evidence rather than an informal local build.

**Independent Test**: Can be tested by publishing a demo release record, installing it on pilot devices, verifying the shown version and audience, and confirming that obsolete or revoked releases are blocked.

**Acceptance Scenarios**:

1. **Given** a demo APK release is approved for a tenant pilot audience, **When** a pilot user opens the release link or scans the install code, **Then** the user can install the approved version and see release notes, version, environment, and support contact information.
2. **Given** a release is revoked or replaced, **When** a user opens an obsolete app version, **Then** the app clearly instructs the user to update or blocks access if the version is no longer supported.
3. **Given** support reviews a mobile issue, **When** the support user searches by user, device, version, tenant, or time range, **Then** the support user can see install, sign-in, denied access, and sync evidence within their permission scope.

### Edge Cases

- A user has valid credentials but no tenant role with mobile access.
- A user has roles in multiple tenants and tries to view data before selecting an active tenant.
- A role or permission is revoked while the user has an active session.
- A guardian link to a student is removed after the guardian has cached student information.
- A tenant disables a feature that was previously visible in the mobile app.
- A mobile device is offline during a workflow that supports queued actions.
- A mobile device is offline during a workflow that requires online confirmation.
- A demo APK expires, is revoked, or is replaced by a newer release.
- A shared device is used by multiple family members or staff members.
- The user tries to access a deep link for a feature hidden by role, feature flag, tenant, or audience rules.
- Arabic text expansion or right-to-left layout changes the available screen space for role-critical actions.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST provide one mobile app experience that changes visible workspaces, navigation, summaries, and actions based on the authenticated user's active tenant role and permissions.
- **FR-002**: System MUST NOT require or promote a separate parent-only app for guardian access; guardian access is a role and permission inside the same mobile app.
- **FR-003**: System MUST resolve the active tenant before showing tenant-owned mobile data or permitting tenant-owned actions.
- **FR-004**: System MUST enforce role, permission, tenant, and feature availability checks before every sensitive mobile view or action.
- **FR-005**: System MUST allow authorized administrators to configure which roles and users can access mobile workspaces and actions.
- **FR-006**: System MUST hide unavailable mobile features by default and show clear denied-access messages when a user attempts a blocked direct link or stale cached action.
- **FR-007**: System MUST support users with multiple roles or tenants by providing a clear active role and tenant context before role-specific actions are taken.
- **FR-008**: System MUST limit guardian mobile data to approved linked students and configured guardian-visible information.
- **FR-009**: System MUST limit student mobile data to the signed-in student's own configured information and student-appropriate actions.
- **FR-010**: System MUST expose production-complete mobile workspaces for every role listed in this specification, including guardian, student, transport driver, gate/access staff, canteen cashier, teacher, medical staff, complaint handler, communication sender, document administrator, school administrator, and platform support duties where the tenant enables them.
- **FR-011**: System MUST preserve source-domain authority by routing mobile actions to the owning feature workflow and preventing mobile screens from directly changing unrelated domain outcomes.
- **FR-012**: System MUST support offline capture or queued action behavior only for workflows where offline continuity is already part of the approved user journey.
- **FR-013**: System MUST provide a unified mobile notification center that displays notifications according to the user's active tenant, role, student links, and feature permissions.
- **FR-014**: System MUST track mobile device sessions, app version, active tenant, active role, sign-in state, and logout events for audit and support review.
- **FR-015**: System MUST reflect revoked role, permission, tenant, or feature access within a defined short operational window and block further sensitive actions after revocation.
- **FR-016**: System MUST maintain an approved APK release record with version, release audience, release notes, release status, distribution instructions, support contact, and integrity evidence.
- **FR-017**: System MUST allow authorized release or support users to review install, upgrade, obsolete-version, revoked-version, and app launch evidence for the users and tenants they are permitted to support.
- **FR-018**: System MUST provide a controlled update and rollback path so unsupported or revoked APK versions can be blocked or directed to an approved replacement.
- **FR-019**: System MUST provide production-complete role experiences for guardian, student, driver, cashier, gate/access staff, teacher, medical staff, complaint handler, communication sender, document administrator, school administrator, and platform support users, while also providing demo-safe seed access for authorized presenters without exposing real tenant data.
- **FR-020**: System MUST record audit events and operational metrics for mobile sign-in, role selection, denied access, device registration, APK install, version checks, offline queue activity, sync outcomes, and sensitive mobile actions.
- **FR-021**: System MUST present mobile errors and empty states in plain language so users know whether the issue is missing permission, disabled feature, stale release, offline state, or support-required failure.
- **FR-022**: System MUST protect shared-device privacy by requiring explicit sign-out support and by preventing one user's cached tenant data from becoming visible to another user.
- **FR-023**: System MUST support Arabic and English mobile content, including right-to-left layout behavior for Arabic across all role workspaces, forms, notifications, errors, release notes, and support messages.

### Key Entities *(include if feature involves data)*

- **Mobile User Profile**: The signed-in user's mobile identity, active tenant, available tenants, available roles, student links, and role-specific visibility.
- **Role Workspace**: A role-specific mobile surface that defines which summaries, actions, notifications, and shortcuts are available in the current tenant context.
- **Mobile Permission Grant**: A tenant-scoped authorization assignment that allows a user or role to view or perform a mobile action.
- **Tenant Mobile Feature Availability**: The tenant configuration that determines which platform modules are visible and usable from the mobile app.
- **Mobile Language Preference**: The user's selected mobile language and text direction context used to present role workspaces, notifications, forms, errors, release notes, and support messages.
- **Device Session**: A signed-in mobile device context containing user, tenant, role, version, sign-in state, revocation status, and support evidence.
- **APK Release**: An approved installable app release with version, audience, status, release notes, support details, and integrity evidence.
- **Release Audience**: The tenant, role, user group, or pilot group allowed to install or use a specific APK release.
- **Install or Upgrade Event**: Evidence that a user installed, launched, upgraded, used, or was blocked from a specific app version.
- **Offline Action Queue**: A mobile record of queued work for workflows that support offline continuity, including actor, tenant, source feature, local timestamp, sync state, and conflict outcome.
- **Mobile Audit Event**: A reviewable event for sensitive access, action, denied access, version, sync, support, and release behavior.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: At least 95% of pilot users can install the approved demo APK and reach the sign-in screen within 10 minutes using provided distribution instructions.
- **SC-002**: At least 95% of successful sign-ins show the correct role workspace within 5 seconds after tenant and role context are resolved.
- **SC-003**: Permission validation tests block 100% of attempted mobile access to student data, staff actions, or admin/support views outside the user's assigned tenant, role, and feature permissions.
- **SC-004**: A presenter can demonstrate guardian access to linked student summaries across enabled prior modules in under 8 minutes without switching to a separate parent app.
- **SC-005**: Production acceptance covers all listed role workspaces, including guardian, student, driver, canteen cashier, gate/access staff, teacher, medical staff, complaint handler, communication sender, document administrator, school administrator, and platform support, with 100% of role-specific critical journeys passing validation.
- **SC-006**: 95% of role or permission revocations are reflected in mobile access decisions within 5 minutes.
- **SC-007**: Support users can identify a pilot user's app version, install status, sign-in result, and most recent denied-access or sync event within 2 minutes.
- **SC-008**: 90% of pilot users report that the app clearly shows what they can do for their role without needing separate instructions for hidden or unavailable features.
- **SC-009**: Controlled release validation confirms that revoked or obsolete APK versions are blocked or directed to upgrade in 100% of tested cases.
- **SC-010**: Arabic and English validation confirms that 100% of role-critical journeys remain readable, correctly directed, and usable without clipped primary actions or incorrect text direction.

## Assumptions

- Phase 12 defines one production-complete role-based mobile app, not separate apps for guardians, students, drivers, staff, or administrators.
- APK release scope means an Android production or pilot package with controlled distribution. Public app-store release and iOS release may be planned later unless explicitly added to this phase.
- Existing authentication, tenant resolution, role assignment, permission, and feature-flag concepts from earlier phases remain the source of truth.
- Mobile access to each domain depends on the corresponding domain being implemented and enabled for the tenant.
- Offline behavior is limited to workflows already designed for offline continuity; online-only workflows must fail safely with clear user guidance.
- Demo data must be visibly separate from real tenant data and must be safe for sales presentations, but demo mode does not reduce the production-complete coverage required for listed roles.
- Arabic and English are required in Phase 12; additional languages may be planned after the APK release scope is stable.
- Production payment, transport, medical, attendance, document, and communication rules remain owned by their original modules; this phase only adds the mobile role experience and APK release control around them.
