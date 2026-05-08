import { describe, expect, it } from "vitest";
import { studentProfileRoutes } from "../../src/features/identity-access/student-profiles/studentProfilesApi";

describe("student profile routes", () => {
  it("keeps student profile operations under the identity area", () => {
    expect(studentProfileRoutes.list).toBe("/students");
    expect(studentProfileRoutes.deactivate("student-1")).toBe("/students/student-1/deactivate");
  });
});
