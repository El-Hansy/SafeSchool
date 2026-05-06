import 'credential_status_cache.dart';
import 'credential_status_snapshot.dart';
import 'identity_access_api.dart';

class CredentialStatusRepository {
  const CredentialStatusRepository({
    required this.apiClient,
    required this.cache,
  });

  final IdentityAccessApiClient apiClient;
  final CredentialStatusCache cache;

  Future<List<CredentialStatusSnapshot>> refresh() async {
    final snapshots = await apiClient.fetchCredentialStatusSnapshot();
    await cache.saveSnapshots(snapshots);
    return snapshots;
  }

  Future<List<CredentialStatusSnapshot>> currentIdentityEvidence() async {
    try {
      final refreshed = await refresh();
      return refreshed.where((snapshot) => snapshot.isUsableIdentityEvidence).toList(growable: false);
    } catch (_) {
      final cached = await cache.readCurrent();
      if (cached.isNotEmpty) {
        return cached.where((snapshot) => snapshot.isUsableIdentityEvidence).toList(growable: false);
      }
      return cache.readOfflineFallback();
    }
  }
}
