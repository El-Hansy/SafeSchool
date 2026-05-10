#!/usr/bin/env bash
set -euo pipefail

if [[ -f "android/key.properties" ]]; then
  echo "Building SafeSchool APK with configured release signing."
else
  echo "Building SafeSchool APK with debug signing for local demo use."
  echo "Create apps/mobile/android/key.properties from key.properties.example for pilot/production signing."
fi

declare -a dart_defines=()
if [[ -n "${SAFE_SCHOOL_API_BASE_URL:-}" ]]; then
  dart_defines+=("--dart-define=SAFE_SCHOOL_API_BASE_URL=$SAFE_SCHOOL_API_BASE_URL")
fi
if [[ -n "${SAFE_SCHOOL_TENANT_ID:-}" ]]; then
  dart_defines+=("--dart-define=SAFE_SCHOOL_TENANT_ID=$SAFE_SCHOOL_TENANT_ID")
fi
if [[ -n "${SAFE_SCHOOL_AUTH_TOKEN:-}" ]]; then
  echo "SAFE_SCHOOL_AUTH_TOKEN is for local QA only; do not bake long-lived secrets into release APKs." >&2
  dart_defines+=("--dart-define=SAFE_SCHOOL_AUTH_TOKEN=$SAFE_SCHOOL_AUTH_TOKEN")
fi

if [[ ${#dart_defines[@]} -gt 0 ]]; then
  flutter build apk --release \
    --build-name="${SAFE_SCHOOL_VERSION_NAME:-12.0.0}" \
    --build-number="${SAFE_SCHOOL_VERSION_CODE:-1200}" \
    "${dart_defines[@]}"
else
  flutter build apk --release \
    --build-name="${SAFE_SCHOOL_VERSION_NAME:-12.0.0}" \
    --build-number="${SAFE_SCHOOL_VERSION_CODE:-1200}"
fi
