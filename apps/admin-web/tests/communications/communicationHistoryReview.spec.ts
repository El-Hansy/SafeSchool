import { describe, expect, it } from "vitest";
import { communicationHistoryReviewApi } from "../../src/features/communications/api/communicationHistoryReviewApi";
import { communicationsDemoData } from "../../src/features/communications/api/communicationsApi";

describe("communication history review", () => {
  it("covers source events, direct messages, and broadcasts", () => {
    expect(communicationHistoryReviewApi.phase).toBe("009-011");
    expect(communicationsDemoData.rows).toEqual(
      expect.arrayContaining([
        "Attendance source event - notification created",
        "Direct message - sent",
        "Emergency broadcast - published",
      ]),
    );
  });
});
