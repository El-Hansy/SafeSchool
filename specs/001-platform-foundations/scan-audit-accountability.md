# Scan and Audit Accountability

## Scan and Audit ID Rules

| Prefix | Record Type | Example | Rule |
|--------|-------------|---------|------|
| IE- | Identity Evidence | IE-001 | Connects a subject, credential, method, or reviewer to a School Account. |
| SE- | Scan Event | SE-001 | Captures NFC or QR interaction evidence before business outcomes. |
| AE- | Audit Event | AE-001 | Records sensitive identity, access, scan, configuration, or review activity. |
| TC- | Configuration Change | TC-001 | Records tenant configuration, capability, or foundation decision changes. |

## Identity Evidence

Identity Evidence is the baseline proof later specs use before scans, fallback
credentials, or access-sensitive records can be trusted.

| Evidence Field | Required Expectation |
|----------------|----------------------|
| Subject type | Student, Guardian, Staff, Device, Card, QR Credential, or Reviewer. |
| School account | Exactly one owning School Account. |
| Evidence type | NFC, QR, Manual Review, Administrative Record, or Imported Record. |
| Status | Proposed, Verified, Rejected, or Revoked. |
| Verifier | Actor category or reviewer that verified, rejected, or revoked evidence. |
| Verification time | Timestamp showing when the evidence became trusted or untrusted. |

Identity Evidence does not provision cards, link guardians, or create identity
records. Those workflows belong to Phase 1.

## NFC Capture

NFC capture accountability requires the following evidence before later specs
attach attendance, access, transport, or payment meaning:

| Evidence | Requirement |
|----------|-------------|
| Capture source | Device, gate station, bus device, staff device, or other source declared by later spec. |
| Captured time | Time claimed by the capture source. |
| Received time | Time the platform received or could review the event. |
| Identity evidence reference | Link to the Identity Evidence that made the NFC interaction meaningful. |
| Offline indicator | Whether the capture happened while offline. |
| Review outcome | Accepted, Rejected, Needs Review, or Deferred. |

## QR Fallback

QR fallback must meet the same accountability expectations as NFC. A QR scan is
not weaker evidence by default; it must still include school account, capture
source, captured time, received time, identity evidence reference, sync status,
duplicate/conflict state, and review outcome.

Later specs may add QR-specific fraud controls, expiry rules, or presentation
requirements, but they may not remove the Phase 0 evidence baseline.

## Offline Sync

| Sync State | Meaning | Required Review Evidence |
|------------|---------|--------------------------|
| Pending | Captured but not yet reconciled by the platform. | Capture source, captured time, offline indicator, identity evidence reference. |
| Synced | Received and accepted into the reviewable event stream. | Received time and audit event. |
| Duplicate | Matched to an existing Scan Event. | Duplicate reference and reviewer or system reason. |
| Conflict | Evidence conflicts with another record or expected state. | Conflict reason and review assignment. |
| Rejected | Event cannot be trusted or is outside allowed scope. | Rejection reason and audit event. |
| Deferred | Review cannot finish yet because evidence or connectivity is incomplete. | Deferral reason and next review owner. |

## Duplicate and Conflict Review

| Review Outcome | Meaning | Later Business Outcome Permission |
|----------------|---------|-----------------------------------|
| Accepted | Evidence is sufficient for the later feature to consider its own workflow. | Later spec decides attendance, access, transport, or other product result. |
| Rejected | Evidence is not trusted for later workflow use. | Later spec must not create a success outcome from this event. |
| Needs Review | Human or configured review is required. | Later spec must hold or minimize dependent outcome until review completes. |
| Deferred | Review is postponed with recorded reason. | Later spec must not silently treat the event as accepted. |

Duplicate and conflict review preserves the original Scan Event and records a
new Audit Event rather than overwriting evidence.

## Audit Event Categories

| Category | Covered Activity | Minimum Evidence |
|----------|------------------|------------------|
| Identity | Evidence verification, rejection, or revocation. | Actor category, subject reference, school account, reason, time. |
| Access | Sensitive access attempt, approval, denial, or scope review. | Actor category, permission rule, boundary, result, reason, time. |
| Tenant Configuration | Capability or school setting change. | Configuration Change reference, requester, approver, reason, status. |
| Feature Availability | Capability enabled, disabled, suspended, restored, or retired. | Capability key, prior status, new status, effective time. |
| Scan Capture | NFC or QR event captured. | Scan Event reference, capture method, source, captured time. |
| Scan Reconciliation | Offline sync, duplicate detection, conflict review, or final review outcome. | Scan Event reference, sync status, review outcome, reason. |
| Administrative Review | Foundation decision, review assignment, or reviewer outcome. | Foundation Decision or record reference, reviewer, status, follow-up. |
