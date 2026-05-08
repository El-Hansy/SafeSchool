import { AccessDecisionTable } from "@/features/identity-access/access-control/AccessDecisionTable";
import { AuditEventTimeline } from "@/features/identity-access/access-control/AuditEventTimeline";

const decisions = [
  {
    accessDecisionId: "decision-1",
    actorReference: "staff:reviewer",
    attemptedAction: "identity.student_profiles.update",
    targetType: "Student Profile",
    targetReference: "S-1001",
    decision: "Denied" as const,
    decisionReason: "Missing permission.",
    decidedAt: new Date().toISOString(),
  },
];

const events = [
  {
    auditEventId: "audit-1",
    eventCategory: "Access",
    eventType: "identity.access.denied",
    actorReference: "staff:reviewer",
    subjectType: "Student Profile",
    subjectReference: "S-1001",
    reason: "Missing permission.",
    eventTime: new Date().toISOString(),
  },
];

export default function AccessControlPage() {
  return (
    <main style={{ padding: "32px", display: "grid", gap: "20px" }}>
      <h1 style={{ margin: 0 }}>Access Control</h1>
      <AccessDecisionTable decisions={decisions} />
      <AuditEventTimeline events={events} />
    </main>
  );
}
