import { MobileAdminLayout, MobileAuditTimelinePanel, MobileDeviceSessionPanel, MobileInstallEvidencePanel } from "../../../../features/mobile";

export default function MobileSupportPage() {
  return (
    <MobileAdminLayout title="Mobile Support">
      <MobileDeviceSessionPanel />
      <MobileInstallEvidencePanel />
      <MobileAuditTimelinePanel />
    </MobileAdminLayout>
  );
}
