import { walletRoutes } from "../api/client";

export const spendingLimitRoutes = {
  list: (schoolAccountId: string) => walletRoutes.limits(schoolAccountId),
  trace: (schoolAccountId: string, id: string) => `${walletRoutes.base(schoolAccountId)}/trace/${id}`,
};
