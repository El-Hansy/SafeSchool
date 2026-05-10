import { describe, expect, it } from "vitest";
import { communicationsDemoData } from "../../src/features/communications/api/communicationsApi";
import { deliveryAcknowledgementApi } from "../../src/features/communications/api/deliveryAcknowledgementApi";

describe("delivery acknowledgement", () => {
  it("tracks delivery exceptions and acknowledgement phase ownership", () => {
    expect(deliveryAcknowledgementApi.phase).toBe("009-011");
    expect(communicationsDemoData.metrics.map((metric) => metric.label)).toContain("Delivery exceptions");
  });
});
