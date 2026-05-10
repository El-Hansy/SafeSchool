import { mobileReleases } from "../api/mobileApi";
import { mobileStyles } from "./MobileAdminLayout";

export function MobileReleaseApprovalPanel() {
  return (
    <section style={mobileStyles.panel}>
      <h2>Approval and rollback</h2>
      {mobileReleases.map((release) => (
        <p key={release.id}>{release.version} - {release.status} - {release.audience}</p>
      ))}
    </section>
  );
}
