import { walletRoutes } from "../api/client";

export const topUpRoutes = {
  list: (schoolAccountId: string) => walletRoutes.topUps(schoolAccountId),
  trace: (schoolAccountId: string, id: string) => `${walletRoutes.base(schoolAccountId)}/trace/${id}`,
};
