import type { GuardianLink } from "./guardiansApi";

export function GuardianLinkTable({ links }: { links: GuardianLink[] }) {
  return (
    <table style={{ width: "100%", borderCollapse: "collapse", background: "#ffffff" }}>
      <thead>
        <tr>
          {["Guardian", "Student", "Relationship", "Scope", "State"].map((header) => (
            <th key={header} style={{ textAlign: "left", padding: "10px", borderBottom: "1px solid #d8dee8" }}>
              {header}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {links.map((link) => (
          <tr key={link.guardianLinkId}>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{link.guardianId}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{link.studentProfileId}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{link.relationshipType}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>
              {Object.entries(link.accessScope).map(([key, value]) => `${key}:${value}`).join(", ")}
            </td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{link.linkStatus}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
