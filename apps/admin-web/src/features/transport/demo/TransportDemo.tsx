import type { CSSProperties, ReactNode } from "react";

type SchoolSection =
  | "overview"
  | "routes"
  | "assignments"
  | "scans"
  | "trips"
  | "eta"
  | "notifications"
  | "anomalies"
  | "settings"
  | "review";

type GuardianSection = "overview" | "progress" | "eta" | "notifications";

const schoolNav: Array<{ href: string; label: string; section: SchoolSection }> = [
  { href: "/transport", label: "Overview", section: "overview" },
  { href: "/transport/routes", label: "Routes", section: "routes" },
  { href: "/transport/assignments", label: "Assignments", section: "assignments" },
  { href: "/transport/scans", label: "Scans", section: "scans" },
  { href: "/transport/trips", label: "Trips", section: "trips" },
  { href: "/transport/eta", label: "ETA", section: "eta" },
  { href: "/transport/notifications", label: "Notifications", section: "notifications" },
  { href: "/transport/anomalies", label: "Anomalies", section: "anomalies" },
  { href: "/transport/settings", label: "Settings", section: "settings" },
  { href: "/transport/review", label: "Review", section: "review" },
];

const guardianNav: Array<{ href: string; label: string; section: GuardianSection }> = [
  { href: "/guardian/transport", label: "Plan", section: "overview" },
  { href: "/guardian/transport/progress", label: "Progress", section: "progress" },
  { href: "/guardian/transport/eta", label: "ETA", section: "eta" },
  { href: "/guardian/transport/notifications", label: "Notifications", section: "notifications" },
];

const routes = [
  { code: "NORTH-AM", name: "North Morning Route", stops: 5, status: "Active", version: "v3", readiness: "Ready" },
  { code: "EAST-PM", name: "East Dropoff Route", stops: 7, status: "Active", version: "v2", readiness: "Ready" },
  { code: "WEST-AM", name: "West Morning Route", stops: 4, status: "Draft", version: "v1", readiness: "Needs stop review" },
];

const assignments = [
  { student: "Amina Hassan", route: "NORTH-AM", vehicle: "BUS-12", pickup: "Gate 4 Community", drop: "North Campus", visibility: "Guardian visible" },
  { student: "Omar Saleh", route: "NORTH-AM", vehicle: "BUS-12", pickup: "Library Corner", drop: "North Campus", visibility: "Guardian visible" },
  { student: "Lina Kareem", route: "EAST-PM", vehicle: "BUS-08", pickup: "East Campus", drop: "Palm Stop", visibility: "Staff only" },
];

const scans = [
  { time: "06:42", student: "Amina Hassan", event: "Boarding", decision: "Accepted", source: "NFC", status: "Onboard" },
  { time: "06:44", student: "Omar Saleh", event: "Boarding", decision: "Accepted", source: "QR", status: "Onboard" },
  { time: "06:47", student: "Unknown card", event: "Boarding", decision: "Flagged", source: "NFC", status: "Needs review" },
  { time: "07:18", student: "Amina Hassan", event: "Drop", decision: "Accepted", source: "NFC", status: "Dropped" },
];

const trips = [
  { id: "TRIP-042", route: "NORTH-AM", bus: "BUS-12", device: "staff-device-12", status: "Active", progress: "Approaching Stop 4", freshness: "Current" },
  { id: "TRIP-043", route: "NORTH-AM", bus: "BUS-18", device: "staff-device-18", status: "Active", progress: "En route", freshness: "Current" },
  { id: "TRIP-031", route: "EAST-PM", bus: "BUS-08", device: "staff-device-08", status: "Planned", progress: "Not started", freshness: "Unavailable" },
];

const etas = [
  { stop: "Gate 4 Community", student: "Amina Hassan", eta: "06:40", confidence: "High", freshness: "Current" },
  { stop: "Library Corner", student: "Omar Saleh", eta: "06:48", confidence: "Medium", freshness: "Current" },
  { stop: "Palm Stop", student: "Lina Kareem", eta: "Unavailable", confidence: "Unavailable", freshness: "No current progress" },
];

const notifications = [
  { guardian: "Mariam Hassan", student: "Amina Hassan", event: "Boarding", status: "Visible", reason: "Accepted scan" },
  { guardian: "Saleh Omar", student: "Omar Saleh", event: "ETA change", status: "Visible", reason: "Material ETA change" },
  { guardian: "Kareem Parent", student: "Lina Kareem", event: "Delay", status: "Suppressed", reason: "Stale location evidence" },
];

const anomalies = [
  { type: "Invalid credential", severity: "High", target: "Unknown card", status: "Open", evidence: "NFC scan scan-889" },
  { type: "Wrong stop", severity: "Medium", target: "Omar Saleh", status: "Assigned", evidence: "Stop mismatch" },
  { type: "Stale location", severity: "Low", target: "TRIP-031", status: "Resolved", evidence: "Mobile device retry" },
];

const stats = [
  { label: "Active routes", value: "2", detail: "1 draft pending review" },
  { label: "Students assigned", value: "128", detail: "117 guardian visible" },
  { label: "Active trips", value: "2", detail: "No bus/device conflict" },
  { label: "Scans today", value: "246", detail: "3 need review" },
  { label: "Current ETA records", value: "34", detail: "95% within freshness target" },
  { label: "Notifications", value: "91", detail: "7 suppressed with reason" },
];

const styles: Record<string, CSSProperties> = {
  page: {
    minHeight: "100vh",
    background: "#eef2f7",
  },
  shell: {
    maxWidth: "1280px",
    margin: "0 auto",
    padding: "28px",
    display: "grid",
    gap: "22px",
  },
  header: {
    display: "grid",
    gap: "18px",
    gridTemplateColumns: "minmax(0, 1fr) auto",
    alignItems: "end",
  },
  eyebrow: {
    margin: 0,
    color: "#5b6475",
    fontSize: "13px",
    fontWeight: 700,
    textTransform: "uppercase",
    letterSpacing: "0.04em",
  },
  title: {
    margin: "4px 0 0",
    color: "#101828",
    fontSize: "34px",
    lineHeight: 1.1,
  },
  subtitle: {
    margin: "10px 0 0",
    color: "#475467",
    maxWidth: "760px",
    lineHeight: 1.55,
  },
  badge: {
    display: "inline-flex",
    alignItems: "center",
    minHeight: "34px",
    padding: "0 12px",
    borderRadius: "6px",
    background: "#dff6e8",
    color: "#116c3d",
    fontWeight: 700,
    border: "1px solid #b8e7c9",
    whiteSpace: "nowrap",
  },
  nav: {
    display: "flex",
    flexWrap: "wrap",
    gap: "8px",
  },
  navLink: {
    padding: "9px 12px",
    borderRadius: "6px",
    border: "1px solid #d0d7e2",
    color: "#25324a",
    background: "#ffffff",
    textDecoration: "none",
    fontSize: "14px",
    fontWeight: 650,
  },
  navLinkActive: {
    borderColor: "#2f6fed",
    background: "#eaf1ff",
    color: "#174ea6",
  },
  grid: {
    display: "grid",
    gridTemplateColumns: "repeat(auto-fit, minmax(230px, 1fr))",
    gap: "14px",
  },
  panel: {
    background: "#ffffff",
    border: "1px solid #d8dee8",
    borderRadius: "8px",
    padding: "18px",
    boxShadow: "0 1px 2px rgba(16, 24, 40, 0.04)",
  },
  panelTitle: {
    margin: "0 0 12px",
    color: "#101828",
    fontSize: "18px",
  },
  statValue: {
    display: "block",
    color: "#101828",
    fontSize: "30px",
    fontWeight: 800,
    lineHeight: 1,
  },
  muted: {
    color: "#667085",
    lineHeight: 1.5,
  },
  tableWrap: {
    overflowX: "auto",
  },
  table: {
    width: "100%",
    borderCollapse: "collapse",
    minWidth: "720px",
  },
  th: {
    textAlign: "left",
    color: "#475467",
    fontSize: "12px",
    textTransform: "uppercase",
    padding: "10px 8px",
    borderBottom: "1px solid #e4e7ec",
  },
  td: {
    padding: "12px 8px",
    borderBottom: "1px solid #eef2f7",
    color: "#25324a",
  },
  status: {
    display: "inline-flex",
    padding: "4px 8px",
    borderRadius: "6px",
    background: "#f2f4f7",
    color: "#344054",
    fontSize: "12px",
    fontWeight: 700,
    whiteSpace: "nowrap",
  },
};

function PageShell({
  title,
  subtitle,
  active,
  children,
}: {
  title: string;
  subtitle: string;
  active: SchoolSection;
  children: ReactNode;
}) {
  return (
    <main style={styles.page}>
      <div style={styles.shell}>
        <header style={styles.header}>
          <div>
            <p style={styles.eyebrow}>SafeSchool Phase 3</p>
            <h1 style={styles.title}>{title}</h1>
            <p style={styles.subtitle}>{subtitle}</p>
          </div>
          <span style={styles.badge}>Demo data</span>
        </header>
        <nav aria-label="Transport sections" style={styles.nav}>
          {schoolNav.map((item) => (
            <a key={item.href} href={item.href} style={{ ...styles.navLink, ...(item.section === active ? styles.navLinkActive : {}) }}>
              {item.label}
            </a>
          ))}
        </nav>
        {children}
      </div>
    </main>
  );
}

function GuardianShell({
  active,
  children,
}: {
  active: GuardianSection;
  children: ReactNode;
}) {
  return (
    <main style={styles.page}>
      <div style={styles.shell}>
        <header style={styles.header}>
          <div>
            <p style={styles.eyebrow}>Guardian view</p>
            <h1 style={styles.title}>Student Transport</h1>
            <p style={styles.subtitle}>
              Linked guardian visibility shows pickup ETA before boarding, exact bus location while onboard,
              and drop status after arrival.
            </p>
          </div>
          <span style={styles.badge}>Linked student only</span>
        </header>
        <nav aria-label="Guardian transport sections" style={styles.nav}>
          {guardianNav.map((item) => (
            <a key={item.href} href={item.href} style={{ ...styles.navLink, ...(item.section === active ? styles.navLinkActive : {}) }}>
              {item.label}
            </a>
          ))}
        </nav>
        {children}
      </div>
    </main>
  );
}

function Table({
  columns,
  rows,
}: {
  columns: string[];
  rows: Array<Record<string, string | number>>;
}) {
  return (
    <div style={{ ...styles.panel, ...styles.tableWrap }}>
      <table style={styles.table}>
        <thead>
          <tr>
            {columns.map((column) => (
              <th key={column} style={styles.th}>{column}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {rows.map((row, index) => (
            <tr key={`${columns[0]}-${index}`}>
              {columns.map((column) => (
                <td key={column} style={styles.td}>
                  {String(row[column.toLowerCase().replaceAll(" ", "")] ?? row[column] ?? "")}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function StatusList({ title, items }: { title: string; items: Array<{ label: string; value: string; detail: string }> }) {
  return (
    <section style={styles.panel}>
      <h2 style={styles.panelTitle}>{title}</h2>
      <div style={{ display: "grid", gap: "12px" }}>
        {items.map((item) => (
          <div key={item.label} style={{ display: "grid", gap: "4px" }}>
            <strong>{item.label}</strong>
            <span style={styles.muted}>{item.value} - {item.detail}</span>
          </div>
        ))}
      </div>
    </section>
  );
}

export function TransportDemo({ section = "overview" }: { section?: SchoolSection }) {
  const title = section === "overview" ? "Transport Command Center" : `Transport ${schoolNav.find((item) => item.section === section)?.label}`;
  return (
    <PageShell
      title={title}
      subtitle="A school operations demo for route planning, student bus assignments, NFC/QR boarding and drop scans, live tracking, ETA calculation, notifications, anomaly review, and audit evidence."
      active={section}
    >
      {section === "overview" && (
        <>
          <section style={styles.grid}>
            {stats.map((stat) => (
              <article key={stat.label} style={styles.panel}>
                <span style={styles.statValue}>{stat.value}</span>
                <strong>{stat.label}</strong>
                <p style={{ ...styles.muted, margin: "8px 0 0" }}>{stat.detail}</p>
              </article>
            ))}
          </section>
          <section style={styles.grid}>
            <StatusList title="Today" items={[
              { label: "NORTH-AM", value: "TRIP-042 active", detail: "Amina and Omar onboard" },
              { label: "Scan review", value: "3 records", detail: "Invalid credential and stop mismatch" },
              { label: "Guardian visibility", value: "Current", detail: "Exact location only while onboard" },
            ]} />
            <StatusList title="Safety boundaries" items={[
              { label: "No campus mutation", value: "Enforced", detail: "Transport does not create attendance or access outcomes" },
              { label: "Idempotency", value: "Enabled", detail: "Client scan, batch, request, and location IDs" },
              { label: "Retention", value: "30 days", detail: "Detailed location converted to summaries" },
            ]} />
          </section>
        </>
      )}

      {section === "routes" && (
        <Table
          columns={["Code", "Name", "Stops", "Status", "Version", "Readiness"]}
          rows={routes}
        />
      )}

      {section === "assignments" && (
        <Table
          columns={["Student", "Route", "Vehicle", "Pickup", "Drop", "Visibility"]}
          rows={assignments}
        />
      )}

      {section === "scans" && (
        <Table
          columns={["Time", "Student", "Event", "Decision", "Source", "Status"]}
          rows={scans}
        />
      )}

      {section === "trips" && (
        <Table
          columns={["Id", "Route", "Bus", "Device", "Status", "Progress", "Freshness"]}
          rows={trips}
        />
      )}

      {section === "eta" && (
        <Table
          columns={["Stop", "Student", "Eta", "Confidence", "Freshness"]}
          rows={etas}
        />
      )}

      {section === "notifications" && (
        <Table
          columns={["Guardian", "Student", "Event", "Status", "Reason"]}
          rows={notifications}
        />
      )}

      {section === "anomalies" && (
        <Table
          columns={["Type", "Severity", "Target", "Status", "Evidence"]}
          rows={anomalies}
        />
      )}

      {section === "settings" && (
        <section style={styles.grid}>
          <StatusList title="Active rule setting" items={[
            { label: "Pickup window", value: "20 minutes", detail: "Pickup scans outside window need review" },
            { label: "ETA change threshold", value: "5 minutes", detail: "Material changes create notification records" },
            { label: "Clock drift", value: "3 minutes", detail: "Offline scans preserve local and received time" },
          ]} />
          <StatusList title="Feature gates" items={[
            { label: "Route and stop management", value: "Enabled", detail: "School operations" },
            { label: "Live tracking", value: "Enabled", detail: "Authorized mobile devices only" },
            { label: "Notifications", value: "Enabled", detail: "Suppression records preserve reasons" },
          ]} />
        </section>
      )}

      {section === "review" && (
        <section style={styles.grid}>
          <StatusList title="Review summary" items={[
            { label: "Accepted scans", value: "243", detail: "Visible to authorized reviewers" },
            { label: "Needs review", value: "3", detail: "No normal guardian status until approved" },
            { label: "Open anomalies", value: "2", detail: "Assigned to transport reviewer" },
          ]} />
          <StatusList title="Trace links" items={[
            { label: "Trip trace", value: "Under 60 seconds", detail: "Route, assignments, scans, locations, ETA, notifications" },
            { label: "Audit evidence", value: "Append-only", detail: "Sensitive mutations require audit write" },
            { label: "Scope review", value: "Passed", detail: "No attendance or campus access side effects" },
          ]} />
        </section>
      )}
    </PageShell>
  );
}

export function GuardianTransportDemo({ section = "overview" }: { section?: GuardianSection }) {
  return (
    <GuardianShell active={section}>
      {section === "overview" && (
        <section style={styles.grid}>
          <StatusList title="Amina Hassan" items={[
            { label: "Assigned route", value: "NORTH-AM", detail: "Bus 12, Gate 4 Community pickup" },
            { label: "Visibility", value: "Guardian visible", detail: "Approved active guardian link" },
            { label: "Today", value: "Boarded", detail: "Exact location available until drop" },
          ]} />
          <StatusList title="Privacy boundary" items={[
            { label: "Other students", value: "Hidden", detail: "No unrelated route, bus, or stop leakage" },
            { label: "Before boarding", value: "ETA only", detail: "Exact bus location is withheld" },
            { label: "After drop", value: "Drop status only", detail: "Exact location is hidden again" },
          ]} />
        </section>
      )}
      {section === "progress" && (
        <Table
          columns={["Student", "Route", "Status", "Progress", "Freshness"]}
          rows={[{ student: "Amina Hassan", route: "NORTH-AM", status: "Onboard", progress: "Approaching Stop 4", freshness: "Current" }]}
        />
      )}
      {section === "eta" && (
        <Table
          columns={["Stop", "Student", "Eta", "Confidence", "Freshness"]}
          rows={[etas[0]]}
        />
      )}
      {section === "notifications" && (
        <Table
          columns={["Guardian", "Student", "Event", "Status", "Reason"]}
          rows={[notifications[0], notifications[1]]}
        />
      )}
    </GuardianShell>
  );
}
