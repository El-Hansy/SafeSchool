# Mobile Demo Runbook

## Admin Web

1. Open the mobile command center at `/mobile`.
2. Review the role matrix and confirm 12 workspaces.
3. Open `/mobile/releases` and show active APK version `12.0.0`.
4. Open `/mobile/support` and show install, denied access, and sync evidence.

## Android APK

1. Install `apps/mobile/build/app/outputs/flutter-apk/app-release.apk`.
2. Open the app and start on the Guardian live view for Amina Hassan.
3. Show the parent idea: NFC identity, attendance, campus status, bus ETA,
   wallet balance, documents, notifications, request form, complaint form, and
   audit timeline.
4. Tap `Simulate gate NFC` and show the guardian notification in the live audit
   timeline.
5. Tap `Move bus / refresh ETA` and show the bus location and ETA update.
6. Tap `Top up SAR 50` and show the wallet top-up audit event.
7. Tap `Submit request` and `Submit complaint` to show parent submissions tied
   to the selected student.
8. Switch to Gate/access staff and tap `Scan NFC entry/exit` to show scan
   evidence.
9. Switch to Transport driver and tap `Scan student boarding` or
   `Scan student drop-off` to show bus registration evidence.
10. Switch to Canteen cashier and tap `Scan and charge lunch` to show wallet
    deduction evidence.
11. Switch to Arabic using the language icon and verify RTL content.
12. Toggle Offline mode, perform a supported action, then tap `Sync` to show
    queued mobile action behavior.
