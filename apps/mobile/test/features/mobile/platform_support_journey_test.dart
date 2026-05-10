import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/platform_support/platform_support_workspace_screen.dart';

void main() {
  test('platform support diagnostics include correlation context', () {
    expect(const PlatformSupportDiagnostic('corr-1', 'blocked').correlationId, 'corr-1');
  });
}
