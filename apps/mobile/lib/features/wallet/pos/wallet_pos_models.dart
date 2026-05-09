enum WalletPosDecision { pending, approved, held, rejected, duplicate }

class WalletPosPurchase {
  const WalletPosPurchase({required this.clientPurchaseId, required this.walletId, required this.credentialReference, required this.amountMinor, this.currencyCode = 'SAR', this.itemCategoryCode = 'meal', this.itemSummary = 'canteen item'});

  final String clientPurchaseId;
  final String walletId;
  final String credentialReference;
  final int amountMinor;
  final String currencyCode;
  final String itemCategoryCode;
  final String itemSummary;
}

class WalletPosSyncResponse {
  const WalletPosSyncResponse({required this.clientPurchaseId, required this.decision, required this.safeDisplayMessage});

  final String clientPurchaseId;
  final WalletPosDecision decision;
  final String safeDisplayMessage;
}

class WalletTerminalCacheEntry {
  const WalletTerminalCacheEntry({required this.terminalCode, required this.deviceReference, required this.offlineEnabled, required this.perTerminalReserveMinor});

  final String terminalCode;
  final String deviceReference;
  final bool offlineEnabled;
  final int perTerminalReserveMinor;
}
