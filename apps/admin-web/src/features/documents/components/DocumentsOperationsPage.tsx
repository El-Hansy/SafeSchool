import type { ReactNode } from "react";
import {
  loadDocumentsOperations,
  type CertificateResponse,
  type DocumentAudience,
  type DocumentResponse,
  type DocumentsOperationsData,
} from "../api/documentStorageApi";
import {
  CertificateIssueAction,
  CertificateVerifyAction,
  DocumentHoldExportAction,
  DocumentUploadAction,
  SearchSubmitAction,
} from "./DocumentActionForms";

export type DocumentsView = "documents" | "upload" | "certificates" | "issue" | "search" | "index-health";

const schoolTabs: Array<[DocumentsView, string, string]> = [
  ["documents", "Documents", "/documents"],
  ["upload", "Upload", "/documents/upload"],
  ["certificates", "Certificates", "/certificates"],
  ["issue", "Issue", "/certificates/issue"],
  ["search", "Search", "/search"],
  ["index-health", "Index Health", "/search/index-health"],
];

export async function DocumentsOperationsPage({ audience = "school", view = "documents" }: { audience?: DocumentAudience; view?: DocumentsView }) {
  const data = await loadDocumentsOperations();
  const title = audience === "school" ? "Documents, Certificates & Search" : audience === "guardian" ? "Guardian Documents" : "Student Documents";
  const documents = audience === "guardian" ? data.guardianDocuments : audience === "student" ? data.studentDocuments : data.schoolDocuments;
  const certificates = audience === "guardian" ? data.guardianCertificates : audience === "student" ? data.studentCertificates : data.certificates;

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 010/011</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>{title}</h1>
          <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>
            Tenant-scoped document metadata, certificate issuance, authorized search, index health, retention, legal hold, controlled export, and access decision evidence.
          </p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      {audience === "school" && (
        <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>
          {schoolTabs.map(([key, label, href]) => (
            <a key={key} href={href} style={{ textDecoration: "none", color: view === key ? "#1d4ed8" : "#1f2937", border: `1px solid ${view === key ? "#3b82f6" : "#cbd5e1"}`, background: view === key ? "#eff6ff" : "#ffffff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 }}>{label}</a>
          ))}
        </nav>
      )}

      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))", gap: 16, marginBottom: 24 }}>
        <Metric label="Documents" value={String(data.board.documents)} detail={`${documents.length} visible in this scope`} />
        <Metric label="Certificates" value={String(data.board.certificates)} detail={`${certificates.length} loaded here`} />
        <Metric label="Legal holds" value={String(data.board.legalHolds)} detail="Retention protected" />
        <Metric label="Indexed" value={String(data.indexHealth.indexed)} detail={`${data.indexHealth.stale} stale, ${data.indexHealth.failed} failed`} />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title={panelTitle(audience, view)}>
          <SectionContent audience={audience} view={view} data={data} documents={documents} certificates={certificates} />
        </Panel>
        <Panel title="Boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Metadata only" detail="Object content is represented by safe references; binary content remains in controlled storage." />
            <Boundary title="Access revalidation" detail="Search results are checked again when opened, not only when listed." />
            <Boundary title="Retention and legal hold" detail="Protected documents cannot be deleted while hold or retention policy applies." />
            <Boundary title="No workflow mutation" detail="Documents and search do not create attendance, wallet, transport, complaint, communication, or admin outcomes." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function SectionContent({ audience, view, data, documents, certificates }: { audience: DocumentAudience; view: DocumentsView; data: DocumentsOperationsData; documents: DocumentResponse[]; certificates: CertificateResponse[] }) {
  if (audience !== "school") {
    return (
      <Stack>
        <DocumentTable documents={documents} />
        <CertificateTable certificates={certificates} />
      </Stack>
    );
  }

  if (view === "upload") return <Stack><DocumentUploadAction schoolAccountId={data.schoolAccountId} /><DocumentTable documents={documents} /></Stack>;
  if (view === "certificates") return <Stack><CertificateVerifyAction schoolAccountId={data.schoolAccountId} certificates={certificates} /><CertificateTable certificates={certificates} /></Stack>;
  if (view === "issue") return <Stack><CertificateIssueAction schoolAccountId={data.schoolAccountId} /><CertificateTable certificates={certificates} /></Stack>;
  if (view === "search") return <Stack><SearchSubmitAction schoolAccountId={data.schoolAccountId} /><SearchResults data={data} /></Stack>;
  if (view === "index-health") return <IndexHealth data={data} />;

  return <Stack><DocumentHoldExportAction schoolAccountId={data.schoolAccountId} documents={documents} /><DocumentTable documents={documents} /></Stack>;
}

function DataSourceBadge({ source }: { source: DocumentsOperationsData["dataSource"] }) {
  const api = source === "api";
  return (
    <div style={{ border: `1px solid ${api ? "#a7f3d0" : "#fed7aa"}`, background: api ? "#dcfce7" : "#fff7ed", color: api ? "#166534" : "#9a3412", padding: "12px 18px", borderRadius: 6, fontWeight: 800 }}>
      {api ? "API connected" : "API fallback"}
    </div>
  );
}

function Stack({ children }: { children: ReactNode }) {
  return <div style={{ display: "grid", gap: 18 }}>{children}</div>;
}

function Metric({ label, value, detail }: { label: string; value: string; detail: string }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 18, boxShadow: "0 1px 2px rgba(15,23,42,.08)" }}><div style={{ fontSize: 34, fontWeight: 900 }}>{value}</div><div style={{ fontWeight: 800 }}>{label}</div><div style={{ color: "#64748b", marginTop: 8 }}>{detail}</div></article>;
}

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)", overflowX: "auto" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
}

function DocumentTable({ documents }: { documents: DocumentResponse[] }) {
  return <DataTable headers={["Reference", "Status", "Visibility", "Evidence"]} rows={documents.map((document) => [document.reference, document.status, document.visibility, document.evidence.join(", ")])} />;
}

function CertificateTable({ certificates }: { certificates: CertificateResponse[] }) {
  return <DataTable headers={["Reference", "Status", "Verification", "Evidence"]} rows={certificates.map((certificate) => [certificate.reference, certificate.status, certificate.verificationState, certificate.evidence.join(", ")])} />;
}

function SearchResults({ data }: { data: DocumentsOperationsData }) {
  return <DataTable headers={["Query log", "Freshness", "Result", "Suppressed"]} rows={data.search.results.map((result) => [data.search.queryLogReference, data.search.freshnessState, result, data.search.suppressedReasons.join(", ")])} />;
}

function IndexHealth({ data }: { data: DocumentsOperationsData }) {
  return (
    <dl style={{ display: "grid", gap: 14, margin: 0 }}>
      <Boundary title="Indexed" detail={String(data.indexHealth.indexed)} />
      <Boundary title="Pending" detail={String(data.indexHealth.pending)} />
      <Boundary title="Stale" detail={String(data.indexHealth.stale)} />
      <Boundary title="Failed" detail={String(data.indexHealth.failed)} />
      <Boundary title="Suppressed" detail={String(data.indexHealth.suppressed)} />
    </dl>
  );
}

function DataTable({ headers, rows }: { headers: string[]; rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No records returned for this scope.</p>;

  return (
    <table style={{ width: "100%", borderCollapse: "collapse", minWidth: 680 }}>
      <thead>
        <tr>{headers.map((header) => <th key={header} style={{ textAlign: "left", color: "#475467", fontSize: 12, textTransform: "uppercase", padding: "10px 8px", borderBottom: "1px solid #e4e7ec" }}>{header}</th>)}</tr>
      </thead>
      <tbody>
        {rows.map((row) => (
          <tr key={row.join(":")}>{row.map((cell, index) => <td key={`${cell}-${index}`} style={{ padding: "12px 8px", borderBottom: "1px solid #eef2f7", color: index === 0 ? "#101828" : "#344054", fontWeight: index === 0 ? 800 : 500 }}>{cell}</td>)}</tr>
        ))}
      </tbody>
    </table>
  );
}

function panelTitle(audience: DocumentAudience, view: DocumentsView) {
  if (audience === "guardian") return "Guardian visible documents and certificates";
  if (audience === "student") return "Student visible documents and certificates";
  const titles: Record<DocumentsView, string> = { documents: "Document records", upload: "Upload document metadata", certificates: "Certificate records", issue: "Issue certificate", search: "Authorized search", "index-health": "Search index health" };
  return titles[view];
}
