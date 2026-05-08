import { describe, expect, it } from "vitest";
import { trackingRoutes } from "../../src/features/transport/tracking/trackingApi";

describe("live-tracking", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(trackingRoutes.location("trip-1")).toBe("/trips/trip-1/location-updates");
  });
});
