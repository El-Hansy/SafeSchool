import { assertApiDataAvailable, serverApiAuthorizationHeader, serverApiBaseUrl } from "../../common/apiReadiness";

export const requestRoutes = {
  school: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/requests`,
  outing: (schoolAccountId: string) => `${requestRoutes.school(schoolAccountId)}/outing`,
  earlyLeave: (schoolAccountId: string) => `${requestRoutes.school(schoolAccountId)}/early-leave`,
  approvals: (schoolAccountId: string) => `${requestRoutes.school(schoolAccountId)}/approvals`,
  history: (schoolAccountId: string) => `${requestRoutes.school(schoolAccountId)}/history`,
  statusEvents: (schoolAccountId: string) => `${requestRoutes.school(schoolAccountId)}/status-events`,
  reviewSummaries: (schoolAccountId: string) => `${requestRoutes.school(schoolAccountId)}/review-summaries`,
  configuration: (schoolAccountId: string) => `${requestRoutes.school(schoolAccountId)}/configuration`,
  starRule: (schoolAccountId: string, ruleId: string) => `${requestRoutes.configuration(schoolAccountId)}/star-rules/${ruleId}`,
  detail: (schoolAccountId: string, requestId: string) => `${requestRoutes.school(schoolAccountId)}/${requestId}`,
  approve: (schoolAccountId: string, requestId: string) => `${requestRoutes.detail(schoolAccountId, requestId)}/approve`,
  reject: (schoolAccountId: string, requestId: string) => `${requestRoutes.detail(schoolAccountId, requestId)}/reject`,
  cancel: (schoolAccountId: string, requestId: string) => `${requestRoutes.detail(schoolAccountId, requestId)}/cancel`,
  trace: (schoolAccountId: string, requestId: string) => `${requestRoutes.detail(schoolAccountId, requestId)}/trace`,
  guardian: () => "/api/v1/guardians/me/requests",
  student: () => "/api/v1/students/me/requests",
};

export type RequestDataSource = "api" | "fallback";
export type RequestAudience = "school" | "guardian" | "student";

export type RequestBoardResponse = {
  schoolAccountId: string;
  phase: string;
  status: string;
  capabilities: string[];
  open: number;
  pendingApproval: number;
  approvedToday: number;
  statusEvents: number;
  reviewSummaries: number;
};

export type RequestResponse = {
  requestId: string;
  trackingReference: string;
  requestType: string;
  status: string;
  priority: string;
  studentProfileId: string;
  visibleSummary: string;
  startsAt?: string | null;
  endsAt?: string | null;
  auditTrail: string[];
};

export type RequestStatusEvent = {
  requestId: string;
  trackingReference: string;
  studentProfileId: string;
  requestType: string;
  status: string;
  sourceEventType: string;
  notificationEligible: boolean;
  reviewRequired: boolean;
  occurredAt: string;
  availableForNotificationsAt: string;
};

export type RequestReviewSummary = {
  requestId: string;
  trackingReference: string;
  studentProfileId: string;
  requestType: string;
  status: string;
  currentAssignee: string;
  exceptionState: string;
  starOutcome: string;
  lastEventType: string;
  updatedAt: string;
};

export type RequestOperationsData = {
  schoolAccountId: string;
  dataSource: RequestDataSource;
  board: RequestBoardResponse;
  schoolRequests: RequestResponse[];
  guardianRequests: RequestResponse[];
  studentRequests: RequestResponse[];
  outingRequests: RequestResponse[];
  earlyLeaveRequests: RequestResponse[];
  approvalQueue: RequestResponse[];
  history: RequestResponse[];
  statusEvents: RequestStatusEvent[];
  reviewSummaries: RequestReviewSummary[];
  configuration: Array<{ key: string; value: string; evidence: string }>;
  starRules: Array<{ ruleId: string; trigger: string; status: string; review: string; evidence: string[] }>;
};

export const requestCapabilities = [
  "requests.outing",
  "requests.early_leave",
  "requests.approval",
  "requests.star_rules",
  "requests.history",
  "requests.configuration",
];

export const requestsFallbackData: RequestOperationsData = {
  schoolAccountId: "school-demo",
  dataSource: "fallback",
  board: {
    schoolAccountId: "school-demo",
    phase: "requests-permissions",
    status: "ready",
    capabilities: requestCapabilities,
    open: 18,
    pendingApproval: 7,
    approvedToday: 4,
    statusEvents: 3,
    reviewSummaries: 2,
  },
  schoolRequests: [
    { requestId: "req-1", trackingReference: "REQ-2026-0001", requestType: "outing", status: "PendingApproval", priority: "Normal", studentProfileId: "student-amina", visibleSummary: "Guardian requested library outing permission.", startsAt: "2026-05-12T08:00:00Z", endsAt: "2026-05-12T10:00:00Z", auditTrail: ["submitted", "approval-routing"] },
    { requestId: "req-2", trackingReference: "REQ-2026-0002", requestType: "early-leave", status: "Approved", priority: "High", studentProfileId: "student-omar", visibleSummary: "Early leave approved with staff release pending.", startsAt: "2026-05-12T11:30:00Z", endsAt: "2026-05-12T12:00:00Z", auditTrail: ["submitted", "approved"] },
  ],
  guardianRequests: [
    { requestId: "req-1", trackingReference: "REQ-2026-0001", requestType: "outing", status: "PendingApproval", priority: "Normal", studentProfileId: "student-amina", visibleSummary: "Your outing request is waiting for approval.", startsAt: "2026-05-12T08:00:00Z", endsAt: "2026-05-12T10:00:00Z", auditTrail: ["submitted"] },
  ],
  studentRequests: [
    { requestId: "req-3", trackingReference: "REQ-2026-0003", requestType: "outing", status: "NeedsGuardian", priority: "Normal", studentProfileId: "student-lina", visibleSummary: "Student request is waiting for guardian approval.", startsAt: "2026-05-13T09:00:00Z", endsAt: "2026-05-13T10:00:00Z", auditTrail: ["submitted"] },
  ],
  outingRequests: [
    { requestId: "req-1", trackingReference: "REQ-2026-0001", requestType: "outing", status: "PendingApproval", priority: "Normal", studentProfileId: "student-amina", visibleSummary: "Library outing request.", startsAt: "2026-05-12T08:00:00Z", endsAt: "2026-05-12T10:00:00Z", auditTrail: ["submitted", "approval-routing"] },
  ],
  earlyLeaveRequests: [
    { requestId: "req-2", trackingReference: "REQ-2026-0002", requestType: "early-leave", status: "Approved", priority: "High", studentProfileId: "student-omar", visibleSummary: "Early leave approved with release control.", startsAt: "2026-05-12T11:30:00Z", endsAt: "2026-05-12T12:00:00Z", auditTrail: ["submitted", "approved"] },
  ],
  approvalQueue: [
    { requestId: "req-1", trackingReference: "REQ-2026-0001", requestType: "outing", status: "PendingApproval", priority: "Normal", studentProfileId: "student-amina", visibleSummary: "Approval required by grade supervisor.", startsAt: "2026-05-12T08:00:00Z", endsAt: "2026-05-12T10:00:00Z", auditTrail: ["submitted", "approval-routing"] },
  ],
  history: [],
  statusEvents: [
    { requestId: "req-1", trackingReference: "REQ-2026-0001", studentProfileId: "student-amina", requestType: "outing", status: "PendingApproval", sourceEventType: "submitted", notificationEligible: true, reviewRequired: false, occurredAt: "2026-05-11T08:00:00Z", availableForNotificationsAt: "2026-05-11T08:02:00Z" },
    { requestId: "req-2", trackingReference: "REQ-2026-0002", studentProfileId: "student-omar", requestType: "early-leave", status: "Approved", sourceEventType: "approved", notificationEligible: true, reviewRequired: false, occurredAt: "2026-05-11T08:15:00Z", availableForNotificationsAt: "2026-05-11T08:17:00Z" },
    { requestId: "req-4", trackingReference: "REQ-2026-0004", studentProfileId: "student-lina", requestType: "outing", status: "NeedsReview", sourceEventType: "overlap_manual_review", notificationEligible: true, reviewRequired: true, occurredAt: "2026-05-11T08:25:00Z", availableForNotificationsAt: "2026-05-11T08:27:00Z" },
  ],
  reviewSummaries: [
    { requestId: "req-1", trackingReference: "REQ-2026-0001", studentProfileId: "student-amina", requestType: "outing", status: "PendingApproval", currentAssignee: "workflow-reviewer", exceptionState: "none", starOutcome: "not-evaluated", lastEventType: "submitted", updatedAt: "2026-05-11T08:00:00Z" },
    { requestId: "req-4", trackingReference: "REQ-2026-0004", studentProfileId: "student-lina", requestType: "outing", status: "NeedsReview", currentAssignee: "workflow-reviewer", exceptionState: "overlap_manual_review", starOutcome: "not-evaluated", lastEventType: "overlap_manual_review", updatedAt: "2026-05-11T08:25:00Z" },
  ],
  configuration: [
    { key: "approvalLevels", value: "2", evidence: "tenant-scoped" },
    { key: "earlyLeaveRequiresStaffRelease", value: "true", evidence: "role-reviewed" },
  ],
  starRules: [
    { ruleId: "star-outing-basic", trigger: "Positive star balance and guardian consent", status: "Enabled", review: "Human approval required", evidence: ["rule-versioned", "source-read-only"] },
  ],
};

requestsFallbackData.history = [...requestsFallbackData.schoolRequests, ...requestsFallbackData.studentRequests];

export function requestsApiBaseUrl() {
  return serverApiBaseUrl();
}

export function requestHeaders(schoolAccountId: string, actorReference = "request-operator") {
  return {
    "content-type": "application/json",
    "x-school-account-id": schoolAccountId,
    "x-actor-reference": actorReference,
    ...serverApiAuthorizationHeader(),
  };
}

async function fetchRequestsJson<T>(path: string, schoolAccountId: string, actorReference?: string): Promise<T | null> {
  const baseUrl = requestsApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: requestHeaders(schoolAccountId, actorReference),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadRequestOperations(schoolAccountId = requestsFallbackData.schoolAccountId): Promise<RequestOperationsData> {
  const [board, guardianRequests, studentRequests, outingRequests, earlyLeaveRequests, approvalQueue, history, statusEvents, reviewSummaries] = await Promise.all([
    fetchRequestsJson<RequestBoardResponse>(requestRoutes.school(schoolAccountId), schoolAccountId),
    fetchRequestsJson<RequestResponse[]>(requestRoutes.guardian(), schoolAccountId, "guardian-demo"),
    fetchRequestsJson<RequestResponse[]>(requestRoutes.student(), schoolAccountId, "student-demo"),
    fetchRequestsJson<RequestResponse[]>(requestRoutes.outing(schoolAccountId), schoolAccountId),
    fetchRequestsJson<RequestResponse[]>(requestRoutes.earlyLeave(schoolAccountId), schoolAccountId),
    fetchRequestsJson<RequestResponse[]>(requestRoutes.approvals(schoolAccountId), schoolAccountId),
    fetchRequestsJson<RequestResponse[]>(requestRoutes.history(schoolAccountId), schoolAccountId),
    fetchRequestsJson<RequestStatusEvent[]>(requestRoutes.statusEvents(schoolAccountId), schoolAccountId),
    fetchRequestsJson<RequestReviewSummary[]>(requestRoutes.reviewSummaries(schoolAccountId), schoolAccountId),
  ]);

  const hasApiData = [board, guardianRequests, studentRequests, outingRequests, earlyLeaveRequests, approvalQueue, history, statusEvents, reviewSummaries].some((item) => item !== null);
  assertApiDataAvailable("Requests operations", [board, guardianRequests, studentRequests, outingRequests, earlyLeaveRequests, approvalQueue, history, statusEvents, reviewSummaries], requestsApiBaseUrl());
  if (!hasApiData) return { ...requestsFallbackData, schoolAccountId, dataSource: "fallback" };

  return {
    ...requestsFallbackData,
    schoolAccountId,
    dataSource: "api",
    board: board ?? { ...requestsFallbackData.board, schoolAccountId },
    guardianRequests: guardianRequests ?? [],
    studentRequests: studentRequests ?? [],
    outingRequests: outingRequests ?? [],
    earlyLeaveRequests: earlyLeaveRequests ?? [],
    approvalQueue: approvalQueue ?? [],
    history: history ?? [],
    statusEvents: statusEvents ?? [],
    reviewSummaries: reviewSummaries ?? [],
    schoolRequests: history ?? [],
  };
}
