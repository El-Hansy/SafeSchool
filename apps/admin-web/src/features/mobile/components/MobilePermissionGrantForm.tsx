import { mobileWorkspaces } from "../api/mobileApi";
import { mobileStyles } from "./MobileAdminLayout";

export function MobilePermissionGrantForm() {
  return (
    <section style={mobileStyles.panel}>
      <h2>Permission assignment</h2>
      <p>Tenant-scoped grants control which users or roles can access mobile workspaces and actions.</p>
      <div style={mobileStyles.grid}>
        {mobileWorkspaces.slice(0, 4).map((workspace) => (
          <div key={workspace.role} style={mobileStyles.panel}>
            <strong>{workspace.label}</strong>
            <p>{`Permission: mobile.${workspace.role}.access`}</p>
          </div>
        ))}
      </div>
    </section>
  );
}
