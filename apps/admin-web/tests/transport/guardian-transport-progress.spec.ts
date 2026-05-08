import { describe, expect, it } from "vitest";
import { guardianTripProgressRoutes } from "../../src/features/guardian-transport/progress/guardianTripProgressApi";

describe("guardian-transport-progress", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(guardianTripProgressRoutes.progress("student-1", "trip-1")).toContain("/progress");
  });
});
