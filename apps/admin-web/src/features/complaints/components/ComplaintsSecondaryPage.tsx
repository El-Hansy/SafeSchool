import type { ReactNode } from "react";
import { loadComplaintOperations, type ComplaintOperationsData, type ComplaintResponse } from "../api/complaintsApi";
import { ComplaintStatusAction } from "./ComplaintActionForms";

export type ComplaintSecondaryView =
  | "detail"
  | "trace"
  | "triage"
  | "assigned"
  | "assigned-detail"
  | "escalations"
  | "exceptions"
  | "summaries"
  | "category"
  | "escalation-rule";

export async function ComplaintsSecondaryPage({ view, recordId }: { view: ComplaintSecondaryView; recordId?: string }) {
  const data = await loadComplaintOperations();
  const complaint = findComplaint(data, recordId);
  const category = data.categories.find((item) => item.categoryId === recordId) ?? data.categories[0];
  const escalationRule = data.escalationRules.find((item) => item.ruleId === recordId) ?? data.escalationRules[0];

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 009</div>
          <h1 style={{ fontSize: 40, lineHeight: 1.05, margin: "8px 0" }}>{titleFor(view)}</h1>
          <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 19, lineHeight: 1.45, margin: 0 }}>{detailFor(view)}</p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title="Operational records">
          <ViewContent view={view} data={data} complaint={complaint} category={category} escalationRule={escalationRule} />
        </Panel>
        <Panel title="Workflow boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Tenant scoped" detail={data.schoolAccountId} />
            <Boundary title="Restricted details" detail="Guardian and student views receive safe summaries, not sensitive investigation notes." />
            <Boundary title="Idempotent intake" detail="Client request IDs protect against duplicate complaint submissions." />
            <Boundary title="No source mutation" detail="Complaint review does not change attendance, transport, wallet, document, search, or communication delivery outcomes." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function ViewContent({
  view,
  data,
  complaint,
  category,
  escalationRule,
}: {
  view: ComplaintSecondaryView;
  data: ComplaintOperationsData;
  complaint?: ComplaintResponse;
  category?: ComplaintOperationsData["categories"][number];
  escalationRule?: ComplaintOperationsData["escalationRules"][number];
}) {
  if (view === "detail" && complaint) return <Stack><ComplaintStatusAction schoolAccountId={data.schoolAccountId} complaints={[complaint]} /><ComplaintTable complaints={[complaint]} /></Stack>;
  if (view === "trace" && complaint) return <DataTable headers={["Complaint", "Trace"]} rows={complaint.auditTrail.map((entry) => [complaint.trackingReference, entry])} />;
  if (view === "triage") return <Stack><ComplaintStatusAction schoolAccountId={data.schoolAccountId} complaints={data.triageComplaints} /><ComplaintTable complaints={data.triageComplaints} /></Stack>;
  if (view === "assigned") return <Stack><ComplaintStatusAction schoolAccountId={data.schoolAccountId} complaints={data.assignedComplaints} /><ComplaintTable complaints={data.assignedComplaints} /></Stack>;
  if (view === "assigned-detail" && complaint) return <Stack><ComplaintStatusAction schoolAccountId={data.schoolAccountId} complaints={[complaint]} /><ComplaintTable complaints={[complaint]} /></Stack>;
  if (view === "escalations") return <Stack><ComplaintStatusAction schoolAccountId={data.schoolAccountId} complaints={data.escalatedComplaints} /><ComplaintTable complaints={data.escalatedComplaints} /></Stack>;
  if (view === "exceptions") return <DataTable headers={["Exception", "Reason", "Owner", "Status"]} rows={data.exceptions.map((item) => [item.exceptionReference, item.reason, item.owner, item.status])} />;
  if (view === "summaries") return <DataTable headers={["Summary", "Status", "Audience", "Evidence"]} rows={data.summaries.map((item) => [item.summaryReference, item.status, item.audience, item.evidence])} />;
  if (view === "category" && category) return <DataTable headers={["Category", "Name", "Owner", "SLA", "Evidence"]} rows={[[category.categoryId, category.name, category.ownerRole, category.sla, category.evidence.join(", ")]]} />;
  if (view === "escalation-rule" && escalationRule) return <DataTable headers={["Rule", "Trigger", "Owner", "Window", "Evidence"]} rows={[[escalationRule.ruleId, escalationRule.trigger, escalationRule.ownerRole, escalationRule.window, escalationRule.evidence.join(", ")]]} />;

  return <p style={{ color: "#667085", margin: 0 }}>No complaint records returned for this scope.</p>;
}

function DataSourceBadge({ source }: { source: ComplaintOperationsData["dataSource"] }) {
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

function ComplaintTable({ complaints }: { complaints: ComplaintResponse[] }) {
  return <DataTable headers={["Tracking", "Status", "Priority", "Summary", "Audit"]} rows={complaints.map((complaint) => [complaint.trackingReference, complaint.status, complaint.priority, complaint.visibleSummary, complaint.auditTrail.join(", ")])} />;
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

function findComplaint(data: ComplaintOperationsData, recordId?: string) {
  return data.schoolComplaints.find((item) => item.complaintId === recordId || item.trackingReference === recordId)
    ?? data.assignedComplaints.find((item) => item.complaintId === recordId || item.trackingReference === recordId)
    ?? data.schoolComplaints[0];
}

function titleFor(view: ComplaintSecondaryView) {
  const titles: Record<ComplaintSecondaryView, string> = {
    detail: "Complaint Detail",
    trace: "Complaint Trace",
    triage: "Complaint Triage",
    assigned: "Assigned Complaints",
    "assigned-detail": "Assigned Complaint Detail",
    escalations: "Escalation Queue",
    exceptions: "Complaint Exceptions",
    summaries: "Complaint Summaries",
    category: "Complaint Category",
    "escalation-rule": "Escalation Rule",
  };
  return titles[view];
}

function detailFor(view: ComplaintSecondaryView) {
  const details: Record<ComplaintSecondaryView, string> = {
    detail: "Open one complaint with current status, assigned owner, safe visible summary, priority, and audit trail.",
    trace: "Trace a complaint lifecycle through submission, categorization, assignment, escalation, resolution, feedback, and audit references.",
    triage: "Review newly received complaints, suggested categories, restricted details, and first owner routing.",
    assigned: "Inspect owner queues, due items, priority, safe summaries, and next action evidence.",
    "assigned-detail": "Review one assigned complaint with safe details, owner action controls, and audit evidence.",
    escalations: "Track escalated complaints, urgency, owner routing, SLA windows, and resolution evidence.",
    exceptions: "Review duplicate, conflicting, or incomplete complaint workflow records requiring manual intervention.",
    summaries: "Inspect guardian or student visible summaries and publication evidence.",
    category: "Review one complaint category, owner role, SLA, visibility rules, and routing evidence.",
    "escalation-rule": "Review one escalation rule, trigger condition, owner route, escalation window, and audit evidence.",
  };
  return details[view];
}
