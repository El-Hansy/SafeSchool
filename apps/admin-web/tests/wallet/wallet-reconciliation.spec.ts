import { describe, expect, it } from "vitest";
import { walletRoutes } from "../../src/features/wallet/api/client";
import { walletTestData } from "./walletTestData";

describe("wallet-reconciliation", () => {
  it("keeps wallet routes scoped under the school account", () => {
    expect(walletRoutes.reconciliation(walletTestData.schoolAccountId)).toContain(`/schools/${walletTestData.schoolAccountId}/wallet`);
  });
});
