export const anomalyRoutes = {
  runs: "/anomaly-runs",
  list: "/anomalies",
  assign: (anomalyId: string) => `/anomalies/${anomalyId}/assign`,
  resolve: (anomalyId: string) => `/anomalies/${anomalyId}/resolve`,
  dismiss: (anomalyId: string) => `/anomalies/${anomalyId}/dismiss`,
  reopen: (anomalyId: string) => `/anomalies/${anomalyId}/reopen`,
};

