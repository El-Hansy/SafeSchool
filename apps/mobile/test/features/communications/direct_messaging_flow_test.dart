import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/communications/communications.dart';

void main() {
  test('direct message acknowledgement preserves the message reference', () async {
    final repo = CommunicationsRepository();
    final row = (await repo.notifications()).single;

    expect(await repo.acknowledge(row.reference), contains(row.reference));
  });
}
