import { mobileWorkspaces } from "../api/mobileApi";
import { mobileStyles } from "./MobileAdminLayout";

export function MobileReleaseAudienceEditor() {
  return (
    <section style={mobileStyles.panel}>
      <h2>Audience</h2>
      <p>Release audiences can target tenant, pilot group, user, or role.</p>
      <p>{mobileWorkspaces.map((workspace) => workspace.label).join(", ")}</p>
    </section>
  );
}
