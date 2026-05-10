import { learningDemoData, learningRoutes } from "../api/learningApi";

export function listStarRewardApiDemo(schoolAccountId = learningDemoData.schoolAccountId) {
  return { route: learningRoutes.stars(schoolAccountId), items: learningDemoData.activities };
}
