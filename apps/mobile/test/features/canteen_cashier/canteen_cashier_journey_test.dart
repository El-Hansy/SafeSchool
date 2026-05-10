import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/canteen_cashier/canteen_pos_screen.dart';

void main() {
  test('cashier can charge valid wallet purchase', () {
    expect(const CanteenPosAction('NFC-AMINA-001', 8.5).canCharge, isTrue);
  });
}
