import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/medical_staff/medical_staff_workspace_screen.dart';

void main() {
  test('medical emergency can capture offline', () {
    expect(const MedicalStaffAction('student-amina', 'emergency').canCaptureOffline, isTrue);
  });
}
