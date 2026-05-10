import { mobileWorkspaces } from "../api/mobileApi";
import { mobileStyles } from "./MobileAdminLayout";

export function MobileRoleMatrix() {
  return (
    <section style={mobileStyles.panel}>
      <h2>Role permission matrix</h2>
      <table style={mobileStyles.table}>
        <thead>
          <tr>
            <th style={mobileStyles.th}>Role</th>
            <th style={mobileStyles.th}>Arabic</th>
            <th style={mobileStyles.th}>Features</th>
            <th style={mobileStyles.th}>Critical actions</th>
            <th style={mobileStyles.th}>Offline</th>
          </tr>
        </thead>
        <tbody>
          {mobileWorkspaces.map((workspace) => (
            <tr key={workspace.role}>
              <td style={mobileStyles.td}><strong>{workspace.label}</strong></td>
              <td style={mobileStyles.td} dir="rtl">{workspace.arabicLabel}</td>
              <td style={mobileStyles.td}>{workspace.features.join(", ")}</td>
              <td style={mobileStyles.td}>{workspace.actions.join(", ")}</td>
              <td style={mobileStyles.td}>{workspace.offline ? "Allowed where source workflow supports it" : "Online confirmation"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  );
}
