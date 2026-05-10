import { describe, expect, it } from "vitest";
import { broadcastAnnouncementApi } from "../../src/features/communications/api/broadcastAnnouncementApi";
import { communicationsDemoData, communicationsRoutes } from "../../src/features/communications/api/communicationsApi";

describe("broadcast announcements", () => {
  it("keeps broadcasts tenant scoped and represented in demo evidence", () => {
    expect(broadcastAnnouncementApi.phase).toBe("009-011");
    expect(communicationsRoutes.school(communicationsDemoData.schoolAccountId)).toBe(
      "/api/v1/schools/school-demo/communications",
    );
    expect(communicationsDemoData.rows).toContain("Emergency broadcast - published");
  });
});
