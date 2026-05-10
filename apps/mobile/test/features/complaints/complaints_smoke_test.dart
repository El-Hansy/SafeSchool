import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/complaints/complaints.dart';

void main() {
  test('complaints repository submits valid drafts and exposes tracking', () async {
    final repo = ComplaintRepository();
    const draft = ComplaintDraft(
      studentProfileId: 'student-amina',
      categoryCode: 'canteen',
      description: 'Meal issue',
      clientRequestId: 'client-1',
    );

    expect(await repo.submit(draft), startsWith('CMP-'));
    expect(await repo.tracking(), contains('CMP-2026-0002 escalated'));
  });
}
