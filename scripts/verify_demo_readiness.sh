#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
APK_PATH="$ROOT_DIR/apps/mobile/build/app/outputs/flutter-apk/app-release.apk"

log() {
  printf '\n==> %s\n' "$1"
}

fail_if_matches() {
  local label="$1"
  shift

  log "$label"
  if rg "$@" "$ROOT_DIR/apps"; then
    echo "Readiness check failed: $label" >&2
    exit 1
  fi
}

resolve_adb() {
  if command -v adb >/dev/null 2>&1; then
    command -v adb
    return 0
  fi

  local candidates=(
    "${ANDROID_HOME:-}/platform-tools/adb"
    "${ANDROID_SDK_ROOT:-}/platform-tools/adb"
    "$HOME/Library/Android/sdk/platform-tools/adb"
  )

  local candidate
  for candidate in "${candidates[@]}"; do
    if [[ -x "$candidate" ]]; then
      echo "$candidate"
      return 0
    fi
  done

  return 1
}

log "SafeSchool demo readiness verifier"
echo "Repository: $ROOT_DIR"
echo "Branch: $(git -C "$ROOT_DIR" branch --show-current)"
echo "Commit: $(git -C "$ROOT_DIR" rev-parse --short HEAD)"

log "Checking for stale app demo markers"
fail_if_matches "No stale app placeholder markers" \
  -n "SafeSchool phase demo|com\\.example|version: 0\\.1\\.0\\+1|SafeSchool Phase [0-9]" \
  --glob '!**/tests/**' \
  --glob '!**/test/**' \
  --glob '!**/build/**' \
  --glob '!**/node_modules/**' \
  --glob '!**/.next/**' \
  -S

fail_if_matches "No false-green placeholder tests" \
  -n "test\\('placeholder'|describe\\(\\\"placeholder\\\"|describe\\('placeholder'|expect\\(true\\)\\.toBe\\(true\\)|expect\\(true, isTrue\\)" \
  --glob '!**/build/**' \
  --glob '!**/node_modules/**' \
  --glob '!**/.next/**' \
  -S

log "Backend API tests"
dotnet test "$ROOT_DIR/apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj" -v minimal

log "Admin web tests"
(cd "$ROOT_DIR/apps/admin-web" && npm test)

log "Admin web production build"
(cd "$ROOT_DIR/apps/admin-web" && npm run build)

log "Mobile Flutter tests"
(cd "$ROOT_DIR/apps/mobile" && flutter test)

log "Mobile APK build"
(cd "$ROOT_DIR/apps/mobile" && ./tool/build_controlled_apk.sh)

log "APK artifact"
if [[ ! -f "$APK_PATH" ]]; then
  echo "Readiness check failed: APK not found at $APK_PATH" >&2
  exit 1
fi
ls -lh "$APK_PATH"
shasum -a 256 "$APK_PATH"

log "Android device readiness"
ADB_BIN="$(resolve_adb || true)"
if [[ -n "$ADB_BIN" ]]; then
  echo "adb: $ADB_BIN"
  "$ADB_BIN" devices
  authorized_count="$("$ADB_BIN" devices | awk 'NR > 1 && $2 == "device" { count++ } END { print count + 0 }')"

  if [[ "$authorized_count" -eq 0 ]]; then
    echo "No authorized Android device detected. Set SAFE_SCHOOL_VERIFY_ANDROID_INSTALL=1 only after USB debugging is available."
  elif [[ "${SAFE_SCHOOL_VERIFY_ANDROID_INSTALL:-0}" == "1" ]]; then
    "$ROOT_DIR/apps/mobile/tool/install_demo_apk.sh" "$APK_PATH"
  else
    echo "Authorized Android device detected. To install and launch the APK, rerun with SAFE_SCHOOL_VERIFY_ANDROID_INSTALL=1."
  fi
else
  echo "adb not found. Install Android platform-tools or set ANDROID_HOME/ANDROID_SDK_ROOT before physical-device validation."
fi

log "Readiness verifier completed"
