import { loadMedicalOperations, type MedicalOperationsData, type MedicalResponse } from "../api/medicalApi";
import { MedicalReviewAction } from "./MedicalActionForms";

export type MedicalSecondaryView = "record" | "trace";

export async function MedicalSecondaryPage({ view, recordId }: { view: MedicalSecondaryView; recordId?: string }) {
  const data = await loadMedicalOperations();
  const record = findRecord(data, recordId);
  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: 32 }}>
      <section style={{ marginBottom: 24 }}>
        <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 008</div>
        <h1 style={{ fontSize: 40, lineHeight: 1.05, margin: "8px 0" }}>{view === "trace" ? "Medical Trace" : "Medical Record"}</h1>
        <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 19, lineHeight: 1.45, margin: 0 }}>Open medical evidence with minimum necessary detail, review status, and audit references.</p>
      </section>
      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <article style={panelStyle}><h2 style={{ marginTop: 0 }}>Operational record</h2>{view === "trace" && record ? <DataTable headers={["Record", "Trace"]} rows={record.auditTrail.map((entry) => [record.recordReference, entry])} /> : <><MedicalReviewAction schoolAccountId={data.schoolAccountId} records={record ? [record] : []} /><DataTable headers={["Reference", "Type", "Status", "Summary"]} rows={record ? [[record.recordReference, record.recordType, record.status, record.visibleSummary]] : []} /></>}</article>
        <article style={panelStyle}><h2 style={{ marginTop: 0 }}>Privacy boundary</h2><p style={{ color: "#64748b" }}>Restricted medical details remain scoped to explicit medical, emergency, guardian, student, audit, or review authority.</p></article>
      </section>
    </main>
  );
}

function findRecord(data: MedicalOperationsData, recordId?: string): MedicalResponse | undefined {
  return data.history.find((item) => item.medicalRecordId === recordId || item.recordReference === recordId || item.studentProfileId === recordId) ?? data.history[0];
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No medical record returned for this scope.</p>;
  return <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 680 }}><thead><tr>{headers.map((header) => <th key={header} style={thStyle}>{header}</th>)}</tr></thead><tbody>{rows.map((row) => <tr key={row.join(":")}>{row.map((cell, index) => <td key={`${cell}-${index}`} style={{ padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: index === 0 ? "#101828" : "#344054", fontWeight: index === 0 ? 800 : 500 }}>{cell}</td>)}</tr>)}</tbody></table>;
}

const panelStyle = { background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)", overflowX: "auto" } satisfies React.CSSProperties;
const thStyle = { textAlign: "left", color: "#475467", fontSize: 12, textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" } satisfies React.CSSProperties;

