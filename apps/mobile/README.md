# SafeSchool Mobile

Role-based Android app for the SafeSchool NFC demo. One APK supports guardian,
student, driver, gate/access, canteen cashier, teacher, medical, complaints,
communications, document administrator, school administrator, and platform
support workspaces.

## Demo Build

```bash
cd apps/mobile
./tool/build_controlled_apk.sh
```

The APK is written to:

```text
apps/mobile/build/app/outputs/flutter-apk/app-release.apk
```

If `android/key.properties` is not present, the build uses debug signing for a
local demo install.

## Pilot Or Production Signing

1. Create an upload keystore outside the repository.
2. Copy `android/key.properties.example` to `android/key.properties`.
3. Set the absolute keystore path, passwords, and key alias.
4. Run `./tool/build_controlled_apk.sh`.

`android/key.properties`, `*.jks`, and `*.keystore` are ignored by git.

## Validation

```bash
flutter test
./tool/build_controlled_apk.sh
```

Use the app to show the Guardian live view first, then switch roles for gate
NFC/QR scans, transport boarding/drop-off, canteen POS, medical incident,
documents, communications, admin, support, Arabic RTL, and offline queue sync.
