import type { ReactNode } from "react";
import {
  loadDocumentsOperations,
  type CertificateResponse,
  type DocumentsOperationsData,
  type DocumentResponse,
} from "../api/documentStorageApi";
import { CertificateVerifyAction, DocumentHoldExportAction } from "./DocumentActionForms";

export type DocumentsSecondaryView =
  | "document-detail"
  | "document-categories"
  | "document-category"
  | "certificate-detail"
  | "certificate-types"
  | "certificate-type"
  | "search-result";

export async function DocumentsSecondaryPage({ view, recordId }: { view: DocumentsSecondaryView; recordId?: string }) {
  const data = await loadDocumentsOperations();
  const document = findDocument(data, recordId);
  const certificate = findCertificate(data, recordId);
  const category = data.documentCategories.find((item) => item.categoryId === recordId) ?? data.documentCategories[0];
  const certificateType = data.certificateTypes.find((item) => item.typeId === recordId) ?? data.certificateTypes[0];
  const searchResult = data.searchResultDetails.find((item) => item.entryId === recordId) ?? data.searchResultDetails[0];

  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 010/011</div>
          <h1 style={{ fontSize: 40, lineHeight: 1.05, margin: "8px 0" }}>{titleFor(view)}</h1>
          <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 19, lineHeight: 1.45, margin: 0 }}>{detailFor(view)}</p>
        </div>
        <DataSourceBadge source={data.dataSource} />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(300px, .8fr)", gap: 18 }}>
        <Panel title="Operational records">
          <ViewContent view={view} data={data} document={document} certificate={certificate} category={category} certificateType={certificateType} searchResult={searchResult} />
        </Panel>
        <Panel title="Access boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="Tenant scoped" detail={data.schoolAccountId} />
            <Boundary title="Access revalidated" detail="Search results and record opens re-check visibility before exposing detail." />
            <Boundary title="Retention aware" detail="Legal hold and retention state prevent unsafe content deletion or uncontrolled export." />
            <Boundary title="No source mutation" detail="Document, certificate, and search views reference prior modules without changing their records." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

function ViewContent({
  view,
  data,
  document,
  certificate,
  category,
  certificateType,
  searchResult,
}: {
  view: DocumentsSecondaryView;
  data: DocumentsOperationsData;
  document?: DocumentResponse;
  certificate?: CertificateResponse;
  category?: DocumentsOperationsData["documentCategories"][number];
  certificateType?: DocumentsOperationsData["certificateTypes"][number];
  searchResult?: DocumentsOperationsData["searchResultDetails"][number];
}) {
  if (view === "document-detail" && document) return <Stack><DocumentHoldExportAction schoolAccountId={data.schoolAccountId} documents={[document]} /><DataTable headers={["Document", "Status", "Visibility", "Evidence"]} rows={[[document.reference, document.status, document.visibility, document.evidence.join(", ")]]} /></Stack>;
  if (view === "document-categories") return <DataTable headers={["Category", "Name", "Retention", "Visibility", "Approval"]} rows={data.documentCategories.map((item) => [item.categoryId, item.name, item.retention, item.visibility, item.approval])} />;
  if (view === "document-category" && category) return <DataTable headers={["Category", "Name", "Retention", "Visibility", "Approval", "Evidence"]} rows={[[category.categoryId, category.name, category.retention, category.visibility, category.approval, category.evidence?.join(", ") ?? "policy-reviewed"]]} />;
  if (view === "certificate-detail" && certificate) return <Stack><CertificateVerifyAction schoolAccountId={data.schoolAccountId} certificates={[certificate]} /><DataTable headers={["Certificate", "Status", "Verification", "Evidence"]} rows={[[certificate.reference, certificate.status, certificate.verificationState, certificate.evidence.join(", ")]]} /></Stack>;
  if (view === "certificate-types") return <DataTable headers={["Type", "Name", "Issuing role", "Visibility", "Expiry"]} rows={data.certificateTypes.map((item) => [item.typeId, item.name, item.issuingRole, item.visibility, item.expiry ?? "None"])} />;
  if (view === "certificate-type" && certificateType) return <DataTable headers={["Type", "Name", "Fields", "Visibility", "Evidence"]} rows={[[certificateType.typeId, certificateType.name, certificateType.requiredFields?.join(", ") ?? "standard", certificateType.visibility, certificateType.evidence?.join(", ") ?? "template-versioned"]]} />;
  if (view === "search-result" && searchResult) return <DataTable headers={["Entry", "Source", "Freshness", "Decision", "Evidence"]} rows={[[searchResult.entryId, searchResult.sourceModule, searchResult.freshness, searchResult.visibilityDecision, searchResult.evidence.join(", ")]]} />;

  return <p style={{ color: "#667085", margin: 0 }}>No document records returned for this scope.</p>;
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

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)", overflowX: "auto" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
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

function findDocument(data: DocumentsOperationsData, recordId?: string) {
  return data.schoolDocuments.find((item) => item.reference === recordId) ?? data.schoolDocuments[0];
}

function findCertificate(data: DocumentsOperationsData, recordId?: string) {
  return data.certificates.find((item) => item.reference === recordId) ?? data.certificates[0];
}

function titleFor(view: DocumentsSecondaryView) {
  const titles: Record<DocumentsSecondaryView, string> = {
    "document-detail": "Document Detail",
    "document-categories": "Document Categories",
    "document-category": "Document Category Detail",
    "certificate-detail": "Certificate Detail",
    "certificate-types": "Certificate Types",
    "certificate-type": "Certificate Type Detail",
    "search-result": "Search Result Detail",
  };
  return titles[view];
}

function detailFor(view: DocumentsSecondaryView) {
  const details: Record<DocumentsSecondaryView, string> = {
    "document-detail": "Open one document with category, version, subject, visibility decision, retention state, and access audit evidence.",
    "document-categories": "Manage document categories, retention defaults, guardian and student visibility, approval needs, and legal hold behavior.",
    "document-category": "Review one document category with retention policy, visibility defaults, upload rules, and approval history.",
    "certificate-detail": "Open a certificate record with issuer, student scope, verification state, visibility, expiry, and audit evidence.",
    "certificate-types": "Manage certificate templates, issuing permissions, expiry rules, guardian visibility, and verification requirements.",
    "certificate-type": "Review one certificate type, required fields, issuing role, visibility policy, and lifecycle configuration.",
    "search-result": "Open an authorized search result with source module, freshness, visibility decision, and access audit trail.",
  };
  return details[view];
}
