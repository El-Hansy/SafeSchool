import type { ReactNode } from "react";
import { loadMedicalOperations, type MedicalAudience, type MedicalOperationsData, type MedicalResponse } from "../api/medicalApi";
import { EmergencyAccessAction, MedicalIncidentAction, MedicalProfileAction, MedicalReviewAction } from "./MedicalActionForms";

export type MedicalView = "overview" | "records" | "emergency" | "incidents" | "notifications" | "history" | "configuration";

const tabs: Array<[MedicalView, string, string]> = [
  ["overview", "Overview", "/medical"],
  ["records", "Records", "/medical/records/student-amina"],
  ["emergency", "Emergency", "/medical/emergency"],
  ["incidents", "Incidents", "/medical/incidents"],
  ["notifications", "Notifications", "/medical/notifications"],
  ["history", "History", "/medical/history"],
  ["configuration", "Configuration", "/medical/configuration"],
];

export async function MedicalOperationsPage({ audience = "school", view = "overview" }: { audience?: MedicalAudience; view?: MedicalView }) {
  const data = await loadMedicalOperations();
  const title = audience === "school" ? "Medical & Emergency Command Center" : audience === "guardian" ? "Guardian Medical" : "Student Medical";
  const records = audience === "guardian" ? data.guardianRecords : audience === "student" ? data.studentRecords : data.history;

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: 32 }}>
      <Header title={title} source={data.dataSource} />
      {audience === "school" && <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>{tabs.map(([key, label, href]) => <a key={key} href={href} style={tabStyle(view === key)}>{label}</a>)}</nav>}
      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))", gap: 16, marginBottom: 24 }}>
        <Metric label="Profiles" value={String(data.board.profiles)} detail="Medical summaries" />
        <Metric label="Emergency sessions" value={String(data.board.openEmergencySessions)} detail="30 minute windows" />
        <Metric label="Incidents" value={String(data.board.incidents)} detail="Care evidence" />
        <Metric label="Notifications" value={String(data.board.notifications)} detail="Minimized audience" />
        <Metric label="Status events" value={String(data.board.statusEvents)} detail="Notification eligibility" />
        <Metric label="Review summaries" value={String(data.board.reviewSummaries)} detail="Mandatory reviews" />
      </section>
      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title={panelTitle(audience, view)}>
          <SectionContent audience={audience} view={view} data={data} records={records} />
        </Panel>
        <Panel title="Privacy Boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Minimum necessary" detail="Emergency access exposes only critical details and expires after the configured window." />
            <Boundary title="Break-glass review" detail="Emergency override is restricted to pre-authorized roles and creates mandatory review evidence." />
            <Boundary title="No clinical authority" detail="The system records school evidence; it does not diagnose, prescribe, or replace emergency protocols." />
            <Boundary title="No source mutation" detail="Medical records do not create attendance, transport, wallet, request, complaint, document, or search outcomes." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ audience, view, data, records }: { audience: MedicalAudience; view: MedicalView; data: MedicalOperationsData; records: MedicalResponse[] }) {
  if (audience === "guardian") return <Stack><MedicalProfileAction schoolAccountId={data.schoolAccountId} guardian /><MedicalTable records={records} /></Stack>;
  if (audience === "student") return <MedicalTable records={records} />;
  if (view === "records") return <Stack><MedicalProfileAction schoolAccountId={data.schoolAccountId} /><MedicalTable records={data.records} /></Stack>;
  if (view === "emergency") return <Stack><EmergencyAccessAction schoolAccountId={data.schoolAccountId} /><EmergencyAccessAction schoolAccountId={data.schoolAccountId} breakGlass /><MedicalTable records={data.emergency} /></Stack>;
  if (view === "incidents") return <Stack><MedicalIncidentAction schoolAccountId={data.schoolAccountId} /><MedicalTable records={data.incidents} /></Stack>;
  if (view === "notifications") return <MedicalTable records={data.notifications} />;
  if (view === "history") return <Stack><MedicalReviewAction schoolAccountId={data.schoolAccountId} records={data.history} /><MedicalTable records={data.history} /><MedicalLifecycleEvidence data={data} /></Stack>;
  if (view === "configuration") return <DataTable headers={["Key", "Value", "Evidence"]} rows={data.configuration.map((item) => [item.key, item.value, item.evidence])} />;
  return <MedicalTable records={records} />;
}

function MedicalLifecycleEvidence({ data }: { data: MedicalOperationsData }) {
  return (
    <Stack>
      <h3 style={{ margin: "4px 0 0", fontSize: 20 }}>Lifecycle evidence</h3>
      <DataTable
        headers={["Reference", "Type", "Status", "Severity", "Event", "Notify", "Review"]}
        rows={data.statusEvents.map((event) => [
          event.recordReference,
          event.recordType,
          event.status,
          event.severity,
          event.sourceEventType,
          event.notificationEligible ? "eligible" : "held",
          event.reviewRequired ? "required" : "not required",
        ])}
      />
      <h3 style={{ margin: "4px 0 0", fontSize: 20 }}>Review summaries</h3>
      <DataTable
        headers={["Reference", "Student", "Type", "Status", "Severity", "Review", "Last event"]}
        rows={data.reviewSummaries.map((summary) => [
          summary.recordReference,
          summary.studentProfileId,
          summary.recordType,
          summary.status,
          summary.severity,
          summary.reviewState,
          summary.lastEventType,
        ])}
      />
    </Stack>
  );
}

function Header({ title, source }: { title: string; source: MedicalOperationsData["dataSource"] }) {
  return <section style={{ display: "flex", justifyContent: "space-between", gap: 24, marginBottom: 24 }}><div><div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 008</div><h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>{title}</h1><p style={{ maxWidth: 920, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>Secure medical summaries, emergency access, break-glass review, incidents, notifications, history, and privacy evidence.</p></div><DataSourceBadge source={source} /></section>;
}

function DataSourceBadge({ source }: { source: MedicalOperationsData["dataSource"] }) {
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

function MedicalTable({ records }: { records: MedicalResponse[] }) {
  return <DataTable headers={["Reference", "Type", "Status", "Severity", "Student", "Summary", "Audit"]} rows={records.map((record) => [record.recordReference, record.recordType, record.status, record.severity, record.studentProfileId, record.visibleSummary, record.auditTrail.join(", ")])} />;
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No medical records returned for this scope.</p>;
  return <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 800 }}><thead><tr>{headers.map((header) => <th key={header} style={thStyle}>{header}</th>)}</tr></thead><tbody>{rows.map((row) => <tr key={row.join(":")}>{row.map((cell, index) => <td key={`${cell}-${index}`} style={{ padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: index === 0 ? "#101828" : "#344054", fontWeight: index === 0 ? 800 : 500 }}>{cell}</td>)}</tr>)}</tbody></table>;
}

function panelTitle(audience: MedicalAudience, view: MedicalView) {
  if (audience === "guardian") return "Guardian medical updates";
  if (audience === "student") return "Student medical summary";
  return ({ overview: "Medical operations", records: "Medical records", emergency: "Emergency access", incidents: "Incident logging", notifications: "Medical notifications", history: "History and review", configuration: "Medical controls" } satisfies Record<MedicalView, string>)[view];
}

const cardStyle = { background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 18, boxShadow: "0 1px 2px rgba(15,23,42,.08)" } satisfies React.CSSProperties;
const thStyle = { textAlign: "left", color: "#475467", fontSize: 12, textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" } satisfies React.CSSProperties;
const tabStyle = (active: boolean) => ({ textDecoration: "none", color: active ? "#1d4ed8" : "#1f2937", border: `1px solid ${active ? "#3b82f6" : "#cbd5e1"}`, background: active ? "#eff6ff" : "#fff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 });
