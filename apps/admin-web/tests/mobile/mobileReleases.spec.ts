import { describe, expect, it } from "vitest";
import { mobileReleases } from "../../src/features/mobile";

describe("mobile releases", () => {
  it("tracks active APK release evidence", () => {
    expect(mobileReleases[0].status).toBe("Active");
    expect(mobileReleases[0].checksum).toContain("sha256");
  });
});
