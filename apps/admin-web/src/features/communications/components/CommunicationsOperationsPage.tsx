import type { ReactNode } from "react";
import {
  loadCommunicationsOperations,
  type CommunicationAudience,
  type CommunicationResponse,
  type CommunicationsOperationsData,
} from "../api/communicationsApi";
import { BroadcastAction, DirectMessageAction, SourceEventAction } from "./CommunicationActionForms";

export type CommunicationsView = "overview" | "notifications" | "messages" | "broadcasts";

const schoolTabs: Array<[CommunicationsView, string, string]> = [
  ["overview", "Overview", "/communications"],
  ["notifications", "Notifications", "/communications/notifications"],
  ["messages", "Messages", "/communications/messages"],
  ["broadcasts", "Broadcasts", "/communications/broadcasts"],
];

export async function CommunicationsOperationsPage({ audience = "school", view = "overview" }: { audience?: CommunicationAudience; view?: CommunicationsView }) {
  const data = await loadCommunicationsOperations();
  const title = audience === "school" ? "Communications Command Center" : audience === "guardian" ? "Guardian Notifications" : "Student Notifications";
  const notifications = audience === "guardian" ? data.guardianNotifications : audience === "student" ? data.studentNotifications : data.schoolEvents;

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 010</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>{title}</h1>
          <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>
            Notification source events, direct messaging, broadcasts, delivery attempts, acknowledgements, recipient visibility, moderation, and lifecycle audit.
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
        <Metric label="Unread" value={String(data.board.unread)} detail={`${notifications.length} records visible here`} />
        <Metric label="Broadcasts" value={String(data.board.broadcasts)} detail="Audience snapshots preserved" />
        <Metric label="Delivery exceptions" value={String(data.board.deliveryExceptions)} detail="Moderation and delivery review" />
        <Metric label="Capabilities" value={String(data.board.capabilities.length)} detail="Tenant communication controls" />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title={panelTitle(audience, view)}>
          <SectionContent audience={audience} view={view} data={data} notifications={notifications} />
        </Panel>
        <Panel title="Boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Source read-only" detail="Source events reference attendance, transport, wallet, medical, or complaint records without mutating them." />
            <Boundary title="Recipient snapshot" detail="Broadcasts and notifications preserve resolved recipients for audit and later review." />
            <Boundary title="Delivery evidence" detail="Messages record moderation checks and delivery attempts before visible status changes." />
            <Boundary title="Restricted detail" detail="Guardian and student notification center records show safe status without exposing staff-only source data." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ audience, view, data, notifications }: { audience: CommunicationAudience; view: CommunicationsView; data: CommunicationsOperationsData; notifications: CommunicationResponse[] }) {
  if (audience !== "school") return <CommunicationTable rows={notifications} />;
  if (view === "notifications") return <Stack><SourceEventAction schoolAccountId={data.schoolAccountId} /><CommunicationTable rows={notifications} /></Stack>;
  if (view === "messages") return <Stack><DirectMessageAction schoolAccountId={data.schoolAccountId} /><CommunicationTable rows={notifications} /></Stack>;
  if (view === "broadcasts") return <Stack><BroadcastAction schoolAccountId={data.schoolAccountId} /><CommunicationTable rows={notifications} /></Stack>;
  return <CommunicationTable rows={notifications} />;
}

function DataSourceBadge({ source }: { source: CommunicationsOperationsData["dataSource"] }) {
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

function CommunicationTable({ rows }: { rows: CommunicationResponse[] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No communications returned for this scope.</p>;

  return (
    <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 680 }}>
      <thead>
        <tr>{["Reference", "Status", "Recipients", "Evidence"].map((header) => <th key={header} style={{ textAlign: "left", color: "#475467", fontSize: 12, textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" }}>{header}</th>)}</tr>
      </thead>
      <tbody>
        {rows.map((row) => (
          <tr key={row.reference}>
            <td style={cellStyle(true)}>{row.reference}</td>
            <td style={cellStyle()}>{row.status}</td>
            <td style={cellStyle()}>{row.recipients.join(", ")}</td>
            <td style={cellStyle()}>{row.evidence.join(", ")}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

function cellStyle(primary = false) {
  return { padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: primary ? "#101828" : "#344054", fontWeight: primary ? 800 : 500 };
}

function panelTitle(audience: CommunicationAudience, view: CommunicationsView) {
  if (audience === "guardian") return "Guardian notification center";
  if (audience === "student") return "Student notification center";
  const titles: Record<CommunicationsView, string> = { overview: "Communication activity", notifications: "Source event notifications", messages: "Direct messaging", broadcasts: "Broadcast announcements" };
  return titles[view];
}
