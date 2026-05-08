import 'boarding_drop_scan_event.dart';
class OfflineTransportScanSyncBatch { const OfflineTransportScanSyncBatch({required this.clientBatchId, required this.scans}); final String clientBatchId; final List<BoardingDropScanEvent> scans; }
class OfflineTransportScanSyncResponse { const OfflineTransportScanSyncResponse({required this.clientBatchId, required this.acceptedCount, required this.needsReviewCount}); final String clientBatchId; final int acceptedCount; final int needsReviewCount; }
