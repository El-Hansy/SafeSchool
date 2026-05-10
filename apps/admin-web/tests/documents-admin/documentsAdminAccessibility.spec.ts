import { describe, expect, it } from "vitest";
import { adminDemoData } from "../../src/features/administration/api/adminApi";

describe("documents admin accessibility", () => {
  it("keeps admin summary metrics labelled for review surfaces", () => {
    expect(adminDemoData.metrics.every((metric) => metric.label && metric.value)).toBe(true);
    expect(adminDemoData.rows).toContain("Audit export prepared");
  });
});
