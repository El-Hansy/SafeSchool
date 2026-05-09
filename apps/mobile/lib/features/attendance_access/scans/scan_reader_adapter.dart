abstract interface class ScanReaderAdapter {
  Future<String> readCredentialReference();
}

class FakeScanReaderAdapter implements ScanReaderAdapter {
  FakeScanReaderAdapter(this.credentialReference);

  final String credentialReference;

  @override
  Future<String> readCredentialReference() async => credentialReference;
}

