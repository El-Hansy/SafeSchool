export type SafeSchoolModuleLink = {
  phase: string;
  title: string;
  href: string;
  summary: string;
  demoAction: string;
};

export const safeSchoolModuleLinks: SafeSchoolModuleLink[] = [
  {
    phase: "001",
    title: "Platform Foundations",
    href: "/admin",
    summary: "Tenant boundaries, feature flags, audit evidence, and operational guardrails.",
    demoAction: "Start with the admin dashboard and explain the source-of-truth model.",
  },
  {
    phase: "002",
    title: "Identity & Access",
    href: "/identity-access",
    summary: "Student profiles, guardian links, NFC cards, QR fallback, roles, and permissions.",
    demoAction: "Show a student profile, linked guardian, and active NFC credential.",
  },
  {
    phase: "003",
    title: "Attendance & Campus Access",
    href: "/attendance-access",
    summary: "Gate configuration, scan capture, attendance generation, anomalies, and notifications.",
    demoAction: "Show gate scan evidence and generated attendance status.",
  },
  {
    phase: "004",
    title: "Transport & Bus Tracking",
    href: "/transport",
    summary: "Routes, student assignments, boarding/drop scans, live location, ETA, and review.",
    demoAction: "Show route progress, ETA, and guardian-visible bus status.",
  },
  {
    phase: "005",
    title: "Wallet & Payments",
    href: "/wallet",
    summary: "Student wallets, guardian top-ups, canteen POS, spending limits, and reconciliation.",
    demoAction: "Show top-up approval and canteen wallet deduction.",
  },
  {
    phase: "006",
    title: "Learning Engagement",
    href: "/learning",
    summary: "Assignments, quizzes, course content, behavior stars, rewards, and learning history.",
    demoAction: "Show student learning and guardian summary views.",
  },
  {
    phase: "007",
    title: "Requests & Permissions",
    href: "/requests",
    summary: "Guardian request submission, approval workflow, early leave, and review history.",
    demoAction: "Show a guardian outing request and staff approval decision.",
  },
  {
    phase: "008",
    title: "Medical & Emergency",
    href: "/medical",
    summary: "Emergency profile access, clinic incident capture, guardian notification, and audit.",
    demoAction: "Open emergency access, log a clinic incident, and review audit evidence.",
  },
  {
    phase: "009",
    title: "Complaints & Escalations",
    href: "/complaints",
    summary: "Complaint submission, triage, escalation rules, resolution, feedback, and SLA evidence.",
    demoAction: "Show guardian complaint submission and handler escalation.",
  },
  {
    phase: "010",
    title: "Communications & Notifications",
    href: "/communications",
    summary: "Notification center, direct messages, broadcasts, acknowledgements, and delivery evidence.",
    demoAction: "Show a targeted bus delay notice and delivery status.",
  },
  {
    phase: "011",
    title: "Documents, Search & Admin Observability",
    href: "/documents",
    summary: "Document storage, certificates, search, audit trails, metrics, alerts, and incidents.",
    demoAction: "Show guardian-visible documents and admin search/observability.",
  },
  {
    phase: "012",
    title: "Role-Based Mobile App & APK",
    href: "/mobile",
    summary: "One Android app with role-based guardian, student, staff, admin, and support workspaces.",
    demoAction: "Install the APK and run the Guardian-first NFC demo flow.",
  },
];

export const mobileDemoSteps = [
  "Install apps/mobile/build/app/outputs/flutter-apk/app-release.apk on an Android device.",
  "Open the APK on the Guardian live view for Amina Hassan.",
  "Tap Simulate gate NFC, Move bus / refresh ETA, Top up SAR 50, Submit request, and Submit complaint.",
  "Switch to Gate/access staff, Transport driver, and Canteen cashier to show scan and POS duties.",
  "Toggle Arabic and Offline mode to show RTL and queued action behavior.",
];

const pageStyle = {
  maxWidth: "1240px",
  margin: "0 auto",
  padding: "32px",
} satisfies React.CSSProperties;

const gridStyle = {
  display: "grid",
  gridTemplateColumns: "repeat(auto-fit, minmax(260px, 1fr))",
  gap: "12px",
} satisfies React.CSSProperties;

const panelStyle = {
  border: "1px solid #d8dee8",
  borderRadius: "8px",
  background: "#ffffff",
  padding: "16px",
} satisfies React.CSSProperties;

export function SafeSchoolHomeDashboard() {
  return (
    <main style={pageStyle}>
      <header style={{ marginBottom: "24px" }}>
        <p style={{ margin: "0 0 6px", color: "#536179", fontWeight: 700 }}>
          SAFESCHOOL MASTER DEMO
        </p>
        <h1 style={{ margin: "0 0 10px", fontSize: "38px", lineHeight: 1.1 }}>
          SafeSchool Command Center
        </h1>
        <p style={{ margin: 0, maxWidth: "840px", color: "#536179", fontSize: "18px", lineHeight: 1.55 }}>
          One operational school platform for NFC identity, campus access, attendance, transport,
          wallet, learning, requests, medical, complaints, communications, documents, admin
          observability, and role-based mobile APK demos.
        </p>
      </header>

      <section style={{ ...gridStyle, marginBottom: "20px" }} aria-label="Demo entry points">
        <a href="/mobile" style={{ ...panelStyle, color: "inherit", textDecoration: "none" }}>
          <strong style={{ display: "block", fontSize: "22px", marginBottom: "8px" }}>Mobile Command Center</strong>
          <span style={{ color: "#536179" }}>Roles, APK releases, install evidence, and support diagnostics.</span>
        </a>
        <a href="/identity-access" style={{ ...panelStyle, color: "inherit", textDecoration: "none" }}>
          <strong style={{ display: "block", fontSize: "22px", marginBottom: "8px" }}>NFC Identity Setup</strong>
          <span style={{ color: "#536179" }}>Students, guardians, credentials, QR fallback, and permissions.</span>
        </a>
        <a href="/transport" style={{ ...panelStyle, color: "inherit", textDecoration: "none" }}>
          <strong style={{ display: "block", fontSize: "22px", marginBottom: "8px" }}>Transport Operations</strong>
          <span style={{ color: "#536179" }}>Routes, trips, scans, location, ETA, and guardian visibility.</span>
        </a>
        <a href="/wallet" style={{ ...panelStyle, color: "inherit", textDecoration: "none" }}>
          <strong style={{ display: "block", fontSize: "22px", marginBottom: "8px" }}>Wallet & POS</strong>
          <span style={{ color: "#536179" }}>Guardian top-ups, cashier POS, limits, history, and reconciliation.</span>
        </a>
      </section>

      <section style={{ ...panelStyle, marginBottom: "20px" }}>
        <h2 style={{ margin: "0 0 12px", fontSize: "24px" }}>Android APK Demo Script</h2>
        <ol style={{ margin: 0, paddingInlineStart: "22px", color: "#344055", lineHeight: 1.7 }}>
          {mobileDemoSteps.map((step) => (
            <li key={step}>{step}</li>
          ))}
        </ol>
      </section>

      <section>
        <h2 style={{ margin: "0 0 12px", fontSize: "24px" }}>Phase Coverage</h2>
        <div style={gridStyle}>
          {safeSchoolModuleLinks.map((module) => (
            <a
              key={module.phase}
              href={module.href}
              style={{ ...panelStyle, color: "inherit", textDecoration: "none" }}
            >
              <span style={{ color: "#2563eb", fontWeight: 800 }}>{module.phase}</span>
              <strong style={{ display: "block", marginTop: "6px", marginBottom: "8px" }}>
                {module.title}
              </strong>
              <span style={{ display: "block", color: "#536179", lineHeight: 1.45, marginBottom: "10px" }}>
                {module.summary}
              </span>
              <span style={{ display: "block", color: "#172033", fontSize: "14px" }}>
                Demo: {module.demoAction}
              </span>
            </a>
          ))}
        </div>
      </section>
    </main>
  );
}
