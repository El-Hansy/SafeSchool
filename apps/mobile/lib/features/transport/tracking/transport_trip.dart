class TransportTrip { const TransportTrip({required this.transportTripId, required this.trackingDeviceReference, required this.status}); final String transportTripId; final String trackingDeviceReference; final String status; }
class LocalTripCacheEntry { const LocalTripCacheEntry(this.trip, this.cachedAt); final TransportTrip trip; final DateTime cachedAt; }
