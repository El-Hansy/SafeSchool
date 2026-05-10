import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/api/mobile_api_client.dart';
import 'package:safeschool_mobile/features/demo/safeschool_demo_app.dart';

void main() {
  void useLargePhoneSurface(WidgetTester tester) {
    tester.view.physicalSize = const Size(1080, 1920);
    tester.view.devicePixelRatio = 1;
    addTearDown(() {
      tester.view.resetPhysicalSize();
      tester.view.resetDevicePixelRatio();
    });
  }

  testWidgets('guardian demo shows the full School NFC parent journey',
      (tester) async {
    useLargePhoneSurface(tester);
    await tester.pumpWidget(const SafeSchoolDemoApp());

    expect(find.text('Guardian live view'), findsOneWidget);
    expect(find.text('Amina Hassan'), findsOneWidget);
    expect(find.text('NFC-AMINA-001'), findsOneWidget);
    expect(find.text('Demo data'), findsOneWidget);
    expect(find.text('Live bus tracking'), findsOneWidget);

    await tester.tap(find.byKey(const ValueKey('action-Top up SAR 50')));
    await tester.pump();

    await tester.tap(find.byKey(const ValueKey('action-Submit request')));
    await tester.pump();

    await tester.tap(find.byKey(const ValueKey('action-Simulate gate NFC')));
    await tester.pump();

    expect(find.textContaining('Guardian top-up confirmed'), findsOneWidget);
    expect(find.textContaining('Guardian request submitted'), findsOneWidget);
    expect(find.textContaining('Guardian notified - Gate NFC'), findsWidgets);
  });

  testWidgets('staff demo roles perform operational NFC flows', (tester) async {
    useLargePhoneSurface(tester);
    await tester
        .pumpWidget(const SafeSchoolDemoApp(initialRoleCode: 'gate_access'));

    expect(find.text('Gate / access staff'), findsOneWidget);
    await tester.tap(find.byKey(const ValueKey('action-Scan NFC entry/exit')));
    await tester.pump();
    expect(find.textContaining('gate scan for NFC-AMINA-001'), findsOneWidget);

    await tester.pumpWidget(SafeSchoolDemoApp(
      key: UniqueKey(),
      initialRoleCode: 'canteen_cashier',
    ));

    expect(find.text('Canteen cashier'), findsWidgets);
    await tester
        .tap(find.byKey(const ValueKey('action-Scan and charge lunch')));
    await tester.pump();
    expect(find.textContaining('POS approved - Lunch meal'), findsOneWidget);

    await tester.pumpWidget(SafeSchoolDemoApp(
      key: UniqueKey(),
      initialRoleCode: 'transport_driver',
    ));

    expect(find.text('Transport driver'), findsWidgets);
    await tester
        .tap(find.byKey(const ValueKey('action-Scan student drop-off')));
    await tester.pump();
    expect(find.textContaining('bus drop-off scan for Amina'), findsOneWidget);
  });

  testWidgets('language toggle switches to Arabic and RTL content',
      (tester) async {
    useLargePhoneSurface(tester);
    await tester.pumpWidget(const SafeSchoolDemoApp());

    await tester.tap(find.byIcon(Icons.language));
    await tester.pumpAndSettle();

    expect(find.text('تطبيق المدرسة الآمن'), findsOneWidget);
    expect(find.text('عرض ولي الأمر المباشر'), findsOneWidget);
  });

  testWidgets('configured API mode bootstraps from backend mobile contracts',
      (tester) async {
    useLargePhoneSurface(tester);
    final client = MobileApiClient(
      baseUrl: 'https://school.example.com',
      schoolAccountId: 'school-demo',
      jsonGet: (uri, headers) async {
        if (uri.path.endsWith('/profile')) {
          return {
            'userId': 'guardian-demo',
            'activeTenantId': 'school-demo',
            'availableTenants': ['school-demo'],
            'availableRoles': ['guardian', 'transport_driver'],
            'linkedStudents': ['student-amina'],
            'languageCode': 'en',
            'textDirection': 'ltr',
            'mobileAccessStatus': 'active',
            'deniedReason': null,
          };
        }

        if (uri.path.endsWith('/workspaces')) {
          return [
            {
              'workspaceCode': 'guardian',
              'displayName': 'Guardian',
              'roleCode': 'guardian',
              'actions': [
                {'actionCode': 'guardian.view'}
              ],
              'offlineCapabilities': <String>[],
            },
            {
              'workspaceCode': 'transport_driver',
              'displayName': 'Transport driver',
              'roleCode': 'transport_driver',
              'actions': [
                {'actionCode': 'transport_driver.scan'}
              ],
              'offlineCapabilities': ['transport_driver.scan'],
            },
          ];
        }

        return {
          'releaseId': 'release-12',
          'versionName': '12.0.0',
          'versionCode': 1200,
          'updateRequired': false,
          'checksum': 'sha256-demo-phase12',
        };
      },
      jsonPost: (uri, headers, body) async {
        if (uri.path.endsWith('/context')) {
          return {
            'activeTenantId': 'school-demo',
            'activeRoleCode': body['roleCode'],
            'languageCode': body['languageCode'],
            'textDirection': 'ltr',
            'workspaceSummary': {
              'workspaceCode': body['roleCode'],
              'displayName': body['roleCode'] == 'guardian'
                  ? 'Guardian'
                  : 'Transport driver',
              'roleCode': body['roleCode'],
              'actions': [
                {'actionCode': '${body['roleCode']}.view'}
              ],
              'offlineCapabilities': <String>[],
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

    await tester.pumpWidget(SafeSchoolDemoApp(apiClient: client));
    await tester.pumpAndSettle();

    expect(find.text('API connected'), findsOneWidget);
    expect(find.text('Mobile session'), findsOneWidget);
    expect(
        find.text(
            'school-demo / guardian / APK 12.0.0 build 1200 / Install continue'),
        findsOneWidget);

    await tester.tap(find.byKey(const ValueKey('role-transport_driver')));
    await tester.pumpAndSettle();

    expect(find.textContaining('API role context selected'), findsOneWidget);
  });
}
