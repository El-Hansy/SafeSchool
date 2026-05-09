import 'wallet_pos_models.dart';

class OfflinePosPurchaseQueue {
  final Map<String, WalletPosPurchase> _pending = <String, WalletPosPurchase>{};
  final Map<String, WalletPosDecision> _decisions = <String, WalletPosDecision>{};

  Future<void> enqueue(WalletPosPurchase purchase) async {
    _pending.putIfAbsent(purchase.clientPurchaseId, () => purchase);
    _decisions.putIfAbsent(purchase.clientPurchaseId, () => WalletPosDecision.pending);
  }

  Future<List<WalletPosPurchase>> pending() async => _pending.values.where((purchase) => _decisions[purchase.clientPurchaseId] == WalletPosDecision.pending).toList(growable: false);

  Future<void> markSynced(String clientPurchaseId) async => _decisions[clientPurchaseId] = WalletPosDecision.approved;
  Future<void> markHeld(String clientPurchaseId) async => _decisions[clientPurchaseId] = WalletPosDecision.held;
  Future<void> markRejected(String clientPurchaseId) async => _decisions[clientPurchaseId] = WalletPosDecision.rejected;

  WalletPosDecision? decisionFor(String clientPurchaseId) => _decisions[clientPurchaseId];
}
