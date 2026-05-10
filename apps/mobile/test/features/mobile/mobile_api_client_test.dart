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
}
