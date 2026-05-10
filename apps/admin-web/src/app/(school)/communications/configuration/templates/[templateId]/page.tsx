import { CommunicationsSecondaryPage } from "../../../../../../features/communications";

export default async function Page({ params }: { params: Promise<{ templateId: string }> }) {
  const { templateId } = await params;
  return <CommunicationsSecondaryPage view="template" recordId={templateId} />;
}
