import { CommunicationsSecondaryPage } from "../../../../../features/communications";

export default async function Page({ params }: { params: Promise<{ broadcastId: string }> }) {
  const { broadcastId } = await params;
  return <CommunicationsSecondaryPage view="broadcast-detail" recordId={broadcastId} />;
}
