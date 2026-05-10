import { readFileSync } from "node:fs";
import { join } from "node:path";
import { describe, expect, it } from "vitest";
import { guardianWalletRoutes, loadGuardianWalletData } from "../../src/features/guardian-wallet/api/client";
import { loadSchoolWalletOperations, walletRoutes } from "../../src/features/wallet/api/client";
import { walletTestData } from "./walletTestData";

describe("wallet operational pages", () => {
  it("uses operational wallet pages instead of static demo pages", () => {
    const pages = [
      "src/app/(school)/wallet/page.tsx",
      "src/app/(school)/wallet/top-ups/page.tsx",
      "src/app/(school)/wallet/pos/page.tsx",
      "src/app/(guardian)/guardian/wallet/page.tsx",
      "src/app/(guardian)/guardian/wallet/top-ups/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("WalletDemo");
      expect(source).toMatch(/WalletOperationsPage|GuardianWalletExperience/);
    }
  });

  it("loads tenant-scoped fallback data when the API base URL is not configured", async () => {
    const data = await loadSchoolWalletOperations(walletTestData.schoolAccountId);

    expect(data.dataSource).toBe("fallback");
    expect(data.schoolAccountId).toBe(walletTestData.schoolAccountId);
    expect(data.wallets.length).toBeGreaterThan(0);
    expect(data.topUps.length).toBeGreaterThan(0);
    expect(data.purchases.length).toBeGreaterThan(0);
  });

  it("keeps guardian wallet summary and top-up routes linked-student scoped", async () => {
    expect(guardianWalletRoutes.wallet(walletTestData.studentProfileId)).toBe(
      `/api/v1/guardians/me/students/${walletTestData.studentProfileId}/wallet`,
    );
    expect(walletRoutes.cashierTopUp(walletTestData.schoolAccountId)).toBe(
      `/api/v1/schools/${walletTestData.schoolAccountId}/wallet/top-ups/cashier`,
    );

    const data = await loadGuardianWalletData(walletTestData.studentProfileId, walletTestData.schoolAccountId);
    expect(data.dataSource).toBe("fallback");
    expect(data.wallet?.studentProfileId).toBe(walletTestData.studentProfileId);
  });
});
