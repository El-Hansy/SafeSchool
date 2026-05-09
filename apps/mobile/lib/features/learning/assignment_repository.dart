import 'learning_api.dart';
import 'assignment_models.dart';

class LearningAssignmentRepository {
  const LearningAssignmentRepository({required this.apiClient});

  final LearningApiClient apiClient;

  Future<List<LearningAssignmentSummary>> list() async => const [LearningAssignmentSummary(assignmentId: "assignment-1", title: "Planet worksheet", status: "submitted")];
}
