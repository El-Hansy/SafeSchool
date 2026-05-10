export const communicationsRoutes = {
  school: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/communications`,
  sourceEvents: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/source-events`,
  conversations: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/conversations`,
  broadcasts: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/broadcasts`,
  trace: (schoolAccountId: string, reference: string) => `${communicationsRoutes.school(schoolAccountId)}/trace/${reference}`,
  guardianNotifications: () => "/api/v1/guardians/me/communications/notifications",
  studentNotifications: () => "/api/v1/students/me/communications/notifications",
};

export const communicationsDemoData = {
  schoolAccountId: "school-demo",
  metrics: [{ label: "Unread", value: "42" }, { label: "Broadcasts", value: "6" }, { label: "Delivery exceptions", value: "4" }],
  rows: ["Attendance source event - notification created", "Direct message - sent", "Emergency broadcast - published"],
};

export type CommunicationDataSource = "api" | "fallback";
export type CommunicationAudience = "school" | "guardian" | "student";

export type CommunicationBoardResponse = {
  schoolAccountId: string;
  phase: string;
  status: string;
  unread: number;
  broadcasts: number;
  deliveryExceptions: number;
  capabilities: string[];
};

export type CommunicationResponse = {
  reference: string;
  status: string;
  recipients: string[];
  evidence: string[];
};

export type CommunicationsOperationsData = {
  schoolAccountId: string;
  dataSource: CommunicationDataSource;
  board: CommunicationBoardResponse;
  schoolEvents: CommunicationResponse[];
  guardianNotifications: CommunicationResponse[];
  studentNotifications: CommunicationResponse[];
};

export const communicationsFallbackData: CommunicationsOperationsData = {
  schoolAccountId: communicationsDemoData.schoolAccountId,
  dataSource: "fallback",
  board: {
    schoolAccountId: communicationsDemoData.schoolAccountId,
    phase: "communication-notifications",
    status: "ready",
    unread: 42,
    broadcasts: 6,
    deliveryExceptions: 4,
    capabilities: ["communications.notification_center", "communications.direct_messaging", "communications.broadcasts", "communications.delivery_acknowledgement", "communications.configuration", "communications.history"],
  },
  schoolEvents: [
    { reference: "event-attendance-1", status: "Accepted", recipients: ["guardian-amina"], evidence: ["source-read-only", "notification-created"] },
    { reference: "conversation-1", status: "Sent", recipients: ["guardian-amina"], evidence: ["moderation-checked", "delivery-attempt-recorded"] },
    { reference: "broadcast-1", status: "Published", recipients: ["grade-4"], evidence: ["audience-snapshot", "quiet-hours-applied"] },
  ],
  guardianNotifications: [
    { reference: "guardian-notification-1", status: "Unread", recipients: ["guardian"], evidence: ["recipient-snapshot", "read-state-pending"] },
  ],
  studentNotifications: [
    { reference: "student-notification-1", status: "Unread", recipients: ["student"], evidence: ["recipient-snapshot", "read-state-pending"] },
  ],
};

export function communicationsApiBaseUrl() {
  return process.env.NEXT_PUBLIC_API_BASE_URL ?? "";
}

export function communicationsHeaders(schoolAccountId: string, actorReference = "communications-operator") {
  return {
    "content-type": "application/json",
    "x-school-account-id": schoolAccountId,
    "x-actor-reference": actorReference,
  };
}

async function fetchCommunicationsJson<T>(path: string, schoolAccountId: string, actorReference?: string): Promise<T | null> {
  const baseUrl = communicationsApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: communicationsHeaders(schoolAccountId, actorReference),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadCommunicationsOperations(schoolAccountId = communicationsFallbackData.schoolAccountId): Promise<CommunicationsOperationsData> {
  const [board, guardianNotifications, studentNotifications] = await Promise.all([
    fetchCommunicationsJson<CommunicationBoardResponse>(communicationsRoutes.school(schoolAccountId), schoolAccountId),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.guardianNotifications(), schoolAccountId, "guardian-demo"),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.studentNotifications(), schoolAccountId, "student-demo"),
  ]);

  const hasApiData = [board, guardianNotifications, studentNotifications].some((item) => item !== null);
  if (!hasApiData) return { ...communicationsFallbackData, schoolAccountId, dataSource: "fallback" };

  return {
    ...communicationsFallbackData,
    schoolAccountId,
    dataSource: "api",
    board: board ?? { ...communicationsFallbackData.board, schoolAccountId },
    guardianNotifications: guardianNotifications ?? communicationsFallbackData.guardianNotifications,
    studentNotifications: studentNotifications ?? communicationsFallbackData.studentNotifications,
  };
}
