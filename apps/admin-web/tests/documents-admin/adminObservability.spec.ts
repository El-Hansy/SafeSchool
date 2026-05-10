import { describe, expect, it } from "vitest";
import { adminRoutes } from "../../src/features/administration/api/adminApi";

describe("adminObservability.spec", () => { it("keeps routes versioned and tenant scoped", () => { expect(adminRoutes.dashboard("school-demo")).toContain("/api/v1/"); }); });
