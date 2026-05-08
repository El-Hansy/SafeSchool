const links = [
  { href: "/identity-access/students", label: "Students", detail: "Profiles, duplicate review, status history" },
  { href: "/identity-access/access-control", label: "Access Control", detail: "Roles, assignments, audit, decisions" },
  { href: "/identity-access/guardians", label: "Guardians", detail: "Guardian records and scoped links" },
  { href: "/identity-access/credentials", label: "Credentials", detail: "NFC cards and QR fallback status" },
];

export default function IdentityAccessPage() {
  return (
    <main style={{ padding: "32px", maxWidth: "1120px", margin: "0 auto" }}>
      <h1 style={{ margin: "0 0 8px", fontSize: "32px" }}>Identity & Access</h1>
      <p style={{ margin: "0 0 24px", color: "#536179" }}>
        Phase 1 administration for school-scoped student identity, guardians, permissions, and credentials.
      </p>
      <section style={{ display: "grid", gap: "16px", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))" }}>
        {links.map((link) => (
          <a
            key={link.href}
            href={link.href}
            style={{
              display: "block",
              minHeight: "120px",
              padding: "18px",
              border: "1px solid #d8dee8",
              borderRadius: "8px",
              color: "inherit",
              background: "#ffffff",
              textDecoration: "none",
            }}
          >
            <strong style={{ display: "block", marginBottom: "8px" }}>{link.label}</strong>
            <span style={{ color: "#536179", lineHeight: 1.45 }}>{link.detail}</span>
          </a>
        ))}
      </section>
    </main>
  );
}
