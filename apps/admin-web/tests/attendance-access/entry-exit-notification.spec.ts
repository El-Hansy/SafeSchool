import { describe, expect, it } from "vitest";
import { notificationRoutes } from "../../src/features/attendance-access/notifications/notificationsApi";

describe("notification routes", () => {
  it("models notification review endpoints", () => {
    expect(notificationRoutes.list).toBe("/notifications");
    expect(notificationRoutes.withdraw("notification-1")).toBe("/notifications/notification-1/withdraw");
  });
});

