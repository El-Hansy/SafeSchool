import { assertApiDataAvailable } from "../../common/apiReadiness";

export const documentsRoutes = {
  documents: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/documents`,
  documentDetail: (schoolAccountId: string, documentId: string) => `${documentsRoutes.documents(schoolAccountId)}/${documentId}`,
  documentCategories: (schoolAccountId: string) => `${documentsRoutes.documents(schoolAccountId)}/categories`,
  documentCategory: (schoolAccountId: string, categoryId: string) => `${documentsRoutes.documentCategories(schoolAccountId)}/${categoryId}`,
  documentHold: (schoolAccountId: string, documentId: string) => `${documentsRoutes.documents(schoolAccountId)}/${documentId}/hold`,
  documentExport: (schoolAccountId: string, documentId: string) => `${documentsRoutes.documents(schoolAccountId)}/${documentId}/export`,
  certificates: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/certificates`,
  certificateDetail: (schoolAccountId: string, certificateId: string) => `${documentsRoutes.certificates(schoolAccountId)}/${certificateId}`,
  certificateTypes: (schoolAccountId: string) => `${documentsRoutes.certificates(schoolAccountId)}/types`,
  certificateType: (schoolAccountId: string, typeId: string) => `${documentsRoutes.certificateTypes(schoolAccountId)}/${typeId}`,
  certificateVerify: (schoolAccountId: string, certificateId: string) => `${documentsRoutes.certificates(schoolAccountId)}/${certificateId}/verify`,
  search: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/search`,
  searchResult: (schoolAccountId: string, entryId: string) => `${documentsRoutes.search(schoolAccountId)}/results/${entryId}`,
  searchOpen: (schoolAccountId: string, entryId: string) => `${documentsRoutes.search(schoolAccountId)}/${entryId}/open`,
  searchIndexHealth: (schoolAccountId: string) => `${documentsRoutes.search(schoolAccountId)}/index-health`,
  guardianDocuments: () => "/api/v1/guardians/me/documents",
  studentDocuments: () => "/api/v1/students/me/documents",
};
export const documentStorageApi = documentsRoutes;

export type DocumentDataSource = "api" | "fallback";
export type DocumentAudience = "school" | "guardian" | "student";

export type DocumentBoardResponse = {
  schoolAccountId: string;
  phase: string;
  status: string;
  documents: number;
  certificates: number;
  legalHolds: number;
  capabilities: string[];
};

export type DocumentResponse = {
  reference: string;
  status: string;
  visibility: string;
  evidence: string[];
};

export type CertificateResponse = {
  reference: string;
  status: string;
  verificationState: string;
  evidence: string[];
};

export type SearchResponse = {
  queryLogReference: string;
  results: string[];
  freshnessState: string;
  suppressedReasons: string[];
};

export type SearchIndexHealthResponse = {
  pending: number;
  indexed: number;
  stale: number;
  failed: number;
  suppressed: number;
};

export type DocumentCategoryResponse = {
  categoryId: string;
  name: string;
  retention: string;
  visibility: string;
  approval: string;
  evidence?: string[];
};

export type CertificateTypeResponse = {
  typeId: string;
  name: string;
  issuingRole: string;
  visibility: string;
  expiry?: string;
  requiredFields?: string[];
  evidence?: string[];
};

export type SearchResultDetailResponse = {
  entryId: string;
  sourceModule: string;
  freshness: string;
  visibilityDecision: string;
  evidence: string[];
};

export type DocumentsOperationsData = {
  schoolAccountId: string;
  dataSource: DocumentDataSource;
  board: DocumentBoardResponse;
  schoolDocuments: DocumentResponse[];
  guardianDocuments: DocumentResponse[];
  studentDocuments: DocumentResponse[];
  certificates: CertificateResponse[];
  guardianCertificates: CertificateResponse[];
  studentCertificates: CertificateResponse[];
  documentCategories: DocumentCategoryResponse[];
  certificateTypes: CertificateTypeResponse[];
  searchResultDetails: SearchResultDetailResponse[];
  search: SearchResponse;
  indexHealth: SearchIndexHealthResponse;
};

export const documentsFallbackData: DocumentsOperationsData = {
  schoolAccountId: "school-demo",
  dataSource: "fallback",
  board: {
    schoolAccountId: "school-demo",
    phase: "documents-search",
    status: "ready",
    documents: 128,
    certificates: 34,
    legalHolds: 3,
    capabilities: ["documents.storage", "documents.certificates", "documents.search", "documents.retention", "documents.legal_hold", "documents.exports"],
  },
  schoolDocuments: [
    { reference: "DOC-2026-0001", status: "Active", visibility: "StaffVisible", evidence: ["object-reference-stored", "metadata-validated", "audit-written"] },
    { reference: "DOC-2026-0002", status: "LegalHold", visibility: "ReviewerOnly", evidence: ["retention-protected", "legal-hold-active"] },
    { reference: "DOC-2026-0003", status: "ExportPrepared", visibility: "Controlled", evidence: ["scope-validated", "export-reason-captured"] },
  ],
  guardianDocuments: [
    { reference: "guardian-doc-1", status: "Active", visibility: "Summary", evidence: ["access-revalidated", "restricted-details-minimized"] },
  ],
  studentDocuments: [
    { reference: "student-doc-1", status: "Active", visibility: "Summary", evidence: ["student-scope-checked", "restricted-details-minimized"] },
  ],
  certificates: [
    { reference: "CERT-2026-0001", status: "Issued", verificationState: "Verified", evidence: ["school-demo", "source-preserved"] },
    { reference: "CERT-2026-0002", status: "Issued", verificationState: "PendingVerification", evidence: ["type-validated", "audit-written"] },
  ],
  guardianCertificates: [
    { reference: "CERT-2026-0001", status: "Issued", verificationState: "Verified", evidence: ["guardian-visible", "source-preserved"] },
  ],
  studentCertificates: [
    { reference: "CERT-2026-0001", status: "Issued", verificationState: "Verified", evidence: ["student-visible", "source-preserved"] },
  ],
  documentCategories: [
    { categoryId: "guardian-consent", name: "Guardian Consent", retention: "7 years", visibility: "Guardian summary", approval: "Registrar", evidence: ["policy-versioned", "visibility-reviewed"] },
    { categoryId: "medical-note", name: "Medical Note", retention: "Legal hold aware", visibility: "Restricted", approval: "Medical staff", evidence: ["restricted-category", "audit-written"] },
  ],
  certificateTypes: [
    { typeId: "attendance-letter", name: "Attendance Letter", issuingRole: "Registrar", visibility: "Guardian and student", expiry: "None", requiredFields: ["student", "term", "issuer"], evidence: ["template-versioned", "visibility-reviewed"] },
    { typeId: "enrollment-proof", name: "Enrollment Proof", issuingRole: "Registrar", visibility: "Guardian and student", expiry: "One term", requiredFields: ["student", "year"], evidence: ["type-validated"] },
  ],
  searchResultDetails: [
    { entryId: "entry-doc-amina", sourceModule: "documents", freshness: "Fresh", visibilityDecision: "Allowed", evidence: ["tenant-checked", "permission-revalidated", "audit-written"] },
    { entryId: "entry-cert-amina", sourceModule: "certificates", freshness: "Fresh", visibilityDecision: "Allowed", evidence: ["source-preserved", "certificate-visible"] },
  ],
  search: {
    queryLogReference: "search-log-1",
    results: ["Document: Amina consent form", "Certificate: Attendance letter", "Audit: export approved"],
    freshnessState: "Fresh",
    suppressedReasons: ["restricted-counts-hidden"],
  },
  indexHealth: { pending: 2, indexed: 128, stale: 3, failed: 1, suppressed: 4 },
};

export function documentsApiBaseUrl() {
  return process.env.NEXT_PUBLIC_API_BASE_URL ?? "";
}

export function documentsHeaders(schoolAccountId: string, actorReference = "documents-operator") {
  return {
    "content-type": "application/json",
    "x-school-account-id": schoolAccountId,
    "x-actor-reference": actorReference,
  };
}

async function fetchDocumentsJson<T>(path: string, schoolAccountId: string, actorReference?: string): Promise<T | null> {
  const baseUrl = documentsApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: documentsHeaders(schoolAccountId, actorReference),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadDocumentsOperations(schoolAccountId = documentsFallbackData.schoolAccountId): Promise<DocumentsOperationsData> {
  const [board, guardianDocuments, studentDocuments, certificates, documentCategories, certificateTypes, indexHealth] = await Promise.all([
    fetchDocumentsJson<DocumentBoardResponse>(documentsRoutes.documents(schoolAccountId), schoolAccountId),
    fetchDocumentsJson<DocumentResponse[]>(documentsRoutes.guardianDocuments(), schoolAccountId, "guardian-demo"),
    fetchDocumentsJson<DocumentResponse[]>(documentsRoutes.studentDocuments(), schoolAccountId, "student-demo"),
    fetchDocumentsJson<CertificateResponse[]>(documentsRoutes.certificates(schoolAccountId), schoolAccountId),
    fetchDocumentsJson<DocumentCategoryResponse[]>(documentsRoutes.documentCategories(schoolAccountId), schoolAccountId),
    fetchDocumentsJson<CertificateTypeResponse[]>(documentsRoutes.certificateTypes(schoolAccountId), schoolAccountId),
    fetchDocumentsJson<SearchIndexHealthResponse>(documentsRoutes.searchIndexHealth(schoolAccountId), schoolAccountId),
  ]);

  const hasApiData = [board, guardianDocuments, studentDocuments, certificates, documentCategories, certificateTypes, indexHealth].some((item) => item !== null);
  assertApiDataAvailable("Documents operations", [board, guardianDocuments, studentDocuments, certificates, documentCategories, certificateTypes, indexHealth], documentsApiBaseUrl());
  if (!hasApiData) return { ...documentsFallbackData, schoolAccountId, dataSource: "fallback" };

  return {
    ...documentsFallbackData,
    schoolAccountId,
    dataSource: "api",
    board: board ?? { ...documentsFallbackData.board, schoolAccountId },
    guardianDocuments: guardianDocuments ?? documentsFallbackData.guardianDocuments,
    studentDocuments: studentDocuments ?? documentsFallbackData.studentDocuments,
    certificates: certificates ?? documentsFallbackData.certificates,
    documentCategories: documentCategories ?? documentsFallbackData.documentCategories,
    certificateTypes: certificateTypes ?? documentsFallbackData.certificateTypes,
    indexHealth: indexHealth ?? documentsFallbackData.indexHealth,
  };
}
