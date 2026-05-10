import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/teacher/teacher_workspace_screen.dart';

void main() {
  test('teacher actions require online confirmation', () {
    expect(const TeacherWorkspaceAction('class-1', 'record_behavior').requiresOnlineConfirmation, isTrue);
  });
}
