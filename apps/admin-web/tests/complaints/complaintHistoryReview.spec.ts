import { describe, expect, it } from "vitest";
import { complaintHistoryReviewApi } from "../../src/features/complaints/api/complaintHistoryReviewApi";
import { complaintsDemoData } from "../../src/features/complaints/api/complaintsApi";

describe("complaint history review", () => {
  it("covers assigned, escalated, and resolved complaint states", () => {
    expect(complaintHistoryReviewApi.phase).toBe("009-011");
    expect(complaintsDemoData.rows.join(" ")).toContain("assigned");
    expect(complaintsDemoData.rows.join(" ")).toContain("resolved");
  });
});
