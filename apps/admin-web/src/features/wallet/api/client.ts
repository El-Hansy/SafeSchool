export type WalletApiError = { code: string; message: string; field?: string };
export type WalletMoney = { amountMinor: number; currencyCode: string };
export type WalletDataSource = "api" | "fallback";

export type WalletResponse = {
  walletId: string;
  tenantId: string;
  studentProfileId: string;
  walletCode: string;
  currencyCode: string;
  availableBalanceMinor: number;
  pendingBalanceMinor: number;
  heldBalanceMinor: number;
  settledBalanceMinor: number;
  pendingRecoveryMinor: number;
  walletStatus: string;
  restrictionReason: string;
};

export type TopUpResponse = {
  topUpId: string;
  walletId: string;
  studentProfileId: string;
  source: string;
  amountMinor: number;
  netCreditMinor: number;
  currencyCode: string;
  status: string;
  safeReference: string;
  reviewReason: string;
};

export type PurchaseResponse = {
  purchaseId: string;
  walletId: string;
  merchantId: string;
  posTerminalId: string;
  clientPurchaseId: string;
  amountMinor: number;
  currencyCode: string;
  decision: string;
  decisionReason: string;
  ruleSnapshotReference: string;
  reserveSnapshotReference: string;
};

export type SpendingLimitResponse = {
  spendingLimitId: string;
  walletId: string;
  ownerType: string;
  limitType: string;
  amountMinor: number;
  currencyCode: string;
  merchantCode: string;
  itemCategoryCode: string;
  status: string;
  ruleVersion: string;
};

export type TransactionHistoryResponse = {
  transactionId: string;
  walletId: string;
  transactionType: string;
  amountMinor: number;
  currencyCode: string;
  status: string;
  merchantOrSource: string;
  evidenceTime: string;
  staffOnlyDetailSuppressed: boolean;
};

export type ReconciliationRunResponse = {
  reconciliationRunId: string;
  scope: string;
  dateFrom: string;
  dateUntil: string;
  status: string;
  ledgerTotalMinor: number;
  sourceTotalMinor: number;
  differenceMinor: number;
};

export type WalletOperationsData = {
  schoolAccountId: string;
  dataSource: WalletDataSource;
  wallets: WalletResponse[];
  topUps: TopUpResponse[];
  purchases: PurchaseResponse[];
  limits: SpendingLimitResponse[];
  transactions: TransactionHistoryResponse[];
  reconciliationRuns: ReconciliationRunResponse[];
};

export const walletRoutes = {
  base: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet`,
  wallets: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet/student-wallets`,
  walletTrace: (schoolAccountId: string, walletId: string) => `/api/v1/schools/${schoolAccountId}/wallet/student-wallets/${walletId}/trace`,
  topUps: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet/top-ups`,
  cashierTopUp: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet/top-ups/cashier`,
  posPurchases: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet/canteen/purchases`,
  limits: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet/spending-limits`,
  transactions: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet/transactions`,
  reconciliation: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet/reconciliation-runs`,
  ruleSettings: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet/rule-settings/current`,
  reviewSummaries: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/wallet/review-summaries`,
};

export function formatMoney(amountMinor: number, currencyCode = "SAR") {
  return `${currencyCode} ${(amountMinor / 100).toFixed(2)}`;
}

export const walletDemoData: WalletOperationsData = {
  schoolAccountId: "school-demo",
  dataSource: "fallback",
  wallets: [
    {
      walletId: "11111111-1111-4111-8111-111111111111",
      tenantId: "school-demo",
      studentProfileId: "student-amina",
      walletCode: "WALLET-AMINA",
      currencyCode: "SAR",
      availableBalanceMinor: 18250,
      pendingBalanceMinor: 0,
      heldBalanceMinor: 0,
      settledBalanceMinor: 18250,
      pendingRecoveryMinor: 0,
      walletStatus: "Active",
      restrictionReason: "",
    },
    {
      walletId: "22222222-2222-4222-8222-222222222222",
      tenantId: "school-demo",
      studentProfileId: "student-omar",
      walletCode: "WALLET-OMAR",
      currencyCode: "SAR",
      availableBalanceMinor: 6400,
      pendingBalanceMinor: 0,
      heldBalanceMinor: 2500,
      settledBalanceMinor: 8900,
      pendingRecoveryMinor: 0,
      walletStatus: "Restricted",
      restrictionReason: "Daily limit review",
    },
    {
      walletId: "33333333-3333-4333-8333-333333333333",
      tenantId: "school-demo",
      studentProfileId: "student-lina",
      walletCode: "WALLET-LINA",
      currencyCode: "SAR",
      availableBalanceMinor: 12100,
      pendingBalanceMinor: 0,
      heldBalanceMinor: 0,
      settledBalanceMinor: 12100,
      pendingRecoveryMinor: 0,
      walletStatus: "Active",
      restrictionReason: "",
    },
  ],
  topUps: [
    { topUpId: "44444444-4444-4444-8444-444444444444", walletId: "11111111-1111-4111-8111-111111111111", studentProfileId: "student-amina", source: "GuardianOnline", amountMinor: 5000, netCreditMinor: 5000, currencyCode: "SAR", status: "Confirmed", safeReference: "PAY-1442", reviewReason: "" },
    { topUpId: "55555555-5555-4555-8555-555555555555", walletId: "22222222-2222-4222-8222-222222222222", studentProfileId: "student-omar", source: "AuthorizedCashier", amountMinor: 2500, netCreditMinor: 2500, currencyCode: "SAR", status: "Confirmed", safeReference: "CASH-219", reviewReason: "" },
  ],
  purchases: [
    { purchaseId: "66666666-6666-4666-8666-666666666666", walletId: "11111111-1111-4111-8111-111111111111", merchantId: "77777777-7777-4777-8777-777777777777", posTerminalId: "88888888-8888-4888-8888-888888888888", clientPurchaseId: "POS-8841", amountMinor: 850, currencyCode: "SAR", decision: "Approved", decisionReason: "within limits", ruleSnapshotReference: "rule-active", reserveSnapshotReference: "" },
    { purchaseId: "99999999-9999-4999-8999-999999999999", walletId: "22222222-2222-4222-8222-222222222222", merchantId: "77777777-7777-4777-8777-777777777777", posTerminalId: "88888888-8888-4888-8888-888888888888", clientPurchaseId: "POS-8842", amountMinor: 1900, currencyCode: "SAR", decision: "Denied", decisionReason: "daily limit exceeded", ruleSnapshotReference: "limit-rule", reserveSnapshotReference: "" },
  ],
  limits: [
    { spendingLimitId: "aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa", walletId: "22222222-2222-4222-8222-222222222222", ownerType: "Guardian", limitType: "Daily", amountMinor: 2000, currencyCode: "SAR", merchantCode: "", itemCategoryCode: "snack", status: "Active", ruleVersion: "v1" },
  ],
  transactions: [
    { transactionId: "txn-1", walletId: "11111111-1111-4111-8111-111111111111", transactionType: "Credit", amountMinor: 5000, currencyCode: "SAR", status: "Posted", merchantOrSource: "PAY-1442", evidenceTime: new Date("2026-05-10T09:00:00Z").toISOString(), staffOnlyDetailSuppressed: false },
    { transactionId: "txn-2", walletId: "11111111-1111-4111-8111-111111111111", transactionType: "Debit", amountMinor: 850, currencyCode: "SAR", status: "Posted", merchantOrSource: "North Canteen", evidenceTime: new Date("2026-05-10T10:30:00Z").toISOString(), staffOnlyDetailSuppressed: false },
  ],
  reconciliationRuns: [
    { reconciliationRunId: "bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb", scope: "daily", dateFrom: "2026-05-10", dateUntil: "2026-05-10", status: "Matched", ledgerTotalMinor: 7500, sourceTotalMinor: 7500, differenceMinor: 0 },
  ],
};

export function walletApiBaseUrl() {
  return process.env.NEXT_PUBLIC_API_BASE_URL ?? "";
}

export function walletHeaders(schoolAccountId: string, actorReference = "finance-admin") {
  return {
    "content-type": "application/json",
    "x-school-account-id": schoolAccountId,
    "x-actor-reference": actorReference,
  };
}

async function fetchWalletJson<T>(path: string, schoolAccountId: string): Promise<T | null> {
  const baseUrl = walletApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: walletHeaders(schoolAccountId),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadSchoolWalletOperations(schoolAccountId = walletDemoData.schoolAccountId): Promise<WalletOperationsData> {
  const [wallets, topUps, purchases, limits, transactions, reconciliationRuns] = await Promise.all([
    fetchWalletJson<WalletResponse[]>(walletRoutes.wallets(schoolAccountId), schoolAccountId),
    fetchWalletJson<TopUpResponse[]>(walletRoutes.topUps(schoolAccountId), schoolAccountId),
    fetchWalletJson<PurchaseResponse[]>(walletRoutes.posPurchases(schoolAccountId), schoolAccountId),
    fetchWalletJson<SpendingLimitResponse[]>(walletRoutes.limits(schoolAccountId), schoolAccountId),
    fetchWalletJson<TransactionHistoryResponse[]>(walletRoutes.transactions(schoolAccountId), schoolAccountId),
    fetchWalletJson<ReconciliationRunResponse[]>(walletRoutes.reconciliation(schoolAccountId), schoolAccountId),
  ]);

  if (!wallets) return { ...walletDemoData, schoolAccountId, dataSource: "fallback" };

  return {
    schoolAccountId,
    dataSource: "api",
    wallets,
    topUps: topUps ?? [],
    purchases: purchases ?? [],
    limits: limits ?? [],
    transactions: transactions ?? [],
    reconciliationRuns: reconciliationRuns ?? [],
  };
}
