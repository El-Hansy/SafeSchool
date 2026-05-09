class WalletSummary {
  const WalletSummary({required this.walletId, required this.studentProfileId, required this.availableBalanceMinor, this.currencyCode = 'SAR', this.visibleIssueCount = 0});

  final String walletId;
  final String studentProfileId;
  final int availableBalanceMinor;
  final String currencyCode;
  final int visibleIssueCount;

  String get displayBalance => '$currencyCode ${(availableBalanceMinor / 100).toStringAsFixed(2)}';
}
