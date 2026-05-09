import '../../../lib/features/wallet/pos/wallet_pos_models.dart';

WalletPosPurchase testWalletPurchase({String id = 'purchase-1'}) => WalletPosPurchase(clientPurchaseId: id, walletId: 'wallet-1', credentialReference: 'nfc-1', amountMinor: 1250);
