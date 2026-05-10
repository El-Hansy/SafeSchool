import { guardianWalletRoutes } from "../api/client";

export const guardianTransactionRoutes = {
  list: (studentProfileId: string) => guardianWalletRoutes.transactions(studentProfileId),
};
