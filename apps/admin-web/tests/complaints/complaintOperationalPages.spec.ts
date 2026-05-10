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
      "src/app/(school)/complaints/[complaintId]/page.tsx",
      "src/app/(school)/complaints/[complaintId]/trace/page.tsx",
      "src/app/(school)/complaints/triage/page.tsx",
      "src/app/(school)/complaints/assigned/page.tsx",
      "src/app/(school)/complaints/assigned/[complaintId]/page.tsx",
      "src/app/(school)/complaints/escalations/page.tsx",
      "src/app/(school)/complaints/exceptions/page.tsx",
      "src/app/(school)/complaints/summaries/page.tsx",
      "src/app/(school)/complaints/configuration/categories/[categoryId]/page.tsx",
      "src/app/(school)/complaints/configuration/escalation-rules/[ruleId]/page.tsx",
      "src/app/(guardian)/guardian/complaints/page.tsx",
      "src/app/(student)/student/complaints/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("ComplaintsDemo");
      expect(source).not.toContain("OperationalRoutePage");
      expect(source).toMatch(/Complaints(Operations|Secondary)Page/);
    }
  });

  it("loads complaint fallback queues for school, guardian, and student scopes", async () => {
    const data = await loadComplaintOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.schoolComplaints.length).toBeGreaterThan(0);
    expect(data.guardianComplaints.length).toBeGreaterThan(0);
    expect(data.studentComplaints.length).toBeGreaterThan(0);
    expect(data.triageComplaints.length).toBeGreaterThan(0);
    expect(data.assignedComplaints.length).toBeGreaterThan(0);
    expect(data.escalatedComplaints.length).toBeGreaterThan(0);
    expect(data.exceptions.length).toBeGreaterThan(0);
    expect(data.summaries.length).toBeGreaterThan(0);
    expect(data.categories.length).toBeGreaterThan(0);
    expect(data.escalationRules.length).toBeGreaterThan(0);
    expect(data.board.capabilities).toContain("complaints.submission");
  });

  it("keeps complaint submission and workflow routes scoped", () => {
    expect(complaintsRoutes.school("school-demo")).toBe("/api/v1/schools/school-demo/complaints");
    expect(complaintsRoutes.guardian()).toBe("/api/v1/guardians/me/complaints");
    expect(complaintsRoutes.student()).toBe("/api/v1/students/me/complaints");
    expect(complaintsRoutes.detail("school-demo", "cmp-1")).toBe("/api/v1/schools/school-demo/complaints/cmp-1");
    expect(complaintsRoutes.triage("school-demo")).toBe("/api/v1/schools/school-demo/complaints/triage");
    expect(complaintsRoutes.assigned("school-demo")).toBe("/api/v1/schools/school-demo/complaints/assigned");
    expect(complaintsRoutes.assignedDetail("school-demo", "cmp-1")).toBe("/api/v1/schools/school-demo/complaints/assigned/cmp-1");
    expect(complaintsRoutes.escalations("school-demo")).toBe("/api/v1/schools/school-demo/complaints/escalations");
    expect(complaintsRoutes.exceptions("school-demo")).toBe("/api/v1/schools/school-demo/complaints/exceptions");
    expect(complaintsRoutes.summaries("school-demo")).toBe("/api/v1/schools/school-demo/complaints/summaries");
    expect(complaintsRoutes.category("school-demo", "wellbeing")).toBe("/api/v1/schools/school-demo/complaints/configuration/categories/wellbeing");
    expect(complaintsRoutes.escalationRule("school-demo", "urgent-overdue")).toBe("/api/v1/schools/school-demo/complaints/configuration/escalation-rules/urgent-overdue");
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

  it("keeps backend complaint secondary routes mapped", () => {
    const source = readFileSync(join(process.cwd(), "../api/src/SafeSchool.Api/Features/Complaints/ComplaintsModule.cs"), "utf8");

    expect(source).toContain('MapGet("/triage"');
    expect(source).toContain('MapGet("/assigned"');
    expect(source).toContain('MapGet("/assigned/{complaintId}"');
    expect(source).toContain('MapGet("/escalations"');
    expect(source).toContain('MapGet("/exceptions"');
    expect(source).toContain('MapGet("/summaries"');
    expect(source).toContain('MapGet("/configuration/categories/{categoryId}"');
    expect(source).toContain('MapGet("/configuration/escalation-rules/{ruleId}"');
    expect(source).toContain('MapGet("/{complaintId}"');
  });
});
