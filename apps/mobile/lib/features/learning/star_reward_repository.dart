import 'learning_api.dart';
import 'star_reward_models.dart';

class LearningStarRewardRepository {
  const LearningStarRewardRepository({required this.apiClient});

  final LearningApiClient apiClient;

  Future<List<LearningStarRewardSummary>> list() async => const [LearningStarRewardSummary(sourceId: "star-1", title: "Library pass", status: "reserved")];
}
