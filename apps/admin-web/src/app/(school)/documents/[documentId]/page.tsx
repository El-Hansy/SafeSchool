import { DocumentsSecondaryPage } from "../../../../features/documents";

export default async function Page({ params }: { params: Promise<{ documentId: string }> }) {
  const { documentId } = await params;
  return <DocumentsSecondaryPage view="document-detail" recordId={documentId} />;
}
