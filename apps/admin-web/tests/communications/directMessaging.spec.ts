import { describe, expect, it } from "vitest";
import { communicationsDemoData } from "../../src/features/communications/api/communicationsApi";
import { directMessagingApi } from "../../src/features/communications/api/directMessagingApi";

describe("direct messaging", () => {
  it("keeps direct-message evidence distinct from broadcasts", () => {
    expect(directMessagingApi.phase).toBe("009-011");
    expect(communicationsDemoData.rows).toContain("Direct message - sent");
    expect(communicationsDemoData.rows).toContain("Emergency broadcast - published");
  });
});
