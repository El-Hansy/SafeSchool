import { describe, expect, it } from "vitest";
import { guardianWalletRoutes } from "../../src/features/guardian-wallet/api/client";
import { walletTestData } from "./walletTestData";

describe("guardian-wallet-summary", () => {
  it("keeps guardian wallet routes linked-student scoped", () => {
    expect(guardianWalletRoutes.reviewSummary(walletTestData.studentProfileId)).toContain(`/students/${walletTestData.studentProfileId}/wallet`);
  });
});
