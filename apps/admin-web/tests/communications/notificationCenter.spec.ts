import { describe, expect, it } from "vitest";
import { communicationsRoutes } from "../../src/features/communications/api/communicationsApi";
import { notificationsApi } from "../../src/features/communications/api/notificationsApi";

describe("notification center", () => {
  it("exposes guardian and student notification scopes", () => {
    expect(notificationsApi.phase).toBe("009-011");
    expect(communicationsRoutes.guardianNotifications()).toBe(
      "/api/v1/guardians/me/communications/notifications",
    );
    expect(communicationsRoutes.studentNotifications()).toBe(
      "/api/v1/students/me/communications/notifications",
    );
  });
});
