import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/communications/communications.dart';

void main() {
  test('communications repository lists and acknowledges notifications', () async {
    final repo = CommunicationsRepository();
    final rows = await repo.notifications();

    expect(rows, hasLength(1));
    expect(rows.single.reference, 'notification-1');
    expect(await repo.acknowledge(rows.single.reference), 'ack:notification-1');
  });
}
