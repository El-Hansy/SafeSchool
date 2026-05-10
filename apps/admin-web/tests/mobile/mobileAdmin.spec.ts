import { describe, expect, it } from "vitest";
import { mobileStats } from "../../src/features/mobile";

describe("mobile admin", () => {
  it("summarizes phase 12 mobile coverage", () => {
    expect(mobileStats().roles).toBe(12);
    expect(mobileStats().rtlCoverage).toContain("Arabic");
  });
});
