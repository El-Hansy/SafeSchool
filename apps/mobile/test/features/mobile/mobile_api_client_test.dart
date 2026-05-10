import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/api/mobile_api_client.dart';

void main() {
  test('mobile API client defaults to offline demo mode', () {
    const client = MobileApiClient();

    expect(client.usesDemoData, isTrue);
    expect(client.apiUri('/api/v1/mobile/releases').toString(),
        '/api/v1/mobile/releases');
    expect(client.headers(), {'X-School-Account-Id': 'school-demo'});
  });

  test('mobile API client builds production API routes and auth headers', () {
    const client = MobileApiClient(
      baseUrl: 'https://school.example.com/',
      schoolAccountId: 'school-1',
      authToken: 'token-1',
    );

    expect(client.isConfigured, isTrue);
    expect(client.schoolUri('/wallet/student-wallets/wallet-1').toString(),
        'https://school.example.com/api/v1/schools/school-1/wallet/student-wallets/wallet-1');
    expect(client.guardianUri('/complaints').toString(),
        'https://school.example.com/api/v1/guardians/me/complaints');
    expect(client.studentUri('/documents').toString(),
        'https://school.example.com/api/v1/students/me/documents');
    expect(client.headers(idempotencyKey: 'request-1'), {
      'X-School-Account-Id': 'school-1',
      'Authorization': 'Bearer token-1',
      'Idempotency-Key': 'request-1',
    });
  });

  test('mobile API client maps backend profile and workspace contracts',
      () async {
    final seenUris = <String>[];
    final client = MobileApiClient(
      baseUrl: 'https://school.example.com',
      schoolAccountId: 'school-1',
      jsonGet: (uri, headers) async {
        seenUris.add(uri.toString());
        expect(headers['X-School-Account-Id'], 'school-1');

        if (uri.path.endsWith('/profile')) {
          return {
            'userId': 'guardian-demo',
            'activeTenantId': 'school-1',
            'availableTenants': ['school-1'],
            'availableRoles': ['guardian'],
            'linkedStudents': ['student-amina'],
            'languageCode': 'ar',
            'textDirection': 'rtl',
            'mobileAccessStatus': 'active',
            'deniedReason': null,
          };
        }

        return {
          'items': [
            {
              'workspaceCode': 'guardian',
              'displayName': 'Guardian',
              'roleCode': 'guardian',
              'actions': [
                {'actionCode': 'guardian.view'},
                {'actionCode': 'guardian.act'},
              ],
              'offlineCapabilities': <String>[],
            }
          ],
        };
      },
    );

    final profile =
        await client.fetchProfile(roleCode: 'guardian', languageCode: 'ar');
    final workspaces = await client.fetchLiveWorkspaces(
        roleCode: 'guardian', languageCode: 'ar');

    expect(profile.linkedStudents, ['student-amina']);
    expect(profile.textDirection, 'rtl');
    expect(workspaces.single.roleCode, 'guardian');
    expect(workspaces.single.actions, ['guardian.view', 'guardian.act']);
    expect(
        seenUris,
        contains(
            'https://school.example.com/api/v1/mobile/profile?tenantId=school-1&roleCode=guardian&languageCode=ar'));
    expect(
        seenUris,
        contains(
            'https://school.example.com/api/v1/mobile/workspaces?tenantId=school-1&roleCode=guardian&languageCode=ar'));
  });

  test('mobile API client accepts direct workspace arrays from backend',
      () async {
    final client = MobileApiClient(
      baseUrl: 'https://school.example.com',
      schoolAccountId: 'school-1',
      jsonGet: (uri, headers) async => [
        {
          'workspaceCode': 'guardian',
          'displayName': 'Guardian',
          'roleCode': 'guardian',
          'actions': [
            {'actionCode': 'wallet.topup'},
          ],
          'offlineCapabilities': <String>[],
        }
      ],
    );

    final workspaces = await client.fetchLiveWorkspaces(roleCode: 'guardian');

    expect(workspaces, hasLength(1));
    expect(workspaces.single.roleCode, 'guardian');
    expect(workspaces.single.actions, ['wallet.topup']);
  });

  test('mobile API client maps context, release, and install event contracts',
      () async {
    final postBodies = <Map<String, dynamic>>[];
    final client = MobileApiClient(
      baseUrl: 'https://school.example.com',
      schoolAccountId: 'school-1',
      jsonGet: (uri, headers) async => {
        'releaseId': 'release-12',
        'versionName': '12.0.0',
        'versionCode': 1200,
        'updateRequired': false,
        'checksum': 'sha256-demo-phase12',
      },
      jsonPost: (uri, headers, body) async {
        postBodies.add(body);

        if (uri.path.endsWith('/context')) {
          return {
            'activeTenantId': 'school-1',
            'activeRoleCode': 'transport_driver',
            'languageCode': 'en',
            'textDirection': 'ltr',
            'workspaceSummary': {
              'workspaceCode': 'transport_driver',
              'displayName': 'Transport driver',
              'roleCode': 'transport_driver',
              'actions': [
                {'actionCode': 'transport_driver.act'}
              ],
              'offlineCapabilities': ['transport_driver.act'],
            },
          };
        }

        return {
          'eventId': 'event-1',
          'accepted': true,
          'nextAction': 'continue',
          'userMessage': 'Version accepted.',
        };
      },
    );

    final context = await client.selectContext(
      roleCode: 'transport_driver',
      languageCode: 'en',
      deviceId: 'device-1',
    );
    final release = await client.fetchCurrentRelease(
      roleCode: 'transport_driver',
      deviceId: 'device-1',
    );
    final installEvent = await client.recordInstallEvent(
      deviceId: 'device-1',
      releaseId: release.id,
      versionName: release.versionName,
      versionCode: release.versionCode,
    );

    expect(context.workspace.offlineAllowed, isTrue);
    expect(release.versionCode, 1200);
    expect(installEvent.nextAction, 'continue');
    expect(postBodies.first['roleCode'], 'transport_driver');
    expect(postBodies.last['releaseId'], 'release-12');
  });
}
