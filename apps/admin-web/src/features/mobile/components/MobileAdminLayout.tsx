import type { CSSProperties, ReactNode } from "react";

const nav = [
  { href: "/mobile", label: "Overview" },
  { href: "/mobile/roles", label: "Roles" },
  { href: "/mobile/roles/staff", label: "Staff" },
  { href: "/mobile/releases", label: "Releases" },
  { href: "/mobile/support", label: "Support" },
];

export const mobileStyles: Record<string, CSSProperties> = {
  page: { minHeight: "100vh", background: "#eef2f7" },
  shell: { maxWidth: "1280px", margin: "0 auto", padding: "28px", display: "grid", gap: "22px" },
  eyebrow: { margin: 0, color: "#5b6475", fontSize: "13px", fontWeight: 700, textTransform: "uppercase", letterSpacing: "0.04em" },
  title: { margin: "4px 0 0", color: "#101828", fontSize: "34px", lineHeight: 1.1 },
  subtitle: { margin: "10px 0 0", color: "#475467", maxWidth: "820px", lineHeight: 1.55 },
  nav: { display: "flex", flexWrap: "wrap", gap: "8px" },
  navLink: { padding: "9px 12px", borderRadius: "6px", border: "1px solid #d0d7e2", color: "#25324a", background: "#fff", textDecoration: "none", fontSize: "14px", fontWeight: 650 },
  grid: { display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(240px, 1fr))", gap: "14px" },
  panel: { background: "#fff", border: "1px solid #d8dee8", borderRadius: "8px", padding: "18px", boxShadow: "0 1px 2px rgba(16, 24, 40, 0.04)" },
  table: { width: "100%", borderCollapse: "collapse" },
  th: { textAlign: "left", color: "#475467", fontSize: "12px", textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" },
  td: { padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: "#25324a", verticalAlign: "top" },
  badge: { display: "inline-flex", padding: "4px 8px", borderRadius: "6px", background: "#eaf1ff", color: "#174ea6", fontSize: "12px", fontWeight: 700 },
};

export function MobileAdminLayout({ title, children }: { title: string; children: ReactNode }) {
  return (
    <main style={mobileStyles.page}>
      <section style={mobileStyles.shell}>
        <header>
          <p style={mobileStyles.eyebrow}>Safeschool phase 12</p>
          <h1 style={mobileStyles.title}>{title}</h1>
          <p style={mobileStyles.subtitle}>
            One production role-based mobile app with guardian access as a role, staff workspaces, Arabic/English RTL, controlled APK release, and support evidence.
          </p>
        </header>
        <nav style={mobileStyles.nav}>
          {nav.map((item) => (
            <a key={item.href} href={item.href} style={mobileStyles.navLink}>
              {item.label}
            </a>
          ))}
        </nav>
        {children}
      </section>
    </main>
  );
}
