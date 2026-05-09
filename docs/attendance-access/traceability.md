# AttendanceAccess Traceability

| Requirement | Implementation Evidence |
|-------------|--------------------------|
| FR-001 gate setup | `Gate`, `ScanPoint`, `GateService` |
| FR-002 scan capture | `OfflineScanSyncService`, `ScanValidationService` |
| FR-003 campus decision | `CampusAccessDecisionService` |
| FR-004 offline sync | mobile `OfflineScanQueue`, backend idempotency |
| FR-005 attendance generation | `AttendanceGenerationService` |
| FR-006 correction | `AttendanceCorrectionService` |
| FR-007 guardian notification | `EntryExitNotificationService` |
| FR-008 anomaly workflow | `AnomalyDetectionService`, `AnomalyReviewService` |
| SC-001 scan latency | `GateScanLatencyTests`, mobile latency test |
| SC-004 generation latency | `AttendanceGenerationLatencyTests` |
| SC-005 notification latency | `EntryExitNotificationLatencyTests` |
| SC-007 trace latency | `ScanTraceLatencyTests` |

