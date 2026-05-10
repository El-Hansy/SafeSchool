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
      "src/app/(school)/admin/features/page.tsx",
      "src/app/(school)/admin/features/history/page.tsx",
      "src/app/(school)/admin/audit/exports/page.tsx",
      "src/app/(school)/admin/audit/[auditEventId]/page.tsx",
      "src/app/(school)/admin/monitoring/alert-rules/page.tsx",
      "src/app/(school)/admin/monitoring/alerts/page.tsx",
      "src/app/(school)/admin/monitoring/exceptions/page.tsx",
      "src/app/(school)/admin/monitoring/incidents/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("AdminDemo");
      expect(source).not.toContain("OperationalRoutePage");
      expect(source).toMatch(/Admin(Operations|Secondary)Page/);
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
    expect(data.configurationHistory.length).toBeGreaterThan(0);
    expect(data.auditEvents.length).toBeGreaterThan(0);
    expect(data.auditExports.length).toBeGreaterThan(0);
    expect(data.alertRules.length).toBeGreaterThan(0);
    expect(data.operationalExceptions.length).toBeGreaterThan(0);
  });

  it("keeps admin action routes tenant scoped", () => {
    expect(adminRoutes.configurationHistory("school-demo")).toBe("/api/v1/schools/school-demo/admin/configuration/history");
    expect(adminRoutes.auditEvent("school-demo", "audit-1")).toBe("/api/v1/schools/school-demo/admin/audit/audit-1");
    expect(adminRoutes.auditExports("school-demo")).toBe("/api/v1/schools/school-demo/admin/audit/exports");
    expect(adminRoutes.auditExport("school-demo")).toBe("/api/v1/schools/school-demo/admin/audit/export");
    expect(adminRoutes.alertRules("school-demo")).toBe("/api/v1/schools/school-demo/admin/monitoring/alert-rules");
    expect(adminRoutes.monitoringAlerts("school-demo")).toBe("/api/v1/schools/school-demo/admin/monitoring/alerts");
    expect(adminRoutes.monitoringExceptions("school-demo")).toBe("/api/v1/schools/school-demo/admin/monitoring/exceptions");
    expect(adminRoutes.monitoringIncidents("school-demo")).toBe("/api/v1/schools/school-demo/admin/monitoring/incidents");
    expect(adminRoutes.alertIncident("school-demo", "alert-1")).toBe("/api/v1/schools/school-demo/admin/alerts/alert-1/incidents");
  });

  it("exposes configuration, audit export, and incident forms through route helpers", () => {
    const source = readFileSync(join(process.cwd(), "src/features/administration/components/AdminActionForms.tsx"), "utf8");

    expect(source).toContain("adminRoutes.configuration");
    expect(source).toContain("adminRoutes.auditExport");
    expect(source).toContain("adminRoutes.alertIncident");
  });

  it("keeps backend admin secondary contracts mapped", () => {
    const source = readFileSync(join(process.cwd(), "../api/src/SafeSchool.Api/Features/Administration/AdministrationModule.cs"), "utf8");

    expect(source).toContain('MapGet("/configuration/history"');
    expect(source).toContain('MapGet("/audit/exports"');
    expect(source).toContain('MapGet("/audit/{auditEventId}"');
    expect(source).toContain('MapGet("/monitoring/alert-rules"');
    expect(source).toContain('MapGet("/monitoring/alerts"');
    expect(source).toContain('MapGet("/monitoring/exceptions"');
    expect(source).toContain('MapGet("/monitoring/incidents"');
  });
});
