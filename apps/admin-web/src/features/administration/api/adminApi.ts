export const adminRoutes = {
  dashboard: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/dashboard`,
  configuration: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/configuration`,
  audit: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/audit`,
  monitoring: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/monitoring`,
};
export const adminDemoData = { schoolAccountId: "school-demo", metrics: [{ label: "Enabled modules", value: "11" }, { label: "Pending reviews", value: "24" }, { label: "Alerts", value: "5" }], rows: ["Audit export prepared", "Feature dependency validated", "Metric threshold opened incident"] };
