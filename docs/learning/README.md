# Phase 5 Learning & Engagement

This implementation adds a demo-ready Learning feature area for courses, content delivery, assignments, quizzes, stars, rewards, behavior logging, history, exception review, configuration, status-event export, and audit evidence.

## Capability Keys

- `learning.content_delivery`
- `learning.assignments`
- `learning.quizzes`
- `learning.stars_rewards`
- `learning.rewards`
- `learning.behavior_logging`
- `learning.progress_history`
- `learning.configuration`
- `learning.review_summaries`

## Endpoint Groups

- School: `/api/v1/schools/{schoolAccountId}/learning`
- Student: `/api/v1/students/me/learning`
- Guardian: `/api/v1/guardians/me/students/{studentProfileId}/learning`

## Invariants

- Every Learning record is school-account scoped.
- Backend feature gates and permissions are required; UI gates are not authority.
- Assignment submissions, quiz attempts, behavior events, manual reviews, and star evidence preserve prior evidence.
- Star balances are derived from append-only ledger entries; snapshots are read models, not authority.
- Rewards consume engagement stars only and do not mutate wallet balances, payments, refunds, canteen purchases, or settlements.
- Behavior visibility hides staff-only and sensitive details from student and guardian surfaces.
- Phase 6 star evidence export is read-only, and Phase 9 notification eligibility writes status events without sending notifications.

## Review Notes

Run backend, web, and mobile tests before committing. Use the school web route `/learning`, guardian route `/guardian/learning`, and student route `/student/learning` for demo navigation.
