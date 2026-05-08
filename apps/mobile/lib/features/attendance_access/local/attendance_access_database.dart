import 'package:sqflite/sqflite.dart';

class AttendanceAccessDatabase {
  AttendanceAccessDatabase({this.database});

  final Database? database;

  static const offlineScanQueueTable = 'attendance_access_offline_scan_queue';

  Future<void> migrate(Database db) async {
    await db.execute('''
      CREATE TABLE IF NOT EXISTS $offlineScanQueueTable (
        client_scan_id TEXT PRIMARY KEY,
        payload TEXT NOT NULL,
        local_scan_time TEXT NOT NULL,
        sync_state TEXT NOT NULL
      )
    ''');
  }
}

