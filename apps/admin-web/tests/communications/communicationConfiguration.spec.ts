import { describe, expect, it } from "vitest";
import { communicationConfigurationApi } from "../../src/features/communications/api/communicationConfigurationApi";
import { communicationsRoutes } from "../../src/features/communications/api/communicationsApi";

describe("communication configuration", () => {
  it("keeps configuration in the phase communications boundary", () => {
    expect(communicationConfigurationApi.phase).toBe("009-011");
    expect(communicationsRoutes.school("school-demo")).toContain("/api/v1/schools/school-demo/");
  });
});
