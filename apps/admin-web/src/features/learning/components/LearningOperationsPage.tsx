import type { ReactNode } from "react";
import { loadLearningOperations, type LearningActivity, type LearningAudience, type LearningOperationsData } from "../api/learningApi";
import {
  ConfigureLearningRuleAction,
  CreateAssignmentAction,
  CreateManualReviewAction,
  LogBehaviorEventAction,
  PostStarSourceEventAction,
  PublishContentAction,
  RedeemRewardAction,
  StartQuizAttemptAction,
  SubmitAssignmentAction,
} from "./LearningActionForms";

export type LearningOperationsView = "overview" | "content" | "assignments" | "quizzes" | "stars" | "rewards" | "behavior" | "history" | "review" | "configuration" | "student" | "guardian";

const schoolTabs: Array<[LearningOperationsView, string, string]> = [
  ["overview", "Overview", "/learning"],
  ["content", "Content", "/learning/content"],
  ["assignments", "Assignments", "/learning/assignments"],
  ["quizzes", "Quizzes", "/learning/quizzes"],
  ["stars", "Stars", "/learning/stars"],
  ["rewards", "Rewards", "/learning/rewards"],
  ["behavior", "Behavior", "/learning/behavior"],
  ["history", "History", "/learning/history"],
  ["review", "Review", "/learning/review"],
  ["configuration", "Configuration", "/learning/configuration"],
];

export async function LearningOperationsPage({ audience = "school", view = "overview" }: { audience?: LearningAudience; view?: LearningOperationsView }) {
  const data = await loadLearningOperations();
  const title = audience === "guardian" ? "Guardian Learning View" : audience === "student" ? "Student Learning Workspace" : "Learning Command Center";
  const subtitle = audience === "guardian"
    ? "Linked-student learning progress, assignment outcomes, quiz results, stars, rewards, and allowed behavior summaries."
    : audience === "student"
      ? "Student-facing content, assignments, quizzes, rewards, behavior summaries, and history."
      : "School operations for courses, content delivery, assignments, quizzes, stars, rewards, behavior logging, history, review, and configuration.";
  const activities = audience === "guardian" ? data.guardianActivities : audience === "student" ? data.studentActivities : data.schoolActivities;

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 006</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>{title}</h1>
          <p style={{ maxWidth: 960, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>{subtitle}</p>
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
        {data.metrics.map((metric) => <Metric key={metric.label} label={metric.label} value={metric.value} detail={metric.detail} />)}
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(320px, 1fr)", gap: 18 }}>
        <Panel title={panelTitle(view, audience)}>
          <SectionContent audience={audience} view={view} data={data} activities={activities} />
        </Panel>
        <Panel title="Controls and boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            {data.capabilities.map((capability) => <Boundary key={capability.key} title={capability.key} detail={`${capability.enabled ? "Enabled" : "Disabled"} - ${capability.detail}`} />)}
            {data.boundaryNotes.map((note) => <Boundary key={note} title="Phase boundary" detail={note} />)}
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ audience, view, data, activities }: { audience: LearningAudience; view: LearningOperationsView; data: LearningOperationsData; activities: LearningActivity[] }) {
  if (audience === "student") {
    if (view === "assignments") return <Stack><SubmitAssignmentAction schoolAccountId={data.schoolAccountId} studentProfileId={data.studentProfileId} /><ActivityTable activities={activities} /></Stack>;
    if (view === "quizzes") return <Stack><StartQuizAttemptAction schoolAccountId={data.schoolAccountId} studentProfileId={data.studentProfileId} /><ActivityTable activities={activities} /></Stack>;
    return <ActivityTable activities={activities} />;
  }

  if (audience === "guardian") {
    return <Stack><EndpointStatus label="Guardian scope" endpoint={data.endpoints.guardian} /><ActivityTable activities={activities} /></Stack>;
  }

  if (view === "content") return <Stack><PublishContentAction schoolAccountId={data.schoolAccountId} /><EndpointStatus label="Content endpoint" endpoint={data.endpoints.content} /><ActivityTable activities={activitiesFor(activities, "Science content")} /></Stack>;
  if (view === "assignments") return <Stack><CreateAssignmentAction schoolAccountId={data.schoolAccountId} /><EndpointStatus label="Assignments endpoint" endpoint={data.endpoints.assignments} /><ActivityTable activities={activitiesFor(activities, "Assignment")} /></Stack>;
  if (view === "quizzes") return <Stack><StartQuizAttemptAction schoolAccountId={data.schoolAccountId} studentProfileId={data.studentProfileId} /><EndpointStatus label="Quiz endpoint" endpoint={data.endpoints.quizAttempts} /><ActivityTable activities={activitiesFor(activities, "Quiz")} /></Stack>;
  if (view === "stars") return <Stack><PostStarSourceEventAction schoolAccountId={data.schoolAccountId} studentProfileId={data.studentProfileId} /><EndpointStatus label="Star endpoint" endpoint={data.endpoints.stars} /><ActivityTable activities={activitiesFor(activities, "Stars")} /></Stack>;
  if (view === "rewards") return <Stack><RedeemRewardAction schoolAccountId={data.schoolAccountId} studentProfileId={data.studentProfileId} /><EndpointStatus label="Reward endpoint" endpoint={data.endpoints.rewards} /><ActivityTable activities={activitiesFor(activities, "Reward")} /></Stack>;
  if (view === "behavior") return <Stack><LogBehaviorEventAction schoolAccountId={data.schoolAccountId} studentProfileId={data.studentProfileId} /><EndpointStatus label="Behavior endpoint" endpoint={data.endpoints.behavior} /><ActivityTable activities={activitiesFor(activities, "Behavior")} /></Stack>;
  if (view === "history") return <Stack><EndpointStatus label="History endpoint" endpoint={data.endpoints.history} /><ActivityTable activities={activities} /></Stack>;
  if (view === "review") return <Stack><CreateManualReviewAction schoolAccountId={data.schoolAccountId} /><EndpointStatus label="Review endpoint" endpoint={data.endpoints.review} /><ActivityTable activities={activities} /></Stack>;
  if (view === "configuration") return <Stack><ConfigureLearningRuleAction schoolAccountId={data.schoolAccountId} /><EndpointStatus label="Rule settings endpoint" endpoint={data.endpoints.configuration} /><CapabilityTable data={data} /></Stack>;

  return <Stack><EndpointStatus label="Learning root" endpoint={data.root} /><ActivityTable activities={activities} /></Stack>;
}

function DataSourceBadge({ source }: { source: LearningOperationsData["dataSource"] }) {
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

function EndpointStatus({ label, endpoint }: { label: string; endpoint: LearningOperationsData["root"] }) {
  return <DataTable headers={["Endpoint", "Area", "Status"]} rows={[[label, endpoint.area ?? endpoint.scope ?? endpoint.phase ?? "learning", endpoint.status]]} />;
}

function CapabilityTable({ data }: { data: LearningOperationsData }) {
  return <DataTable headers={["Capability", "State", "Detail"]} rows={data.capabilities.map((capability) => [capability.key, capability.enabled ? "Enabled" : "Disabled", capability.detail])} />;
}

function ActivityTable({ activities }: { activities: LearningActivity[] }) {
  return <DataTable headers={["Activity", "Detail", "Status"]} rows={activities.map((activity) => [activity.label, activity.detail, activity.status])} />;
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No learning records returned for this scope.</p>;

  return (
    <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 660 }}>
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

function activitiesFor(activities: LearningActivity[], label: string) {
  return activities.filter((activity) => activity.label === label);
}

function panelTitle(section: LearningOperationsView, audience: LearningAudience) {
  if (audience === "guardian") return "Allowed linked-student evidence";
  if (audience === "student") return "My learning work";
  const titles: Record<LearningOperationsView, string> = {
    overview: "Learning lifecycle",
    content: "Course content",
    assignments: "Assignment tracking",
    quizzes: "Quiz engine",
    stars: "Star ledger",
    rewards: "Reward catalog",
    behavior: "Behavior logging",
    history: "History search",
    review: "Exception review",
    configuration: "Learning rules",
    student: "Student",
    guardian: "Guardian",
  };
  return titles[section];
}
