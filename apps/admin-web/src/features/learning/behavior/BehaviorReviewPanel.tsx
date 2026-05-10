import { learningDemoData } from "../api/learningApi";

export function BehaviorReviewPanel() {
  return (
    <section style={{ display: "grid", gap: 10 }}>
      {learningDemoData.activities.slice(0, 3).map((item) => (
        <div key={item.label} style={{ borderBottom: "1px solid #e5e7eb", padding: "10px 0" }}>
          <strong>{item.label}</strong>
          <div style={{ color: "#64748b" }}>{item.detail}</div>
          <span style={{ color: "#166534", fontWeight: 800 }}>{item.status}</span>
        </div>
      ))}
    </section>
  );
}
