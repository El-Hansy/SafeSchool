import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/api/mobile_api_client.dart';

void main() {
  test('all role workspaces are available for phase 12', () {
    expect(MobileRoleWorkspace.demo, hasLength(12));
  });
}
