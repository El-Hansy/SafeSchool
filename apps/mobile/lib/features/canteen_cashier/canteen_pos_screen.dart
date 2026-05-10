class CanteenPosAction {
  const CanteenPosAction(this.credential, this.amount);
  final String credential;
  final double amount;
  bool get canCharge => credential.isNotEmpty && amount > 0;
}
