import { DocumentsSecondaryPage } from "../../../../../features/documents";

export default async function Page({ params }: { params: Promise<{ typeId: string }> }) {
  const { typeId } = await params;
  return <DocumentsSecondaryPage view="certificate-type" recordId={typeId} />;
}
