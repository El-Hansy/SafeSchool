import { describe, expect, it } from "vitest";
import { documentsRoutes } from "../../src/features/documents/api/documentStorageApi";

describe("document audience visibility", () => {
  it("keeps document access behind school-scoped document APIs", () => {
    expect(documentsRoutes.documents("school-demo")).toBe("/api/v1/schools/school-demo/documents");
  });
});
