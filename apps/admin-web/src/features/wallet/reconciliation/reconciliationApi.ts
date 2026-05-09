import { walletRoutes } from "../api/client";

export const reconciliationRoutes = {
  list: (schoolAccountId: string) => walletRoutes.reconciliation(schoolAccountId),
  trace: (schoolAccountId: string, id: string) => `${walletRoutes.base(schoolAccountId)}/trace/${id}`,
};
