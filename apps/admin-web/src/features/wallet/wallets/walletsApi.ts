import { walletRoutes } from "../api/client";

export const walletsRoutes = {
  list: (schoolAccountId: string) => walletRoutes.wallets(schoolAccountId),
  trace: (schoolAccountId: string, id: string) => `${walletRoutes.base(schoolAccountId)}/trace/${id}`,
};
