import 'credential_status_snapshot.dart';

typedef IdentityAccessJsonTransport = Future<Object?> Function(Uri uri, Map<String, String> headers);

class IdentityAccessApiClient {
  const IdentityAccessApiClient({
    required this.baseUri,
    required this.transport,
    required this.schoolAccountId,
    this.actorReference,
  });

  final Uri baseUri;
  final IdentityAccessJsonTransport transport;
  final String schoolAccountId;
  final String? actorReference;

  Future<List<CredentialStatusSnapshot>> fetchCredentialStatusSnapshot() async {
    final uri = baseUri.replace(
      path: '/api/v1/schools/$schoolAccountId/identity/credentials/status-snapshot',
    );
    final payload = await transport(uri, {
      'x-school-account-id': schoolAccountId,
      if (actorReference != null) 'x-actor-reference': actorReference!,
    });
    final rows = payload is List ? payload : <Object?>[];
    return rows
        .whereType<Map>()
        .map((row) => CredentialStatusSnapshot.fromJson(row.cast<String, Object?>()))
        .toList(growable: false);
  }
}
