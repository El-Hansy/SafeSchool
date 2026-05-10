import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/communications/communications.dart';
void main() { test('notification_center_test', () async { final repo = CommunicationsRepository(); final rows = await repo.notifications(); expect(rows.single.status, 'Unread'); expect(await repo.acknowledge(rows.single.reference), contains('ack')); }); }
