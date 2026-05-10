import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/mobile/mobile.dart';

void main() {
  test('sign in and role switcher use active role context', () {
    final context = MobileSignInShell().signInAs('guardian');
    expect(context.hasMobileAccess, isTrue);
    expect(RoleSwitcher(['guardian', 'teacher']).switchTo('teacher'), 'teacher');
  });
}
