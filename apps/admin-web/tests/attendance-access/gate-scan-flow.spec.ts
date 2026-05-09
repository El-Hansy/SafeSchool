import { describe, expect, it } from "vitest";
import { gateRoutes } from "../../src/features/attendance-access/gates/gatesApi";
import { scanRoutes } from "../../src/features/attendance-access/scans/scansApi";

describe("gate scan routes", () => {
  it("keeps gate and scan operations under the attendance access area", () => {
    expect(gateRoutes.list).toBe("/gates");
    expect(scanRoutes.offlineSync).toBe("/scans/offline-sync");
    expect(scanRoutes.trace("scan-1")).toBe("/scans/scan-1/trace");
  });
});

