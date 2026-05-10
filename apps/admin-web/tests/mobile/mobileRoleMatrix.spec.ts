import { describe, expect, it } from "vitest";
import { mobileWorkspaces } from "../../src/features/mobile";

describe("mobile role matrix", () => {
  it("contains guardian as a role and all staff workspaces", () => {
    expect(mobileWorkspaces.map((workspace) => workspace.role)).toContain("guardian");
    expect(mobileWorkspaces.map((workspace) => workspace.role)).toContain("platform_support");
    expect(mobileWorkspaces).toHaveLength(12);
  });
});
