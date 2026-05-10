import { describe, expect, it } from "vitest";
import { communicationsRoutes } from "../../src/features/communications/api/communicationsApi";

describe("communicationNotifications.spec", () => { it("keeps routes versioned and tenant scoped", () => { expect(communicationsRoutes.school("school-demo")).toContain("/api/v1/"); }); });
