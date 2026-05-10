import { readFileSync } from "node:fs";
import { join } from "node:path";
import { describe, expect, it } from "vitest";
import { certificateManagementApi } from "../../src/features/documents/api/certificateManagementApi";
import { documentsRoutes, loadDocumentsOperations } from "../../src/features/documents/api/documentStorageApi";
import { searchApi } from "../../src/features/documents/api/searchApi";

describe("documents operational pages", () => {
  it("uses operational documents pages instead of static demo pages", () => {
    const pages = [
      "src/app/(school)/documents/page.tsx",
      "src/app/(school)/documents/upload/page.tsx",
      "src/app/(school)/certificates/page.tsx",
      "src/app/(school)/certificates/issue/page.tsx",
      "src/app/(school)/search/page.tsx",
      "src/app/(school)/search/index-health/page.tsx",
      "src/app/(guardian)/guardian/documents/page.tsx",
      "src/app/(guardian)/guardian/certificates/page.tsx",
      "src/app/(student)/student/documents/page.tsx",
      "src/app/(student)/student/certificates/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("DocumentsDemo");
      expect(source).toContain("DocumentsOperationsPage");
    }
  });

  it("loads document fallback data for school, guardian, student, certificate, and search scopes", async () => {
    const data = await loadDocumentsOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.schoolDocuments.length).toBeGreaterThan(0);
    expect(data.guardianDocuments.length).toBeGreaterThan(0);
    expect(data.studentDocuments.length).toBeGreaterThan(0);
    expect(data.certificates.length).toBeGreaterThan(0);
    expect(data.search.results.length).toBeGreaterThan(0);
    expect(data.indexHealth.indexed).toBeGreaterThan(0);
  });

  it("keeps document, certificate, and search action routes scoped", () => {
    expect(documentsRoutes.documentHold("school-demo", "DOC-1")).toBe("/api/v1/schools/school-demo/documents/DOC-1/hold");
    expect(documentsRoutes.documentExport("school-demo", "DOC-1")).toBe("/api/v1/schools/school-demo/documents/DOC-1/export");
    expect(certificateManagementApi.verify("school-demo", "CERT-1")).toBe("/api/v1/schools/school-demo/certificates/CERT-1/verify");
    expect(searchApi.open("school-demo", "entry-1")).toBe("/api/v1/schools/school-demo/search/entry-1/open");
    expect(documentsRoutes.guardianDocuments()).toBe("/api/v1/guardians/me/documents");
    expect(documentsRoutes.studentDocuments()).toBe("/api/v1/students/me/documents");
  });

  it("exposes document upload, hold/export, certificate, and search forms through route helpers", () => {
    const source = readFileSync(join(process.cwd(), "src/features/documents/components/DocumentActionForms.tsx"), "utf8");

    expect(source).toContain("documentsRoutes.documents");
    expect(source).toContain("documentsRoutes.documentHold");
    expect(source).toContain("documentsRoutes.documentExport");
    expect(source).toContain("documentsRoutes.certificates");
    expect(source).toContain("documentsRoutes.search");
  });
});
