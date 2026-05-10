import { describe, expect, it } from "vitest";
import { mobileDemoSteps, safeSchoolModuleLinks } from "../../src/features/home";

describe("home dashboard", () => {
  it("links every implemented phase from the root command center", () => {
    expect(safeSchoolModuleLinks).toHaveLength(12);
    expect(safeSchoolModuleLinks.map((module) => module.phase)).toEqual([
      "001",
      "002",
      "003",
      "004",
      "005",
      "006",
      "007",
      "008",
      "009",
      "010",
      "011",
      "012",
    ]);
  });

  it("includes the Android APK demo actions", () => {
    expect(mobileDemoSteps.join(" ")).toContain("app-release.apk");
    expect(mobileDemoSteps.join(" ")).toContain("Simulate gate NFC");
    expect(mobileDemoSteps.join(" ")).toContain("Canteen cashier");
  });
});
