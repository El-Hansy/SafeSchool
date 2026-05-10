import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/communication_sender/communication_sender_workspace_screen.dart';

void main() {
  test('communication sender validates audience and message', () {
    expect(const CommunicationSenderAction('guardians', 'Update').canSend, isTrue);
  });
}
