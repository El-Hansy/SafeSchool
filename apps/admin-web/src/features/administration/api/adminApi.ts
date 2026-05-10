export const adminRoutes = {
  dashboard: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/dashboard`,
  configuration: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/configuration`,
  audit: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/audit`,
  auditExport: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/audit/export`,
  monitoring: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/monitoring`,
  alertIncident: (schoolAccountId: string, alertId: string) => `/api/v1/schools/${schoolAccountId}/admin/alerts/${alertId}/incidents`,
};
export const adminDemoData = { schoolAccountId: "school-demo", metrics: [{ label: "Enabled modules", value: "11" }, { label: "Pending reviews", value: "24" }, { label: "Alerts", value: "5" }], rows: ["Audit export prepared", "Feature dependency validated", "Metric threshold opened incident"] };

export type AdminDataSource = "api" | "fallback";

export type AdminDashboardResponse = {
  schoolAccountId: string;
  status: string;
  modulesEnabled: number;
  pendingReviews: number;
  alerts: number;
  incidents: number;
  searchFreshness: string;
};

export type AuditTrailResponse = {
  total: number;
  appendOnly: boolean;
  minimizedPayloads: boolean;
  filters: string[];
};

export type MonitoringResponse = {
  metrics: number;
  thresholdBreaches: number;
  alerts: number;
  operationalExceptions: number;
};

export type AdminOperationsData = {
  schoolAccountId: string;
  dataSource: AdminDataSource;
  dashboard: AdminDashboardResponse;
  audit: AuditTrailResponse;
  monitoring: MonitoringResponse;
  capabilities: Array<{ key: string; enabled: boolean; dependency: string; owner: string }>;
  alertsList: Array<{ id: string; metric: string; severity: string; owner: string }>;
  incidentsList: Array<{ id: string; status: string; evidence: string }>;
  events: string[];
};

export const adminFallbackData: AdminOperationsData = {
  schoolAccountId: adminDemoData.schoolAccountId,
  dataSource: "fallback",
  dashboard: {
    schoolAccountId: adminDemoData.schoolAccountId,
    status: "ready",
    modulesEnabled: 11,
    pendingReviews: 24,
    alerts: 5,
    incidents: 2,
    searchFreshness: "95% within target",
  },
  audit: { total: 316, appendOnly: true, minimizedPayloads: true, filters: ["actor", "action", "target", "correlation"] },
  monitoring: { metrics: 84, thresholdBreaches: 4, alerts: 5, operationalExceptions: 6 },
  capabilities: [
    { key: "guardian.mobile.access", enabled: true, dependency: "identity-access", owner: "Registrar" },
    { key: "wallet.topups", enabled: true, dependency: "wallet-payments", owner: "Finance" },
    { key: "transport.live.tracking", enabled: true, dependency: "transport-bus-tracking", owner: "Operations" },
    { key: "documents.search", enabled: true, dependency: "documents-search-admin-observability", owner: "Compliance" },
  ],
  alertsList: [
    { id: "alert-metrics-001", metric: "Search index freshness", severity: "medium", owner: "Platform admin" },
    { id: "alert-audit-002", metric: "Audit export queue", severity: "high", owner: "Compliance" },
    { id: "alert-mobile-003", metric: "Mobile API failures", severity: "medium", owner: "Support" },
  ],
  incidentsList: [
    { id: "incident-alert-metrics-001", status: "Open", evidence: "metric-threshold, owner-assigned, audit-written" },
    { id: "incident-wallet-recon-002", status: "Investigating", evidence: "reconciliation-gap, reviewer-assigned" },
  ],
  events: ["Audit export prepared", "Feature dependency validated", "Metric threshold opened incident"],
};

export function adminApiBaseUrl() {
  return process.env.NEXT_PUBLIC_API_BASE_URL ?? "";
}

export function adminHeaders(schoolAccountId: string, actorReference = "school-admin") {
  return {
    "content-type": "application/json",
    "x-school-account-id": schoolAccountId,
    "x-actor-reference": actorReference,
  };
}

async function fetchAdminJson<T>(path: string, schoolAccountId: string): Promise<T | null> {
  const baseUrl = adminApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: adminHeaders(schoolAccountId),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadAdminOperations(schoolAccountId = adminFallbackData.schoolAccountId): Promise<AdminOperationsData> {
  const [dashboard, audit, monitoring] = await Promise.all([
    fetchAdminJson<AdminDashboardResponse>(adminRoutes.dashboard(schoolAccountId), schoolAccountId),
    fetchAdminJson<AuditTrailResponse>(adminRoutes.audit(schoolAccountId), schoolAccountId),
    fetchAdminJson<MonitoringResponse>(adminRoutes.monitoring(schoolAccountId), schoolAccountId),
  ]);

  const hasApiData = [dashboard, audit, monitoring].some((item) => item !== null);
  if (!hasApiData) return { ...adminFallbackData, schoolAccountId, dataSource: "fallback" };

  return {
    ...adminFallbackData,
    schoolAccountId,
    dataSource: "api",
    dashboard: dashboard ?? { ...adminFallbackData.dashboard, schoolAccountId },
    audit: audit ?? adminFallbackData.audit,
    monitoring: monitoring ?? adminFallbackData.monitoring,
  };
}
