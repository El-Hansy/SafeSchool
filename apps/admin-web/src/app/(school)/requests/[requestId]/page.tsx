import { RequestsSecondaryPage } from "../../../../features/requests";

export default async function Page({ params }: { params: Promise<{ requestId: string }> }) {
  const { requestId } = await params;
  return <RequestsSecondaryPage view="detail" recordId={requestId} />;
}
