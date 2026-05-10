import { learningDemoData, learningRoutes } from "../api/learningApi";

export function listCourseContentApiDemo(schoolAccountId = learningDemoData.schoolAccountId) {
  return { route: learningRoutes.content(schoolAccountId), items: learningDemoData.activities };
}
