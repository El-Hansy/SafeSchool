import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/api/mobile_api_client.dart';
import 'package:safeschool_mobile/features/medical/medical.dart';

void main() {
  test('mobile medical repository opens emergency access through backend',
      () async {
    final seenUris = <String>[];
    final seenHeaders = <Map<String, String>>[];
    final seenBodies = <Map<String, dynamic>>[];
    final repo = MobileMedicalRepository(
      apiClient: MobileApiClient(
        baseUrl: 'https://school.example.com',
        schoolAccountId: 'school-1',
        authToken: 'token-1',
        jsonPost: (uri, headers, body) async {
          seenUris.add(uri.toString());
          seenHeaders.add(headers);
          seenBodies.add(body);
          return {
            'medicalRecordId': 'medical-1',
            'recordReference': 'EMG-2026-0001',
            'studentProfileId': 'student-amina',
            'recordType': 'emergency-access',
            'status': 'Open',
            'severity': 'Urgent',
            'visibleSummary': 'Emergency access opened.',
            'expiresAt': '2026-05-11T10:30:00Z',
            'auditTrail': ['emergency-access-opened'],
          };
        },
      ),
    );

    final result = await repo.openEmergencyAccess(
      const MobileEmergencyAccessDraft(
        studentProfileId: 'student-amina',
        reason: 'Clinic emergency',
        actorId: 'nurse-1',
        clientRequestId: 'mobile-emg-1',
      ),
    );

    expect(result.recordReference, 'EMG-2026-0001');
    expect(result.expiresAt, DateTime.utc(2026, 5, 11, 10, 30));
    expect(
      seenUris.single,
      'https://school.example.com/api/v1/schools/school-1/medical/emergency/access',
    );
    expect(seenHeaders.single['Idempotency-Key'], 'mobile-emg-1');
    expect(seenHeaders.single['X-Actor-Permissions'], 'medical.emergency.read');
    expect(seenBodies.single['reason'], 'Clinic emergency');
  });

  test('mobile medical repository records high-severity incident route',
      () async {
    final seenUris = <String>[];
    final repo = MobileMedicalRepository(
      apiClient: MobileApiClient(
        baseUrl: 'https://school.example.com',
        schoolAccountId: 'school-1',
        jsonPost: (uri, headers, body) async {
          seenUris.add(uri.toString());
          expect(headers['X-Actor-Permissions'], 'medical.incidents.create');
          return {
            'medicalRecordId': 'incident-1',
            'recordReference': 'INC-2026-0001',
            'studentProfileId': 'student-amina',
            'recordType': 'incident',
            'status': 'Open',
            'severity': 'High',
            'visibleSummary': 'Incident logged.',
            'expiresAt': null,
            'auditTrail': [
              'incident-logged',
              'notification-eligibility-exported'
            ],
          };
        },
      ),
    );

    final result = await repo.logIncident(
      const MobileMedicalIncidentDraft(
        studentProfileId: 'student-amina',
        severity: 'High',
        observation: 'Dizzy',
        careAction: 'Guardian call',
        clientRequestId: 'mobile-inc-1',
      ),
    );

    expect(result.recordType, 'incident');
    expect(result.auditTrail, contains('notification-eligibility-exported'));
    expect(
      seenUris.single,
      'https://school.example.com/api/v1/schools/school-1/medical/incidents',
    );
  });
}
