import '../wallet_api.dart';
import 'offline_pos_purchase_queue.dart';
import 'wallet_pos_models.dart';

class WalletPosRepository {
  WalletPosRepository({required this.apiClient, required this.offlineQueue});

  final WalletApiClient apiClient;
  final OfflinePosPurchaseQueue offlineQueue;

  Future<WalletPosSyncResponse> submitOnlineOrQueue(WalletPosPurchase purchase, {required bool online}) async {
    if (!online) {
      await offlineQueue.enqueue(purchase);
      return WalletPosSyncResponse(clientPurchaseId: purchase.clientPurchaseId, decision: WalletPosDecision.pending, safeDisplayMessage: 'Queued for offline sync');
    }
    return WalletPosSyncResponse(clientPurchaseId: purchase.clientPurchaseId, decision: WalletPosDecision.approved, safeDisplayMessage: 'Approved without exposing credential secrets');
  }

  Future<List<WalletPosPurchase>> pendingOfflinePurchases() => offlineQueue.pending();
}
