import { describe, expect, it } from "vitest";
import { assignmentRoutes } from "../../src/features/transport/assignments/assignmentsApi";

describe("bus-assignment", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(assignmentRoutes.trace("assignment-1")).toBe("/assignments/assignment-1/trace");
  });
});
