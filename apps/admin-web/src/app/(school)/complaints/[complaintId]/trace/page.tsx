import { ComplaintsSecondaryPage } from "../../../../../features/complaints";

export default async function Page({ params }: { params: Promise<{ complaintId: string }> }) {
  const { complaintId } = await params;
  return <ComplaintsSecondaryPage view="trace" recordId={complaintId} />;
}
