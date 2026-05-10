export const attendanceRoutes = {
  sessions: "/attendance/sessions",
  generate: (sessionId: string) => `/attendance/sessions/${sessionId}/generate`,
  records: "/attendance/records",
  correct: (recordId: string) => `/attendance/records/${recordId}/correct`,
  summary: (sessionId: string) => `/attendance/sessions/${sessionId}/summary`,
};

export type AttendanceRecordSummary = {
  attendanceRecordId: string;
  studentProfileId: string;
  status: "Present" | "Late" | "Absent" | "EarlyExit" | "NeedsReview" | "Excused";
};

