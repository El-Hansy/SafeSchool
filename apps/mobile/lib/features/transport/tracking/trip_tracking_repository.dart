import '../transport_api.dart';
import 'location_update_queue.dart';
import 'transport_location_update.dart';
import 'transport_trip.dart';
class TripTrackingRepository { TripTrackingRepository({required this.api, required this.locationQueue}); final TransportApiClient api; final LocationUpdateQueue locationQueue; Future<String> startTrip(TransportTrip trip) async => api.transportPath('/trips/${trip.transportTripId}/start'); Future<String> endTrip(TransportTrip trip) async => api.transportPath('/trips/${trip.transportTripId}/end'); Future<String> submitLocation(TransportLocationUpdate update, {required bool online}) async { if (!online) { await locationQueue.enqueue(update); return 'queued'; } return api.transportPath('/trips/${update.transportTripId}/location-updates'); } }
