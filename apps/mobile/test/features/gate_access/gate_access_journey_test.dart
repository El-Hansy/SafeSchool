import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/gate_access/gate_access_scan_screen.dart';

void main() {
  test('gate nfc scan can queue offline', () {
    expect(const GateAccessScanAction('NFC-AMINA-001', 'NFC').canQueueOffline, isTrue);
  });
}
