import { describe, expect, it } from "vitest";
import { complaintsRoutes } from "../../src/features/complaints/api/complaintsApi";
import { complaintTestFixtures } from "./complaintTestFixtures";
describe("complaintSubmission.spec", () => { it("keeps routes versioned and tenant scoped", () => { expect(complaintsRoutes.school(complaintTestFixtures.schoolAccountId)).toContain("/api/v1/"); }); });
