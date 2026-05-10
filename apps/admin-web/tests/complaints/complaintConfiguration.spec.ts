import { describe, expect, it } from "vitest";
import { complaintConfigurationApi } from "../../src/features/complaints/api/complaintConfigurationApi";
import { complaintsRoutes } from "../../src/features/complaints/api/complaintsApi";

describe("complaint configuration", () => {
  it("keeps complaint configuration tenant scoped", () => {
    expect(complaintConfigurationApi.phase).toBe("009-011");
    expect(complaintsRoutes.school("school-demo")).toBe("/api/v1/schools/school-demo/complaints");
  });
});
