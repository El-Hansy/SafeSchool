import type { ReactNode } from "react";
import {
  loadComplaintOperations,
  type ComplaintAudience,
  type ComplaintOperationsData,
  type ComplaintResponse,
} from "../api/complaintsApi";
import { ComplaintFeedbackAction, ComplaintStatusAction, ComplaintSubmitAction } from "./ComplaintActionForms";

export type ComplaintsView = "overview" | "new" | "history" | "configuration";

const schoolTabs: Array<[ComplaintsView, string, string]> = [
  ["overview", "Overview", "/complaints"],
  ["new", "New", "/complaints/new"],
  ["history", "History", "/complaints/history"],
  ["configuration", "Configuration", "/complaints/configuration"],
];

export async function ComplaintsOperationsPage({ audience = "school", view = "overview" }: { audience?: ComplaintAudience; view?: ComplaintsView }) {
  const data = await loadComplaintOperations();
  const title = audience === "school" ? "Complaints Command Center" : audience === "guardian" ? "Guardian Complaints" : "Student Complaints";
  const complaints = audience === "guardian" ? data.guardianComplaints : audience === "student" ? data.studentComplaints : data.schoolComplaints;

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 009</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>{title}</h1>
          <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>
            Structured complaint intake, safe summaries, assignment, escalation, resolution, feedback, tracking, and audit evidence without mutating source-domain records.
          </p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      {audience === "school" && (
        <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>
          {schoolTabs.map(([key, label, href]) => (
            <a key={key} href={href} style={{ textDecoration: "none", color: view === key ? "#1d4ed8" : "#1f2937", border: `1px solid ${view === key ? "#3b82f6" : "#cbd5e1"}`, background: view === key ? "#eff6ff" : "#ffffff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 }}>{label}</a>
          ))}
        </nav>
      )}

      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))", gap: 16, marginBottom: 24 }}>
        <Metric label="Open complaints" value={String(data.board.open)} detail={`${complaints.length} records visible here`} />
        <Metric label="Escalated" value={String(data.board.escalated)} detail="Urgent owner review" />
        <Metric label="Pending feedback" value={String(data.board.pendingFeedback)} detail="Guardian or student response" />
        <Metric label="Capabilities" value={String(data.board.capabilities.length)} detail="Tenant feature controls" />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title={panelTitle(audience, view)}>
          <SectionContent audience={audience} view={view} data={data} complaints={complaints} />
        </Panel>
        <Panel title="Boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Safe summary" detail="Sensitive complaint details are reduced to visible summaries for tracking and feedback." />
            <Boundary title="No source mutation" detail="Complaint workflows do not update attendance, access, transport, wallet, learning, medical, documents, search, or admin outcomes." />
            <Boundary title="Idempotent submission" detail="Client request IDs protect duplicate guardian, student, and staff submissions." />
            <Boundary title="Audit trail" detail="Assignment, escalation, resolution, and feedback actions produce traceable workflow evidence." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ audience, view, data, complaints }: { audience: ComplaintAudience; view: ComplaintsView; data: ComplaintOperationsData; complaints: ComplaintResponse[] }) {
  if (audience !== "school") {
    return (
      <Stack>
        <ComplaintSubmitAction audience={audience} schoolAccountId={data.schoolAccountId} />
        {audience === "guardian" && <ComplaintFeedbackAction schoolAccountId={data.schoolAccountId} complaints={complaints} />}
        <ComplaintTable complaints={complaints} />
      </Stack>
    );
  }

  if (view === "new") return <Stack><ComplaintSubmitAction audience="school" schoolAccountId={data.schoolAccountId} /><ComplaintTable complaints={complaints} /></Stack>;
  if (view === "history") return <Stack><ComplaintStatusAction schoolAccountId={data.schoolAccountId} complaints={complaints} /><ComplaintTable complaints={complaints} /></Stack>;
  if (view === "configuration") {
    return (
      <dl style={{ display: "grid", gap: 14, margin: 0 }}>
        {data.board.capabilities.map((capability) => <Boundary key={capability} title={capability} detail="Enabled for this tenant when API policy allows it." />)}
      </dl>
    );
  }

  return <ComplaintTable complaints={complaints} />;
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

function Metric({ label, value, detail }: { label: string; value: string; detail: string }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 18, boxShadow: "0 1px 2px rgba(15,23,42,.08)" }}><div style={{ fontSize: 34, fontWeight: 900 }}>{value}</div><div style={{ fontWeight: 800 }}>{label}</div><div style={{ color: "#64748b", marginTop: 8 }}>{detail}</div></article>;
}

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)", overflowX: "auto" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
}

function ComplaintTable({ complaints }: { complaints: ComplaintResponse[] }) {
  if (complaints.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No complaints returned for this scope.</p>;

  return (
    <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 680 }}>
      <thead>
        <tr>{["Tracking", "Status", "Priority", "Visible summary", "Audit"].map((header) => <th key={header} style={{ textAlign: "left", color: "#475467", fontSize: 12, textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" }}>{header}</th>)}</tr>
      </thead>
      <tbody>
        {complaints.map((complaint) => (
          <tr key={complaint.complaintId}>
            <td style={cellStyle(true)}>{complaint.trackingReference}</td>
            <td style={cellStyle()}>{complaint.status}</td>
            <td style={cellStyle()}>{complaint.priority}</td>
            <td style={cellStyle()}>{complaint.visibleSummary}</td>
            <td style={cellStyle()}>{complaint.auditTrail.join(", ")}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

function cellStyle(primary = false) {
  return { padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: primary ? "#101828" : "#344054", fontWeight: primary ? 800 : 500 };
}

function panelTitle(audience: ComplaintAudience, view: ComplaintsView) {
  if (audience === "guardian") return "Guardian submission and tracking";
  if (audience === "student") return "Student submission and tracking";
  const titles: Record<ComplaintsView, string> = { overview: "Complaint queue", new: "Submit complaint", history: "History and workflow actions", configuration: "Complaint controls" };
  return titles[view];
}
