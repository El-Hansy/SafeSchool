import type { ReactNode } from "react";
import {
  loadSchoolTransportOperations,
  transportLabel,
  type AssignmentResponse,
  type BoardingDropScanResponse,
  type EtaRecordResponse,
  type RouteResponse,
  type StopResponse,
  type TransportAnomalySummary,
  type TransportNotificationRecordResponse,
  type TransportOperationsData,
  type TransportReviewSummary,
  type TripProgressResponse,
  type VehicleResponse,
} from "../api/client";
import {
  BoardingDropScanAction,
  CreateAssignmentAction,
  CreateRouteAction,
  CreateScanReadyTripAction,
  CreateVehicleAction,
  EtaRecalculateAction,
  LocationUpdateAction,
  ManualTransportReviewAction,
} from "./TransportActionForms";

export type TransportSection = "overview" | "routes" | "assignments" | "scans" | "trips" | "eta" | "notifications" | "anomalies" | "settings" | "review";

const schoolTabs: Array<[TransportSection, string, string]> = [
  ["overview", "Overview", "/transport"],
  ["routes", "Routes", "/transport/routes"],
  ["assignments", "Assignments", "/transport/assignments"],
  ["scans", "Scans", "/transport/scans"],
  ["trips", "Trips", "/transport/trips"],
  ["eta", "ETA", "/transport/eta"],
  ["notifications", "Notifications", "/transport/notifications"],
  ["anomalies", "Anomalies", "/transport/anomalies"],
  ["settings", "Settings", "/transport/settings"],
  ["review", "Review", "/transport/review"],
];

const labelMaps = {
  routeStatus: { 0: "Draft", 1: "Active", 2: "Suspended", 3: "Retired" },
  vehicleStatus: { 0: "Draft", 1: "Active", 2: "Suspended", 3: "Retired" },
  serviceDirection: { 0: "Pickup", 1: "Dropoff", 2: "Both", 3: "Combined" },
  visibilityState: { 0: "Guardian Visible", 1: "Staff Only", 2: "Suspended" },
  assignmentStatus: { 0: "Draft", 1: "Active", 2: "Suspended", 3: "Expired", 4: "Removed" },
  tripStatus: { 0: "Planned", 1: "Active", 2: "Paused", 3: "Completed", 4: "Cancelled", 5: "Needs Review" },
  scanDirection: { 0: "Boarding", 1: "Drop" },
  scanDecision: { 0: "Accepted", 1: "Denied", 2: "Flagged", 3: "Needs Review", 4: "Duplicate" },
  progressState: { 0: "Not Started", 1: "En Route", 2: "Approaching Stop", 3: "At Stop", 4: "Delayed", 5: "Completed" },
  etaState: { 0: "Available", 1: "Unavailable", 2: "Stale", 3: "Needs Review" },
  confidenceState: { 0: "High", 1: "Medium", 2: "Low", 3: "Unavailable" },
  notificationStatus: { 0: "Eligible", 1: "Created", 2: "Visible", 3: "Suppressed", 4: "Withdrawn", 5: "Failed" },
  reviewStatus: { 0: "Not Required", 1: "Needs Review", 2: "In Review", 3: "Corrected", 4: "Closed", 5: "Rejected" },
  freshnessStatus: { 0: "Current", 1: "Stale", 2: "Untrusted", 3: "Unavailable", 4: "Retained Summary Only" },
};

export async function TransportOperationsPage({ section = "overview" }: { section?: TransportSection }) {
  const data = await loadSchoolTransportOperations();
  const activeRoutes = data.routes.filter((route) => display(route.routeStatus, labelMaps.routeStatus) === "Active").length;
  const activeTrips = data.trips.filter((trip) => display(trip.tripStatus, labelMaps.tripStatus) === "Active").length;
  const needsReview = data.scans.filter((scan) => display(scan.reviewStatus, labelMaps.reviewStatus) !== "Not Required").length;

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 004</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>Transport Command Center</h1>
          <p style={{ maxWidth: 940, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>
            Route planning, student bus assignments, driver boarding and drop scans, live trip progress, ETA calculation, guardian visibility, notifications, review, and audit evidence.
          </p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>
        {schoolTabs.map(([key, label, href]) => (
          <a key={key} href={href} style={{ textDecoration: "none", color: section === key ? "#1d4ed8" : "#1f2937", border: `1px solid ${section === key ? "#3b82f6" : "#cbd5e1"}`, background: section === key ? "#eff6ff" : "#ffffff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 }}>{label}</a>
        ))}
      </nav>

      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))", gap: 16, marginBottom: 24 }}>
        <Metric label="Active routes" value={String(activeRoutes)} detail={`${data.routes.length} route records`} />
        <Metric label="Students assigned" value={String(data.assignments.length)} detail={`${data.assignments.filter((item) => display(item.visibilityState, labelMaps.visibilityState) === "Guardian Visible").length} guardian visible`} />
        <Metric label="Active trips" value={String(activeTrips)} detail={`${data.trips.length} trips loaded`} />
        <Metric label="Scans loaded" value={String(data.scans.length)} detail={`${needsReview} need review`} />
        <Metric label="ETA records" value={String(data.etaRecords.length)} detail={`${data.etaRecords.filter((eta) => display(eta.freshnessStatus, labelMaps.freshnessStatus) === "Current").length} current`} />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.25fr) minmax(320px, .85fr)", gap: 18 }}>
        <Panel title={panelTitle(section)}>
          <SectionContent section={section} data={data} />
        </Panel>
        <Panel title="Safety boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="No campus mutation" detail="Transport scans do not create attendance or campus access outcomes." />
            <Boundary title="Driver registration" detail="Drivers register boarding and drop events through scan submissions tied to trip, stop sequence, credential, actor, and device evidence." />
            <Boundary title="Guardian visibility" detail="Parents see only linked-student route plan, ETA, trip phase, and notifications; exact location is limited to onboard visibility." />
            <Boundary title="Idempotency" detail="Client request, scan, location, and batch IDs protect retries and offline sync." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ section, data }: { section: TransportSection; data: TransportOperationsData }) {
  if (section === "routes") {
    return <Stack><CreateRouteAction data={data} /><RouteTable routes={data.routes} /><StopTable stops={data.stops} /></Stack>;
  }

  if (section === "assignments") {
    return <Stack><CreateVehicleAction data={data} /><CreateAssignmentAction data={data} /><VehicleTable vehicles={data.vehicles} /><AssignmentTable data={data} /></Stack>;
  }

  if (section === "scans") return <Stack><BoardingDropScanAction data={data} /><ScanTable data={data} /></Stack>;
  if (section === "trips") return <Stack><CreateScanReadyTripAction data={data} /><LocationUpdateAction data={data} /><TripTable data={data} /></Stack>;
  if (section === "eta") return <Stack><EtaRecalculateAction data={data} /><EtaTable data={data} /></Stack>;
  if (section === "notifications") return <NotificationTable data={data} />;
  if (section === "anomalies") return <AnomalyTable anomalies={data.anomalies} />;
  if (section === "settings") return <RuleSettings data={data} />;
  if (section === "review") return <Stack><ManualTransportReviewAction data={data} /><ReviewTable summaries={data.reviewSummaries} /></Stack>;

  return (
    <Stack>
      <StatusList title="Today" items={[
        { label: routeCode(data, data.routes[0]?.transportRouteId ?? ""), value: `${display(data.trips[0]?.tripStatus, labelMaps.tripStatus)} trip`, detail: `${data.scans.filter((scan) => scan.transportTripId === data.trips[0]?.transportTripId).length} scan events loaded` },
        { label: "Scan review", value: `${data.scans.filter((scan) => display(scan.reviewStatus, labelMaps.reviewStatus) !== "Not Required").length} records`, detail: "Invalid credential and stop mismatch records stay in review" },
        { label: "Guardian visibility", value: "Scoped", detail: "Linked guardians see only their student transport status" },
      ]} />
      <RouteTable routes={data.routes.slice(0, 3)} />
    </Stack>
  );
}

function DataSourceBadge({ source }: { source: TransportOperationsData["dataSource"] }) {
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

function RouteTable({ routes }: { routes: RouteResponse[] }) {
  return <DataTable headers={["Code", "Name", "Direction", "Status", "Version"]} rows={routes.map((route) => [route.routeCode, route.routeName, display(route.serviceDirection, labelMaps.serviceDirection), display(route.routeStatus, labelMaps.routeStatus), route.routeVersion])} />;
}

function StopTable({ stops }: { stops: StopResponse[] }) {
  return <DataTable headers={["Code", "Name", "Pickup", "Drop", "Status"]} rows={stops.map((stop) => [stop.stopCode, stop.stopName, yesNo(stop.pickupAllowed), yesNo(stop.dropAllowed), display(stop.stopStatus, labelMaps.routeStatus)])} />;
}

function VehicleTable({ vehicles }: { vehicles: VehicleResponse[] }) {
  return <DataTable headers={["Code", "Name", "Capacity", "Status"]} rows={vehicles.map((vehicle) => [vehicle.vehicleCode, vehicle.vehicleName, String(vehicle.capacity), display(vehicle.vehicleStatus, labelMaps.vehicleStatus)])} />;
}

function AssignmentTable({ data }: { data: TransportOperationsData }) {
  return <DataTable headers={["Student", "Route", "Vehicle", "Direction", "Visibility", "Status"]} rows={data.assignments.map((assignment: AssignmentResponse) => [assignment.studentProfileId, routeCode(data, assignment.transportRouteId), vehicleCode(data, assignment.transportVehicleId ?? ""), display(assignment.serviceDirection, labelMaps.serviceDirection), display(assignment.visibilityState, labelMaps.visibilityState), display(assignment.assignmentStatus, labelMaps.assignmentStatus)])} />;
}

function ScanTable({ data }: { data: TransportOperationsData }) {
  return <DataTable headers={["Time", "Student", "Stop", "Scan", "Decision", "After"]} rows={data.scans.map((scan: BoardingDropScanResponse) => [formatTime(scan.localScanTime), scan.studentProfileId, stopFromSequence(data, scan.routeStopSequenceId), display(scan.scanDirection, labelMaps.scanDirection), display(scan.scanDecision, labelMaps.scanDecision), transportLabel(scan.transportStatusAfter)])} />;
}

function TripTable({ data }: { data: TransportOperationsData }) {
  return <DataTable headers={["Trip", "Route", "Vehicle", "Status", "Progress", "Freshness"]} rows={data.trips.map((trip: TripProgressResponse) => [shortId(trip.transportTripId), routeCode(data, trip.transportRouteId), vehicleCode(data, trip.transportVehicleId), display(trip.tripStatus, labelMaps.tripStatus), display(trip.latestLocation?.progressState, labelMaps.progressState), display(trip.latestLocation?.freshnessStatus, labelMaps.freshnessStatus)])} />;
}

function EtaTable({ data }: { data: TransportOperationsData }) {
  return <DataTable headers={["Student", "Stop", "ETA", "State", "Confidence", "Freshness"]} rows={data.etaRecords.map((eta: EtaRecordResponse) => [eta.studentProfileId, stopFromSequence(data, eta.routeStopSequenceId), eta.estimatedArrivalTime ? formatTime(eta.estimatedArrivalTime) : "Unavailable", display(eta.etaState, labelMaps.etaState), display(eta.confidenceState, labelMaps.confidenceState), display(eta.freshnessStatus, labelMaps.freshnessStatus)])} />;
}

function NotificationTable({ data }: { data: TransportOperationsData }) {
  return <DataTable headers={["Guardian", "Student", "Event", "Status", "Visible status"]} rows={data.notifications.map((notification: TransportNotificationRecordResponse) => [notification.guardianRecordId, notification.studentProfileId, display(notification.eventType), display(notification.notificationStatus, labelMaps.notificationStatus), notification.visibleStatus || notification.suppressionReason])} />;
}

function AnomalyTable({ anomalies }: { anomalies: TransportAnomalySummary[] }) {
  return <DataTable headers={["Type", "Severity", "Target", "Status", "Evidence"]} rows={anomalies.map((anomaly) => [anomaly.anomalyType, anomaly.severity, anomaly.target, anomaly.status, anomaly.evidence])} />;
}

function ReviewTable({ summaries }: { summaries: TransportReviewSummary[] }) {
  return <DataTable headers={["Scope", "Accepted", "Needs review", "Current locations", "Notifications", "Open anomalies"]} rows={summaries.map((summary) => [summary.scopeReference, String(summary.acceptedScans), String(summary.needsReviewScans), String(summary.currentLocations), String(summary.visibleNotifications), String(summary.openAnomalies)])} />;
}

function RuleSettings({ data }: { data: TransportOperationsData }) {
  return (
    <dl style={{ display: "grid", gap: 14, margin: 0 }}>
      <Boundary title="Rule status" detail={display(data.ruleSetting.status)} />
      <Boundary title="Location staleness threshold" detail={data.ruleSetting.locationStalenessThreshold} />
      <Boundary title="Location detail retention" detail={`${data.ruleSetting.locationDetailRetentionDays} days`} />
      <Boundary title="Change reason" detail={data.ruleSetting.changeReason} />
    </dl>
  );
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No records returned for this tenant.</p>;

  return (
    <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 720 }}>
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

function panelTitle(section: TransportSection) {
  const titles: Record<TransportSection, string> = { overview: "Transport operations", routes: "Route and stop management", assignments: "Vehicle and student assignments", scans: "Boarding and drop scans", trips: "Live trip tracking", eta: "ETA calculation", notifications: "Guardian notifications", anomalies: "Anomaly review", settings: "Transport rule settings", review: "Operational review" };
  return titles[section];
}

function display(value: unknown, labels: Record<number, string> = {}) {
  return transportLabel(value as string | number | null | undefined, labels);
}

function yesNo(value: boolean) {
  return value ? "Yes" : "No";
}

function shortId(value: string) {
  return value.length > 12 ? value.slice(0, 8) : value;
}

function formatTime(value: string) {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return date.toLocaleTimeString("en", { hour: "2-digit", minute: "2-digit" });
}

function routeCode(data: TransportOperationsData, routeId: string) {
  return data.routes.find((route) => route.transportRouteId === routeId)?.routeCode ?? routeId;
}

function vehicleCode(data: TransportOperationsData, vehicleId: string) {
  return data.vehicles.find((vehicle) => vehicle.transportVehicleId === vehicleId)?.vehicleCode ?? vehicleId;
}

function stopFromSequence(data: TransportOperationsData, sequenceId: string) {
  const sequence = data.routeStopSequences.find((item) => item.routeStopSequenceId === sequenceId);
  return data.stops.find((stop) => stop.transportStopId === sequence?.transportStopId)?.stopName ?? sequenceId;
}
