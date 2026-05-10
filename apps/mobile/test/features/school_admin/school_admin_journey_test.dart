import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/school_admin/school_admin_dashboard_screen.dart';

void main() {
  test('school admin dashboard exposes metrics', () {
    expect(const SchoolAdminDashboardMetric('roles', 12).value, 12);
  });
}
