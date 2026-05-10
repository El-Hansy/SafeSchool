import type { ReactNode } from "react";
import { loadAdminOperations, type AdminOperationsData } from "../api/adminApi";
import { AuditExportAction, OpenIncidentAction, TenantConfigurationAction } from "./AdminActionForms";

export type AdminOperationsView = "dashboard" | "configuration" | "audit" | "monitoring" | "alerts" | "incidents";

const tabs: Array<[AdminOperationsView, string, string]> = [
  ["dashboard", "Dashboard", "/admin"],
  ["configuration", "Configuration", "/admin/configuration"],
  ["audit", "Audit", "/admin/audit"],
  ["monitoring", "Monitoring", "/admin/monitoring"],
  ["alerts", "Alerts", "/admin/alerts"],
  ["incidents", "Incidents", "/admin/incidents"],
];

export async function AdminOperationsPage({ view = "dashboard" }: { view?: AdminOperationsView }) {
  const data = await loadAdminOperations();

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 011</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>Admin, Audit & Observability</h1>
          <p style={{ maxWidth: 930, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>
            Tenant feature configuration, dashboard health, append-only audit trail, controlled exports, metrics monitoring, alerts, incidents, and operational exception review.
          </p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>
        {tabs.map(([key, label, href]) => (
          <a key={key} href={href} style={{ textDecoration: "none", color: view === key ? "#1d4ed8" : "#1f2937", border: `1px solid ${view === key ? "#3b82f6" : "#cbd5e1"}`, background: view === key ? "#eff6ff" : "#ffffff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 }}>{label}</a>
        ))}
      </nav>

      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))", gap: 16, marginBottom: 24 }}>
        <Metric label="Modules enabled" value={String(data.dashboard.modulesEnabled)} detail={data.dashboard.status} />
        <Metric label="Pending reviews" value={String(data.dashboard.pendingReviews)} detail="Operational review queue" />
        <Metric label="Alerts" value={String(data.dashboard.alerts)} detail={`${data.monitoring.thresholdBreaches} threshold breaches`} />
        <Metric label="Incidents" value={String(data.dashboard.incidents)} detail={`${data.monitoring.operationalExceptions} exceptions monitored`} />
        <Metric label="Search freshness" value={data.dashboard.searchFreshness} detail={`${data.monitoring.metrics} metrics tracked`} />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title={panelTitle(view)}>
          <SectionContent view={view} data={data} />
        </Panel>
        <Panel title="Boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Tenant scoped" detail="Every route is under the school account administration boundary." />
            <Boundary title="Audit required" detail="Configuration changes, exports, and incident actions write evidence through the API contract." />
            <Boundary title="No domain mutation" detail="Administration does not create attendance, transport, wallet, learning, request, medical, complaint, communication, or document outcomes." />
            <Boundary title="Least privilege" detail="Views expose operational status and only the action controls tied to the current admin role." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ view, data }: { view: AdminOperationsView; data: AdminOperationsData }) {
  if (view === "configuration") {
    return <Stack><TenantConfigurationAction schoolAccountId={data.schoolAccountId} /><DataTable headers={["Capability", "State", "Dependency", "Owner"]} rows={data.capabilities.map((item) => [item.key, item.enabled ? "Enabled" : "Disabled", item.dependency, item.owner])} /></Stack>;
  }

  if (view === "audit") {
    return <Stack><AuditExportAction schoolAccountId={data.schoolAccountId} /><AuditSummary data={data} /></Stack>;
  }

  if (view === "monitoring") {
    return (
      <dl style={{ display: "grid", gap: 14, margin: 0 }}>
        <Boundary title="Metrics" detail={String(data.monitoring.metrics)} />
        <Boundary title="Threshold breaches" detail={String(data.monitoring.thresholdBreaches)} />
        <Boundary title="Alerts" detail={String(data.monitoring.alerts)} />
        <Boundary title="Operational exceptions" detail={String(data.monitoring.operationalExceptions)} />
      </dl>
    );
  }

  if (view === "alerts") {
    return <Stack><OpenIncidentAction schoolAccountId={data.schoolAccountId} alertId={data.alertsList[0]?.id ?? "alert-metrics-001"} /><DataTable headers={["Alert", "Metric", "Severity", "Owner"]} rows={data.alertsList.map((alert) => [alert.id, alert.metric, alert.severity, alert.owner])} /></Stack>;
  }

  if (view === "incidents") {
    return <DataTable headers={["Incident", "Status", "Evidence"]} rows={data.incidentsList.map((incident) => [incident.id, incident.status, incident.evidence])} />;
  }

  return <Stack><AuditSummary data={data} /><DataTable headers={["Event"]} rows={data.events.map((event) => [event])} /></Stack>;
}

function DataSourceBadge({ source }: { source: AdminOperationsData["dataSource"] }) {
  const api = source === "api";
  return (
    <div style={{ border: `1px solid ${api ? "#a7f3d0" : "#fed7aa"}`, background: api ? "#dcfce7" : "#fff7ed", color: api ? "#166534" : "#9a3412", padding: "12px 18px", borderRadius: 6, fontWeight: 800 }}>
      {api ? "API connected" : "API fallback"}
    </div>
  );
}

function Stack({ children }: { children: ReactNode }) {
  return <div style={{ display: "grid", gap: 18 }}>{children}</div>;
}

function Metric({ label, value, detail }: { label: string; value: string; detail: string }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 18, boxShadow: "0 1px 2px rgba(15,23,42,.08)" }}><div style={{ fontSize: 34, fontWeight: 900 }}>{value}</div><div style={{ fontWeight: 800 }}>{label}</div><div style={{ color: "#64748b", marginTop: 8 }}>{detail}</div></article>;
}

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)", overflowX: "auto" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
}

function AuditSummary({ data }: { data: AdminOperationsData }) {
  return (
    <DataTable
      headers={["Total events", "Append only", "Payloads", "Filters"]}
      rows={[[String(data.audit.total), data.audit.appendOnly ? "Yes" : "No", data.audit.minimizedPayloads ? "Minimized" : "Full", data.audit.filters.join(", ")]]}
    />
  );
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No records returned for this scope.</p>;

  return (
    <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 620 }}>
      <thead>
        <tr>{headers.map((header) => <th key={header} style={{ textAlign: "left", color: "#475467", fontSize: 12, textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" }}>{header}</th>)}</tr>
      </thead>
      <tbody>
        {rows.map((row) => (
          <tr key={row.join(":")}>{row.map((cell, index) => <td key={`${cell}-${index}`} style={{ padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: index === 0 ? "#101828" : "#344054", fontWeight: index === 0 ? 800 : 500 }}>{cell}</td>)}</tr>
        ))}
      </tbody>
    </table>
  );
}

function panelTitle(view: AdminOperationsView) {
  const titles: Record<AdminOperationsView, string> = {
    dashboard: "Administration evidence",
    configuration: "Tenant feature configuration",
    audit: "Audit trail and controlled export",
    monitoring: "Metrics monitoring",
    alerts: "Alert management",
    incidents: "Incident board",
  };
  return titles[view];
}
