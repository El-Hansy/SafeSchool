import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/complaint_handler/complaint_handler_workspace_screen.dart';

void main() {
  test('complaint handler actions are sensitive', () {
    expect(const ComplaintHandlerAction('cmp-1', 'escalate').isSensitive, isTrue);
  });
}
