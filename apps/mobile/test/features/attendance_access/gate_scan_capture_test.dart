import 'package:flutter_test/flutter_test.dart';

import 'attendance_access_test_data.dart';
import '../../../lib/features/attendance_access/scans/gate_scan_event.dart';
import '../../../lib/features/attendance_access/scans/scan_reader_adapter.dart';

void main() {
  test('scan reader maps NFC capture to scan request metadata', () async {
    final reader = FakeScanReaderAdapter('credential-active');
    final scan = testEntryScan();

    expect(await reader.readCredentialReference(), scan.credentialReference);
    expect(scan.direction, AttendanceDirection.entry);
    expect(scan.method, ScanMethod.nfc);
  });
}

