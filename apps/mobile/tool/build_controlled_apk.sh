#!/usr/bin/env bash
set -euo pipefail

if [[ -f "android/key.properties" ]]; then
  echo "Building SafeSchool APK with configured release signing."
else
  echo "Building SafeSchool APK with debug signing for local demo use."
  echo "Create apps/mobile/android/key.properties from key.properties.example for pilot/production signing."
fi

flutter build apk --release \
  --build-name="${SAFE_SCHOOL_VERSION_NAME:-12.0.0}" \
  --build-number="${SAFE_SCHOOL_VERSION_CODE:-1200}"
