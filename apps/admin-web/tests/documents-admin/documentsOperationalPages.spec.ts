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
      "src/app/(school)/documents/[documentId]/page.tsx",
      "src/app/(school)/documents/categories/page.tsx",
      "src/app/(school)/documents/categories/[categoryId]/page.tsx",
      "src/app/(school)/certificates/page.tsx",
      "src/app/(school)/certificates/issue/page.tsx",
      "src/app/(school)/certificates/[certificateId]/page.tsx",
      "src/app/(school)/certificates/types/page.tsx",
      "src/app/(school)/certificates/types/[typeId]/page.tsx",
      "src/app/(school)/search/page.tsx",
      "src/app/(school)/search/index-health/page.tsx",
      "src/app/(school)/search/results/[entryId]/page.tsx",
      "src/app/(guardian)/guardian/documents/page.tsx",
      "src/app/(guardian)/guardian/certificates/page.tsx",
      "src/app/(student)/student/documents/page.tsx",
      "src/app/(student)/student/certificates/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("DocumentsDemo");
      expect(source).not.toContain("OperationalRoutePage");
      expect(source).toMatch(/Documents(Operations|Secondary)Page/);
    }
  });

  it("loads document fallback data for school, guardian, student, certificate, and search scopes", async () => {
    const data = await loadDocumentsOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.schoolDocuments.length).toBeGreaterThan(0);
    expect(data.guardianDocuments.length).toBeGreaterThan(0);
    expect(data.studentDocuments.length).toBeGreaterThan(0);
    expect(data.certificates.length).toBeGreaterThan(0);
    expect(data.documentCategories.length).toBeGreaterThan(0);
    expect(data.certificateTypes.length).toBeGreaterThan(0);
    expect(data.searchResultDetails.length).toBeGreaterThan(0);
    expect(data.search.results.length).toBeGreaterThan(0);
    expect(data.indexHealth.indexed).toBeGreaterThan(0);
  });

  it("keeps document, certificate, and search action routes scoped", () => {
    expect(documentsRoutes.documentHold("school-demo", "DOC-1")).toBe("/api/v1/schools/school-demo/documents/DOC-1/hold");
    expect(documentsRoutes.documentExport("school-demo", "DOC-1")).toBe("/api/v1/schools/school-demo/documents/DOC-1/export");
    expect(documentsRoutes.documentDetail("school-demo", "DOC-1")).toBe("/api/v1/schools/school-demo/documents/DOC-1");
    expect(documentsRoutes.documentCategories("school-demo")).toBe("/api/v1/schools/school-demo/documents/categories");
    expect(documentsRoutes.documentCategory("school-demo", "guardian-consent")).toBe("/api/v1/schools/school-demo/documents/categories/guardian-consent");
    expect(documentsRoutes.certificateDetail("school-demo", "CERT-1")).toBe("/api/v1/schools/school-demo/certificates/CERT-1");
    expect(documentsRoutes.certificateTypes("school-demo")).toBe("/api/v1/schools/school-demo/certificates/types");
    expect(documentsRoutes.certificateType("school-demo", "attendance-letter")).toBe("/api/v1/schools/school-demo/certificates/types/attendance-letter");
    expect(documentsRoutes.searchResult("school-demo", "entry-1")).toBe("/api/v1/schools/school-demo/search/results/entry-1");
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

  it("keeps backend document secondary routes mapped", () => {
    const source = readFileSync(join(process.cwd(), "../api/src/SafeSchool.Api/Features/Documents/DocumentsModule.cs"), "utf8");

    expect(source).toContain('MapGet("/categories"');
    expect(source).toContain('MapGet("/categories/{categoryId}"');
    expect(source).toContain('MapGet("/{documentId}"');
    expect(source).toContain('MapGet("/types"');
    expect(source).toContain('MapGet("/types/{typeId}"');
    expect(source).toContain('MapGet("/{certificateId}"');
    expect(source).toContain('MapGet("/results/{entryId}"');
  });
});
