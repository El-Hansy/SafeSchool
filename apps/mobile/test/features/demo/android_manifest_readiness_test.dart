import 'dart:io';

import 'package:flutter_test/flutter_test.dart';

void main() {
  test('release manifest declares SafeSchool mobile hardware permissions', () {
    final manifest =
        File('android/app/src/main/AndroidManifest.xml').readAsStringSync();

    expect(manifest, contains('android:label="SafeSchool NFC"'));
    expect(manifest, contains('android.permission.INTERNET'));
    expect(manifest, contains('android.permission.ACCESS_NETWORK_STATE'));
    expect(manifest, contains('android.permission.NFC'));
    expect(manifest, contains('android.permission.CAMERA'));
    expect(manifest, contains('android.permission.ACCESS_FINE_LOCATION'));
    expect(manifest, contains('android.permission.ACCESS_COARSE_LOCATION'));
    expect(manifest, contains('android.permission.POST_NOTIFICATIONS'));
    expect(manifest, contains('android.permission.VIBRATE'));
  });

  test('hardware features remain optional for demo install coverage', () {
    final manifest =
        File('android/app/src/main/AndroidManifest.xml').readAsStringSync();

    expect(
        manifest, contains('android.hardware.nfc" android:required="false"'));
    expect(manifest,
        contains('android.hardware.camera" android:required="false"'));
    expect(manifest,
        contains('android.hardware.location.gps" android:required="false"'));
  });
}
