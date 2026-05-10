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
      "src/app/(guardian)/guardian/communications/page.tsx",
      "src/app/(student)/student/communications/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("CommunicationsDemo");
      expect(source).toContain("CommunicationsOperationsPage");
    }
  });

  it("loads communication fallback data for school, guardian, and student scopes", async () => {
    const data = await loadCommunicationsOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.schoolEvents.length).toBeGreaterThan(0);
    expect(data.guardianNotifications.length).toBeGreaterThan(0);
    expect(data.studentNotifications.length).toBeGreaterThan(0);
    expect(data.board.capabilities).toContain("communications.notification_center");
  });

  it("keeps communication action routes scoped", () => {
    expect(communicationsRoutes.sourceEvents("school-demo")).toBe("/api/v1/schools/school-demo/communications/source-events");
    expect(communicationsRoutes.conversations("school-demo")).toBe("/api/v1/schools/school-demo/communications/conversations");
    expect(communicationsRoutes.broadcasts("school-demo")).toBe("/api/v1/schools/school-demo/communications/broadcasts");
    expect(communicationsRoutes.guardianNotifications()).toBe("/api/v1/guardians/me/communications/notifications");
    expect(communicationsRoutes.studentNotifications()).toBe("/api/v1/students/me/communications/notifications");
  });

  it("exposes source event, direct message, and broadcast forms through route helpers", () => {
    const source = readFileSync(join(process.cwd(), "src/features/communications/components/CommunicationActionForms.tsx"), "utf8");

    expect(source).toContain("communicationsRoutes.sourceEvents");
    expect(source).toContain("communicationsRoutes.conversations");
    expect(source).toContain("communicationsRoutes.broadcasts");
  });
});
