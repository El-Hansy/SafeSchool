import { describe, expect, it } from "vitest";
import { certificateManagementApi } from "../../src/features/documents/api/certificateManagementApi";

describe("certificate audience visibility", () => {
  it("keeps certificate listing and issue routes school scoped", () => {
    expect(certificateManagementApi.list("school-demo")).toBe("/api/v1/schools/school-demo/certificates");
    expect(certificateManagementApi.issue("school-demo")).toBe("/api/v1/schools/school-demo/certificates");
  });
});
