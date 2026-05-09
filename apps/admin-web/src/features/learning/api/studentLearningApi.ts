import { learningRoutes } from "./learningApi";

export const studentLearningRoutes = {
  overview: () => learningRoutes.student(),
  content: () => `${learningRoutes.student()}/content`,
  assignments: () => `${learningRoutes.student()}/assignments`,
  quizzes: () => `${learningRoutes.student()}/quizzes`,
  rewards: () => `${learningRoutes.student()}/rewards`,
  history: () => `${learningRoutes.student()}/history`,
};
