import type { ReactNode } from "react";
import { loadRequestOperations, type RequestAudience, type RequestOperationsData, type RequestResponse } from "../api/requestsApi";
import { RequestApprovalAction, RequestSubmitAction } from "./RequestActionForms";

export type RequestsView = "overview" | "new" | "approvals" | "history" | "configuration";

const tabs: Array<[RequestsView, string, string]> = [
  ["overview", "Overview", "/requests"],
  ["new", "New", "/requests/new"],
  ["approvals", "Approvals", "/requests/approvals"],
  ["history", "History", "/requests/history"],
  ["configuration", "Configuration", "/requests/configuration"],
];

export async function RequestsOperationsPage({ audience = "school", view = "overview" }: { audience?: RequestAudience; view?: RequestsView }) {
  const data = await loadRequestOperations();
  const title = audience === "school" ? "Requests Command Center" : audience === "guardian" ? "Guardian Requests" : "Student Requests";
  const requests = audience === "guardian" ? data.guardianRequests : audience === "student" ? data.studentRequests : data.schoolRequests;

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: 32 }}>
      <Header title={title} source={data.dataSource} />
      {audience === "school" && <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>{tabs.map(([key, label, href]) => <a key={key} href={href} style={tabStyle(view === key)}>{label}</a>)}</nav>}
      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))", gap: 16, marginBottom: 24 }}>
        <Metric label="Open requests" value={String(data.board.open)} detail={`${requests.length} visible in this scope`} />
        <Metric label="Pending approval" value={String(data.board.pendingApproval)} detail="Human approval required" />
        <Metric label="Approved today" value={String(data.board.approvedToday)} detail="Release evidence preserved" />
        <Metric label="Capabilities" value={String(data.board.capabilities.length)} detail="Feature-controlled actions" />
      </section>
      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title={panelTitle(audience, view)}>
          <SectionContent audience={audience} view={view} data={data} requests={requests} />
        </Panel>
        <Panel title="Boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Source-domain safe" detail="Approval records do not mutate attendance, transport, wallet, medical, or learning outcomes." />
            <Boundary title="Guardian and student scoped" detail="Submission is tied to actor, tenant, student, timestamp, and audit evidence." />
            <Boundary title="Idempotent workflow" detail="Client request IDs prevent duplicate outing and early-leave submissions." />
            <Boundary title="Star rules are read-only" detail="Learning stars can inform eligibility but approval remains a request workflow decision." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ audience, view, data, requests }: { audience: RequestAudience; view: RequestsView; data: RequestOperationsData; requests: RequestResponse[] }) {
  if (audience !== "school") return <Stack><RequestSubmitAction audience={audience} schoolAccountId={data.schoolAccountId} /><RequestTable requests={requests} /></Stack>;
  if (view === "new") return <Stack><RequestSubmitAction audience="school" schoolAccountId={data.schoolAccountId} /><RequestTable requests={requests} /></Stack>;
  if (view === "approvals") return <Stack><RequestApprovalAction schoolAccountId={data.schoolAccountId} requests={data.approvalQueue} /><RequestTable requests={data.approvalQueue} /></Stack>;
  if (view === "history") return <Stack><RequestApprovalAction schoolAccountId={data.schoolAccountId} requests={data.history} /><RequestTable requests={data.history} /></Stack>;
  if (view === "configuration") return <DataTable headers={["Key", "Value", "Evidence"]} rows={data.configuration.map((item) => [item.key, item.value, item.evidence]).concat(data.starRules.map((item) => [item.ruleId, item.trigger, item.evidence.join(", ")]))} />;
  return <RequestTable requests={requests} />;
}

function Header({ title, source }: { title: string; source: RequestOperationsData["dataSource"] }) {
  return (
    <section style={{ display: "flex", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
      <div><div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 007</div><h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>{title}</h1><p style={{ maxWidth: 920, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>Guardian and student outing, early-leave, star-permission, approval, cancellation, history, and audit workflows.</p></div>
      <DataSourceBadge source={source} />
    </section>
  );
}

function DataSourceBadge({ source }: { source: RequestOperationsData["dataSource"] }) {
  const api = source === "api";
  return <div style={{ border: `1px solid ${api ? "#a7f3d0" : "#fed7aa"}`, background: api ? "#dcfce7" : "#fff7ed", color: api ? "#166534" : "#9a3412", padding: "12px 18px", borderRadius: 6, fontWeight: 800, height: "fit-content" }}>{api ? "API connected" : "API fallback"}</div>;
}

function Stack({ children }: { children: ReactNode }) {
  return <div style={{ display: "grid", gap: 18 }}>{children}</div>;
}

function Metric({ label, value, detail }: { label: string; value: string; detail: string }) {
  return <article style={cardStyle}><div style={{ fontSize: 34, fontWeight: 900 }}>{value}</div><div style={{ fontWeight: 800 }}>{label}</div><div style={{ color: "#64748b", marginTop: 8 }}>{detail}</div></article>;
}

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ ...cardStyle, padding: 22, overflowX: "auto" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
}

function RequestTable({ requests }: { requests: RequestResponse[] }) {
  return <DataTable headers={["Tracking", "Type", "Status", "Student", "Summary", "Audit"]} rows={requests.map((request) => [request.trackingReference, request.requestType, request.status, request.studentProfileId, request.visibleSummary, request.auditTrail.join(", ")])} />;
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No request records returned for this scope.</p>;
  return <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 760 }}><thead><tr>{headers.map((header) => <th key={header} style={thStyle}>{header}</th>)}</tr></thead><tbody>{rows.map((row) => <tr key={row.join(":")}>{row.map((cell, index) => <td key={`${cell}-${index}`} style={{ padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: index === 0 ? "#101828" : "#344054", fontWeight: index === 0 ? 800 : 500 }}>{cell}</td>)}</tr>)}</tbody></table>;
}

function panelTitle(audience: RequestAudience, view: RequestsView) {
  if (audience === "guardian") return "Guardian request submission";
  if (audience === "student") return "Student request tracking";
  return ({ overview: "Request queue", new: "Submit request", approvals: "Approval decisions", history: "Request history", configuration: "Request controls" } satisfies Record<RequestsView, string>)[view];
}

const cardStyle = { background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 18, boxShadow: "0 1px 2px rgba(15,23,42,.08)" } satisfies React.CSSProperties;
const thStyle = { textAlign: "left", color: "#475467", fontSize: 12, textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" } satisfies React.CSSProperties;
const tabStyle = (active: boolean) => ({ textDecoration: "none", color: active ? "#1d4ed8" : "#1f2937", border: `1px solid ${active ? "#3b82f6" : "#cbd5e1"}`, background: active ? "#eff6ff" : "#fff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 });

