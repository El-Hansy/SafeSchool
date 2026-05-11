import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/api/mobile_api_client.dart';
import 'package:safeschool_mobile/features/requests/requests.dart';

void main() {
  test('mobile requests repository submits guardian request to backend route',
      () async {
    final seenUris = <String>[];
    final seenHeaders = <Map<String, String>>[];
    final seenBodies = <Map<String, dynamic>>[];
    final repo = MobileRequestsRepository(
      apiClient: MobileApiClient(
        baseUrl: 'https://school.example.com',
        schoolAccountId: 'school-1',
        authToken: 'token-1',
        jsonPost: (uri, headers, body) async {
          seenUris.add(uri.toString());
          seenHeaders.add(headers);
          seenBodies.add(body);
          return {
            'requestId': 'request-1',
            'trackingReference': 'REQ-2026-0001',
            'requestType': 'early-leave',
            'status': 'PendingApproval',
            'priority': 'High',
            'studentProfileId': 'student-amina',
            'visibleSummary': 'Request accepted.',
            'auditTrail': ['submitted', 'approval-routing'],
          };
        },
      ),
    );

    final result = await repo.submit(
      MobileRequestDraft(
        studentProfileId: 'student-amina',
        requestType: 'early-leave',
        reason: 'Medical appointment',
        requestedOutcome: 'Release to guardian',
        clientRequestId: 'mobile-req-1',
        startsAt: DateTime.utc(2026, 5, 11, 10),
        endsAt: DateTime.utc(2026, 5, 11, 11),
      ),
    );

    expect(result.trackingReference, 'REQ-2026-0001');
    expect(result.accepted, isTrue);
    expect(
      seenUris.single,
      'https://school.example.com/api/v1/guardians/me/students/student-amina/requests',
    );
    expect(seenHeaders.single['X-School-Account-Id'], 'school-1');
    expect(seenHeaders.single['Authorization'], 'Bearer token-1');
    expect(seenHeaders.single['Idempotency-Key'], 'mobile-req-1');
    expect(
      seenHeaders.single['X-Actor-Permissions'],
      'requests.requests.create',
    );
    expect(seenBodies.single['requestType'], 'early-leave');
  });

  test('mobile requests repository exposes local demo fallback', () async {
    const repo = MobileRequestsRepository();

    final result = await repo.submit(
      const MobileRequestDraft(
        studentProfileId: 'student-amina',
        requestType: 'outing',
        reason: 'Library outing',
        requestedOutcome: 'Allow outing',
        clientRequestId: 'demo-req',
      ),
    );

    expect(result.trackingReference, startsWith('REQ-'));
    expect(result.auditTrail, contains('demo-local'));
  });
}
