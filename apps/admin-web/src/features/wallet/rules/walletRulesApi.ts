import { walletRoutes } from "../api/client";

export const walletRuleRoutes = {
  list: (schoolAccountId: string) => walletRoutes.ruleSettings(schoolAccountId),
  trace: (schoolAccountId: string, id: string) => `${walletRoutes.base(schoolAccountId)}/trace/${id}`,
};
