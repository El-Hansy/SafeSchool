import 'package:flutter/material.dart';

import 'credential_status_repository.dart';
import 'credential_status_snapshot.dart';

class CredentialStatusScreen extends StatelessWidget {
  const CredentialStatusScreen({required this.repository, super.key});

  final CredentialStatusRepository repository;

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<CredentialStatusSnapshot>>(
      future: repository.currentIdentityEvidence(),
      builder: (context, snapshot) {
        final rows = snapshot.data ?? const <CredentialStatusSnapshot>[];
        return Scaffold(
          appBar: AppBar(title: const Text('Identity credentials')),
          body: ListView.separated(
            itemCount: rows.length,
            separatorBuilder: (_, __) => const Divider(height: 1),
            itemBuilder: (context, index) {
              final row = rows[index];
              return ListTile(
                title: Text(row.credentialType),
                subtitle: Text('${row.credentialStatus} until ${row.snapshotExpiresAt.toLocal()}'),
                trailing: row.isUsableIdentityEvidence ? const Icon(Icons.verified_user) : const Icon(Icons.block),
              );
            },
          ),
        );
      },
    );
  }
}
