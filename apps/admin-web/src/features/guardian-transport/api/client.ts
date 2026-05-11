import {
  transportApiBaseUrl,
  transportDemoData,
  transportHeaders,
  type AssignmentResponse,
  type EtaRecordResponse,
  type TransportDataSource,
  type TransportNotificationRecordResponse,
} from "../../transport/api/client";
import { assertApiDataAvailable } from "../../common/apiReadiness";

export type GuardianTransportPlanResponse = {
  studentProfileId: string;
  assignments: AssignmentResponse[];
};

export type GuardianTripProgressResponse = {
  studentProfileId: string;
  transportTripId: string;
  visibilityPhase: string;
  pickupEta?: string | null;
  exactLiveLocation?: string | null;
  dropStatus?: string | null;
};

export type GuardianEtaResponse = {
  studentProfileId: string;
  transportTripId: string;
  routeStopSequenceId: string;
  visibilityPhase: string;
  pickupEta?: EtaRecordResponse | null;
  exactLiveLocationAvailable: boolean;
};

export type GuardianTransportNotificationListResponse = {
  studentProfileId: string;
  notifications: TransportNotificationRecordResponse[];
};

export type GuardianTransportData = {
  schoolAccountId: string;
  studentProfileId: string;
  tripId: string;
  dataSource: TransportDataSource;
  plan: GuardianTransportPlanResponse;
  progress: GuardianTripProgressResponse;
  eta: GuardianEtaResponse;
  notifications: GuardianTransportNotificationListResponse;
};

export const guardianTransportApi = {
  base: (studentProfileId: string) => `/api/v1/guardians/me/students/${studentProfileId}/transport`,
  plan: (studentProfileId: string) => `${guardianTransportApi.base(studentProfileId)}/plan`,
  progress: (studentProfileId: string, tripId: string) => `${guardianTransportApi.base(studentProfileId)}/trips/${tripId}/progress`,
  eta: (studentProfileId: string, tripId: string) => `${guardianTransportApi.base(studentProfileId)}/trips/${tripId}/eta`,
  notifications: (studentProfileId: string) => `${guardianTransportApi.base(studentProfileId)}/notifications`
};

export const guardianTransportRoutes = guardianTransportApi;

const fallbackStudentId = "student-amina";
const fallbackTripId = transportDemoData.trips[0]?.transportTripId ?? "";

export const guardianTransportDemoData: GuardianTransportData = {
  schoolAccountId: transportDemoData.schoolAccountId,
  studentProfileId: fallbackStudentId,
  tripId: fallbackTripId,
  dataSource: "fallback",
  plan: {
    studentProfileId: fallbackStudentId,
    assignments: transportDemoData.assignments.filter((assignment) => assignment.studentProfileId === fallbackStudentId),
  },
  progress: {
    studentProfileId: fallbackStudentId,
    transportTripId: fallbackTripId,
    visibilityPhase: "Onboard",
    pickupEta: "2026-05-10T06:58:00Z",
    exactLiveLocation: "Bus 12 approaching Library Corner",
    dropStatus: "Not dropped yet",
  },
  eta: {
    studentProfileId: fallbackStudentId,
    transportTripId: fallbackTripId,
    routeStopSequenceId: transportDemoData.routeStopSequences[0]?.routeStopSequenceId ?? "",
    visibilityPhase: "Pickup",
    pickupEta: transportDemoData.etaRecords[0] ?? null,
    exactLiveLocationAvailable: true,
  },
  notifications: {
    studentProfileId: fallbackStudentId,
    notifications: transportDemoData.notifications.filter((notification) => notification.studentProfileId === fallbackStudentId),
  },
};

async function fetchGuardianTransportJson<T>(path: string, schoolAccountId: string): Promise<T | null> {
  const baseUrl = transportApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: transportHeaders(schoolAccountId, "guardian-demo"),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadGuardianTransportData(
  studentProfileId = guardianTransportDemoData.studentProfileId,
  tripId = guardianTransportDemoData.tripId,
  schoolAccountId = guardianTransportDemoData.schoolAccountId,
): Promise<GuardianTransportData> {
  const [plan, progress, eta, notifications] = await Promise.all([
    fetchGuardianTransportJson<GuardianTransportPlanResponse>(guardianTransportApi.plan(studentProfileId), schoolAccountId),
    fetchGuardianTransportJson<GuardianTripProgressResponse>(guardianTransportApi.progress(studentProfileId, tripId), schoolAccountId),
    fetchGuardianTransportJson<GuardianEtaResponse>(guardianTransportApi.eta(studentProfileId, tripId), schoolAccountId),
    fetchGuardianTransportJson<GuardianTransportNotificationListResponse>(guardianTransportApi.notifications(studentProfileId), schoolAccountId),
  ]);

  const hasApiData = [plan, progress, eta, notifications].some((item) => item !== null);
  assertApiDataAvailable("Guardian transport", [plan, progress, eta, notifications], transportApiBaseUrl());
  if (!hasApiData) {
    return { ...guardianTransportDemoData, schoolAccountId, studentProfileId, tripId, dataSource: "fallback" };
  }

  return {
    schoolAccountId,
    studentProfileId,
    tripId,
    dataSource: "api",
    plan: plan ?? { ...guardianTransportDemoData.plan, studentProfileId },
    progress: progress ?? { ...guardianTransportDemoData.progress, studentProfileId, transportTripId: tripId },
    eta: eta ?? { ...guardianTransportDemoData.eta, studentProfileId, transportTripId: tripId },
    notifications: notifications ?? { ...guardianTransportDemoData.notifications, studentProfileId },
  };
}
