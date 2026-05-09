export type WalletApiError = { code: string; message: string; field?: string };
export type WalletMoney = { amountMinor: number; currencyCode: string };

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

export const walletDemoData = {
  schoolAccountId: "demo-school",
  wallets: [
    { walletId: "wallet-amina", student: "Amina Hassan", balanceMinor: 18250, status: "Active" },
    { walletId: "wallet-omar", student: "Omar Ali", balanceMinor: 6400, status: "Restricted" },
    { walletId: "wallet-lina", student: "Lina Nasser", balanceMinor: 12100, status: "Active" },
  ],
  topUps: [
    { reference: "PAY-1442", amountMinor: 5000, status: "Credited", source: "Guardian online" },
    { reference: "CASH-219", amountMinor: 2500, status: "Credited", source: "Authorized cashier" },
  ],
  purchases: [
    { reference: "POS-8841", merchant: "North Canteen", amountMinor: 850, decision: "Approved" },
    { reference: "POS-8842", merchant: "North Canteen", amountMinor: 1900, decision: "Denied - limit" },
    { reference: "BATCH-18", merchant: "South Kiosk", amountMinor: 1200, decision: "Held - offline reserve" },
  ],
};
