import { mobileWorkspaces } from "../api/mobileApi";
import { mobileStyles } from "./MobileAdminLayout";

export function StaffMobileAssignmentPanel() {
  const staff = mobileWorkspaces.filter((workspace) => !["guardian", "student"].includes(workspace.role));
  return (
    <section style={mobileStyles.panel}>
      <h2>Staff mobile assignments</h2>
      <div style={mobileStyles.grid}>
        {staff.map((workspace) => (
          <article key={workspace.role} style={mobileStyles.panel}>
            <h3>{workspace.label}</h3>
            <p>{workspace.actions.join(" / ")}</p>
            <span style={mobileStyles.badge}>{workspace.offline ? "Offline queue" : "Online"}</span>
          </article>
        ))}
      </div>
    </section>
  );
}
