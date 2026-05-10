class CommunicationSenderAction {
  const CommunicationSenderAction(this.audience, this.message);
  final String audience;
  final String message;
  bool get canSend => audience.isNotEmpty && message.isNotEmpty;
}
