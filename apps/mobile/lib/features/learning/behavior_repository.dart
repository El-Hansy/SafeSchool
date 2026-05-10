import 'learning_api.dart';
import 'behavior_models.dart';

class LearningBehaviorRepository {
  const LearningBehaviorRepository({required this.apiClient});

  final LearningApiClient apiClient;

  Future<List<LearningBehaviorSummary>> list() async => const [LearningBehaviorSummary(eventId: "behavior-1", title: "Teamwork", status: "visible")];
}
