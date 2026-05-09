export type TransportApiError = { code: string; message: string; field?: string };
export type Page<T> = { items: T[]; page: number; pageSize: number; totalCount: number };

export const transportApi = {
  base: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/transport`,
  rules: "/rule-settings/current",
  reviewSummaries: "/review-summaries",
  featureDisabled: (capability: string): TransportApiError => ({ code: "feature_disabled", message: `${capability} is disabled` }),
  guardianScope: (): TransportApiError => ({ code: "guardian_scope_denied", message: "Guardian scope does not include this student" })
};
