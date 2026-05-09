import type { AccessDecision } from "./accessControlApi";

export function AccessDecisionTable({ decisions }: { decisions: AccessDecision[] }) {
  return (
    <table style={{ width: "100%", borderCollapse: "collapse", background: "#ffffff" }}>
      <thead>
        <tr>
          {["Actor", "Action", "Target", "Decision", "Reason"].map((header) => (
            <th key={header} style={{ textAlign: "left", padding: "10px", borderBottom: "1px solid #d8dee8" }}>
              {header}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {decisions.map((decision) => (
          <tr key={decision.accessDecisionId}>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{decision.actorReference}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{decision.attemptedAction}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{decision.targetReference}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{decision.decision}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{decision.decisionReason}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
