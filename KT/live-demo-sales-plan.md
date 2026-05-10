# SafeSchool Live Demo Sales Plan

## Context

This note is for preparing a live demo from branch
`011-documents-search-admin-observability`.

The demo should be presented as a web-based product walkthrough. Do not position
it as an APK demo yet. Phase 12 will later define the production role-based
mobile APK/AAB release. For now, the strongest demo is the platform workflow:
identity, attendance, transport, wallet, and guardian visibility.

## What I Need

- Laptop with Chrome or Safari.
- Stable local development environment.
- The latest implementation branches already pushed:
  - `002-identity-access`
  - `003-attendance-campus-access`
  - `004-transport-bus-tracking`
  - `005-wallet-payments`
- Local admin web demo server.
- Optional external monitor or projector.
- Optional phone or browser responsive mode to show mobile-sized guardian views.

## What I Will Run

Use the latest implemented worktree for the live demo:

```bash
cd /Users/ahmedelhansy/Programming/SafeSchool/ProjectPlanning-005-wallet-payments/apps/admin-web
npm install
npm run dev -- -p 3006
```

Then open:

```text
http://127.0.0.1:3006
```

## Demo URLs

Use this order:

1. Identity & Access
   `http://127.0.0.1:3006/identity-access`

2. Attendance & Campus Access
   `http://127.0.0.1:3006/attendance-access`

3. Transport Command Center
   `http://127.0.0.1:3006/transport`

4. Wallet Command Center
   `http://127.0.0.1:3006/wallet`

5. Guardian Transport View
   `http://127.0.0.1:3006/guardian/transport`

6. Guardian Wallet View
   `http://127.0.0.1:3006/guardian/wallet`

## Sales Story

### 1. Identity & Access

Message:

Every student, guardian, card, QR credential, role, and permission starts here.
The platform knows who the student is, who the guardian is, and what every user
is allowed to do.

Show:

- Student profiles.
- Guardian links.
- Credentials.
- Access decisions.
- Role and permission boundaries.

### 2. Attendance & Campus Access

Message:

When a student enters or exits campus, the system records scan evidence,
generates attendance outcomes, identifies anomalies, and makes guardian-visible
entry/exit information available where permitted.

Show:

- Gate scans.
- Attendance records.
- Entry/exit notifications.
- Anomalies and review.

### 3. Transport & Bus Tracking

Message:

The same identity and credential foundation connects to bus assignment,
boarding/drop scans, live tracking, ETA, notifications, and transport review.

Show:

- Routes.
- Assignments.
- Trips.
- Boarding/drop scans.
- ETA.
- Guardian transport visibility.

### 4. Wallet & Payments

Message:

The same student identity also powers wallet balance, guardian top-ups, cashier
top-ups, canteen POS purchases, spending limits, transaction history,
reconciliation, and financial review.

Show:

- Wallet overview.
- Top-ups.
- POS purchases.
- Spending limits.
- Reconciliation.
- Safety boundaries: no payment credentials, no attendance mutation, no
  transport mutation.

### 5. Guardian Views

Message:

Parents do not get admin access. They only see linked-student information based
on guardian role and permissions.

Show:

- Guardian transport.
- Guardian wallet.
- Linked-student scope.
- Staff-only detail suppression.

## What I Should Say About Mobile

Use this wording:

The APK is not the target of this demo yet. Today we are demonstrating the
working product workflows and the role-based experiences through the web app.
The same permission model will drive the single mobile APK/AAB later, where
guardian, student, staff, gate, transport, and POS experiences are controlled by
roles and permissions.

Do not say there is a production guardian APK today.

## Best Short Demo Flow

```text
Identity -> Attendance -> Transport -> Wallet -> Guardian View
```

This tells the full product story:

```text
Student identity becomes safety, movement, payment, and parent visibility.
```

## Backup Plan

If the local server fails during the meeting:

1. Open screenshots from the browser history or previously captured images.
2. Explain the flow using the same demo order.
3. Avoid discussing internal branch names unless the audience is technical.
4. Keep the message focused on product value and role-based visibility.

## After The Demo

Recommended next actions:

- Prepare seeded demo data with realistic school names, routes, students, and
  guardian accounts.
- Add a single demo landing route that links all modules.
- Build a click-through sales script with screenshots.
- Later, define Phase 12 specs for the single role-based mobile APK/AAB release
  pipeline.
