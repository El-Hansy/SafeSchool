import { assertApiDataAvailable } from "../../common/apiReadiness";

export type LearningError = { code: string; message: string; field?: string };
export type LearningCapabilityStatus = { key: string; enabled: boolean; detail: string };
export type LearningSummary = { label: string; value: string; detail: string };

export const learningRoutes = {
  schoolRoot: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning`,
  content: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/content`,
  assignments: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/assignments`,
  submissions: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/submissions`,
  quizzes: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/quizzes`,
  quizAttempts: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/quiz-attempts`,
  stars: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/stars`,
  starSourceEvents: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/stars/source-events`,
  rewards: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/rewards`,
  rewardRedemptions: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/reward-redemptions`,
  behavior: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/behavior-events`,
  history: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/history`,
  manualReviews: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/manual-reviews`,
  exceptions: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/exceptions`,
  reviewSummaries: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/review-summaries`,
  featureSettings: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/learning/feature-settings`,
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

export type LearningDataSource = "api" | "fallback";
export type LearningAudience = "school" | "student" | "guardian";
export type LearningActivity = (typeof learningDemoData.activities)[number];

export type LearningEndpointResponse = {
  schoolAccountId?: string;
  scope?: string;
  studentProfileId?: string;
  area?: string;
  phase?: string;
  status: string;
  capabilities?: string[];
};

export type LearningOperationsData = {
  schoolAccountId: string;
  studentProfileId: string;
  dataSource: LearningDataSource;
  root: LearningEndpointResponse;
  endpoints: Record<string, LearningEndpointResponse>;
  metrics: LearningSummary[];
  schoolActivities: LearningActivity[];
  studentActivities: LearningActivity[];
  guardianActivities: LearningActivity[];
  capabilities: LearningCapabilityStatus[];
  boundaryNotes: string[];
};

export const learningFallbackData: LearningOperationsData = {
  schoolAccountId: learningDemoData.schoolAccountId,
  studentProfileId: learningDemoData.studentProfileId,
  dataSource: "fallback",
  root: {
    schoolAccountId: learningDemoData.schoolAccountId,
    phase: "learning-engagement",
    status: "ready",
    capabilities: learningCapabilities.map((capability) => capability.key),
  },
  endpoints: {
    content: { area: "content", status: "ready" },
    assignments: { area: "assignments", status: "ready" },
    submissions: { area: "submissions", status: "ready" },
    quizzes: { area: "quizzes", status: "ready" },
    quizAttempts: { area: "quiz-attempts", status: "ready" },
    stars: { area: "stars", status: "ready" },
    rewards: { area: "rewards", status: "ready" },
    behavior: { area: "behavior-events", status: "ready" },
    history: { area: "history", status: "ready" },
    review: { area: "manual-reviews", status: "ready" },
    configuration: { area: "rule-settings", status: "ready" },
    guardian: { scope: "guardian", studentProfileId: learningDemoData.studentProfileId, status: "ready" },
    student: { scope: "student", status: "ready" },
  },
  metrics: learningDemoData.metrics,
  schoolActivities: learningDemoData.activities,
  studentActivities: learningDemoData.activities.filter((activity) => ["Science content", "Assignment", "Quiz", "Reward"].includes(activity.label)),
  guardianActivities: learningDemoData.activities
    .filter((activity) => activity.label !== "Behavior" || !activity.detail.includes("staff-only"))
    .map((activity) => ({ ...activity, detail: activity.detail.replace("staff-only", "restricted") })),
  capabilities: learningCapabilities,
  boundaryNotes: learningBoundaryNotes(),
};

export function learningApiBaseUrl() {
  return process.env.NEXT_PUBLIC_API_BASE_URL ?? "";
}

export function learningHeaders(schoolAccountId: string, actorReference = "learning-admin") {
  return {
    "content-type": "application/json",
    "x-school-account-id": schoolAccountId,
    "x-actor-reference": actorReference,
  };
}

async function fetchLearningJson<T>(path: string, schoolAccountId: string): Promise<T | null> {
  const baseUrl = learningApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: learningHeaders(schoolAccountId),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadLearningOperations(
  schoolAccountId = learningFallbackData.schoolAccountId,
  studentProfileId = learningFallbackData.studentProfileId,
): Promise<LearningOperationsData> {
  const endpointLoaders = {
    root: fetchLearningJson<LearningEndpointResponse>(learningRoutes.schoolRoot(schoolAccountId), schoolAccountId),
    content: fetchLearningJson<LearningEndpointResponse>(learningRoutes.content(schoolAccountId), schoolAccountId),
    assignments: fetchLearningJson<LearningEndpointResponse>(learningRoutes.assignments(schoolAccountId), schoolAccountId),
    submissions: fetchLearningJson<LearningEndpointResponse>(learningRoutes.submissions(schoolAccountId), schoolAccountId),
    quizzes: fetchLearningJson<LearningEndpointResponse>(learningRoutes.quizzes(schoolAccountId), schoolAccountId),
    quizAttempts: fetchLearningJson<LearningEndpointResponse>(learningRoutes.quizAttempts(schoolAccountId), schoolAccountId),
    stars: fetchLearningJson<LearningEndpointResponse>(learningRoutes.stars(schoolAccountId), schoolAccountId),
    rewards: fetchLearningJson<LearningEndpointResponse>(learningRoutes.rewards(schoolAccountId), schoolAccountId),
    behavior: fetchLearningJson<LearningEndpointResponse>(learningRoutes.behavior(schoolAccountId), schoolAccountId),
    history: fetchLearningJson<LearningEndpointResponse>(learningRoutes.history(schoolAccountId), schoolAccountId),
    review: fetchLearningJson<LearningEndpointResponse>(learningRoutes.manualReviews(schoolAccountId), schoolAccountId),
    configuration: fetchLearningJson<LearningEndpointResponse>(learningRoutes.configuration(schoolAccountId), schoolAccountId),
    guardian: fetchLearningJson<LearningEndpointResponse>(learningRoutes.guardian(studentProfileId), schoolAccountId),
    student: fetchLearningJson<LearningEndpointResponse>(learningRoutes.student(), schoolAccountId),
  };

  const entries = await Promise.all(Object.entries(endpointLoaders).map(async ([key, loader]) => [key, await loader] as const));
  const loaded = Object.fromEntries(entries) as Record<keyof typeof endpointLoaders, LearningEndpointResponse | null>;
  assertApiDataAvailable("Learning operations", Object.values(loaded), learningApiBaseUrl());
  const hasApiData = Object.values(loaded).some((item) => item !== null);
  if (!hasApiData) return { ...learningFallbackData, schoolAccountId, studentProfileId, dataSource: "fallback" };

  const fallbackEndpoints = learningFallbackData.endpoints;
  return {
    ...learningFallbackData,
    schoolAccountId,
    studentProfileId,
    dataSource: "api",
    root: loaded.root ?? { ...learningFallbackData.root, schoolAccountId },
    endpoints: {
      content: loaded.content ?? fallbackEndpoints.content,
      assignments: loaded.assignments ?? fallbackEndpoints.assignments,
      submissions: loaded.submissions ?? fallbackEndpoints.submissions,
      quizzes: loaded.quizzes ?? fallbackEndpoints.quizzes,
      quizAttempts: loaded.quizAttempts ?? fallbackEndpoints.quizAttempts,
      stars: loaded.stars ?? fallbackEndpoints.stars,
      rewards: loaded.rewards ?? fallbackEndpoints.rewards,
      behavior: loaded.behavior ?? fallbackEndpoints.behavior,
      history: loaded.history ?? fallbackEndpoints.history,
      review: loaded.review ?? fallbackEndpoints.review,
      configuration: loaded.configuration ?? fallbackEndpoints.configuration,
      guardian: loaded.guardian ?? { ...fallbackEndpoints.guardian, studentProfileId },
      student: loaded.student ?? fallbackEndpoints.student,
    },
  };
}
