import { describe, expect, it } from "vitest";
import { attendanceRoutes } from "../../src/features/attendance-access/attendance/attendanceApi";

describe("attendance routes", () => {
  it("models generation and correction endpoints", () => {
    expect(attendanceRoutes.generate("session-1")).toBe("/attendance/sessions/session-1/generate");
    expect(attendanceRoutes.correct("record-1")).toBe("/attendance/records/record-1/correct");
  });
});

