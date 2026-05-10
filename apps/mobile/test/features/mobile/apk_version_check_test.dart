import 'dart:io';

import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/api/mobile_api_client.dart';
import 'package:safeschool_mobile/core/release/mobile_release_client.dart';
import 'package:safeschool_mobile/core/release/mobile_version_guard.dart';

void main() {
  test('version guard blocks obsolete versions', () {
    final guard =
        MobileVersionGuard(MobileReleaseClient(const MobileApiClient()));
    expect(guard.canContinue(1), isFalse);
    expect(guard.canContinue(1200), isTrue);
  });

  test('pubspec APK metadata matches the active mobile release', () {
    final pubspec = File('pubspec.yaml').readAsStringSync();
    final release =
        MobileReleaseClient(const MobileApiClient()).current(versionCode: 1200);

    expect(pubspec,
        contains('version: ${release.versionName}+${release.versionCode}'));
  });
}
