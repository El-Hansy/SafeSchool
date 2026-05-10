import 'dart:io';

import 'package:flutter_test/flutter_test.dart';

void main() {
  test('controlled install script targets the SafeSchool Android package', () {
    final script = File('tool/install_demo_apk.sh').readAsStringSync();

    expect(script, contains('set -euo pipefail'));
    expect(script, contains('com.safeschool.mobile'));
    expect(script, contains('app-release.apk'));
    expect(script, contains('SAFE_SCHOOL_DEVICE_SERIAL'));
    expect(script, contains('resolve_adb'));
    expect(script, contains(r'$HOME/Library/Android/sdk/platform-tools/adb'));
    expect(script, contains(r'"$ADB_BIN" -s "$serial" install -r "$APK_PATH"'));
    expect(
        script, contains(r'"$ADB_BIN" -s "$serial" shell pm path "$APP_ID"'));
    expect(script, contains(r'monkey -p "$APP_ID"'));
  });
}
