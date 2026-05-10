import { describe, expect, it } from "vitest";
import { guardianTransportApi } from "../../src/features/guardian-transport/api/client";

describe("guardian-transport-plan", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(guardianTransportApi.plan("student-1")).toContain("/students/student-1/transport/plan");
  });
});
