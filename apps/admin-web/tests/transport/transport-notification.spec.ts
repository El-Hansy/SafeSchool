import { describe, expect, it } from "vitest";
import { notificationRoutes } from "../../src/features/transport/notifications/notificationsApi";

describe("transport-notification", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(notificationRoutes.withdraw("n1")).toBe("/notification-records/n1/withdraw");
  });
});
