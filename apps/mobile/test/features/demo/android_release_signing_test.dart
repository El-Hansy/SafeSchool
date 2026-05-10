import 'dart:io';

import 'package:flutter_test/flutter_test.dart';

void main() {
  test('android release build supports configured upload signing', () {
    final buildGradle = File('android/app/build.gradle.kts').readAsStringSync();
    final example = File('android/key.properties.example').readAsStringSync();

    expect(buildGradle, contains('rootProject.file("key.properties")'));
    expect(buildGradle, contains('hasReleaseKeystore'));
    expect(buildGradle, contains('signingConfigs.getByName("release")'));
    expect(buildGradle, contains('signingConfigs.getByName("debug")'));
    expect(example, contains('storeFile='));
    expect(example, contains('storePassword='));
    expect(example, contains('keyAlias='));
    expect(example, contains('keyPassword='));
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
