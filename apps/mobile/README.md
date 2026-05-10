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

The APK can stay in guided demo mode, or it can be built with backend runtime
configuration:

```bash
flutter build apk --release \
  --build-name=12.0.0 \
  --build-number=1200 \
  --dart-define=SAFE_SCHOOL_API_BASE_URL=https://api.example.school \
  --dart-define=SAFE_SCHOOL_TENANT_ID=school-demo
```

If `SAFE_SCHOOL_API_BASE_URL` is not set, the app clearly marks the shell as
`Demo data`. When the base URL is set, the shell marks the runtime as
`API ready` and the shared mobile API client builds `/api/v1/` school,
guardian, and student routes with tenant and bearer-token headers.

## Install On Android

1. Connect the phone by USB.
2. Enable Developer options and USB debugging on the phone.
3. Accept the Android RSA prompt when it appears.
4. Run:

```bash
cd apps/mobile
./tool/install_demo_apk.sh
```

If more than one device is connected, set `SAFE_SCHOOL_DEVICE_SERIAL` to the
target device serial from `adb devices`.

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
./tool/install_demo_apk.sh
```

Use the app to show the Guardian live view first, then switch roles for gate
NFC/QR scans, transport boarding/drop-off, canteen POS, medical incident,
documents, communications, admin, support, Arabic RTL, and offline queue sync.
