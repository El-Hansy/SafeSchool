import { guardianWalletRoutes } from "../api/client";

export const guardianSummaryRoutes = {
  list: (studentProfileId: string) => guardianWalletRoutes.reviewSummary(studentProfileId),
};
