import 'package:sqflite/sqflite.dart';

import 'credential_status_snapshot.dart';

class CredentialStatusCache {
  CredentialStatusCache({this.database});

  final Database? database;
  final Map<String, CredentialStatusSnapshot> _memory = {};

  Future<void> saveSnapshots(List<CredentialStatusSnapshot> snapshots) async {
    for (final snapshot in snapshots) {
      _memory[snapshot.identityCredentialId] = snapshot;
      final db = database;
      if (db != null) {
        await db.insert(
          'credential_status_snapshots',
          snapshot.toJson(),
          conflictAlgorithm: ConflictAlgorithm.replace,
        );
      }
    }
  }

  Future<List<CredentialStatusSnapshot>> readCurrent({DateTime? now}) async {
    final comparisonTime = (now ?? DateTime.now()).toUtc();
    return _memory.values
        .where((snapshot) => snapshot.snapshotExpiresAt.isAfter(comparisonTime))
        .toList(growable: false);
  }

  Future<List<CredentialStatusSnapshot>> readOfflineFallback() async {
    return _memory.values.toList(growable: false);
  }
}
