import { describe, expect, it } from "vitest";
import { learningRoutes, learningBoundaryNotes } from "../../src/features/learning/api/learningApi";
import { learningTestData } from "./learningTestData";

describe("behavior-logging", () => {
  it("keeps Phase 5 learning routes tenant scoped and boundary safe", () => {
    expect(learningRoutes.behavior(learningTestData.schoolAccountId)).toContain(`/schools/${learningTestData.schoolAccountId}/learning`);
    expect(learningBoundaryNotes().join(" ")).toContain("wallet");
  });
});
