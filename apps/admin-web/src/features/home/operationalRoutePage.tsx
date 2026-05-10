export type OperationalRoutePageProps = {
  area: string;
  title: string;
  detail: string;
};

const pageStyle = {
  maxWidth: "960px",
  margin: "0 auto",
  padding: "32px",
} satisfies React.CSSProperties;

const eyebrowStyle = {
  margin: "0 0 6px",
  color: "#536179",
  fontWeight: 800,
  letterSpacing: "0.02em",
} satisfies React.CSSProperties;

const titleStyle = {
  margin: "0 0 10px",
  fontSize: "34px",
  lineHeight: 1.15,
} satisfies React.CSSProperties;

const detailStyle = {
  margin: 0,
  color: "#536179",
  fontSize: "17px",
  lineHeight: 1.55,
} satisfies React.CSSProperties;

const panelStyle = {
  marginTop: "20px",
  border: "1px solid #d8dee8",
  borderRadius: "8px",
  background: "#ffffff",
  padding: "16px",
} satisfies React.CSSProperties;

export function OperationalRoutePage({ area, title, detail }: OperationalRoutePageProps) {
  return (
    <main style={pageStyle}>
      <p style={eyebrowStyle}>{area}</p>
      <h1 style={titleStyle}>{title}</h1>
      <p style={detailStyle}>{detail}</p>

      <section style={panelStyle} aria-label="Operational route evidence">
        <strong style={{ display: "block", marginBottom: "8px" }}>Operational route</strong>
        <p style={{ margin: 0, color: "#536179", lineHeight: 1.5 }}>
          This view is connected to the SafeSchool command-center navigation and preserves the
          module boundary, role ownership, audit story, and demo evidence for this workflow.
        </p>
      </section>
    </main>
  );
}
