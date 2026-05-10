import { describe, expect, it } from "vitest";
import { walletRoutes } from "../../src/features/wallet/api/client";
import { walletTestData } from "./walletTestData";

describe("cashier-top-up", () => {
  it("keeps wallet routes scoped under the school account", () => {
    expect(walletRoutes.cashierTopUp(walletTestData.schoolAccountId)).toContain(`/schools/${walletTestData.schoolAccountId}/wallet`);
  });
});
