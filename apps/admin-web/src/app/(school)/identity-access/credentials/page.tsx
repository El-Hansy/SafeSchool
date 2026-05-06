import { CredentialIssueForm } from "@/features/identity-access/credentials/CredentialIssueForm";
import { CredentialTable } from "@/features/identity-access/credentials/CredentialTable";
import type { Credential } from "@/features/identity-access/credentials/credentialsApi";

const credentials: Credential[] = [
  {
    identityCredentialId: "credential-1",
    studentProfileId: "demo-student-1",
    credentialType: "NfcCard",
    credentialReference: "card-demo-1001",
    credentialStatus: "Active",
    validFrom: new Date().toISOString(),
    statusReason: "Issued after identity review.",
  },
];

export default function CredentialsPage() {
  return (
    <main style={{ padding: "32px", display: "grid", gap: "20px" }}>
      <h1 style={{ margin: 0 }}>Credentials</h1>
      <CredentialIssueForm />
      <CredentialTable credentials={credentials} />
    </main>
  );
}
