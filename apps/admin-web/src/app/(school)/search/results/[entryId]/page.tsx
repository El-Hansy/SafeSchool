import { DocumentsSecondaryPage } from "../../../../../features/documents";

export default async function Page({ params }: { params: Promise<{ entryId: string }> }) {
  const { entryId } = await params;
  return <DocumentsSecondaryPage view="search-result" recordId={entryId} />;
}
