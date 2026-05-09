import { describe, expect, it } from "vitest";
import { walletRoutes } from "../../src/features/wallet/api/client";
import { walletTestData } from "./walletTestData";

describe("spending-limits", () => {
  it("keeps wallet routes scoped under the school account", () => {
    expect(walletRoutes.limits(walletTestData.schoolAccountId)).toContain(`/schools/${walletTestData.schoolAccountId}/wallet`);
  });
});
