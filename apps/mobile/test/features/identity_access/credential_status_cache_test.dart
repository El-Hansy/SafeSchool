import 'package:flutter_test/flutter_test.dart';

import '../../../lib/features/identity_access/credential_status_cache.dart';
import '../../../lib/features/identity_access/credential_status_snapshot.dart';

void main() {
  test('cache returns only unexpired snapshots for current reads', () async {
    final cache = CredentialStatusCache();
    final now = DateTime.now().toUtc();

    await cache.saveSnapshots([
      CredentialStatusSnapshot(
        credentialStatusSnapshotId: 'snapshot-active',
        schoolAccountId: 'school-1',
        studentProfileId: 'student-1',
        identityCredentialId: 'credential-active',
        credentialType: 'NfcCard',
        credentialStatus: 'Active',
        validFrom: now.subtract(const Duration(days: 1)),
        snapshotGeneratedAt: now,
        snapshotExpiresAt: now.add(const Duration(hours: 1)),
      ),
      CredentialStatusSnapshot(
        credentialStatusSnapshotId: 'snapshot-expired',
        schoolAccountId: 'school-1',
        studentProfileId: 'student-1',
        identityCredentialId: 'credential-expired',
        credentialType: 'QrFallback',
        credentialStatus: 'Active',
        validFrom: now.subtract(const Duration(days: 1)),
        snapshotGeneratedAt: now.subtract(const Duration(hours: 2)),
        snapshotExpiresAt: now.subtract(const Duration(hours: 1)),
      ),
    ]);

    final current = await cache.readCurrent(now: now);

    expect(current.map((snapshot) => snapshot.identityCredentialId), ['credential-active']);
  });
}
