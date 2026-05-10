import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/guardian/guardian_repository.dart';
import 'package:safeschool_mobile/features/guardian/guardian_request_screen.dart';
import 'package:safeschool_mobile/features/guardian/guardian_complaint_screen.dart';

void main() {
  test('guardian sees linked students and can submit forms', () {
    final repo = GuardianRepository();
    expect(repo.linkedStudents(), contains('student-amina'));
    expect(const GuardianRequestSubmission('student-amina', 'Pickup change').isValid, isTrue);
    expect(const GuardianComplaintSubmission('student-amina', 'Bus issue').isValid, isTrue);
  });
}
