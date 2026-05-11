#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
API_PROJECT="$ROOT_DIR/apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj"
API_TESTS="$ROOT_DIR/apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj"
API_APPSETTINGS="$ROOT_DIR/apps/api/src/SafeSchool.Api/appsettings.json"

log() {
  printf '\n==> %s\n' "$1"
}

fail() {
  echo "Production readiness failed: $1" >&2
  exit 1
}

log "SafeSchool production readiness verifier"
echo "Repository: $ROOT_DIR"
echo "Branch: $(git -C "$ROOT_DIR" branch --show-current)"
echo "Commit: $(git -C "$ROOT_DIR" rev-parse --short HEAD)"

log "Base API config has no unsafe demo defaults"
if rg -n "identity\\.local|Host=localhost;Database=safeschool;Username=safeschool;Password=safeschool|\"Adapter\"\\s*:\\s*\"DemoPay\"" "$API_APPSETTINGS" -S; then
  fail "appsettings.json must not carry placeholder identity, default local DB, or DemoPay provider values."
fi

if ! rg -n "\"RequireWebhookSignature\"\\s*:\\s*true" "$API_APPSETTINGS" -S >/dev/null; then
  fail "production payment provider webhooks must require signatures."
fi

log "Production guard tests"
dotnet test "$API_TESTS" -v minimal --filter "FullyQualifiedName~RuntimeConfigurationValidatorTests|FullyQualifiedName~PaymentProviderAdapterTests|FullyQualifiedName~ApiAuthorizationBoundaryTests"

log "Admin web production data guard"
(cd "$ROOT_DIR/apps/admin-web" && npm test -- --run tests/production/apiFallbackGuard.spec.ts)

log "EF migration presence"
if ! dotnet ef migrations list \
  --project "$API_PROJECT" \
  --startup-project "$API_PROJECT" \
  --context SafeSchoolDbContext \
  --no-connect | rg "InitialSafeSchoolSchema" -S; then
  fail "Initial EF schema migration was not found."
fi

log "Mobile guarded APK release controls"
(cd "$ROOT_DIR/apps/mobile" && flutter test test/features/demo/android_release_signing_test.dart)

log "Production readiness verifier completed"
echo "This verifier proves production fails closed and release controls are present. Real deployment still requires external secrets and services."
