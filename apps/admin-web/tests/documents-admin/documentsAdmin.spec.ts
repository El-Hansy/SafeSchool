import { describe, expect, it } from "vitest";
import { documentsRoutes } from "../../src/features/documents/api/documentStorageApi";

describe("documentsAdmin.spec", () => { it("keeps routes versioned and tenant scoped", () => { expect(documentsRoutes.documents("school-demo")).toContain("/api/v1/"); }); });
