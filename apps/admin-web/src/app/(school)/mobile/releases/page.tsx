import { MobileAdminLayout, MobileReleaseApprovalPanel, MobileReleaseAudienceEditor, MobileReleaseForm, MobileReleaseNotesEditor } from "../../../../features/mobile";

export default function MobileReleasesPage() {
  return (
    <MobileAdminLayout title="APK Releases">
      <MobileReleaseForm />
      <MobileReleaseAudienceEditor />
      <MobileReleaseNotesEditor />
      <MobileReleaseApprovalPanel />
    </MobileAdminLayout>
  );
}
