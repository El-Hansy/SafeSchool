import { describe, expect, it } from "vitest";
import { scanRoutes } from "../../src/features/transport/scans/scansApi";

describe("boarding-drop-scan", () => {
  it("keeps Phase 3 transport scope explicit", () => {
    expect(scanRoutes.sync).toBe("/scan-events/sync");
  });
});
