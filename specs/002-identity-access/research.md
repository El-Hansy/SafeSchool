# Phase 1 Research: Identity & Access

## Decision: Implement Phase 1 as runtime product behavior

**Rationale**: The Phase 1 spec covers student profile management, guardian
linking, credential provisioning, QR fallback, RBAC, and permission enforcement.
These are user-facing and security-sensitive capabilities, unlike Phase 0,
which was a foundation planning package.

**Alternatives considered**:
- Treat Phase 1 as another documentation-only package: rejected because the
  spec defines operational workflows and measurable task outcomes.
- Combine Phase 1 with attendance or transport behavior: rejected because the
  spec explicitly excludes scan outcomes until later phases.

## Decision: Use the constitution runtime baseline without adding new platforms

**Rationale**: The constitution already defines the technology direction:
Next.js/React/TypeScript for web, ASP.NET Core Web API for backend, PostgreSQL
for storage, and Flutter/Dart for mobile when native NFC or QR support is
needed. No Phase 1 requirement justifies microservices, extra storage, or a
different stack.

**Alternatives considered**:
- Split identity into a separate identity microservice: rejected because the
  modular monolith is the default and Phase 1 has no measured scale pressure.
- Use a separate credential database: rejected because tenant-owned identity
  and credential records fit the single PostgreSQL baseline.

## Decision: Organize implementation around one IdentityAccess feature area

**Rationale**: Student profiles, guardians, credentials, roles, permissions, and
access decisions must share tenant resolution, audit evidence, and permission
rules. A single feature area with internal modules keeps these boundaries
cohesive while avoiding one oversized service.

**Alternatives considered**:
- Separate each module into unrelated feature folders: rejected because
  credential and guardian workflows depend directly on student identity and
  shared authorization decisions.
- Put all behavior in one general identity service: rejected because it would
  combine unrelated responsibilities and violate the constitution's modularity
  guidance.

## Decision: Treat school account as the tenant boundary for every Phase 1 record

**Rationale**: Phase 0 established School Account as the default tenant
boundary. Phase 1 records must include tenant ownership and prevent
cross-school visibility or action unless a platform-level review role
explicitly permits it.

**Alternatives considered**:
- Scope identity records by campus only: rejected because campuses are internal
  subdivisions and cannot replace the parent school account boundary.
- Use globally visible identities by default: rejected because that would create
  cross-school data exposure risk.

## Decision: Enforce feature availability with explicit capability keys

**Rationale**: The constitution requires feature flag enforcement. Phase 1 must
define capability keys so school accounts can enable, disable, suspend, and
review each workflow independently.

**Capability keys**:
- `identity.student_profiles`
- `identity.guardian_linking`
- `identity.nfc_credentials`
- `identity.qr_fallback`
- `identity.role_administration`
- `identity.permission_enforcement`

**Alternatives considered**:
- One `identity_access` flag for all workflows: rejected because schools may
  enable student profiles before QR fallback or staff role administration.
- UI-only feature gates: rejected because the constitution requires backend
  enforcement before business logic executes.

## Decision: Use explicit roles plus fine-grained permissions

**Rationale**: Phase 0 actor categories provide shared vocabulary, but Phase 1
must enforce concrete actions. Roles group common responsibilities, while
permissions define exact action and read scopes. Access decisions are evaluated
within one school account, with platform-level review scope only when granted.

**Alternatives considered**:
- Hard-code permissions by actor category: rejected because schools need
  configurable staff responsibilities and reviewable permission changes.
- Allow roles without permission records: rejected because sensitive actions
  need testable and auditable permission rules.

## Decision: Detect duplicates using active school identity identifiers plus review

**Rationale**: The spec requires duplicate active student identities to be
detected before activation. Configured identifiers, such as school student
number or approved external references, provide deterministic checks. Ambiguous
matches are routed to review rather than merged automatically.

**Alternatives considered**:
- Match only by name and date of birth: rejected because it can produce false
  matches and does not satisfy configured school identifier requirements.
- Automatically merge possible duplicates: rejected because identity merges are
  sensitive and require human review evidence.

## Decision: Model guardian access through Guardian Link state and access scope

**Rationale**: A guardian record alone must not grant access. Visibility is
derived from an approved active Guardian Link to a student profile, the link's
relationship type, the link's access scope, tenant feature availability, and
the guardian's current role or permission.

**Alternatives considered**:
- Give all guardians the same access once linked: rejected because schools need
  relationship-specific visibility.
- Delete disputed links immediately: rejected because review history must
  remain available and access must be suspended while disputes are resolved.

## Decision: Use credential lifecycle state for NFC cards and QR fallback

**Rationale**: Credentials must remain reviewable and must not be valid after
suspension, expiry, replacement, or revocation. Both NFC and QR credentials
share a lifecycle vocabulary, while QR also requires validity windows and
rotation evidence.

**Alternatives considered**:
- Store only a current active credential per student: rejected because lost,
  replaced, and revoked credential history is required for audit review.
- Treat QR fallback as a temporary display value without lifecycle state:
  rejected because QR fallback is sensitive identity evidence and must be
  revocable and auditable.

## Decision: Make credential lifecycle commands retry-safe

**Rationale**: Card issue, replacement, revocation, and QR rotation may be
retried after network or device interruptions. Lifecycle requests must include
enough client request identity or business uniqueness to avoid duplicate active
credentials and duplicate audit events.

**Alternatives considered**:
- Allow repeated lifecycle commands to create repeated records: rejected because
  duplicate cards or QR credentials would violate identity integrity.
- Handle duplicate prevention only through manual review: rejected because
  common retries should resolve deterministically.

## Decision: Provide offline credential evidence without defining scan outcomes

**Rationale**: Later NFC and QR scan flows depend on credential status,
ownership, validity, and revocation evidence. Phase 1 should provide a
credential status snapshot that later mobile scan flows can cache, but it must
not decide attendance, campus access, transport, wallet, or notification
effects.

**Alternatives considered**:
- Build full offline scan queues now: rejected because scan capture and sync
  outcomes belong to later attendance and transport phases.
- Ignore offline needs until later phases: rejected because credential validity
  and revocation evidence must be available for later offline scan planning.

## Decision: Emit audit events for every sensitive identity and access change

**Rationale**: The platform handles student identity and access control. Profile
changes, guardian link lifecycle changes, credential lifecycle changes, role
and permission changes, feature-gated denials, and access denials must produce
reviewable audit evidence.

**Alternatives considered**:
- Log only errors or unusual cases: rejected because normal successful changes
  are also security-relevant.
- Rely only on modified timestamps: rejected because reviewers need actor,
  action, reason, target, and decision context.

## Decision: Use layered validation and testing by story

**Rationale**: Each user story can be implemented and tested independently:
student profiles, permission enforcement, guardian linking, and credentials.
Each story still includes shared tenant, feature, authorization, audit, and
contract validation where it touches those concerns.

**Alternatives considered**:
- Test only final end-to-end workflows: rejected because security regressions
  need lower-level unit, integration, authorization, and contract coverage.
- Delay authorization tests until after all features are built: rejected because
  permission enforcement is a P1 user story and protects every workflow.

## Decision: No unresolved technical clarifications remain

**Rationale**: The Phase 1 spec and constitution define enough scope to plan
implementation. Runtime package versions and exact device integrations are
implementation details to pin when code manifests and device adapters are
created.

**Alternatives considered**:
- Add clarification markers for package versions: rejected because the
  constitution defines "latest supported" and version pinning belongs to
  runtime manifests.
- Ask for specific NFC hardware now: rejected because the Phase 1 contract can
  define credential lifecycle behavior without choosing a specific device.
