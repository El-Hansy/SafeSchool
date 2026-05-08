import 'dart:convert';

import 'gate_scan_event.dart';

class OfflineScanQueue {
  final Map<String, OfflineScanQueueEntry> _entries = {};

  Future<void> enqueue(GateScanEvent event) async {
    _entries.putIfAbsent(event.clientScanId, () => OfflineScanQueueEntry(event: event, state: OfflineScanState.pending));
  }

  Future<List<OfflineScanQueueEntry>> pending() async =>
      _entries.values.where((entry) => entry.state == OfflineScanState.pending).toList(growable: false);

  Future<void> markSynced(String clientScanId) async {
    final entry = _entries[clientScanId];
    if (entry != null) {
      _entries[clientScanId] = OfflineScanQueueEntry(event: entry.event, state: OfflineScanState.synced);
    }
  }

  Future<void> markRejected(String clientScanId) async {
    final entry = _entries[clientScanId];
    if (entry != null) {
      _entries[clientScanId] = OfflineScanQueueEntry(event: entry.event, state: OfflineScanState.rejected);
    }
  }

  String encodePayload(GateScanEvent event) => jsonEncode(event.toJson());
}

