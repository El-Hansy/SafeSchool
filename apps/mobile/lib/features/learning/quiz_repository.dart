import 'learning_api.dart';
import 'quiz_models.dart';

class LearningQuizRepository {
  const LearningQuizRepository({required this.apiClient});

  final LearningApiClient apiClient;

  Future<List<LearningQuizSummary>> list() async => const [LearningQuizSummary(quizId: "quiz-1", title: "Planet quiz", status: "scored")];
}
