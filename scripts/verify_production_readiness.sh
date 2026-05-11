#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
API_PROJECT="$ROOT_DIR/apps/api/src/SafeSchool.Api/SafeSchool.Api.csproj"
API_TESTS="$ROOT_DIR/apps/api/tests/SafeSchool.Api.Tests/SafeSchool.Api.Tests.csproj"
API_APPSETTINGS="$ROOT_DIR/apps/api/src/SafeSchool.Api/appsettings.json"
ADMIN_PACKAGE="$ROOT_DIR/apps/admin-web/package.json"
COMPOSE_FILE="$ROOT_DIR/docker-compose.production.example.yml"
PRODUCTION_ENV_EXAMPLE="$ROOT_DIR/deploy/production.env.example"

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

log "Container deployment artifacts"
for required_file in \
  "$ROOT_DIR/.dockerignore" \
  "$ROOT_DIR/apps/api/Dockerfile" \
  "$ROOT_DIR/apps/admin-web/.dockerignore" \
  "$ROOT_DIR/apps/admin-web/Dockerfile" \
  "$COMPOSE_FILE" \
  "$PRODUCTION_ENV_EXAMPLE"; do
  [[ -f "$required_file" ]] || fail "Missing deployment artifact: $required_file"
done

if ! rg -n '"start"\s*:\s*"next start"' "$ADMIN_PACKAGE" -S >/dev/null; then
  fail "admin-web package.json must expose a production start script."
fi

for required_key in \
  'ASPNETCORE_ENVIRONMENT: Production' \
  'ConnectionStrings__SafeSchool' \
  'Jwt__Authority' \
  'Wallet__PaymentProvider__WebhookSigningSecret' \
  'SAFE_SCHOOL_API_BASE_URL' \
  'SAFE_SCHOOL_API_BEARER_TOKEN' \
  'NEXT_PUBLIC_REQUIRE_API_DATA: "1"'; do
  if ! rg -n "$required_key" "$COMPOSE_FILE" -S >/dev/null; then
    fail "docker-compose.production.example.yml is missing $required_key"
  fi
done

for required_key in \
  POSTGRES_PASSWORD \
  JWT_AUTHORITY \
  WALLET_WEBHOOK_SIGNING_SECRET \
  SAFE_SCHOOL_API_BASE_URL \
  SAFE_SCHOOL_API_BEARER_TOKEN \
  NEXT_PUBLIC_API_BASE_URL; do
  if ! rg -n "^$required_key=" "$PRODUCTION_ENV_EXAMPLE" -S >/dev/null; then
    fail "deploy/production.env.example is missing $required_key"
  fi
done

log "Production guard tests"
dotnet test "$API_TESTS" -v minimal --filter "FullyQualifiedName~RuntimeConfigurationValidatorTests|FullyQualifiedName~PaymentProviderAdapterTests|FullyQualifiedName~ApiAuthorizationBoundaryTests"

log "Admin web production data guard"
(cd "$ROOT_DIR/apps/admin-web" && npm test -- --run tests/production/apiFallbackGuard.spec.ts tests/production/dynamicRuntime.spec.ts)

log "EF migration presence"
migrations="$(dotnet ef migrations list \
  --project "$API_PROJECT" \
  --startup-project "$API_PROJECT" \
  --context SafeSchoolDbContext \
  --no-connect)"
echo "$migrations"

for required_migration in \
  InitialSafeSchoolSchema \
  AddRequestsMedicalOperationalTables \
  AddRequestsMedicalLifecycleEvidence; do
  if ! echo "$migrations" | rg "$required_migration" -S >/dev/null; then
    fail "$required_migration EF schema migration was not found."
  fi
done

log "Mobile guarded APK release controls"
(cd "$ROOT_DIR/apps/mobile" && flutter test test/features/demo/android_release_signing_test.dart)

log "Production readiness verifier completed"
echo "This verifier proves production fails closed and release controls are present. Real deployment still requires external secrets and services."
