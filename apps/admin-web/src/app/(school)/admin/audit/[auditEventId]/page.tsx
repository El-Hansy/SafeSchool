import { AdminSecondaryPage } from "../../../../../features/administration";

export default async function Page({ params }: { params: Promise<{ auditEventId: string }> }) {
  const { auditEventId } = await params;
  return <AdminSecondaryPage view="audit-event" auditEventId={auditEventId} />;
}
