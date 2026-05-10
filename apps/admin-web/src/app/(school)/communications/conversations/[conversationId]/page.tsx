import { CommunicationsSecondaryPage } from "../../../../../features/communications";

export default async function Page({ params }: { params: Promise<{ conversationId: string }> }) {
  const { conversationId } = await params;
  return <CommunicationsSecondaryPage view="conversation-detail" recordId={conversationId} />;
}
