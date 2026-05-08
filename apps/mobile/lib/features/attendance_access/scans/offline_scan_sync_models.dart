import 'gate_scan_event.dart';

class OfflineScanBatch {
  const OfflineScanBatch({required this.clientBatchId, required this.scans});

  final String clientBatchId;
  final List<GateScanEvent> scans;

  Map<String, Object?> toJson() => {
        'client_batch_id': clientBatchId,
        'scans': scans.map((scan) => scan.toJson()).toList(),
      };
}

class OfflineSyncResult {
  const OfflineSyncResult({required this.clientBatchId, required this.acceptedCount, required this.rejectedCount});

  final String clientBatchId;
  final int acceptedCount;
  final int rejectedCount;
}

