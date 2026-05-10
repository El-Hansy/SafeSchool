import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/communications/communications.dart';

void main() {
  test('communication fixtures expose actionable notification states', () {
    const summary = NotificationSummary('notification-1', 'Unread');

    expect(summary.reference, 'notification-1');
    expect(summary.status, 'Unread');
  });
}
