abstract class TransportScanReaderAdapter { Future<String> readCredentialReference(); }
class FakeTransportScanReaderAdapter implements TransportScanReaderAdapter { FakeTransportScanReaderAdapter(this.credentialReference); final String credentialReference; @override Future<String> readCredentialReference() async => credentialReference; }
