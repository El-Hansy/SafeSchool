import { describe, expect, it } from "vitest";
import { adminRoutes } from "../../src/features/administration/api/adminApi";
import { metricsMonitoringApi } from "../../src/features/administration/api/metricsMonitoringApi";

describe("metrics monitoring", () => {
  it("routes monitoring through tenant-scoped admin APIs", () => {
    expect(metricsMonitoringApi.phase).toBe("009-011");
    expect(adminRoutes.monitoring("school-demo")).toBe("/api/v1/schools/school-demo/admin/monitoring");
  });
});
