import { describe, expect, it } from "vitest";
import { mobileSupportEvents } from "../../src/features/mobile";

describe("mobile support", () => {
  it("captures support evidence for allowed and blocked outcomes", () => {
    expect(mobileSupportEvents.some((event) => event.result === "blocked")).toBe(true);
    expect(mobileSupportEvents.some((event) => event.version === "12.0.0")).toBe(true);
  });
});
