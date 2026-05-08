import { describe, expect, it } from "vitest";
import { guardianEtaRoutes } from "../../src/features/guardian-transport/eta/guardianEtaApi";

describe("guardian-transport-eta", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(guardianEtaRoutes.eta("student-1", "trip-1")).toContain("/eta");
  });
});
