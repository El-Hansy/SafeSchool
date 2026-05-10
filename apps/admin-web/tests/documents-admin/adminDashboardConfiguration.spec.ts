import { describe, expect, it } from "vitest";
import { adminDashboardConfigurationApi } from "../../src/features/administration/api/adminDashboardConfigurationApi";
import { adminRoutes } from "../../src/features/administration/api/adminApi";

describe("admin dashboard configuration", () => {
  it("keeps admin dashboard routes tenant scoped", () => {
    expect(adminDashboardConfigurationApi.phase).toBe("009-011");
    expect(adminRoutes.dashboard("school-demo")).toBe("/api/v1/schools/school-demo/admin/dashboard");
    expect(adminRoutes.configuration("school-demo")).toBe("/api/v1/schools/school-demo/admin/configuration");
  });
});
