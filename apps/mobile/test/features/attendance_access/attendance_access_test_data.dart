import '../../../lib/features/attendance_access/scans/gate_scan_event.dart';

GateScanEvent testEntryScan({String clientScanId = 'scan-1'}) => GateScanEvent(
      clientScanId: clientScanId,
      gateId: 'gate-1',
      scanPointId: 'scan-point-1',
      credentialReference: 'credential-active',
      direction: AttendanceDirection.entry,
      method: ScanMethod.nfc,
      localScanTime: DateTime.utc(2026, 5, 4, 7, 45),
    );

