import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/student/student_repository.dart';

void main() {
  test('student uses self scope only', () {
    final repo = StudentRepository();
    expect(repo.studentId(), 'student-self');
    expect(repo.modules(), contains('learning'));
  });
}
