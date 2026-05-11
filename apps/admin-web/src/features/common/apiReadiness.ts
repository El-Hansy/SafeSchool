export function apiDataRequired() {
  const value = process.env.NEXT_PUBLIC_REQUIRE_API_DATA ?? "";
  return value === "1" || value.toLowerCase() === "true";
}

export function serverApiBaseUrl() {
  return process.env.SAFE_SCHOOL_API_BASE_URL ?? process.env.NEXT_PUBLIC_API_BASE_URL ?? "";
}

export function serverApiAuthorizationHeader(): Record<string, string> {
  if (typeof window !== "undefined") return {};

  const token = process.env.SAFE_SCHOOL_API_BEARER_TOKEN ?? "";
  if (!token.trim()) return {};

  return {
    authorization: token.startsWith("Bearer ") ? token : `Bearer ${token}`,
  };
}

export function assertApiDataAvailable(feature: string, apiResults: readonly unknown[], baseUrl = serverApiBaseUrl()) {
  if (!apiDataRequired()) return;

  if (!baseUrl.trim()) {
    throw new Error(`${feature} requires SAFE_SCHOOL_API_BASE_URL or NEXT_PUBLIC_API_BASE_URL when NEXT_PUBLIC_REQUIRE_API_DATA=1.`);
  }

  if (apiResults.some((item) => item === null || item === undefined)) {
    throw new Error(`${feature} requires complete API data when NEXT_PUBLIC_REQUIRE_API_DATA=1.`);
  }
}
