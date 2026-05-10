# APK Release Readiness

- `scripts/verify_demo_readiness.sh` is the top-level demo readiness gate. It
  runs backend tests, admin-web tests and build, Flutter tests, APK build,
  stale-placeholder scans, APK checksum reporting, and optional Android install
  validation.
- Version name and code are set.
- Release audience is active.
- Release notes exist in Arabic and English.
- Checksum is recorded.
- Obsolete versions are blocked or directed to update.
- Local demo APK builds may use debug signing, but pilot or production APKs
  must provide `apps/mobile/android/key.properties` from the tracked
  `key.properties.example` template so Gradle uses the configured upload
  keystore.
- Android manifest declares network, NFC, camera, location, notification, and
  vibration permissions needed by the role-based NFC/QR, live tracking, POS,
  and notification demo flows.
- NFC, camera, and GPS are optional device features so the demo APK can install
  on supported Android phones even when one hardware capability is unavailable.
- `apps/mobile/tool/install_demo_apk.sh` installs, verifies, and launches
  package `com.safeschool.mobile` through `adb` for controlled physical-device
  demo validation.
