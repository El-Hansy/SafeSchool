import { CommunicationsSecondaryPage } from "../../../../../features/communications";

export default async function Page({ params }: { params: Promise<{ notificationId: string }> }) {
  const { notificationId } = await params;
  return <CommunicationsSecondaryPage view="notification-detail" recordId={notificationId} />;
}
