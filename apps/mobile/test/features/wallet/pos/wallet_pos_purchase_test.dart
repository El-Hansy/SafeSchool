import 'package:flutter_test/flutter_test.dart';
import '../wallet_test_data.dart';
import '../../../../lib/features/wallet/wallet_api.dart';
import '../../../../lib/features/wallet/pos/offline_pos_purchase_queue.dart';
import '../../../../lib/features/wallet/pos/wallet_pos_models.dart';
import '../../../../lib/features/wallet/pos/wallet_pos_repository.dart';

void main() {
  test('wallet POS repository returns safe approval online and queues offline', () async {
    final repository = WalletPosRepository(apiClient: WalletApiClient(baseUrl: 'http://localhost', tenantId: 'school-1'), offlineQueue: OfflinePosPurchaseQueue());
    final online = await repository.submitOnlineOrQueue(testWalletPurchase(), online: true);
    expect(online.decision, WalletPosDecision.approved);
    final offline = await repository.submitOnlineOrQueue(testWalletPurchase(id: 'purchase-2'), online: false);
    expect(offline.decision, WalletPosDecision.pending);
    expect(await repository.pendingOfflinePurchases(), hasLength(1));
  });
}
