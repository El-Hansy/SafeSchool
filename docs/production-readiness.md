# SafeSchool Production Readiness

Use this checklist before any pilot or production deployment. The local demo
APK and demo web flows are useful for sales, but production must use real
configuration supplied by the deployment environment.

## Required Runtime Configuration

- `ASPNETCORE_ENVIRONMENT=Production`
- `ConnectionStrings__SafeSchool` pointing to managed PostgreSQL
- `Jwt__Authority` pointing to a real HTTPS identity provider
- `Jwt__Audience=safeschool-api`
- `Demo__AllowAnonymousApi=false`
- `Wallet__PaymentProvider__Adapter=ConfiguredProvider`
- `Wallet__PaymentProvider__RequireWebhookSignature=true`
- `Wallet__PaymentProvider__WebhookSigningSecret` from the payment provider
- `SAFE_SCHOOL_API_BASE_URL` for server-side admin web API calls
- `SAFE_SCHOOL_API_BEARER_TOKEN` supplied by the configured identity provider for server-side admin web API calls and same-origin write proxying
- `NEXT_PUBLIC_REQUIRE_API_DATA=1` on the admin web deployment
- `NEXT_PUBLIC_API_BASE_URL` pointing browsers to the deployed API

The API fails startup outside Development if these values are unsafe or
missing. The admin web fails closed when `NEXT_PUBLIC_REQUIRE_API_DATA=1`
instead of showing fallback demo data while the API is unavailable.

## Required Mobile Release Controls

For pilot or production APK builds:

```bash
cd apps/mobile
SAFE_SCHOOL_REQUIRE_RELEASE_SIGNING=1 \
SAFE_SCHOOL_REQUIRE_API_BASE_URL=1 \
SAFE_SCHOOL_API_BASE_URL=https://api.example.school \
SAFE_SCHOOL_TENANT_ID=school-demo \
./tool/build_controlled_apk.sh
```

`SAFE_SCHOOL_AUTH_TOKEN` must not be set for signed APKs.

## Verification

```bash
./scripts/verify_production_readiness.sh
./scripts/verify_demo_readiness.sh
```

The production verifier checks fail-closed configuration, signed payment
confirmation guards, EF migration availability, and guarded APK release
controls. The demo verifier checks the runnable sales demo, web build, tests,
Flutter tests, APK artifact, and Android device readiness.

## Container Deployment Template

The repository includes a production packaging baseline:

- `apps/api/Dockerfile`
- `apps/admin-web/Dockerfile`
- `docker-compose.production.example.yml`
- `deploy/production.env.example`

Use the compose file as a local pilot template or as source material for a
managed container platform. Replace every value in
`deploy/production.env.example` through the platform's secret manager before
connecting a real school tenant.

For container networks, keep `SAFE_SCHOOL_API_BASE_URL` on the internal API
service URL and `NEXT_PUBLIC_API_BASE_URL` on the browser-reachable API URL.
Do not expose `SAFE_SCHOOL_API_BEARER_TOKEN` through `NEXT_PUBLIC_*`; mount it
as a server-side secret and rotate it through the identity provider.
