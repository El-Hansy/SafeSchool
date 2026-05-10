import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/mobile/mobile.dart';

void main() {
  test('mobile shell has production role coverage', () {
    expect(MobileRoleWorkspaceRegistry.all, hasLength(12));
  });
}
