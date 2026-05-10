class MedicalStaffAction {
  const MedicalStaffAction(this.studentId, this.action);
  final String studentId;
  final String action;
  bool get canCaptureOffline => action == 'emergency';
}
