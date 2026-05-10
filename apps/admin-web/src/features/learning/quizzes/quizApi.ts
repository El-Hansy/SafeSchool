import { learningDemoData, learningRoutes } from "../api/learningApi";

export function listQuizApiDemo(schoolAccountId = learningDemoData.schoolAccountId) {
  return { route: learningRoutes.quizzes(schoolAccountId), items: learningDemoData.activities };
}
