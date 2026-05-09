import { describe, expect, it } from "vitest";
import { guardianWalletRoutes } from "../../src/features/guardian-wallet/api/client";
import { walletTestData } from "./walletTestData";

describe("guardian-top-up", () => {
  it("keeps guardian wallet routes linked-student scoped", () => {
    expect(guardianWalletRoutes.topUps(walletTestData.studentProfileId)).toContain(`/students/${walletTestData.studentProfileId}/wallet`);
  });
});
