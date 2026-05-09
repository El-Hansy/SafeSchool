import 'learning_api.dart';
import 'content_models.dart';

class LearningContentRepository {
  const LearningContentRepository({required this.apiClient});

  final LearningApiClient apiClient;

  Future<List<LearningContentSummary>> list() async => const [LearningContentSummary(contentId: "content-1", title: "Solar System", status: "published")];
}
