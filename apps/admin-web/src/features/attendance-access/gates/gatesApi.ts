export const gateRoutes = {
  list: "/gates",
  create: "/gates",
  update: (gateId: string) => `/gates/${gateId}`,
  scanPoints: "/scan-points",
};

export type GateSummary = {
  gateId: string;
  gateCode: string;
  displayName: string;
  status: "Draft" | "Active" | "Suspended" | "Decommissioned";
  offlineAllowed: boolean;
};

