import '../transport_api.dart';
import 'boarding_drop_scan_event.dart';
import 'offline_transport_scan_queue.dart';
class BoardingDropScanRepository { BoardingDropScanRepository({required this.api, required this.queue}); final TransportApiClient api; final OfflineTransportScanQueue queue; Future<String> submitOrQueue(BoardingDropScanEvent event, {required bool online}) async { if (!online) { await queue.enqueue(event); return 'queued'; } return api.transportPath('/scan-events'); } }
