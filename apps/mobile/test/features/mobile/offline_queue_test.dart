import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/core/offline/mobile_offline_store.dart';

void main() {
  test('offline store queues approved actions and rejects online only', () {
    final store = MobileOfflineStore();
    expect(store.queue(const OfflineMobileAction(clientActionId: '1', sourceFeature: 'transport', actorId: 'driver', offlineAllowed: true)).status, 'queued');
    expect(store.queue(const OfflineMobileAction(clientActionId: '1', sourceFeature: 'transport', actorId: 'driver', offlineAllowed: true)).status, 'duplicate');
    expect(store.queue(const OfflineMobileAction(clientActionId: '2', sourceFeature: 'documents', actorId: 'admin', offlineAllowed: false)).status, 'rejected');
  });
}
