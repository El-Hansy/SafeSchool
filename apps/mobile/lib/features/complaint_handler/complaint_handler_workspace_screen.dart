class ComplaintHandlerAction {
  const ComplaintHandlerAction(this.complaintId, this.action);
  final String complaintId;
  final String action;
  bool get isSensitive => true;
}
