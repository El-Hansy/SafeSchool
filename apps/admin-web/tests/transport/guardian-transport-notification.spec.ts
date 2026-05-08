import { describe, expect, it } from "vitest";
import { guardianNotificationRoutes } from "../../src/features/guardian-transport/notifications/guardianTransportNotificationsApi";

describe("guardian-transport-notification", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(guardianNotificationRoutes.notifications("student-1")).toContain("/notifications");
  });
});
