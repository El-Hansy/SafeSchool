class DocumentAdminAction {
  const DocumentAdminAction(this.documentId, this.action);
  final String documentId;
  final String action;
  bool get requiresAudit => true;
}
