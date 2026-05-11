import { readFileSync } from "node:fs";
import { join } from "node:path";
import { describe, expect, it } from "vitest";
import { loadRequestOperations, requestRoutes } from "../../src/features/requests/api/requestsApi";

describe("request operational pages", () => {
  it("uses operational request pages for school, guardian, and student scopes", () => {
    const pages = [
      "src/app/(school)/requests/page.tsx",
      "src/app/(school)/requests/new/page.tsx",
      "src/app/(school)/requests/approvals/page.tsx",
      "src/app/(school)/requests/history/page.tsx",
      "src/app/(school)/requests/configuration/page.tsx",
      "src/app/(school)/requests/[requestId]/page.tsx",
      "src/app/(school)/requests/[requestId]/trace/page.tsx",
      "src/app/(guardian)/guardian/requests/page.tsx",
      "src/app/(student)/student/requests/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).toMatch(/Requests(Operations|Secondary)Page/);
      expect(source).not.toContain("OperationalRoutePage");
    }
  });

  it("loads request fallback queues and scoped route helpers", async () => {
    const data = await loadRequestOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.guardianRequests.length).toBeGreaterThan(0);
    expect(data.studentRequests.length).toBeGreaterThan(0);
    expect(data.approvalQueue.length).toBeGreaterThan(0);
    expect(data.statusEvents.length).toBeGreaterThan(0);
    expect(data.reviewSummaries.length).toBeGreaterThan(0);
    expect(data.board.capabilities).toContain("requests.approval");
    expect(requestRoutes.guardian()).toBe("/api/v1/guardians/me/requests");
    expect(requestRoutes.student()).toBe("/api/v1/students/me/requests");
    expect(requestRoutes.statusEvents("school-demo")).toBe("/api/v1/schools/school-demo/requests/status-events");
    expect(requestRoutes.reviewSummaries("school-demo")).toBe("/api/v1/schools/school-demo/requests/review-summaries");
    expect(requestRoutes.history("school-demo", { requestType: "early-leave", status: "Approved" })).toBe("/api/v1/schools/school-demo/requests/history?requestType=early-leave&status=Approved");
    expect(requestRoutes.statusEvents("school-demo", { notificationEligible: true })).toBe("/api/v1/schools/school-demo/requests/status-events?notificationEligible=true");
    expect(requestRoutes.approve("school-demo", "req-1")).toBe("/api/v1/schools/school-demo/requests/req-1/approve");
  });

  it("keeps backend request workflow routes mapped", () => {
    const source = readFileSync(join(process.cwd(), "../api/src/SafeSchool.Api/Features/Requests/RequestsModule.cs"), "utf8");

    expect(source).toContain('school.MapPost("/",');
    expect(source).toContain('MapGet("/approvals"');
    expect(source).toContain('MapGet("/status-events"');
    expect(source).toContain('MapGet("/review-summaries"');
    expect(source).toContain('MapGet("/early-leave"');
    expect(source).toContain('MapPost("/{requestId}/approve"');
    expect(source).toContain("GuardianRoutePrefix");
    expect(source).toContain("StudentRoutePrefix");
  });
});
