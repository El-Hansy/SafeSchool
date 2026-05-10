# APK Release Readiness

- Version name and code are set.
- Release audience is active.
- Release notes exist in Arabic and English.
- Checksum is recorded.
- Obsolete versions are blocked or directed to update.
- Android manifest declares network, NFC, camera, location, notification, and
  vibration permissions needed by the role-based NFC/QR, live tracking, POS,
  and notification demo flows.
- NFC, camera, and GPS are optional device features so the demo APK can install
  on supported Android phones even when one hardware capability is unavailable.
