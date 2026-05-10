import { guardianWalletRoutes } from "../api/client";

export const guardianTopUpRoutes = {
  list: (studentProfileId: string) => guardianWalletRoutes.topUps(studentProfileId),
};
