import { ComplaintsSecondaryPage } from "../../../../../../features/complaints";

export default async function Page({ params }: { params: Promise<{ ruleId: string }> }) {
  const { ruleId } = await params;
  return <ComplaintsSecondaryPage view="escalation-rule" recordId={ruleId} />;
}
