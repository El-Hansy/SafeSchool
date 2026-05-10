class GateAccessScanAction {
  const GateAccessScanAction(this.credential, this.source);
  final String credential;
  final String source;
  bool get canQueueOffline => source == 'NFC' || source == 'QR';
}
