import 'package:flutter_test/flutter_test.dart';
import 'package:safeschool_mobile/features/transport_driver/transport_driver_trip_screen.dart';

void main() {
  test('driver boarding can queue offline', () {
    expect(const TransportDriverTripAction('trip-1', 'boarding').canQueueOffline, isTrue);
  });
}
