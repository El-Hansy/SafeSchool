import { readdirSync, readFileSync } from "node:fs";
import { join } from "node:path";
import { fileURLToPath } from "node:url";
import { describe, expect, it } from "vitest";
import { metadata } from "../../src/app/layout";
import { mobileDemoSteps, safeSchoolModuleLinks } from "../../src/features/home";

const appRoot = fileURLToPath(new URL("../../src/app", import.meta.url));
const featuresRoot = fileURLToPath(new URL("../../src/features", import.meta.url));

function collectFiles(directory: string): string[] {
  return readdirSync(directory, { withFileTypes: true }).flatMap((entry) => {
    const fullPath = join(directory, entry.name);

    if (entry.isDirectory()) {
      return collectFiles(fullPath);
    }

    return entry.isFile() ? [fullPath] : [];
  });
}

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

  it("uses master command center metadata", () => {
    expect(metadata.title).toBe("SafeSchool Command Center");
    expect(metadata.description).toContain("Role-based school NFC");
    expect(metadata.description).toContain("mobile");
  });

  it("does not ship literal phase demo route pages", () => {
    const routeFiles = collectFiles(appRoot).filter((file) => file.endsWith("page.tsx"));
    const offenders = routeFiles.filter((file) =>
      readFileSync(file, "utf8").includes("SafeSchool phase demo"),
    );

    expect(offenders).toEqual([]);
  });

  it("keeps visible demo badges aligned with master phase ids", () => {
    const demoBadges = [
      ["transport/demo/TransportDemo.tsx", "SafeSchool 004"],
      ["wallet/demo/WalletDemo.tsx", "SafeSchool 005"],
      ["learning/demo/LearningDemo.tsx", "SafeSchool 006"],
      ["complaints/demo/ComplaintsDemo.tsx", "SafeSchool 009"],
      ["communications/demo/CommunicationsDemo.tsx", "SafeSchool 010"],
      ["documents/demo/DocumentsDemo.tsx", "SafeSchool 011"],
      ["administration/demo/AdminDemo.tsx", "SafeSchool 011"],
      ["mobile/components/MobileReleaseNotesEditor.tsx", "SafeSchool 012"],
    ];

    for (const [relativePath, badge] of demoBadges) {
      const source = readFileSync(join(featuresRoot, relativePath), "utf8");
      expect(source).toContain(badge);
    }
  });
});
