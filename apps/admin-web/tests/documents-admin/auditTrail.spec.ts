import { describe, expect, it } from "vitest";
import { adminRoutes } from "../../src/features/administration/api/adminApi";
import { auditTrailApi } from "../../src/features/administration/api/auditTrailApi";

describe("audit trail", () => {
  it("routes audit review through versioned school APIs", () => {
    expect(auditTrailApi.phase).toBe("009-011");
    expect(adminRoutes.audit("school-demo")).toBe("/api/v1/schools/school-demo/admin/audit");
  });
});
