class TeacherWorkspaceAction {
  const TeacherWorkspaceAction(this.classId, this.action);
  final String classId;
  final String action;
  bool get requiresOnlineConfirmation => true;
}
