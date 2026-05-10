class GuardianRequestSubmission {
  const GuardianRequestSubmission(this.studentId, this.reason);
  final String studentId;
  final String reason;
  bool get isValid => studentId.isNotEmpty && reason.isNotEmpty;
}
