import { describe, expect, it } from "vitest";
import { guardianWalletRoutes } from "../../src/features/guardian-wallet/api/client";
import { walletTestData } from "./walletTestData";

describe("guardian-transaction-history", () => {
  it("keeps guardian wallet routes linked-student scoped", () => {
    expect(guardianWalletRoutes.transactions(walletTestData.studentProfileId)).toContain(`/students/${walletTestData.studentProfileId}/wallet`);
  });
});
