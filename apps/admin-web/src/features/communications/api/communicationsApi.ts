import { assertApiDataAvailable } from "../../common/apiReadiness";

export const communicationsRoutes = {
  school: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/communications`,
  sourceEvents: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/source-events`,
  notificationDetail: (schoolAccountId: string, notificationId: string) => `${communicationsRoutes.school(schoolAccountId)}/notifications/${notificationId}`,
  acknowledgements: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/acknowledgements`,
  delivery: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/delivery`,
  summaries: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/summaries`,
  history: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/history`,
  moderation: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/moderation`,
  exceptions: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/exceptions`,
  configuration: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/configuration`,
  audienceRule: (schoolAccountId: string, ruleId: string) => `${communicationsRoutes.configuration(schoolAccountId)}/audience-rules/${ruleId}`,
  template: (schoolAccountId: string, templateId: string) => `${communicationsRoutes.configuration(schoolAccountId)}/templates/${templateId}`,
  conversations: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/conversations`,
  conversationDetail: (schoolAccountId: string, conversationId: string) => `${communicationsRoutes.conversations(schoolAccountId)}/${conversationId}`,
  broadcasts: (schoolAccountId: string) => `${communicationsRoutes.school(schoolAccountId)}/broadcasts`,
  broadcastDetail: (schoolAccountId: string, broadcastId: string) => `${communicationsRoutes.broadcasts(schoolAccountId)}/${broadcastId}`,
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
  acknowledgements: CommunicationResponse[];
  delivery: CommunicationResponse[];
  summaries: CommunicationResponse[];
  history: CommunicationResponse[];
  conversations: CommunicationResponse[];
  broadcasts: CommunicationResponse[];
  moderation: CommunicationResponse[];
  exceptions: CommunicationResponse[];
  audienceRules: Array<{ ruleId: string; scope: string; channel: string; status: string; evidence: string[] }>;
  templates: Array<{ templateId: string; channel: string; locale: string; status: string; evidence: string[] }>;
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
  acknowledgements: [
    { reference: "ack-guardian-1", status: "Acknowledged", recipients: ["guardian-amina"], evidence: ["read-state-recorded", "audit-written"] },
  ],
  delivery: [
    { reference: "delivery-guardian-1", status: "Delivered", recipients: ["guardian-amina"], evidence: ["attempt-recorded", "provider-reference-minimized"] },
    { reference: "delivery-guardian-2", status: "RetryScheduled", recipients: ["guardian-omar"], evidence: ["delivery-failed", "owner-assigned"] },
  ],
  summaries: [
    { reference: "summary-daily-1", status: "Ready", recipients: ["school-admin"], evidence: ["counts-minimized", "suppression-included"] },
  ],
  history: [
    { reference: "history-message-1", status: "Sent", recipients: ["guardian-amina"], evidence: ["lifecycle-trace", "audit-written"] },
  ],
  conversations: [
    { reference: "conversation-1", status: "Sent", recipients: ["guardian-amina"], evidence: ["moderation-checked", "delivery-attempt-recorded"] },
  ],
  broadcasts: [
    { reference: "broadcast-1", status: "Published", recipients: ["grade-4"], evidence: ["audience-snapshot", "quiet-hours-applied"] },
  ],
  moderation: [
    { reference: "moderation-1", status: "Approved", recipients: ["guardian-amina"], evidence: ["policy-checked", "reviewer-audit"] },
  ],
  exceptions: [
    { reference: "communication-exception-1", status: "RetryScheduled", recipients: ["support"], evidence: ["delivery-failed", "owner-assigned"] },
  ],
  audienceRules: [
    { ruleId: "grade-4-guardians", scope: "grade:4", channel: "guardian", status: "Enabled", evidence: ["audience-snapshot", "visibility-reviewed"] },
  ],
  templates: [
    { templateId: "pickup-update", channel: "push", locale: "en/ar", status: "Approved", evidence: ["template-versioned", "moderation-ready"] },
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
  const [board, guardianNotifications, studentNotifications, acknowledgements, delivery, summaries, history, conversations, moderation, exceptions] = await Promise.all([
    fetchCommunicationsJson<CommunicationBoardResponse>(communicationsRoutes.school(schoolAccountId), schoolAccountId),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.guardianNotifications(), schoolAccountId, "guardian-demo"),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.studentNotifications(), schoolAccountId, "student-demo"),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.acknowledgements(schoolAccountId), schoolAccountId),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.delivery(schoolAccountId), schoolAccountId),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.summaries(schoolAccountId), schoolAccountId),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.history(schoolAccountId), schoolAccountId),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.conversations(schoolAccountId), schoolAccountId),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.moderation(schoolAccountId), schoolAccountId),
    fetchCommunicationsJson<CommunicationResponse[]>(communicationsRoutes.exceptions(schoolAccountId), schoolAccountId),
  ]);

  const hasApiData = [board, guardianNotifications, studentNotifications, acknowledgements, delivery, summaries, history, conversations, moderation, exceptions].some((item) => item !== null);
  assertApiDataAvailable("Communications operations", [board, guardianNotifications, studentNotifications, acknowledgements, delivery, summaries, history, conversations, moderation, exceptions], communicationsApiBaseUrl());
  if (!hasApiData) return { ...communicationsFallbackData, schoolAccountId, dataSource: "fallback" };

  return {
    ...communicationsFallbackData,
    schoolAccountId,
    dataSource: "api",
    board: board ?? { ...communicationsFallbackData.board, schoolAccountId },
    guardianNotifications: guardianNotifications ?? communicationsFallbackData.guardianNotifications,
    studentNotifications: studentNotifications ?? communicationsFallbackData.studentNotifications,
    acknowledgements: acknowledgements ?? communicationsFallbackData.acknowledgements,
    delivery: delivery ?? communicationsFallbackData.delivery,
    summaries: summaries ?? communicationsFallbackData.summaries,
    history: history ?? communicationsFallbackData.history,
    conversations: conversations ?? communicationsFallbackData.conversations,
    moderation: moderation ?? communicationsFallbackData.moderation,
    exceptions: exceptions ?? communicationsFallbackData.exceptions,
  };
}
