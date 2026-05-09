enum AttendanceDirection { entry, exit }
enum ScanMethod { nfc, qr }
enum OfflineScanState { pending, synced, rejected }

class GateScanEvent {
  const GateScanEvent({
    required this.clientScanId,
    required this.gateId,
    required this.scanPointId,
    required this.credentialReference,
    required this.direction,
    required this.method,
    required this.localScanTime,
  });

  final String clientScanId;
  final String gateId;
  final String scanPointId;
  final String credentialReference;
  final AttendanceDirection direction;
  final ScanMethod method;
  final DateTime localScanTime;

  Map<String, Object?> toJson() => {
        'client_scan_id': clientScanId,
        'gate_id': gateId,
        'scan_point_id': scanPointId,
        'credential_reference': credentialReference,
        'direction': direction.name,
        'method': method.name,
        'local_scan_time': localScanTime.toUtc().toIso8601String(),
      };
}

class OfflineScanQueueEntry {
  const OfflineScanQueueEntry({required this.event, required this.state});

  final GateScanEvent event;
  final OfflineScanState state;
}

