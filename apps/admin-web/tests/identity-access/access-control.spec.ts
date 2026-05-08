import { describe, expect, it } from "vitest";
import { accessControlRoutes } from "../../src/features/identity-access/access-control/accessControlApi";

describe("access control routes", () => {
  it("exposes reviewer and role administration routes", () => {
    expect(accessControlRoutes.roles).toBe("/roles");
    expect(accessControlRoutes.accessDecisions).toBe("/access-decisions");
    expect(accessControlRoutes.auditEvents).toBe("/audit-events");
  });
});
