import type { ReactNode } from "react";
import { loadAdminOperations, type AdminOperationsData } from "../api/adminApi";
import { AuditExportAction, OpenIncidentAction, TenantConfigurationAction } from "./AdminActionForms";

export type AdminSecondaryView =
  | "feature-controls"
  | "feature-history"
  | "audit-event"
  | "audit-exports"
  | "alert-rules"
  | "monitoring-alerts"
  | "monitoring-exceptions"
  | "monitoring-incidents";

export async function AdminSecondaryPage({ view, auditEventId = "audit-demo-001" }: { view: AdminSecondaryView; auditEventId?: string }) {
  const data = await loadAdminOperations();
  const auditEvent = data.auditEvents.find((event) => event.auditEventId === auditEventId) ?? data.auditEvents[0];

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 011</div>
          <h1 style={{ fontSize: 40, lineHeight: 1.05, margin: "8px 0" }}>{titleFor(view)}</h1>
          <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 19, lineHeight: 1.45, margin: 0 }}>{detailFor(view)}</p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title="Operational records">
          <ViewContent view={view} data={data} auditEvent={auditEvent} />
        </Panel>
        <Panel title="Evidence controls">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Tenant scoped" detail={data.schoolAccountId} />
            <Boundary title="Permission checked" detail="Admin, audit reviewer, operations, or platform owner role required by action." />
            <Boundary title="Append-only evidence" detail="Audit and monitoring records are reviewed through immutable references." />
            <Boundary title="No domain mutation" detail="These views do not mutate attendance, transport, wallet, learning, request, complaint, communication, document, or certificate outcomes." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function ViewContent({ view, data, auditEvent }: { view: AdminSecondaryView; data: AdminOperationsData; auditEvent?: AdminOperationsData["auditEvents"][number] }) {
  if (view === "feature-controls") return <Stack><TenantConfigurationAction schoolAccountId={data.schoolAccountId} /><CapabilityTable data={data} /></Stack>;
  if (view === "feature-history") return <DataTable headers={["Capability", "State", "Actor", "Evidence"]} rows={data.configurationHistory.map((item) => [item.capabilityKey, item.state, item.actor, item.evidence])} />;
  if (view === "audit-exports") return <Stack><AuditExportAction schoolAccountId={data.schoolAccountId} /><DataTable headers={["Export", "Scope", "Status", "Reason"]} rows={data.auditExports.map((item) => [item.exportReference, item.scope, item.status, item.reason])} /></Stack>;
  if (view === "audit-event" && auditEvent) return <DataTable headers={["Event", "Actor", "Action", "Target", "Correlation", "Evidence"]} rows={[[auditEvent.auditEventId, auditEvent.actor, auditEvent.action, auditEvent.target, auditEvent.correlation, auditEvent.evidence.join(", ")]]} />;
  if (view === "alert-rules") return <DataTable headers={["Rule", "Metric", "Threshold", "Owner"]} rows={data.alertRules.map((item) => [item.ruleReference, item.metric, item.threshold, item.owner])} />;
  if (view === "monitoring-alerts") return <Stack><OpenIncidentAction schoolAccountId={data.schoolAccountId} alertId={data.alertsList[0]?.id ?? "alert-metrics-001"} /><DataTable headers={["Alert", "Metric", "Severity", "Owner or status"]} rows={data.alertsList.map((item) => [item.id, item.metric, item.severity, item.owner])} /></Stack>;
  if (view === "monitoring-exceptions") return <DataTable headers={["Exception", "Module", "Reason", "Owner"]} rows={data.operationalExceptions.map((item) => [item.exceptionReference, item.module, item.reason, item.owner])} />;
  if (view === "monitoring-incidents") return <DataTable headers={["Incident", "Status", "Evidence or owner"]} rows={data.incidentsList.map((item) => [item.id, item.status, item.evidence])} />;

  return <p style={{ color: "#667085", margin: 0 }}>No administration records returned for this scope.</p>;
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

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)", overflowX: "auto" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
}

function CapabilityTable({ data }: { data: AdminOperationsData }) {
  return <DataTable headers={["Capability", "State", "Dependency", "Owner"]} rows={data.capabilities.map((item) => [item.key, item.enabled ? "Enabled" : "Disabled", item.dependency, item.owner])} />;
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No records returned for this scope.</p>;

  return (
    <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 680 }}>
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

function titleFor(view: AdminSecondaryView) {
  const titles: Record<AdminSecondaryView, string> = {
    "feature-controls": "Tenant Feature Controls",
    "feature-history": "Feature Change History",
    "audit-event": "Audit Event Detail",
    "audit-exports": "Audit Exports",
    "alert-rules": "Alert Rules",
    "monitoring-alerts": "Monitoring Alerts",
    "monitoring-exceptions": "Operational Exceptions",
    "monitoring-incidents": "Incident Review",
  };
  return titles[view];
}

function detailFor(view: AdminSecondaryView) {
  const details: Record<AdminSecondaryView, string> = {
    "feature-controls": "Manage role-visible feature switches, dependency warnings, and audit-backed configuration state.",
    "feature-history": "Inspect tenant feature changes, dependency checks, reviewer notes, and rollback evidence across school modules.",
    "audit-event": "Review one audit event with actor, target, correlation, export status, and evidence context.",
    "audit-exports": "Track scoped audit export requests, approval state, retention controls, and delivery evidence for reviewers.",
    "alert-rules": "Review metric thresholds, owner routing, escalation windows, and suppression rules for operational monitoring.",
    "monitoring-alerts": "Inspect active alerts, severity, breached metric evidence, acknowledgement status, and linked incidents.",
    "monitoring-exceptions": "Review cross-module exceptions that need staff action without mutating source records.",
    "monitoring-incidents": "Track incident ownership, status, timeline evidence, affected modules, and post-resolution notes.",
  };
  return details[view];
}
