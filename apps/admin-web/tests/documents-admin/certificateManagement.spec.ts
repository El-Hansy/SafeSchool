import { describe, expect, it } from "vitest";
import { documentsRoutes } from "../../src/features/documents/api/documentStorageApi";

describe("certificate management", () => {
  it("uses versioned certificate routes", () => {
    expect(documentsRoutes.certificates("school-demo")).toBe("/api/v1/schools/school-demo/certificates");
  });
});
