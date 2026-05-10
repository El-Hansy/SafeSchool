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

  testWidgets('configured API mode is visible in the demo shell',
      (tester) async {
    useLargePhoneSurface(tester);
    await tester.pumpWidget(const SafeSchoolDemoApp(
      apiClient: MobileApiClient(baseUrl: 'https://school.example.com'),
    ));

    expect(find.text('API ready'), findsOneWidget);
  });
}
