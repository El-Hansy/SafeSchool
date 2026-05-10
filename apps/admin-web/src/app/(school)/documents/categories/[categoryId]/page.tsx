import { DocumentsSecondaryPage } from "../../../../../features/documents";

export default async function Page({ params }: { params: Promise<{ categoryId: string }> }) {
  const { categoryId } = await params;
  return <DocumentsSecondaryPage view="document-category" recordId={categoryId} />;
}
