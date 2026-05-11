import { MedicalSecondaryPage } from "../../../../../features/medical";

export default async function Page({ params }: { params: Promise<{ recordId: string }> }) {
  const { recordId } = await params;
  return <MedicalSecondaryPage view="trace" recordId={recordId} />;
}
