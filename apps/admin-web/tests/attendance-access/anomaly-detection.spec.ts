import { describe, expect, it } from "vitest";
import { anomalyRoutes } from "../../src/features/attendance-access/anomalies/anomaliesApi";

describe("anomaly routes", () => {
  it("models detection and review endpoints", () => {
    expect(anomalyRoutes.runs).toBe("/anomaly-runs");
    expect(anomalyRoutes.resolve("anomaly-1")).toBe("/anomalies/anomaly-1/resolve");
  });
});

