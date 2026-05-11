import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';
import 'package:safeschool_mobile/core/api/mobile_api_client.dart';

void main() {
  IntegrationTestWidgetsFlutterBinding.ensureInitialized();

  test('all role workspaces are available for phase 12', () async {
    const client = MobileApiClient();
    final workspaces = client.fetchWorkspaces();

    expect(workspaces, hasLength(12));
    expect(
      workspaces.map((workspace) => workspace.roleCode),
      containsAll([
        'guardian',
        'student',
        'transport_driver',
        'gate_access',
        'canteen_cashier',
        'teacher',
        'medical_staff',
        'complaint_handler',
        'communication_sender',
        'document_administrator',
        'school_administrator',
        'platform_support',
      ]),
    );

    final guardian = await client.selectContext(
      roleCode: 'guardian',
      languageCode: 'ar',
      deviceId: 'device-integration',
    );
    expect(guardian.activeRoleCode, 'guardian');
    expect(guardian.textDirection, 'rtl');
    expect(guardian.workspace.actions, contains('Submit request'));

    final driver = await client.selectContext(
      roleCode: 'transport_driver',
      languageCode: 'en',
      deviceId: 'device-integration',
    );
    expect(driver.workspace.offlineAllowed, isTrue);
    expect(driver.workspace.actions, containsAll(['Trip', 'Boarding', 'Drop']));

    final release = await client.fetchCurrentRelease(versionCode: 1200);
    expect(release.versionName, '12.0.0');
    expect(release.updateRequired, isFalse);

    final install = await client.recordInstallEvent(
      deviceId: 'device-integration',
      releaseId: release.id,
      versionName: release.versionName,
      versionCode: release.versionCode,
    );
    expect(install.accepted, isTrue);
    expect(install.nextAction, 'continue');
  });
}
