import { learningDemoData, learningRoutes } from "../api/learningApi";

export function listLearningReviewApiDemo(schoolAccountId = learningDemoData.schoolAccountId) {
  return { route: learningRoutes.history(schoolAccountId), items: learningDemoData.activities };
}
