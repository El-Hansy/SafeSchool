import { describe, expect, it } from "vitest";
import { guardianRoutes } from "../../src/features/identity-access/guardians/guardiansApi";

describe("guardian linking routes", () => {
  it("models scoped guardian link lifecycle actions", () => {
    expect(guardianRoutes.createLink("student-1")).toBe("/students/student-1/guardian-links");
    expect(guardianRoutes.suspendLink("link-1")).toBe("/guardian-links/link-1/suspend");
    expect(guardianRoutes.removeLink("link-1")).toBe("/guardian-links/link-1/remove");
  });
});
