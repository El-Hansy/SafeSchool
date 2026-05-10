import 'package:flutter/material.dart';

import '../../core/api/mobile_api_client.dart';
import '../../core/localization/mobile_localizations.dart';

class WorkspaceDashboard extends StatelessWidget {
  const WorkspaceDashboard({
    super.key,
    required this.workspaces,
    required this.languageCode,
  });

  final List<MobileRoleWorkspace> workspaces;
  final String languageCode;

  @override
  Widget build(BuildContext context) {
    final strings = MobileLocalizations(languageCode);
    return Directionality(
      textDirection: strings.direction,
      child: Scaffold(
        appBar: AppBar(title: Text(strings.text('app.title'))),
        body: ListView(
          padding: const EdgeInsets.all(16),
          children: [
            for (final workspace in workspaces)
              Card(
                child: ListTile(
                  title: Text(strings.isArabic ? workspace.arabicLabel : workspace.label),
                  subtitle: Text(workspace.actions.join(' / ')),
                  trailing: Text(workspace.offlineAllowed ? 'offline' : 'online'),
                ),
              ),
          ],
        ),
      ),
    );
  }
}
