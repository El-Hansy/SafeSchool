import { readFileSync } from "node:fs";
import { join } from "node:path";
import { describe, expect, it } from "vitest";
import { communicationsRoutes, loadCommunicationsOperations } from "../../src/features/communications/api/communicationsApi";

describe("communication operational pages", () => {
  it("uses operational communication pages instead of static demo pages", () => {
    const pages = [
      "src/app/(school)/communications/page.tsx",
      "src/app/(school)/communications/notifications/page.tsx",
      "src/app/(school)/communications/messages/page.tsx",
      "src/app/(school)/communications/broadcasts/page.tsx",
      "src/app/(school)/communications/notifications/[notificationId]/page.tsx",
      "src/app/(school)/communications/acknowledgements/page.tsx",
      "src/app/(school)/communications/delivery/page.tsx",
      "src/app/(school)/communications/summaries/page.tsx",
      "src/app/(school)/communications/history/page.tsx",
      "src/app/(school)/communications/conversations/page.tsx",
      "src/app/(school)/communications/conversations/new/page.tsx",
      "src/app/(school)/communications/conversations/[conversationId]/page.tsx",
      "src/app/(school)/communications/[communicationKind]/[communicationId]/trace/page.tsx",
      "src/app/(school)/communications/exceptions/page.tsx",
      "src/app/(school)/communications/configuration/page.tsx",
      "src/app/(school)/communications/configuration/audience-rules/[ruleId]/page.tsx",
      "src/app/(school)/communications/configuration/templates/[templateId]/page.tsx",
      "src/app/(school)/communications/broadcasts/new/page.tsx",
      "src/app/(school)/communications/broadcasts/[broadcastId]/page.tsx",
      "src/app/(school)/communications/moderation/page.tsx",
      "src/app/(guardian)/guardian/communications/page.tsx",
      "src/app/(student)/student/communications/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("CommunicationsDemo");
      expect(source).not.toContain("OperationalRoutePage");
      expect(source).toMatch(/Communications(Operations|Secondary)Page/);
    }
  });

  it("loads communication fallback data for school, guardian, and student scopes", async () => {
    const data = await loadCommunicationsOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.schoolEvents.length).toBeGreaterThan(0);
    expect(data.guardianNotifications.length).toBeGreaterThan(0);
    expect(data.studentNotifications.length).toBeGreaterThan(0);
    expect(data.acknowledgements.length).toBeGreaterThan(0);
    expect(data.delivery.length).toBeGreaterThan(0);
    expect(data.summaries.length).toBeGreaterThan(0);
    expect(data.history.length).toBeGreaterThan(0);
    expect(data.conversations.length).toBeGreaterThan(0);
    expect(data.broadcasts.length).toBeGreaterThan(0);
    expect(data.moderation.length).toBeGreaterThan(0);
    expect(data.exceptions.length).toBeGreaterThan(0);
    expect(data.audienceRules.length).toBeGreaterThan(0);
    expect(data.templates.length).toBeGreaterThan(0);
    expect(data.board.capabilities).toContain("communications.notification_center");
  });

  it("keeps communication action routes scoped", () => {
    expect(communicationsRoutes.sourceEvents("school-demo")).toBe("/api/v1/schools/school-demo/communications/source-events");
    expect(communicationsRoutes.notificationDetail("school-demo", "notification-1")).toBe("/api/v1/schools/school-demo/communications/notifications/notification-1");
    expect(communicationsRoutes.acknowledgements("school-demo")).toBe("/api/v1/schools/school-demo/communications/acknowledgements");
    expect(communicationsRoutes.delivery("school-demo")).toBe("/api/v1/schools/school-demo/communications/delivery");
    expect(communicationsRoutes.summaries("school-demo")).toBe("/api/v1/schools/school-demo/communications/summaries");
    expect(communicationsRoutes.history("school-demo")).toBe("/api/v1/schools/school-demo/communications/history");
    expect(communicationsRoutes.moderation("school-demo")).toBe("/api/v1/schools/school-demo/communications/moderation");
    expect(communicationsRoutes.exceptions("school-demo")).toBe("/api/v1/schools/school-demo/communications/exceptions");
    expect(communicationsRoutes.configuration("school-demo")).toBe("/api/v1/schools/school-demo/communications/configuration");
    expect(communicationsRoutes.audienceRule("school-demo", "grade-4-guardians")).toBe("/api/v1/schools/school-demo/communications/configuration/audience-rules/grade-4-guardians");
    expect(communicationsRoutes.template("school-demo", "pickup-update")).toBe("/api/v1/schools/school-demo/communications/configuration/templates/pickup-update");
    expect(communicationsRoutes.conversations("school-demo")).toBe("/api/v1/schools/school-demo/communications/conversations");
    expect(communicationsRoutes.conversationDetail("school-demo", "conversation-1")).toBe("/api/v1/schools/school-demo/communications/conversations/conversation-1");
    expect(communicationsRoutes.broadcasts("school-demo")).toBe("/api/v1/schools/school-demo/communications/broadcasts");
    expect(communicationsRoutes.broadcastDetail("school-demo", "broadcast-1")).toBe("/api/v1/schools/school-demo/communications/broadcasts/broadcast-1");
    expect(communicationsRoutes.guardianNotifications()).toBe("/api/v1/guardians/me/communications/notifications");
    expect(communicationsRoutes.studentNotifications()).toBe("/api/v1/students/me/communications/notifications");
  });

  it("exposes source event, direct message, and broadcast forms through route helpers", () => {
    const source = readFileSync(join(process.cwd(), "src/features/communications/components/CommunicationActionForms.tsx"), "utf8");

    expect(source).toContain("communicationsRoutes.sourceEvents");
    expect(source).toContain("communicationsRoutes.conversations");
    expect(source).toContain("communicationsRoutes.broadcasts");
  });

  it("keeps backend communication secondary routes mapped", () => {
    const source = readFileSync(join(process.cwd(), "../api/src/SafeSchool.Api/Features/Communications/CommunicationsModule.cs"), "utf8");

    expect(source).toContain('MapGet("/notifications/{notificationId}"');
    expect(source).toContain('MapGet("/acknowledgements"');
    expect(source).toContain('MapGet("/delivery"');
    expect(source).toContain('MapGet("/summaries"');
    expect(source).toContain('MapGet("/history"');
    expect(source).toContain('MapGet("/moderation"');
    expect(source).toContain('MapGet("/exceptions"');
    expect(source).toContain('MapGet("/configuration"');
    expect(source).toContain('MapGet("/configuration/audience-rules/{ruleId}"');
    expect(source).toContain('MapGet("/configuration/templates/{templateId}"');
    expect(source).toContain('MapGet("/conversations"');
    expect(source).toContain('MapGet("/conversations/{conversationId}"');
    expect(source).toContain('MapGet("/broadcasts/{broadcastId}"');
  });
});
