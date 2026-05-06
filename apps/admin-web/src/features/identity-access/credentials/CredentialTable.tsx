import type { Credential } from "./credentialsApi";

export function CredentialTable({ credentials }: { credentials: Credential[] }) {
  return (
    <table style={{ width: "100%", borderCollapse: "collapse", background: "#ffffff" }}>
      <thead>
        <tr>
          {["Type", "Reference", "Status", "Valid from", "Valid until", "Reason"].map((header) => (
            <th key={header} style={{ textAlign: "left", padding: "10px", borderBottom: "1px solid #d8dee8" }}>
              {header}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {credentials.map((credential) => (
          <tr key={credential.identityCredentialId}>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{credential.credentialType}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{credential.credentialReference}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{credential.credentialStatus}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{credential.validFrom}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{credential.validUntil ?? "Open"}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{credential.statusReason}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
