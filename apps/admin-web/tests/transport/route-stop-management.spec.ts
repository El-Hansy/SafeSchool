import { describe, expect, it } from "vitest";
import { transportRouteRoutes } from "../../src/features/transport/routes/routesApi";

describe("route-stop-management", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(transportRouteRoutes.sequence("route-1")).toBe("/routes/route-1/stop-sequences");
  });
});
