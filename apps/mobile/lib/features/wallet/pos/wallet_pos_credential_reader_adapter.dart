abstract class WalletPosCredentialReaderAdapter {
  Future<String> readCredentialReference();
}

class FakeWalletPosCredentialReaderAdapter implements WalletPosCredentialReaderAdapter {
  FakeWalletPosCredentialReaderAdapter(this.reference);

  final String reference;

  @override
  Future<String> readCredentialReference() async => reference;
}
