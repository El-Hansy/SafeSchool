import { describe, expect, it } from "vitest";
import { etaRoutes } from "../../src/features/transport/eta/etaApi";

describe("eta-calculation", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(etaRoutes.recalculate("trip-1")).toBe("/trips/trip-1/eta/recalculate");
  });
});
