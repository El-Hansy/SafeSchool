# Contract Test: Mobile Profile Context

- `GET /api/v1/mobile/profile` returns active tenant, available roles, linked
  students, language, and denied reason when blocked.
- `POST /api/v1/mobile/context` rejects tenant, role, language, and device
  states that are not allowed.
