class WalletDatabaseMigrationRegistry {
  final List<String> migrations = <String>['wallet_pos_offline_queue_v1', 'wallet_rule_snapshot_cache_v1', 'wallet_terminal_cache_v1'];

  bool get hasOfflineQueue => migrations.contains('wallet_pos_offline_queue_v1');
}
