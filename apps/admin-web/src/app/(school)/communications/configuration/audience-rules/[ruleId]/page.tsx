import { CommunicationsSecondaryPage } from "../../../../../../features/communications";

export default async function Page({ params }: { params: Promise<{ ruleId: string }> }) {
  const { ruleId } = await params;
  return <CommunicationsSecondaryPage view="audience-rule" recordId={ruleId} />;
}
