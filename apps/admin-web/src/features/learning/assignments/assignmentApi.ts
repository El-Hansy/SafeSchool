import { learningDemoData, learningRoutes } from "../api/learningApi";

export function listAssignmentApiDemo(schoolAccountId = learningDemoData.schoolAccountId) {
  return { route: learningRoutes.assignments(schoolAccountId), items: learningDemoData.activities };
}
