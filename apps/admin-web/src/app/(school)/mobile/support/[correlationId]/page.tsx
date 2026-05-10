import { MobileAdminLayout, MobileAuditTimelinePanel, MobileDeviceSessionPanel } from "../../../../../features/mobile";

export default async function MobileSupportDetailPage({ params }: { params: Promise<{ correlationId: string }> }) {
  const { correlationId } = await params;
  return (
    <MobileAdminLayout title={`Support Trace ${correlationId}`}>
      <MobileDeviceSessionPanel />
      <MobileAuditTimelinePanel />
    </MobileAdminLayout>
  );
}
