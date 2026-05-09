import { describe, expect, it } from "vitest";
import { walletRoutes } from "../../src/features/wallet/api/client";
import { walletTestData } from "./walletTestData";

describe("wallet-ledger", () => {
  it("keeps wallet routes scoped under the school account", () => {
    expect(walletRoutes.wallets(walletTestData.schoolAccountId)).toContain(`/schools/${walletTestData.schoolAccountId}/wallet`);
  });
});
