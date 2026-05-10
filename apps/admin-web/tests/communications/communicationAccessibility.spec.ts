import { describe, expect, it } from "vitest";
import { communicationsDemoData } from "../../src/features/communications/api/communicationsApi";

describe("communication accessibility", () => {
  it("provides labelled metrics and review rows for assistive summaries", () => {
    expect(communicationsDemoData.metrics.every((metric) => metric.label && metric.value)).toBe(true);
    expect(communicationsDemoData.rows.join(" ")).toContain("notification created");
  });
});
