import { describe, expect, it } from "vitest";
import { complaintEscalationApi } from "../../src/features/complaints/api/complaintEscalationApi";
import { complaintsDemoData } from "../../src/features/complaints/api/complaintsApi";

describe("complaint escalation", () => {
  it("surfaces escalated complaint evidence", () => {
    expect(complaintEscalationApi.phase).toBe("009-011");
    expect(complaintsDemoData.metrics.map((metric) => metric.label)).toContain("Escalated");
    expect(complaintsDemoData.rows.join(" ")).toContain("escalated");
  });
});
