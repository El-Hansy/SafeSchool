#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
MOBILE_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
APP_ID="${SAFE_SCHOOL_APP_ID:-com.safeschool.mobile}"
APK_PATH="${1:-$MOBILE_DIR/build/app/outputs/flutter-apk/app-release.apk}"

if ! command -v adb >/dev/null 2>&1; then
  echo "adb is not available. Install Android platform-tools and make sure adb is on PATH." >&2
  exit 1
fi

if [[ ! -f "$APK_PATH" ]]; then
  echo "APK not found: $APK_PATH" >&2
  echo "Run: cd apps/mobile && ./tool/build_controlled_apk.sh" >&2
  exit 1
fi

devices=()
while IFS= read -r device; do
  devices+=("$device")
done < <(adb devices | awk 'NR > 1 && $2 == "device" { print $1 }')

if [[ -n "${SAFE_SCHOOL_DEVICE_SERIAL:-}" ]]; then
  devices=("$SAFE_SCHOOL_DEVICE_SERIAL")
fi

if [[ ${#devices[@]} -eq 0 ]]; then
  echo "No authorized Android device detected." >&2
  echo "Connect the phone by USB, enable Developer options + USB debugging, then accept the RSA prompt." >&2
  exit 1
fi

if [[ ${#devices[@]} -gt 1 ]]; then
  echo "Multiple Android devices detected: ${devices[*]}" >&2
  echo "Set SAFE_SCHOOL_DEVICE_SERIAL to the target serial and rerun this script." >&2
  exit 1
fi

serial="${devices[0]}"
echo "Installing $APK_PATH on Android device $serial"
adb -s "$serial" install -r "$APK_PATH"

echo "Verifying package $APP_ID"
adb -s "$serial" shell pm path "$APP_ID" >/dev/null

echo "Launching SafeSchool NFC"
adb -s "$serial" shell monkey -p "$APP_ID" -c android.intent.category.LAUNCHER 1 >/dev/null

echo "SafeSchool NFC installed and launched on $serial"
