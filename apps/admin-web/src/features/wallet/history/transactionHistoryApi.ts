import { walletRoutes } from "../api/client";

export const transactionHistoryRoutes = {
  list: (schoolAccountId: string) => walletRoutes.transactions(schoolAccountId),
  trace: (schoolAccountId: string, id: string) => `${walletRoutes.base(schoolAccountId)}/trace/${id}`,
};
