import '../../../lib/features/transport/scans/boarding_drop_scan_event.dart';
import '../../../lib/features/transport/tracking/transport_location_update.dart';
import '../../../lib/features/transport/tracking/transport_trip.dart';
BoardingDropScanEvent testTransportScan({String clientScanId = 'scan-1'}) => BoardingDropScanEvent(clientScanId: clientScanId, transportTripId: 'trip-1', routeStopSequenceId: 'stop-1', credentialReference: 'credential-1', scanDirection: 'Boarding', localScanTime: DateTime.utc(2026, 5, 4, 6));
TransportLocationUpdate testLocation({String clientLocationId = 'loc-1', bool stale = false}) => TransportLocationUpdate(clientLocationId: clientLocationId, transportTripId: 'trip-1', reportedAt: DateTime.utc(2026, 5, 4, 6), locationReference: 'route-progress-1', stale: stale);
TransportTrip testTrip() => const TransportTrip(transportTripId: 'trip-1', trackingDeviceReference: 'device-1', status: 'Active');
