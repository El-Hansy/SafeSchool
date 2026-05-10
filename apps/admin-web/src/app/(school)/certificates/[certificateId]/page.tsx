import { DocumentsSecondaryPage } from "../../../../features/documents";

export default async function Page({ params }: { params: Promise<{ certificateId: string }> }) {
  const { certificateId } = await params;
  return <DocumentsSecondaryPage view="certificate-detail" recordId={certificateId} />;
}
