import { GuardianForm } from "@/features/identity-access/guardians/GuardianForm";
import { GuardianLinkTable } from "@/features/identity-access/guardians/GuardianLinkTable";
import type { GuardianLink } from "@/features/identity-access/guardians/guardiansApi";

const links: GuardianLink[] = [
  {
    guardianLinkId: "guardian-link-1",
    studentProfileId: "demo-student-1",
    guardianId: "demo-guardian-1",
    relationshipType: "Parent",
    accessScope: { student_profile: "read" },
    linkStatus: "Approved",
    validFrom: new Date().toISOString(),
  },
];

export default function GuardiansPage() {
  return (
    <main style={{ padding: "32px", display: "grid", gap: "20px" }}>
      <h1 style={{ margin: 0 }}>Guardian Linking</h1>
      <GuardianForm />
      <GuardianLinkTable links={links} />
    </main>
  );
}
