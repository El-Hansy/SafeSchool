import 'package:flutter/widgets.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/localization/mobile_localizations.dart';

void main() {
  test('guardian and student rtl validation uses arabic strings', () {
    final strings = const MobileLocalizations('ar');
    expect(strings.direction, TextDirection.rtl);
    expect(strings.text('access.denied'), contains('الدور'));
  });
}
