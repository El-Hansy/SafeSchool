import { MobileAdminLayout, MobileReleaseApprovalPanel, MobileReleaseNotesEditor } from "../../../../../features/mobile";

export default async function MobileReleaseDetailPage({ params }: { params: Promise<{ releaseId: string }> }) {
  const { releaseId } = await params;
  return (
    <MobileAdminLayout title={`APK Release ${releaseId}`}>
      <MobileReleaseNotesEditor />
      <MobileReleaseApprovalPanel />
    </MobileAdminLayout>
  );
}
