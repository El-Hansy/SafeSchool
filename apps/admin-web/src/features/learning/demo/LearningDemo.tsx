import type { ReactNode } from "react";
import { learningBoundaryNotes, learningCapabilities, learningDemoData } from "../api/learningApi";

type LearningSection = "overview" | "content" | "assignments" | "quizzes" | "stars" | "rewards" | "behavior" | "history" | "review" | "configuration" | "student" | "guardian";

const schoolTabs: Array<[LearningSection, string, string]> = [
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

export function LearningDemo({ section = "overview", audience = "school" }: { section?: LearningSection; audience?: "school" | "student" | "guardian" }) {
  const tabs = audience === "school" ? schoolTabs : [];
  const title = audience === "guardian" ? "Guardian Learning View" : audience === "student" ? "Student Learning Workspace" : "Learning Command Center";
  const subtitle = audience === "guardian" ? "Linked-student learning progress, assignment outcomes, quiz results, stars, rewards, and allowed behavior summaries." : audience === "student" ? "Student-facing content, assignments, quizzes, rewards, behavior summaries, and history." : "School operations demo for courses, content delivery, assignments, quizzes, stars, rewards, behavior logging, history, review, and configuration.";

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool Phase 5</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>{title}</h1>
          <p style={{ maxWidth: 960, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>{subtitle}</p>
        </div>
        <div style={{ border: "1px solid #a7f3d0", background: "#dcfce7", color: "#166534", padding: "12px 18px", borderRadius: 6, fontWeight: 800 }}>Demo data</div>
      </section>

      {tabs.length > 0 ? (
        <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>
          {tabs.map(([key, label, href]) => (
            <a key={key} href={href} style={{ textDecoration: "none", color: section === key ? "#1d4ed8" : "#1f2937", border: `1px solid ${section === key ? "#3b82f6" : "#cbd5e1"}`, background: section === key ? "#eff6ff" : "#ffffff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 }}>{label}</a>
          ))}
        </nav>
      ) : null}

      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))", gap: 16, marginBottom: 24 }}>
        {learningDemoData.metrics.map((metric) => <Metric key={metric.label} label={metric.label} value={metric.value} detail={metric.detail} />)}
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(320px, 1fr)", gap: 18 }}>
        <Panel title={panelTitle(section, audience)}>
          <ActivityList audience={audience} />
        </Panel>
        <Panel title="Controls and boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            {learningCapabilities.map((capability) => <Boundary key={capability.key} title={capability.key} detail={capability.detail} />)}
            {learningBoundaryNotes().map((note) => <Boundary key={note} title="Phase boundary" detail={note} />)}
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function Metric({ label, value, detail }: { label: string; value: string; detail: string }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 18, boxShadow: "0 1px 2px rgba(15,23,42,.08)" }}><div style={{ fontSize: 34, fontWeight: 900 }}>{value}</div><div style={{ fontWeight: 800 }}>{label}</div><div style={{ color: "#64748b", marginTop: 8 }}>{detail}</div></article>;
}

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
}

function ActivityList({ audience }: { audience: "school" | "student" | "guardian" }) {
  const rows = audience === "guardian" ? learningDemoData.activities.filter((activity) => activity.label !== "Behavior" || !activity.detail.includes("staff-only")) : learningDemoData.activities;
  return <div style={{ display: "grid", gap: 10 }}>{rows.map((item) => <Row key={item.label} left={item.label} middle={item.detail} right={item.status} />)}</div>;
}

function Row({ left, middle, right }: { left: string; middle: string; right: string }) {
  return <div style={{ display: "grid", gridTemplateColumns: "1fr 1.6fr 1fr", gap: 10, alignItems: "center", borderBottom: "1px solid #e5e7eb", padding: "10px 0" }}><strong>{left}</strong><span style={{ color: "#4b5563" }}>{middle}</span><span style={{ color: "#1f2937", fontWeight: 700 }}>{right}</span></div>;
}

function panelTitle(section: LearningSection, audience: "school" | "student" | "guardian") {
  if (audience === "guardian") return "Allowed linked-student evidence";
  if (audience === "student") return "My learning work";
  const titles: Record<LearningSection, string> = { overview: "Learning lifecycle", content: "Course content", assignments: "Assignment tracking", quizzes: "Quiz engine", stars: "Star ledger", rewards: "Reward catalog", behavior: "Behavior logging", history: "History search", review: "Exception review", configuration: "Learning rules", student: "Student", guardian: "Guardian" };
  return titles[section];
}
