import { readFileSync } from "node:fs";
import { join } from "node:path";
import { describe, expect, it } from "vitest";
import { loadMedicalOperations, medicalRoutes } from "../../src/features/medical/api/medicalApi";

describe("medical operational pages", () => {
  it("uses operational medical pages for school, guardian, and student scopes", () => {
    const pages = [
      "src/app/(school)/medical/page.tsx",
      "src/app/(school)/medical/records/[studentProfileId]/page.tsx",
      "src/app/(school)/medical/emergency/page.tsx",
      "src/app/(school)/medical/incidents/page.tsx",
      "src/app/(school)/medical/notifications/page.tsx",
      "src/app/(school)/medical/history/page.tsx",
      "src/app/(school)/medical/configuration/page.tsx",
      "src/app/(school)/medical/trace/[recordId]/page.tsx",
      "src/app/(guardian)/guardian/medical/page.tsx",
      "src/app/(student)/student/medical/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).toMatch(/Medical(Operations|Secondary)Page/);
      expect(source).not.toContain("OperationalRoutePage");
    }
  });

  it("loads medical fallback records and scoped route helpers", async () => {
    const data = await loadMedicalOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.records.length).toBeGreaterThan(0);
    expect(data.emergency.length).toBeGreaterThan(0);
    expect(data.incidents.length).toBeGreaterThan(0);
    expect(data.guardianRecords.length).toBeGreaterThan(0);
    expect(data.board.capabilities).toContain("medical.emergency_access");
    expect(medicalRoutes.guardianUpdates()).toBe("/api/v1/guardians/me/medical/updates");
    expect(medicalRoutes.breakGlass("school-demo")).toBe("/api/v1/schools/school-demo/medical/emergency/break-glass");
  });

  it("keeps backend medical workflow routes mapped", () => {
    const source = readFileSync(join(process.cwd(), "../api/src/SafeSchool.Api/Features/Medical/MedicalModule.cs"), "utf8");

    expect(source).toContain('MapPost("/emergency/access"');
    expect(source).toContain('MapPost("/emergency/break-glass"');
    expect(source).toContain('MapPost("/incidents"');
    expect(source).toContain('MapPost("/notifications"');
    expect(source).toContain("GuardianRoutePrefix");
    expect(source).toContain("StudentRoutePrefix");
  });
});

