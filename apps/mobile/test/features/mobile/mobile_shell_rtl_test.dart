import 'package:flutter/widgets.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/localization/mobile_localizations.dart';

void main() {
  test('rtl shell remains available for arabic', () {
    final strings = const MobileLocalizations('ar');
    expect(strings.text('app.title'), isNotEmpty);
    expect(strings.direction, TextDirection.rtl);
  });
}
