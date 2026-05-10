import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/api/mobile_api_client.dart';
import 'package:safeschool_mobile/core/release/mobile_release_client.dart';
import 'package:safeschool_mobile/core/release/mobile_version_guard.dart';

void main() {
  test('version guard blocks obsolete versions', () {
    final guard = MobileVersionGuard(MobileReleaseClient(const MobileApiClient()));
    expect(guard.canContinue(1), isFalse);
    expect(guard.canContinue(1200), isTrue);
  });
}
