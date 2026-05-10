import 'package:flutter/material.dart';

import 'core/api/mobile_api_client.dart';
import 'features/demo/safeschool_demo_app.dart';

void main() {
  runApp(SafeSchoolMobileApp(apiClient: MobileApiClient.fromEnvironment()));
}

class SafeSchoolMobileApp extends StatelessWidget {
  const SafeSchoolMobileApp({
    required this.apiClient,
    super.key,
  });

  final MobileApiClient apiClient;

  @override
  Widget build(BuildContext context) {
    return SafeSchoolDemoApp(apiClient: apiClient);
  }
}
