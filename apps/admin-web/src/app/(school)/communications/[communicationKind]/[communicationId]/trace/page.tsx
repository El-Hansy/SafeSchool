import { CommunicationsSecondaryPage } from "../../../../../../features/communications";

export default async function Page({ params }: { params: Promise<{ communicationId: string }> }) {
  const { communicationId } = await params;
  return <CommunicationsSecondaryPage view="trace" recordId={communicationId} />;
}
