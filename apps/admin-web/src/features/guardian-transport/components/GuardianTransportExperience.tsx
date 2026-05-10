import type { ReactNode } from "react";
import {
  loadGuardianTransportData,
  type GuardianTransportData,
} from "../api/client";
import { transportDemoData, transportLabel } from "../../transport/api/client";

export type GuardianTransportSection = "overview" | "progress" | "eta" | "notifications";

const guardianTabs: Array<[GuardianTransportSection, string, string]> = [
  ["overview", "Plan", "/guardian/transport"],
  ["progress", "Progress", "/guardian/transport/progress"],
  ["eta", "ETA", "/guardian/transport/eta"],
  ["notifications", "Notifications", "/guardian/transport/notifications"],
];

const labelMaps = {
  serviceDirection: { 0: "Pickup", 1: "Dropoff", 2: "Both", 3: "Combined" },
  visibilityState: { 0: "Guardian Visible", 1: "Staff Only", 2: "Suspended" },
  assignmentStatus: { 0: "Draft", 1: "Active", 2: "Suspended", 3: "Expired", 4: "Removed" },
  etaState: { 0: "Available", 1: "Unavailable", 2: "Stale", 3: "Needs Review" },
  confidenceState: { 0: "High", 1: "Medium", 2: "Low", 3: "Unavailable" },
  notificationStatus: { 0: "Eligible", 1: "Created", 2: "Visible", 3: "Suppressed", 4: "Withdrawn", 5: "Failed" },
};

export async function GuardianTransportExperience({ section = "overview" }: { section?: GuardianTransportSection }) {
  const data = await loadGuardianTransportData();

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>Guardian transport</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>Student Bus Tracking</h1>
          <p style={{ maxWidth: 860, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>
            Parent view for linked-student bus plan, pickup ETA, onboard progress, drop status, and transport notifications.
          </p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>
        {guardianTabs.map(([key, label, href]) => (
          <a key={key} href={href} style={{ textDecoration: "none", color: section === key ? "#1d4ed8" : "#1f2937", border: `1px solid ${section === key ? "#3b82f6" : "#cbd5e1"}`, background: section === key ? "#eff6ff" : "#ffffff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 }}>{label}</a>
        ))}
      </nav>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.15fr) minmax(300px, .85fr)", gap: 18 }}>
        <Panel title={panelTitle(section)}>
          <SectionContent section={section} data={data} />
        </Panel>
        <Panel title="Parent privacy boundary">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Linked student only" detail={`This view is scoped to ${data.studentProfileId}; unrelated students are not listed.`} />
            <Boundary title="Exact location window" detail="Exact bus location is visible only while the linked student is onboard." />
            <Boundary title="Before and after trip" detail="Before boarding the parent sees ETA; after drop the parent sees drop status, not live location." />
            <Boundary title="No staff controls" detail="Parents cannot mutate routes, assignments, scans, trips, ETA, or notification audit records from this view." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ section, data }: { section: GuardianTransportSection; data: GuardianTransportData }) {
  if (section === "progress") {
    return <StatusList title="Live progress" items={[
      { label: "Student", value: data.progress.studentProfileId, detail: `Trip ${shortId(data.progress.transportTripId)}` },
      { label: "Visibility phase", value: data.progress.visibilityPhase, detail: data.progress.exactLiveLocation || "Exact location not currently available" },
      { label: "Drop status", value: data.progress.dropStatus ?? "Pending", detail: "Visible after school receives drop scan evidence" },
    ]} />;
  }

  if (section === "eta") {
    const eta = data.eta.pickupEta;
    return <StatusList title="Pickup ETA" items={[
      { label: "ETA", value: eta?.estimatedArrivalTime ? formatTime(eta.estimatedArrivalTime) : data.progress.pickupEta ? formatTime(data.progress.pickupEta) : "Unavailable", detail: data.eta.visibilityPhase },
      { label: "State", value: display(eta?.etaState, labelMaps.etaState), detail: `Confidence ${display(eta?.confidenceState, labelMaps.confidenceState)}` },
      { label: "Exact location", value: data.eta.exactLiveLocationAvailable ? "Available while onboard" : "Hidden", detail: "Protected by guardian visibility rules" },
    ]} />;
  }

  if (section === "notifications") {
    return <DataTable headers={["Student", "Event", "Status", "Visible status"]} rows={data.notifications.notifications.map((notification) => [notification.studentProfileId, display(notification.eventType), display(notification.notificationStatus, labelMaps.notificationStatus), notification.visibleStatus || notification.suppressionReason])} />;
  }

  return (
    <Stack>
      <StatusList title="Today" items={[
        { label: "Student", value: data.studentProfileId, detail: `${data.plan.assignments.length} active transport plan record` },
        { label: "Route", value: routeCode(data.plan.assignments[0]?.transportRouteId ?? ""), detail: vehicleCode(data.plan.assignments[0]?.transportVehicleId ?? "") },
        { label: "Visibility", value: display(data.plan.assignments[0]?.visibilityState, labelMaps.visibilityState), detail: "Approved guardian link required" },
      ]} />
      <DataTable headers={["Student", "Route", "Vehicle", "Direction", "Status"]} rows={data.plan.assignments.map((assignment) => [assignment.studentProfileId, routeCode(assignment.transportRouteId), vehicleCode(assignment.transportVehicleId ?? ""), display(assignment.serviceDirection, labelMaps.serviceDirection), display(assignment.assignmentStatus, labelMaps.assignmentStatus)])} />
    </Stack>
  );
}

function DataSourceBadge({ source }: { source: GuardianTransportData["dataSource"] }) {
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

function StatusList({ title, items }: { title: string; items: Array<{ label: string; value: string; detail: string }> }) {
  return (
    <section style={{ display: "grid", gap: 12 }}>
      <h3 style={{ margin: 0, fontSize: 18 }}>{title}</h3>
      {items.map((item) => (
        <div key={item.label}>
          <strong>{item.label}</strong>
          <div style={{ color: "#64748b", marginTop: 4 }}>{item.value} - {item.detail}</div>
        </div>
      ))}
    </section>
  );
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No linked-student transport records returned.</p>;

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

function panelTitle(section: GuardianTransportSection) {
  const titles: Record<GuardianTransportSection, string> = { overview: "Linked-student transport plan", progress: "Trip progress", eta: "Pickup ETA", notifications: "Transport notifications" };
  return titles[section];
}

function display(value: unknown, labels: Record<number, string> = {}) {
  return transportLabel(value as string | number | null | undefined, labels);
}

function shortId(value: string) {
  return value.length > 12 ? value.slice(0, 8) : value;
}

function formatTime(value: string) {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return date.toLocaleTimeString("en", { hour: "2-digit", minute: "2-digit" });
}

function routeCode(routeId: string) {
  return transportDemoData.routes.find((route) => route.transportRouteId === routeId)?.routeCode ?? routeId;
}

function vehicleCode(vehicleId: string) {
  return transportDemoData.vehicles.find((vehicle) => vehicle.transportVehicleId === vehicleId)?.vehicleCode ?? vehicleId;
}
