import { describe, expect, it } from "vitest";
import { guardianEntryExitRoutes } from "../../src/features/attendance-access/guardian-entry-exit/guardianEntryExitApi";

describe("guardian entry exit routes", () => {
  it("uses the guardian scoped me route", () => {
    expect(guardianEntryExitRoutes.me).toBe("/guardian-entry-exit/me");
  });
});

