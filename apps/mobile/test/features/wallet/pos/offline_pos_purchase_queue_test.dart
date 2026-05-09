import 'package:flutter_test/flutter_test.dart';
import '../wallet_test_data.dart';
import '../../../../lib/features/wallet/pos/offline_pos_purchase_queue.dart';
import '../../../../lib/features/wallet/pos/wallet_pos_models.dart';

void main() {
  test('offline POS queue deduplicates client purchase id and tracks decisions', () async {
    final queue = OfflinePosPurchaseQueue();
    final purchase = testWalletPurchase();
    await queue.enqueue(purchase);
    await queue.enqueue(purchase);
    expect(await queue.pending(), hasLength(1));
    await queue.markHeld(purchase.clientPurchaseId);
    expect(queue.decisionFor(purchase.clientPurchaseId), WalletPosDecision.held);
  });
}
