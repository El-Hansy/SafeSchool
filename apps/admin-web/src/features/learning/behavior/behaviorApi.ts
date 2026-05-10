import { learningDemoData, learningRoutes } from "../api/learningApi";

export function listBehaviorApiDemo(schoolAccountId = learningDemoData.schoolAccountId) {
  return { route: learningRoutes.behavior(schoolAccountId), items: learningDemoData.activities };
}
