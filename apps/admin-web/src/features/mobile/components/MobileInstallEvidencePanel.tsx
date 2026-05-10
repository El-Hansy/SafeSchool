import { mobileReleases } from "../api/mobileApi";
import { mobileStyles } from "./MobileAdminLayout";

export function MobileInstallEvidencePanel() {
  return (
    <section style={mobileStyles.panel}>
      <h2>Install evidence</h2>
      {mobileReleases.map((release) => (
        <p key={release.id}>{release.version} - {release.status} - evidence retained</p>
      ))}
    </section>
  );
}
