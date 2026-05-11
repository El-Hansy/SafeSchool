export function apiDataRequired() {
  const value = process.env.NEXT_PUBLIC_REQUIRE_API_DATA ?? "";
  return value === "1" || value.toLowerCase() === "true";
}

export function assertApiDataAvailable(feature: string, apiResults: readonly unknown[], baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL ?? "") {
  if (!apiDataRequired()) return;

  if (!baseUrl.trim()) {
    throw new Error(`${feature} requires NEXT_PUBLIC_API_BASE_URL when NEXT_PUBLIC_REQUIRE_API_DATA=1.`);
  }

  if (apiResults.some((item) => item === null || item === undefined)) {
    throw new Error(`${feature} requires complete API data when NEXT_PUBLIC_REQUIRE_API_DATA=1.`);
  }
}
