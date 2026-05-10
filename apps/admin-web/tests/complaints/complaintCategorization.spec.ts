import { describe, expect, it } from "vitest";
import { complaintCategorizationApi } from "../../src/features/complaints/api/complaintCategorizationApi";
import { complaintsDemoData } from "../../src/features/complaints/api/complaintsApi";

describe("complaint categorization", () => {
  it("captures category evidence in the complaint demo data", () => {
    expect(complaintCategorizationApi.phase).toBe("009-011");
    expect(complaintsDemoData.rows.join(" ")).toContain("bullying concern");
  });
});
