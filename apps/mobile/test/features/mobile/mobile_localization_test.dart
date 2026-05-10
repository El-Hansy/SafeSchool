import 'package:flutter/widgets.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/localization/mobile_localizations.dart';

void main() {
  test('arabic uses rtl and english uses ltr', () {
    expect(const MobileLocalizations('ar').direction, TextDirection.rtl);
    expect(const MobileLocalizations('en').direction, TextDirection.ltr);
  });
}
