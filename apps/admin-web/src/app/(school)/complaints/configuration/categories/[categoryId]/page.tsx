import { ComplaintsSecondaryPage } from "../../../../../../features/complaints";

export default async function Page({ params }: { params: Promise<{ categoryId: string }> }) {
  const { categoryId } = await params;
  return <ComplaintsSecondaryPage view="category" recordId={categoryId} />;
}
