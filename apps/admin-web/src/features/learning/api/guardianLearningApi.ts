import { learningDemoData, learningRoutes } from "./learningApi";

export const guardianLearningRoutes = {
  overview: (studentProfileId = learningDemoData.studentProfileId) => learningRoutes.guardian(studentProfileId),
  assignments: (studentProfileId = learningDemoData.studentProfileId) => `${learningRoutes.guardian(studentProfileId)}/assignments`,
  history: (studentProfileId = learningDemoData.studentProfileId) => `${learningRoutes.guardian(studentProfileId)}/history`,
};

export const guardianLearningDemoData = learningDemoData.activities.map((activity) => ({
  ...activity,
  detail: activity.detail.replace("staff-only", "restricted"),
}));
