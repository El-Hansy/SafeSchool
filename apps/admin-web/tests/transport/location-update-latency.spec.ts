import { describe, expect, it } from "vitest";
import { trackingRoutes } from "../../src/features/transport/tracking/trackingApi";

describe("location-update-latency", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(trackingRoutes.progress("trip-1")).toBe("/trips/trip-1/progress");
  });
});
