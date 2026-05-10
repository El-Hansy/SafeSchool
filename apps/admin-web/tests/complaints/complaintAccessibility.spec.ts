import { describe, expect, it } from "vitest";
import { complaintsDemoData, complaintsRoutes } from "../../src/features/complaints/api/complaintsApi";

describe("complaint accessibility", () => {
  it("keeps guardian and student complaint scopes explicit", () => {
    expect(complaintsRoutes.guardian()).toBe("/api/v1/guardians/me/complaints");
    expect(complaintsRoutes.student()).toBe("/api/v1/students/me/complaints");
    expect(complaintsDemoData.metrics.every((metric) => metric.label && metric.value)).toBe(true);
  });
});
