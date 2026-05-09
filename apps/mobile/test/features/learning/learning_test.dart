import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/learning/learning.dart';

void main() {
  test('learning repositories expose student and guardian demo surfaces without out-of-scope side effects', () async {
    final api = LearningApiClient(baseUrl: 'http://localhost', tenantId: 'school-demo', linkedStudentId: 'student-amina');
    expect(api.schoolLearningUri('/content').path, contains('/learning/content'));
    expect(api.guardianLearningUri('student-amina', '/history').path, contains('/guardians/me/students/student-amina/learning/history'));

    final content = await LearningContentRepository(apiClient: api).list();
    final assignments = await LearningAssignmentRepository(apiClient: api).list();
    final quizzes = await LearningQuizRepository(apiClient: api).list();
    final rewards = await LearningStarRewardRepository(apiClient: api).list();
    final behavior = await LearningBehaviorRepository(apiClient: api).list();
    final history = await LearningHistoryRepository(apiClient: api).list();

    expect(content.single.status, 'published');
    expect(assignments.single.status, 'submitted');
    expect(quizzes.single.status, 'scored');
    expect(rewards.single.status, 'reserved');
    expect(behavior.single.status, 'visible');
    expect(history.single.status, 'filtered');
  });
}
