class GuardianComplaintSubmission {
  const GuardianComplaintSubmission(this.studentId, this.description);
  final String studentId;
  final String description;
  bool get isValid => studentId.isNotEmpty && description.length >= 3;
}
