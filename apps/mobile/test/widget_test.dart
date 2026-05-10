import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/api/mobile_api_client.dart';

void main() {
  test('phase 12 mobile app exposes role workspaces', () {
    expect(const MobileApiClient().fetchWorkspaces(), hasLength(12));
  });
}
