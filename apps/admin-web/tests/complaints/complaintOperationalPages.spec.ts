import { readFileSync } from "node:fs";
import { join } from "node:path";
import { describe, expect, it } from "vitest";
import { complaintsRoutes, loadComplaintOperations } from "../../src/features/complaints/api/complaintsApi";

describe("complaint operational pages", () => {
  it("uses operational complaint pages instead of static demo pages", () => {
    const pages = [
      "src/app/(school)/complaints/page.tsx",
      "src/app/(school)/complaints/new/page.tsx",
      "src/app/(school)/complaints/history/page.tsx",
      "src/app/(school)/complaints/configuration/page.tsx",
      "src/app/(guardian)/guardian/complaints/page.tsx",
      "src/app/(student)/student/complaints/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("ComplaintsDemo");
      expect(source).toContain("ComplaintsOperationsPage");
    }
  });

  it("loads complaint fallback queues for school, guardian, and student scopes", async () => {
    const data = await loadComplaintOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.schoolComplaints.length).toBeGreaterThan(0);
    expect(data.guardianComplaints.length).toBeGreaterThan(0);
    expect(data.studentComplaints.length).toBeGreaterThan(0);
    expect(data.board.capabilities).toContain("complaints.submission");
  });

  it("keeps complaint submission and workflow routes scoped", () => {
    expect(complaintsRoutes.school("school-demo")).toBe("/api/v1/schools/school-demo/complaints");
    expect(complaintsRoutes.guardian()).toBe("/api/v1/guardians/me/complaints");
    expect(complaintsRoutes.student()).toBe("/api/v1/students/me/complaints");
    expect(complaintsRoutes.assign("school-demo", "cmp-1")).toBe("/api/v1/schools/school-demo/complaints/cmp-1/assign");
    expect(complaintsRoutes.guardianFeedback("cmp-1")).toBe("/api/v1/guardians/me/complaints/cmp-1/feedback");
  });

  it("exposes complaint submission and owner action forms through route helpers", () => {
    const source = readFileSync(join(process.cwd(), "src/features/complaints/components/ComplaintActionForms.tsx"), "utf8");

    expect(source).toContain("complaintsRoutes.school");
    expect(source).toContain("complaintsRoutes.guardian");
    expect(source).toContain("complaintsRoutes.student");
    expect(source).toContain("complaintsRoutes.assign");
    expect(source).toContain("complaintsRoutes.resolve");
  });
});
