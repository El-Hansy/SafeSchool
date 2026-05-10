import { describe, expect, it } from "vitest";
import { documentStorageApi } from "../../src/features/documents/api/documentStorageApi";

describe("document storage", () => {
  it("exposes document, certificate, and search API boundaries", () => {
    expect(documentStorageApi.documents("school-demo")).toBe("/api/v1/schools/school-demo/documents");
    expect(documentStorageApi.certificates("school-demo")).toBe("/api/v1/schools/school-demo/certificates");
    expect(documentStorageApi.search("school-demo")).toBe("/api/v1/schools/school-demo/search");
  });
});
