export type LearningError = { code: string; message: string; field?: string };
export type LearningCapabilityStatus = { key: string; enabled: boolean; detail: string };
export type LearningSummary = { label: string; value: string; detail: string };

export const learningRoutes = {
  schoolRoot: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning`,
  content: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/content`,
  assignments: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/assignments`,
  quizzes: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/quizzes`,
  stars: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/stars`,
  rewards: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/rewards`,
  behavior: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/behavior-events`,
  history: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/history`,
  configuration: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/rule-settings`,
  guardian: (studentProfileId: string) => `/api/v1/guardians/me/students/${studentProfileId}/learning`,
  student: () => "/api/v1/students/me/learning",
};

export const learningCapabilities: LearningCapabilityStatus[] = [
  { key: "learning.content_delivery", enabled: true, detail: "Course content and progress" },
  { key: "learning.assignments", enabled: true, detail: "Submissions and review" },
  { key: "learning.quizzes", enabled: true, detail: "Attempts and scoring" },
  { key: "learning.stars_rewards", enabled: true, detail: "Append-only star evidence" },
  { key: "learning.behavior_logging", enabled: true, detail: "Privacy-filtered conduct evidence" },
];

export const learningDemoData = {
  schoolAccountId: "school-demo",
  studentProfileId: "student-amina",
  metrics: [
    { label: "Published content", value: "18", detail: "2 withdrawn but preserved" },
    { label: "Assignments", value: "12", detail: "4 need review" },
    { label: "Quiz attempts", value: "86", detail: "7 manual scoring" },
    { label: "Stars posted", value: "1,240", detail: "append-only ledger" },
    { label: "Rewards redeemed", value: "34", detail: "no wallet mutation" },
    { label: "Behavior records", value: "29", detail: "6 staff-only details hidden" },
  ] satisfies LearningSummary[],
  activities: [
    { label: "Science content", detail: "Solar System published to Grade 5A", status: "Visible" },
    { label: "Assignment", detail: "Planet worksheet submitted by Amina", status: "Submitted" },
    { label: "Quiz", detail: "Planet quiz scored with feedback after close", status: "Scored" },
    { label: "Stars", detail: "5 stars posted from quiz source event", status: "Phase 6 ready" },
    { label: "Reward", detail: "Library pass requested", status: "Reserved" },
    { label: "Behavior", detail: "Teamwork behavior visible as guardian summary", status: "Filtered" },
  ],
};

export function learningBoundaryNotes() {
  return [
    "No attendance, campus gate, NFC, QR, transport, wallet, payment, request, medical, complaint, document, search, broadcast, or message delivery outcomes are created.",
    "Stars are engagement evidence only; wallet balance is never mutated.",
    "Guardian and student views hide staff-only and sensitive behavior details.",
  ];
}
