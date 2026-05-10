# Quickstart: Phase 12 Role-Based Mobile App & APK Release

## Purpose

Use this quickstart to validate the planned Phase 12 behavior after tasks are
generated and implemented. It focuses on production-complete role coverage,
one-app permission behavior, Arabic/English RTL, controlled APK release, and
support evidence.

## Preconditions

- Phase 12 mobile feature is enabled for the demo or pilot tenant.
- Prior modules needed for role workspaces are implemented and enabled for the
  tenant.
- Demo-safe users exist for each listed role:
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
  - Platform support
- Arabic and English localization content is available.
- An approved APK release record exists with an active release audience.

## Backend Validation

1. Start the API service.
2. Verify mobile profile resolution for guardian, student, staff, admin, and
   support users.
3. Verify tenant and role context switching for a multi-role user.
4. Verify denied access for a valid user with no mobile permission.
5. Verify revocation blocks sensitive actions within 5 minutes.
6. Verify release lookup returns the approved APK only for the active audience.
7. Verify revoked and obsolete versions are blocked or directed to upgrade.
8. Verify install, launch, denied-access, sync, and sensitive-action events are
   visible to authorized support users.

## Admin Web Validation

1. Open the mobile administration area.
2. Enable mobile app access for the tenant.
3. Enable role workspaces and inherited feature flags required for each role.
4. Assign or revoke mobile permissions for a test role and a test user.
5. Create or review an APK release record.
6. Assign release audiences by tenant, role, pilot group, or user.
7. Review install/version evidence and denied-access events in support views.
8. Confirm support users cannot see tenants outside their support scope.

## Mobile App Validation

1. Install the approved APK on a test Android device or emulator.
2. Confirm the app reaches the sign-in screen.
3. Sign in as a guardian and verify linked student summaries and permitted
   actions.
4. Sign in as a student and verify student-only visibility.
5. Sign in as each staff role and complete one critical role-specific journey.
6. Sign in as a multi-role user and verify tenant/role selection before
   sensitive actions.
7. Switch to Arabic and verify right-to-left layout across role-critical
   journeys.
8. Switch to English and verify left-to-right layout across the same journeys.
9. Attempt a blocked deep link and verify the app shows a clear denied-access
   message without restricted record details.
10. Sign out on a shared device and verify user-visible cached tenant data is
    cleared.

## Offline Workflow Validation

1. Put the device offline.
2. Complete an approved offline workflow such as scan capture, transport scan,
   wallet POS, or emergency capture.
3. Confirm the action enters the local queue with client id, tenant, actor,
   source feature, and local timestamp.
4. Reconnect the device.
5. Confirm sync result is accepted, rejected, duplicate, or conflict according
   to source-domain rules.
6. Attempt an online-only workflow while offline and confirm it fails safely
   with clear user guidance.

## APK Release Validation

1. Confirm the approved release includes version name, version code, audience,
   Arabic and English release notes, support contact, and checksum.
2. Install with an audience-approved user.
3. Attempt install or launch with an out-of-audience user.
4. Revoke or supersede the release.
5. Confirm obsolete or revoked versions are blocked or directed to upgrade.
6. Confirm install and blocked-version evidence is visible to authorized
   support users.

## Success Evidence

- 95% of pilot users can install and reach sign-in within 10 minutes.
- 95% of successful sign-ins show the correct workspace within 5 seconds after
  tenant and role resolution.
- 100% of unauthorized role/tenant/feature attempts are blocked in validation.
- 100% of listed role-critical journeys pass production acceptance.
- Arabic and English validation confirms no clipped primary actions or wrong
  text direction.
- Support can find user version, install status, sign-in result, denied access,
  or sync evidence within 2 minutes.

## Next Step

Run `/speckit.tasks` after this plan is reviewed.
