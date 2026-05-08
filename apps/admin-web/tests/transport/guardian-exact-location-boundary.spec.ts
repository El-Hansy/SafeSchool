import { describe, expect, it } from "vitest";
import { guardianTripProgressRoutes } from "../../src/features/guardian-transport/progress/guardianTripProgressApi";

describe("guardian-exact-location-boundary", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(guardianTripProgressRoutes.progress("student-1", "trip-1")).not.toContain("school-2");
  });
});
