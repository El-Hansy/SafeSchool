import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/communications/communications.dart';

void main() {
  test('notification center exposes unread guardian notifications', () async {
    final repo = CommunicationsRepository();
    final rows = await repo.notifications();

    expect(rows.single.status, 'Unread');
    expect(rows.single.reference, startsWith('notification-'));
  });
}
