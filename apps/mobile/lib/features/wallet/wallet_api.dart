class WalletApiClient {
  WalletApiClient({required this.baseUrl, required this.tenantId});

  final String baseUrl;
  final String tenantId;

  Map<String, String> headers(String token) => {
        'authorization': 'Bearer $token',
        'x-school-account-id': tenantId,
        'content-type': 'application/json',
      };

  String walletPath(String walletId) => '/api/v1/schools/$tenantId/wallet/student-wallets/$walletId';
  String posPurchasePath() => '/api/v1/schools/$tenantId/wallet/canteen/purchases';
  String offlineSyncPath() => '/api/v1/schools/$tenantId/wallet/canteen/purchases/sync';
}
