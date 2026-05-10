import {
  type SpendingLimitResponse,
  type TopUpResponse,
  type TransactionHistoryResponse,
  type WalletDataSource,
  type WalletResponse,
  walletDemoData,
  walletHeaders,
  walletApiBaseUrl,
} from "../../wallet/api/client";

export const guardianWalletRoutes = {
  wallet: (studentProfileId: string) => `/api/v1/guardians/me/students/${studentProfileId}/wallet`,
  topUps: (studentProfileId: string) => `/api/v1/guardians/me/students/${studentProfileId}/wallet/top-ups`,
  transactions: (studentProfileId: string) => `/api/v1/guardians/me/students/${studentProfileId}/wallet/transactions`,
  limits: (studentProfileId: string) => `/api/v1/guardians/me/students/${studentProfileId}/wallet/spending-limits`,
  reviewSummary: (studentProfileId: string) => `/api/v1/guardians/me/students/${studentProfileId}/wallet/review-summary`,
};

export const guardianWalletDemoData = [
  { studentProfileId: "student-amina", student: "Amina Hassan", balanceMinor: 18250, issueCount: 0 },
  { studentProfileId: "student-omar", student: "Omar Ali", balanceMinor: 6400, issueCount: 1 },
];

export type GuardianWalletData = {
  studentProfileId: string;
  dataSource: WalletDataSource;
  wallet: WalletResponse | null;
  topUps: TopUpResponse[];
  transactions: TransactionHistoryResponse[];
  limits: SpendingLimitResponse[];
};

async function fetchGuardianJson<T>(path: string, schoolAccountId: string): Promise<T | null> {
  const baseUrl = walletApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: walletHeaders(schoolAccountId, "guardian-demo"),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadGuardianWalletData(studentProfileId = "student-amina", schoolAccountId = walletDemoData.schoolAccountId): Promise<GuardianWalletData> {
  const [wallet, topUps, transactions, limits] = await Promise.all([
    fetchGuardianJson<WalletResponse>(guardianWalletRoutes.wallet(studentProfileId), schoolAccountId),
    fetchGuardianJson<TopUpResponse[]>(guardianWalletRoutes.topUps(studentProfileId), schoolAccountId),
    fetchGuardianJson<TransactionHistoryResponse[]>(guardianWalletRoutes.transactions(studentProfileId), schoolAccountId),
    fetchGuardianJson<SpendingLimitResponse[]>(guardianWalletRoutes.limits(studentProfileId), schoolAccountId),
  ]);

  if (!wallet) {
    return {
      studentProfileId,
      dataSource: "fallback",
      wallet: walletDemoData.wallets.find((item) => item.studentProfileId === studentProfileId) ?? walletDemoData.wallets[0] ?? null,
      topUps: walletDemoData.topUps.filter((item) => item.studentProfileId === studentProfileId),
      transactions: walletDemoData.transactions,
      limits: walletDemoData.limits,
    };
  }

  return {
    studentProfileId,
    dataSource: "api",
    wallet,
    topUps: topUps ?? [],
    transactions: transactions ?? [],
    limits: limits ?? [],
  };
}
