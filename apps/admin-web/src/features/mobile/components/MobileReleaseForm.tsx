import { mobileReleases } from "../api/mobileApi";
import { mobileStyles } from "./MobileAdminLayout";

export function MobileReleaseForm() {
  return (
    <section style={mobileStyles.panel}>
      <h2>Release setup</h2>
      <p>Current APK release: {mobileReleases[0].version} with checksum {mobileReleases[0].checksum}.</p>
      <p>Arabic and English release notes are required before approval.</p>
    </section>
  );
}
