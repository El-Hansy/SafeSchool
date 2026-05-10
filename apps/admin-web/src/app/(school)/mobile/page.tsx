import { MobileAdminLayout, MobileRoleMatrix, mobileStats, mobileStyles } from "../../../features/mobile";

export default function MobileOverviewPage() {
  const stats = mobileStats();
  return (
    <MobileAdminLayout title="Mobile Command Center">
      <section style={mobileStyles.grid}>
        <div style={mobileStyles.panel}><strong>{stats.roles}</strong><p>Production role workspaces</p></div>
        <div style={mobileStyles.panel}><strong>{stats.rtlCoverage}</strong><p>Localization coverage</p></div>
        <div style={mobileStyles.panel}><strong>{stats.activeRelease}</strong><p>Active APK release</p></div>
        <div style={mobileStyles.panel}><strong>{stats.supportEvents}</strong><p>Support evidence records</p></div>
      </section>
      <MobileRoleMatrix />
    </MobileAdminLayout>
  );
}
