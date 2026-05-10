import { readFileSync } from "node:fs";
import { join } from "node:path";
import { describe, expect, it } from "vitest";
import { adminRoutes, loadAdminOperations } from "../../src/features/administration/api/adminApi";

describe("admin operational pages", () => {
  it("uses operational admin pages instead of static demo pages", () => {
    const pages = [
      "src/app/(school)/admin/page.tsx",
      "src/app/(school)/admin/configuration/page.tsx",
      "src/app/(school)/admin/audit/page.tsx",
      "src/app/(school)/admin/monitoring/page.tsx",
      "src/app/(school)/admin/alerts/page.tsx",
      "src/app/(school)/admin/incidents/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("AdminDemo");
      expect(source).toContain("AdminOperationsPage");
    }
  });

  it("loads admin fallback data for dashboard, audit, monitoring, alerts, and incidents", async () => {
    const data = await loadAdminOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.dashboard.modulesEnabled).toBeGreaterThan(0);
    expect(data.audit.total).toBeGreaterThan(0);
    expect(data.monitoring.metrics).toBeGreaterThan(0);
    expect(data.alertsList.length).toBeGreaterThan(0);
    expect(data.incidentsList.length).toBeGreaterThan(0);
  });

  it("keeps admin action routes tenant scoped", () => {
    expect(adminRoutes.auditExport("school-demo")).toBe("/api/v1/schools/school-demo/admin/audit/export");
    expect(adminRoutes.alertIncident("school-demo", "alert-1")).toBe("/api/v1/schools/school-demo/admin/alerts/alert-1/incidents");
  });

  it("exposes configuration, audit export, and incident forms through route helpers", () => {
    const source = readFileSync(join(process.cwd(), "src/features/administration/components/AdminActionForms.tsx"), "utf8");

    expect(source).toContain("adminRoutes.configuration");
    expect(source).toContain("adminRoutes.auditExport");
    expect(source).toContain("adminRoutes.alertIncident");
  });
});
