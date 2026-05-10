import { describe, expect, it } from "vitest";
import { searchApi } from "../../src/features/documents/api/searchApi";

describe("global search", () => {
  it("keeps search and index health routes versioned", () => {
    expect(searchApi.query("school-demo")).toBe("/api/v1/schools/school-demo/search");
    expect(searchApi.indexHealth("school-demo")).toBe("/api/v1/schools/school-demo/search/index-health");
  });
});
