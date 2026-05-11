import { assertApiDataAvailable } from "../../common/apiReadiness";

export const adminRoutes = {
  dashboard: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/dashboard`,
  configuration: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/configuration`,
  configurationHistory: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/configuration/history`,
  audit: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/audit`,
  auditEvent: (schoolAccountId: string, auditEventId: string) => `/api/v1/schools/${schoolAccountId}/admin/audit/${auditEventId}`,
  auditExports: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/audit/exports`,
  auditExport: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/audit/export`,
  monitoring: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/monitoring`,
  alertRules: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/monitoring/alert-rules`,
  monitoringAlerts: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/monitoring/alerts`,
  monitoringExceptions: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/monitoring/exceptions`,
  monitoringIncidents: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/admin/monitoring/incidents`,
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

type ConfigurationHistoryResponse = {
  changes: AdminOperationsData["configurationHistory"];
};

type AuditExportsResponse = {
  exports: AdminOperationsData["auditExports"];
};

type AlertRulesResponse = {
  rules: AdminOperationsData["alertRules"];
};

type MonitoringAlertsResponse = {
  alerts: Array<{ alertId: string; metric: string; severity: string; status: string }>;
};

type OperationalExceptionsResponse = {
  exceptions: AdminOperationsData["operationalExceptions"];
};

type MonitoringIncidentsResponse = {
  incidents: Array<{ incidentReference: string; status: string; owner: string }>;
};

export type AdminOperationsData = {
  schoolAccountId: string;
  dataSource: AdminDataSource;
  dashboard: AdminDashboardResponse;
  audit: AuditTrailResponse;
  monitoring: MonitoringResponse;
  capabilities: Array<{ key: string; enabled: boolean; dependency: string; owner: string }>;
  configurationHistory: Array<{ capabilityKey: string; state: string; actor: string; evidence: string }>;
  auditEvents: Array<{ auditEventId: string; actor: string; action: string; target: string; correlation: string; evidence: string[] }>;
  auditExports: Array<{ exportReference: string; scope: string; status: string; reason: string }>;
  alertRules: Array<{ ruleReference: string; metric: string; threshold: string; owner: string }>;
  alertsList: Array<{ id: string; metric: string; severity: string; owner: string }>;
  incidentsList: Array<{ id: string; status: string; evidence: string }>;
  operationalExceptions: Array<{ exceptionReference: string; module: string; reason: string; owner: string }>;
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
  configurationHistory: [
    { capabilityKey: "guardian.mobile.access", state: "Enabled", actor: "tenant-admin", evidence: "dependency-validated" },
    { capabilityKey: "wallet.topups", state: "Enabled", actor: "finance-admin", evidence: "version-preserved" },
    { capabilityKey: "transport.live.tracking", state: "Enabled", actor: "operations-admin", evidence: "audit-written" },
  ],
  auditEvents: [
    { auditEventId: "audit-demo-001", actor: "platform-admin", action: "configuration.applied", target: "guardian.mobile.access", correlation: "cfg-demo-001", evidence: ["tenant-scoped", "permission-checked", "append-only"] },
    { auditEventId: "audit-demo-002", actor: "audit-reviewer", action: "export.prepared", target: "tenant", correlation: "exp-demo-001", evidence: ["scope-approved", "payload-minimized"] },
  ],
  auditExports: [
    { exportReference: "audit-export-sales-demo", scope: "tenant", status: "Prepared", reason: "Sales demo evidence pack" },
    { exportReference: "audit-export-guardian-mobile", scope: "mobile", status: "Reviewed", reason: "Guardian access pilot" },
  ],
  alertRules: [
    { ruleReference: "rule-search-freshness", metric: "Search freshness", threshold: "95% within 5 minutes", owner: "Platform admin" },
    { ruleReference: "rule-api-errors", metric: "Mobile API failures", threshold: "5 failures in 10 minutes", owner: "Support" },
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
  operationalExceptions: [
    { exceptionReference: "exception-search-001", module: "documents", reason: "Index stale", owner: "Compliance" },
    { exceptionReference: "exception-mobile-002", module: "mobile", reason: "Install event delayed", owner: "Support" },
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
  const [dashboard, audit, monitoring, configurationHistory, auditExports, alertRules, monitoringAlerts, operationalExceptions, monitoringIncidents] = await Promise.all([
    fetchAdminJson<AdminDashboardResponse>(adminRoutes.dashboard(schoolAccountId), schoolAccountId),
    fetchAdminJson<AuditTrailResponse>(adminRoutes.audit(schoolAccountId), schoolAccountId),
    fetchAdminJson<MonitoringResponse>(adminRoutes.monitoring(schoolAccountId), schoolAccountId),
    fetchAdminJson<ConfigurationHistoryResponse>(adminRoutes.configurationHistory(schoolAccountId), schoolAccountId),
    fetchAdminJson<AuditExportsResponse>(adminRoutes.auditExports(schoolAccountId), schoolAccountId),
    fetchAdminJson<AlertRulesResponse>(adminRoutes.alertRules(schoolAccountId), schoolAccountId),
    fetchAdminJson<MonitoringAlertsResponse>(adminRoutes.monitoringAlerts(schoolAccountId), schoolAccountId),
    fetchAdminJson<OperationalExceptionsResponse>(adminRoutes.monitoringExceptions(schoolAccountId), schoolAccountId),
    fetchAdminJson<MonitoringIncidentsResponse>(adminRoutes.monitoringIncidents(schoolAccountId), schoolAccountId),
  ]);

  const hasApiData = [dashboard, audit, monitoring, configurationHistory, auditExports, alertRules, monitoringAlerts, operationalExceptions, monitoringIncidents].some((item) => item !== null);
  assertApiDataAvailable("Administration operations", [dashboard, audit, monitoring, configurationHistory, auditExports, alertRules, monitoringAlerts, operationalExceptions, monitoringIncidents], adminApiBaseUrl());
  if (!hasApiData) return { ...adminFallbackData, schoolAccountId, dataSource: "fallback" };

  return {
    ...adminFallbackData,
    schoolAccountId,
    dataSource: "api",
    dashboard: dashboard ?? { ...adminFallbackData.dashboard, schoolAccountId },
    audit: audit ?? adminFallbackData.audit,
    monitoring: monitoring ?? adminFallbackData.monitoring,
    configurationHistory: configurationHistory?.changes ?? adminFallbackData.configurationHistory,
    auditExports: auditExports?.exports ?? adminFallbackData.auditExports,
    alertRules: alertRules?.rules ?? adminFallbackData.alertRules,
    alertsList: monitoringAlerts?.alerts.map((alert) => ({ id: alert.alertId, metric: alert.metric, severity: alert.severity, owner: alert.status })) ?? adminFallbackData.alertsList,
    operationalExceptions: operationalExceptions?.exceptions ?? adminFallbackData.operationalExceptions,
    incidentsList: monitoringIncidents?.incidents.map((incident) => ({ id: incident.incidentReference, status: incident.status, evidence: incident.owner })) ?? adminFallbackData.incidentsList,
  };
}
