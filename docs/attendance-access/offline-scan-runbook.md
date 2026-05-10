# Offline Scan Runbook

1. Confirm the gate and scan point are active and allow offline scanning.
2. Capture NFC or QR evidence with client scan id, direction, source metadata,
   and local scan time.
3. Queue failed submissions locally and retry by client batch id.
4. Review delayed scans using both local scan time and server received time.
5. Investigate duplicate client scan ids before attendance generation.
6. Escalate persistent sync failures with gate, device, actor, and batch
   references.

