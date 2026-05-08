import 'package:flutter_test/flutter_test.dart';

import '../../../lib/features/identity_access/credential_status_snapshot.dart';

void main() {
  test('active unexpired credential is usable identity evidence', () {
    final snapshot = CredentialStatusSnapshot(
      credentialStatusSnapshotId: 'snapshot-1',
      schoolAccountId: 'school-1',
      studentProfileId: 'student-1',
      identityCredentialId: 'credential-1',
      credentialType: 'NfcCard',
      credentialStatus: 'Active',
      validFrom: DateTime.now().toUtc().subtract(const Duration(days: 1)),
      snapshotGeneratedAt: DateTime.now().toUtc(),
      snapshotExpiresAt: DateTime.now().toUtc().add(const Duration(hours: 1)),
    );

    expect(snapshot.isUsableIdentityEvidence, isTrue);
  });

  test('json parser accepts tenantId contract field', () {
    final now = DateTime.utc(2026, 5, 6);

    final snapshot = CredentialStatusSnapshot.fromJson({
      'credentialStatusSnapshotId': 'snapshot-1',
      'tenantId': 'school-1',
      'studentProfileId': 'student-1',
      'identityCredentialId': 'credential-1',
      'credentialType': 'QrFallback',
      'credentialStatus': 'Active',
      'validFrom': now.toIso8601String(),
      'snapshotGeneratedAt': now.toIso8601String(),
      'snapshotExpiresAt': now.add(const Duration(hours: 1)).toIso8601String(),
    });

    expect(snapshot.schoolAccountId, 'school-1');
  });
}
