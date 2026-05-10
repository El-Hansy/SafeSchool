import { walletRoutes } from "../api/client";

export const posRoutes = {
  list: (schoolAccountId: string) => walletRoutes.posPurchases(schoolAccountId),
  trace: (schoolAccountId: string, id: string) => `${walletRoutes.base(schoolAccountId)}/trace/${id}`,
};
