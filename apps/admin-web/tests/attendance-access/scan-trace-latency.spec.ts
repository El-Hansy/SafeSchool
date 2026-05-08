import { describe, expect, it } from "vitest";
import { scanRoutes } from "../../src/features/attendance-access/scans/scansApi";

describe("scan trace latency route", () => {
  it("uses the trace endpoint covered by SC-007", () => {
    expect(scanRoutes.trace("scan-1")).toContain("/trace");
  });
});

