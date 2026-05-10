import { describe, expect, it } from "vitest";
import { documentsRoutes } from "../../src/features/documents/api/documentStorageApi";

describe("search audience visibility", () => {
  it("resolves search under the school account scope", () => {
    expect(documentsRoutes.search("school-demo")).toBe("/api/v1/schools/school-demo/search");
  });
});
