import { loadRequestOperations, type RequestOperationsData, type RequestResponse } from "../api/requestsApi";
import { RequestApprovalAction } from "./RequestActionForms";

export type RequestSecondaryView = "detail" | "trace" | "outing" | "early-leave" | "star-rule";

export async function RequestsSecondaryPage({ view, recordId }: { view: RequestSecondaryView; recordId?: string }) {
  const data = await loadRequestOperations();
  const request = findRequest(data, recordId);
  const rows = view === "outing" ? data.outingRequests : view === "early-leave" ? data.earlyLeaveRequests : request ? [request] : [];

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: 32 }}>
      <section style={{ marginBottom: 24 }}>
        <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 007</div>
        <h1 style={{ fontSize: 40, lineHeight: 1.05, margin: "8px 0" }}>{titleFor(view)}</h1>
        <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 19, lineHeight: 1.45, margin: 0 }}>Review request workflow records, approval evidence, trace events, and source-domain boundaries.</p>
      </section>
      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <article style={panelStyle}><h2 style={{ marginTop: 0 }}>Operational records</h2>{view === "trace" && request ? <DataTable headers={["Request", "Trace"]} rows={request.auditTrail.map((entry) => [request.trackingReference, entry])} /> : view === "star-rule" ? <DataTable headers={["Rule", "Trigger", "Status", "Review"]} rows={data.starRules.map((item) => [item.ruleId, item.trigger, item.status, item.review])} /> : <><RequestApprovalAction schoolAccountId={data.schoolAccountId} requests={rows} /><DataTable headers={["Tracking", "Type", "Status", "Student", "Summary"]} rows={rows.map((item) => [item.trackingReference, item.requestType, item.status, item.studentProfileId, item.visibleSummary])} /></>}</article>
        <article style={panelStyle}><h2 style={{ marginTop: 0 }}>Boundaries</h2><p style={{ color: "#64748b" }}>Requests create approval evidence only. Attendance, access, wallet, medical, transport, learning, complaint, communication, document, and search outcomes stay under their owning modules.</p></article>
      </section>
    </main>
  );
}

function findRequest(data: RequestOperationsData, recordId?: string): RequestResponse | undefined {
  return data.history.find((item) => item.requestId === recordId || item.trackingReference === recordId) ?? data.schoolRequests[0];
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No request records returned for this scope.</p>;
  return <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 680 }}><thead><tr>{headers.map((header) => <th key={header} style={thStyle}>{header}</th>)}</tr></thead><tbody>{rows.map((row) => <tr key={row.join(":")}>{row.map((cell, index) => <td key={`${cell}-${index}`} style={{ padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: index === 0 ? "#101828" : "#344054", fontWeight: index === 0 ? 800 : 500 }}>{cell}</td>)}</tr>)}</tbody></table>;
}

function titleFor(view: RequestSecondaryView) {
  return ({ detail: "Request Detail", trace: "Request Trace", outing: "Outing Requests", "early-leave": "Early Leave Requests", "star-rule": "Star Permission Rule" } satisfies Record<RequestSecondaryView, string>)[view];
}

const panelStyle = { background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)", overflowX: "auto" } satisfies React.CSSProperties;
const thStyle = { textAlign: "left", color: "#475467", fontSize: 12, textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" } satisfies React.CSSProperties;

