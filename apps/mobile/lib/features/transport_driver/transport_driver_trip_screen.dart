class TransportDriverTripAction {
  const TransportDriverTripAction(this.tripId, this.action);
  final String tripId;
  final String action;
  bool get canQueueOffline => action == 'boarding' || action == 'drop' || action == 'location';
}
