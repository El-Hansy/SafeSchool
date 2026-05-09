import 'learning_api.dart';
import 'history_models.dart';

class LearningHistoryRepository {
  const LearningHistoryRepository({required this.apiClient});

  final LearningApiClient apiClient;

  Future<List<LearningHistorySummary>> list() async => const [LearningHistorySummary(recordId: "history-1", title: "Learning lifecycle", status: "filtered")];
}
