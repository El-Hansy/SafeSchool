import { describe, expect, it } from "vitest";
import { readFileSync } from "node:fs";

describe("admin web runtime rendering", () => {
  it("forces app routes to evaluate production API guards at runtime", () => {
    const layout = readFileSync("src/app/layout.tsx", "utf8");

    expect(layout).toContain('export const dynamic = "force-dynamic"');
  });

  it("proxies browser write actions without exposing server API tokens", () => {
    const proxyRoute = readFileSync("src/app/api/safeschool/proxy/route.ts", "utf8");
    const dockerfile = readFileSync("Dockerfile", "utf8");

    expect(proxyRoute).toContain("serverApiAuthorizationHeader");
    expect(proxyRoute).not.toContain("NEXT_PUBLIC_API_BEARER_TOKEN");
    expect(dockerfile).not.toContain("SAFE_SCHOOL_API_BEARER_TOKEN");
  });
});
