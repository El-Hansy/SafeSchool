import 'transport_trip.dart';
class LocalTripCache { final Map<String, LocalTripCacheEntry> _cache = {}; Future<void> put(TransportTrip trip) async => _cache[trip.transportTripId] = LocalTripCacheEntry(trip, DateTime.now().toUtc()); Future<TransportTrip?> get(String tripId) async => _cache[tripId]?.trip; }
