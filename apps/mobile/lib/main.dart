import 'package:flutter/material.dart';

import 'app/mobile_theme.dart';
import 'features/mobile_shell/workspace_dashboard.dart';
import 'features/role_workspaces/role_workspace_registry.dart';

void main() {
  runApp(const SafeSchoolMobileApp());
}

class SafeSchoolMobileApp extends StatelessWidget {
  const SafeSchoolMobileApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'SafeSchool Mobile',
      theme: buildSafeSchoolTheme(TextDirection.ltr),
      home: WorkspaceDashboard(
        workspaces: MobileRoleWorkspaceRegistry.all,
        languageCode: 'en',
      ),
    );
  }
}
