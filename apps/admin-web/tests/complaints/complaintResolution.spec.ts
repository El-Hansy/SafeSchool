import { describe, expect, it } from "vitest";
import { complaintResolutionApi } from "../../src/features/complaints/api/complaintResolutionApi";
import { complaintsDemoData } from "../../src/features/complaints/api/complaintsApi";

describe("complaint resolution", () => {
  it("keeps resolution and feedback evidence in demo data", () => {
    expect(complaintResolutionApi.phase).toBe("009-011");
    expect(complaintsDemoData.rows).toContain("CMP-2026-0003 - canteen issue - resolved");
    expect(complaintsDemoData.metrics.map((metric) => metric.label)).toContain("Pending feedback");
  });
});
