import 'dart:io';

import 'package:flutter_test/flutter_test.dart';

void main() {
  test('android release identity uses SafeSchool package names', () {
    final buildGradle = File('android/app/build.gradle.kts').readAsStringSync();
    final mainActivity = File(
      'android/app/src/main/kotlin/com/safeschool/mobile/MainActivity.kt',
    ).readAsStringSync();

    expect(buildGradle, contains('namespace = "com.safeschool.mobile"'));
    expect(buildGradle, contains('applicationId = "com.safeschool.mobile"'));
    expect(mainActivity, contains('package com.safeschool.mobile'));
    expect(buildGradle, isNot(contains('com.example')));
    expect(mainActivity, isNot(contains('com.example')));
  });

  test('android release build supports configured upload signing', () {
    final buildGradle = File('android/app/build.gradle.kts').readAsStringSync();
    final example = File('android/key.properties.example').readAsStringSync();
    final buildScript = File('tool/build_controlled_apk.sh').readAsStringSync();

    expect(buildGradle, contains('rootProject.file("key.properties")'));
    expect(buildGradle, contains('hasReleaseKeystore'));
    expect(buildGradle, contains('signingConfigs.getByName("release")'));
    expect(buildGradle, contains('signingConfigs.getByName("debug")'));
    expect(example, contains('storeFile='));
    expect(example, contains('storePassword='));
    expect(example, contains('keyAlias='));
    expect(example, contains('keyPassword='));
    expect(buildScript, contains('SAFE_SCHOOL_API_BASE_URL'));
    expect(buildScript, contains('SAFE_SCHOOL_TENANT_ID'));
    expect(buildScript, contains('--dart-define=SAFE_SCHOOL_API_BASE_URL='));
  });

  test('keystore secrets stay ignored', () {
    final androidIgnore = File('android/.gitignore').readAsStringSync();
    final rootIgnore = File('../../.gitignore').readAsStringSync();

    expect(androidIgnore, contains('key.properties'));
    expect(androidIgnore, contains('**/*.keystore'));
    expect(androidIgnore, contains('**/*.jks'));
    expect(rootIgnore, contains('*.keystore'));
    expect(rootIgnore, contains('key.properties'));
  });
}
