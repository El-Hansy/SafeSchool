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
