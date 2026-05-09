class TransportApiClient {
  TransportApiClient({required this.schoolAccountId, required this.authToken});
  final String schoolAccountId;
  final String authToken;
  Map<String, String> headers({String? clientRequestId}) => {'Authorization': 'Bearer $authToken', 'X-School-Account-Id': schoolAccountId, if (clientRequestId != null) 'Idempotency-Key': clientRequestId};
  String transportPath(String path) => '/api/v1/schools/$schoolAccountId/transport$path';
}
