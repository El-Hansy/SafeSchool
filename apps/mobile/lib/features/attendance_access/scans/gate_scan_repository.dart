import 'gate_scan_event.dart';
import 'offline_scan_queue.dart';
import 'offline_scan_sync_models.dart';

class GateScanRepository {
  GateScanRepository({required this.queue});

  final OfflineScanQueue queue;

  Future<GateScanEvent> captureOffline(GateScanEvent event) async {
    await queue.enqueue(event);
    return event;
  }

  Future<OfflineSyncResult> syncPending(String clientBatchId) async {
    final pending = await queue.pending();
    for (final entry in pending) {
      await queue.markSynced(entry.event.clientScanId);
    }
    return OfflineSyncResult(clientBatchId: clientBatchId, acceptedCount: pending.length, rejectedCount: 0);
  }
}

