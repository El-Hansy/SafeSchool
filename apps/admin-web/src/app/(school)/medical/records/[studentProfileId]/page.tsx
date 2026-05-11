import { MedicalSecondaryPage } from "../../../../../features/medical";

export default async function Page({ params }: { params: Promise<{ studentProfileId: string }> }) {
  const { studentProfileId } = await params;
  return <MedicalSecondaryPage view="record" recordId={studentProfileId} />;
}
