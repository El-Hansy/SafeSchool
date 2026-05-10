import type { ReactNode } from "react";
import { loadCommunicationsOperations, type CommunicationResponse, type CommunicationsOperationsData } from "../api/communicationsApi";
import { BroadcastAction, DirectMessageAction, SourceEventAction } from "./CommunicationActionForms";

export type CommunicationSecondaryView =
  | "notification-detail"
  | "acknowledgements"
  | "delivery"
  | "summaries"
  | "history"
  | "conversations"
  | "conversation-detail"
  | "conversation-new"
  | "configuration"
  | "trace"
  | "exceptions"
  | "audience-rule"
  | "template"
  | "broadcast-new"
  | "broadcast-detail"
  | "moderation";

export async function CommunicationsSecondaryPage({ view, recordId }: { view: CommunicationSecondaryView; recordId?: string }) {
  const data = await loadCommunicationsOperations();
  const record = findRecord(data, recordId);
  const rule = data.audienceRules.find((item) => item.ruleId === recordId) ?? data.audienceRules[0];
  const template = data.templates.find((item) => item.templateId === recordId) ?? data.templates[0];

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 010</div>
          <h1 style={{ fontSize: 40, lineHeight: 1.05, margin: "8px 0" }}>{titleFor(view)}</h1>
          <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 19, lineHeight: 1.45, margin: 0 }}>{detailFor(view)}</p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title="Operational records">
          <ViewContent view={view} data={data} record={record} rule={rule} template={template} />
        </Panel>
        <Panel title="Delivery boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Tenant scoped" detail={data.schoolAccountId} />
            <Boundary title="Recipient snapshot" detail="Recipients are captured at send time for audit and delivery evidence." />
            <Boundary title="Moderation and quiet hours" detail="Messages and broadcasts pass audience, moderation, quiet-hour, and suppression checks." />
            <Boundary title="No source mutation" detail="Communications reads source events and sends messages without mutating attendance, transport, wallet, complaints, documents, or admin outcomes." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function ViewContent({
  view,
  data,
  record,
  rule,
  template,
}: {
  view: CommunicationSecondaryView;
  data: CommunicationsOperationsData;
  record?: CommunicationResponse;
  rule?: CommunicationsOperationsData["audienceRules"][number];
  template?: CommunicationsOperationsData["templates"][number];
}) {
  if (view === "notification-detail" && record) return <CommunicationTable records={[record]} />;
  if (view === "acknowledgements") return <CommunicationTable records={data.acknowledgements} />;
  if (view === "delivery") return <CommunicationTable records={data.delivery} />;
  if (view === "summaries") return <CommunicationTable records={data.summaries} />;
  if (view === "history") return <CommunicationTable records={data.history} />;
  if (view === "conversations") return <Stack><DirectMessageAction schoolAccountId={data.schoolAccountId} /><CommunicationTable records={data.conversations} /></Stack>;
  if (view === "conversation-detail" && record) return <CommunicationTable records={[record]} />;
  if (view === "conversation-new") return <DirectMessageAction schoolAccountId={data.schoolAccountId} />;
  if (view === "configuration") return <Stack><DataTable headers={["Rule", "Scope", "Channel", "Status", "Evidence"]} rows={data.audienceRules.map((item) => [item.ruleId, item.scope, item.channel, item.status, item.evidence.join(", ")])} /><DataTable headers={["Template", "Channel", "Locale", "Status", "Evidence"]} rows={data.templates.map((item) => [item.templateId, item.channel, item.locale, item.status, item.evidence.join(", ")])} /></Stack>;
  if (view === "trace" && record) return <DataTable headers={["Reference", "Trace step"]} rows={["source-event", "template", "recipient-snapshot", "delivery-attempt", "read-state", "acknowledgement", "audit"].map((step) => [record.reference, step])} />;
  if (view === "exceptions") return <CommunicationTable records={data.exceptions} />;
  if (view === "audience-rule" && rule) return <DataTable headers={["Rule", "Scope", "Channel", "Status", "Evidence"]} rows={[[rule.ruleId, rule.scope, rule.channel, rule.status, rule.evidence.join(", ")]]} />;
  if (view === "template" && template) return <DataTable headers={["Template", "Channel", "Locale", "Status", "Evidence"]} rows={[[template.templateId, template.channel, template.locale, template.status, template.evidence.join(", ")]]} />;
  if (view === "broadcast-new") return <BroadcastAction schoolAccountId={data.schoolAccountId} />;
  if (view === "broadcast-detail" && record) return <CommunicationTable records={[record]} />;
  if (view === "moderation") return <Stack><SourceEventAction schoolAccountId={data.schoolAccountId} /><CommunicationTable records={data.moderation} /></Stack>;

  return <p style={{ color: "#667085", margin: 0 }}>No communication records returned for this scope.</p>;
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

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)", overflowX: "auto" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
}

function CommunicationTable({ records }: { records: CommunicationResponse[] }) {
  return <DataTable headers={["Reference", "Status", "Recipients", "Evidence"]} rows={records.map((record) => [record.reference, record.status, record.recipients.join(", "), record.evidence.join(", ")])} />;
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

function findRecord(data: CommunicationsOperationsData, recordId?: string) {
  return [
    ...data.schoolEvents,
    ...data.guardianNotifications,
    ...data.studentNotifications,
    ...data.acknowledgements,
    ...data.delivery,
    ...data.summaries,
    ...data.history,
    ...data.conversations,
    ...data.broadcasts,
    ...data.moderation,
    ...data.exceptions,
  ].find((item) => item.reference === recordId) ?? data.schoolEvents[0];
}

function titleFor(view: CommunicationSecondaryView) {
  const titles: Record<CommunicationSecondaryView, string> = {
    "notification-detail": "Notification Detail",
    acknowledgements: "Acknowledgements",
    delivery: "Delivery Attempts",
    summaries: "Communication Summaries",
    history: "Communication History",
    conversations: "Conversations",
    "conversation-detail": "Conversation Detail",
    "conversation-new": "New Conversation",
    configuration: "Communication Configuration",
    trace: "Communication Trace",
    exceptions: "Communication Exceptions",
    "audience-rule": "Audience Rule Detail",
    template: "Template Detail",
    "broadcast-new": "New Broadcast",
    "broadcast-detail": "Broadcast Detail",
    moderation: "Moderation Queue",
  };
  return titles[view];
}

function detailFor(view: CommunicationSecondaryView) {
  const details: Record<CommunicationSecondaryView, string> = {
    "notification-detail": "Open one notification with recipient snapshot, delivery status, read state, and audit evidence.",
    acknowledgements: "Review read receipts, guardian or student acknowledgements, correlation IDs, and audit state.",
    delivery: "Inspect delivery attempts, retries, provider references, suppression reasons, and owner follow-up.",
    summaries: "Compare communication counts, delivery health, suppression, unread state, and exception trends.",
    history: "Search communication lifecycle history across source events, messages, broadcasts, delivery, and acknowledgement.",
    conversations: "Review direct conversations with recipients, moderation state, delivery evidence, and next action.",
    "conversation-detail": "Open one direct conversation with safe body handling, recipient context, and trace evidence.",
    "conversation-new": "Start a direct message with recipient scope, body, moderation checks, and audit evidence.",
    configuration: "Manage templates, audience rules, quiet-hour policy, channel preferences, and suppression defaults.",
    trace: "Trace one communication through source event, template, recipient snapshot, delivery attempt, read state, acknowledgement, and audit.",
    exceptions: "Review blocked, failed, suppressed, or retrying communication records requiring staff follow-up.",
    "audience-rule": "Review one audience rule with channel scope, visibility, snapshot rules, and audit evidence.",
    template: "Review one template with channel, locale, approval, version, and moderation evidence.",
    "broadcast-new": "Publish a broadcast with audience snapshot, quiet-hour handling, moderation, and audit evidence.",
    "broadcast-detail": "Open one broadcast with audience, delivery status, suppression, acknowledgement, and trace evidence.",
    moderation: "Review message and broadcast content that needs policy checks before delivery.",
  };
  return details[view];
}
