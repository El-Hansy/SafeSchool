import 'package:flutter_test/flutter_test.dart';

import 'attendance_access_test_data.dart';
import '../../../lib/features/attendance_access/scans/offline_scan_queue.dart';

void main() {
  test('queue preserves local time and deduplicates client scan id', () async {
    final queue = OfflineScanQueue();
    final scan = testEntryScan();

    await queue.enqueue(scan);
    await queue.enqueue(scan);
    final pending = await queue.pending();

    expect(pending, hasLength(1));
    expect(pending.single.event.localScanTime, scan.localScanTime);
  });
}

