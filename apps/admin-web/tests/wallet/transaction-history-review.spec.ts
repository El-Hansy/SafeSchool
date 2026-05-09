import { describe, expect, it } from "vitest";
import { walletRoutes } from "../../src/features/wallet/api/client";
import { walletTestData } from "./walletTestData";

describe("transaction-history-review", () => {
  it("keeps wallet routes scoped under the school account", () => {
    expect(walletRoutes.transactions(walletTestData.schoolAccountId)).toContain(`/schools/${walletTestData.schoolAccountId}/wallet`);
  });
});
