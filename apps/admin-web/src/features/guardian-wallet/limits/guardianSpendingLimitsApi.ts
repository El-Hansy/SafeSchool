import { guardianWalletRoutes } from "../api/client";

export const guardianLimitRoutes = {
  list: (studentProfileId: string) => guardianWalletRoutes.limits(studentProfileId),
};
