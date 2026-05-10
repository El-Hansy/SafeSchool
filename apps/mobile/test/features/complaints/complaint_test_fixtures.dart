import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/complaints/complaints.dart';

void main() {
  test('complaint draft fixture validates required fields', () {
    const validDraft = ComplaintDraft(
      studentProfileId: 'student-amina',
      categoryCode: 'safety',
      description: 'Concern at pickup',
      clientRequestId: 'client-1',
    );
    const invalidDraft = ComplaintDraft(
      studentProfileId: '',
      categoryCode: 'safety',
      description: 'Concern at pickup',
      clientRequestId: 'client-2',
    );

    expect(validDraft.isValid, isTrue);
    expect(invalidDraft.isValid, isFalse);
  });
}
