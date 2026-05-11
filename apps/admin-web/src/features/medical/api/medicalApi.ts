import { assertApiDataAvailable, serverApiAuthorizationHeader, serverApiBaseUrl } from "../../common/apiReadiness";

export const medicalRoutes = {
  school: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/medical`,
  records: (schoolAccountId: string) => `${medicalRoutes.school(schoolAccountId)}/records`,
  studentRecords: (schoolAccountId: string, studentProfileId: string) => `${medicalRoutes.records(schoolAccountId)}/${studentProfileId}`,
  emergency: (schoolAccountId: string) => `${medicalRoutes.school(schoolAccountId)}/emergency`,
  emergencyAccess: (schoolAccountId: string) => `${medicalRoutes.emergency(schoolAccountId)}/access`,
  breakGlass: (schoolAccountId: string) => `${medicalRoutes.emergency(schoolAccountId)}/break-glass`,
  incidents: (schoolAccountId: string) => `${medicalRoutes.school(schoolAccountId)}/incidents`,
  notifications: (schoolAccountId: string) => `${medicalRoutes.school(schoolAccountId)}/notifications`,
  history: (schoolAccountId: string) => `${medicalRoutes.school(schoolAccountId)}/history`,
  statusEvents: (schoolAccountId: string) => `${medicalRoutes.school(schoolAccountId)}/status-events`,
  reviewSummaries: (schoolAccountId: string) => `${medicalRoutes.school(schoolAccountId)}/review-summaries`,
  configuration: (schoolAccountId: string) => `${medicalRoutes.school(schoolAccountId)}/configuration`,
  resolveReview: (schoolAccountId: string, recordId: string) => `${medicalRoutes.school(schoolAccountId)}/reviews/${recordId}/resolve`,
  trace: (schoolAccountId: string, recordId: string) => `${medicalRoutes.school(schoolAccountId)}/trace/${recordId}`,
  guardian: () => "/api/v1/guardians/me/medical",
  guardianUpdates: () => "/api/v1/guardians/me/medical/updates",
  student: () => "/api/v1/students/me/medical",
};

export type MedicalDataSource = "api" | "fallback";
export type MedicalAudience = "school" | "guardian" | "student";

export type MedicalBoardResponse = {
  schoolAccountId: string;
  phase: string;
  status: string;
  capabilities: string[];
  profiles: number;
  openEmergencySessions: number;
  incidents: number;
  notifications: number;
  statusEvents: number;
  reviewSummaries: number;
};

export type MedicalResponse = {
  medicalRecordId: string;
  recordReference: string;
  studentProfileId: string;
  recordType: string;
  status: string;
  severity: string;
  visibleSummary: string;
  expiresAt?: string | null;
  auditTrail: string[];
};

export type MedicalStatusEvent = {
  medicalRecordId: string;
  recordReference: string;
  studentProfileId: string;
  recordType: string;
  status: string;
  severity: string;
  sourceEventType: string;
  notificationEligible: boolean;
  reviewRequired: boolean;
  occurredAt: string;
  availableForNotificationsAt: string;
};

export type MedicalReviewSummary = {
  medicalRecordId: string;
  recordReference: string;
  studentProfileId: string;
  recordType: string;
  status: string;
  severity: string;
  reviewState: string;
  lastEventType: string;
  updatedAt: string;
};

export type MedicalOperationsData = {
  schoolAccountId: string;
  dataSource: MedicalDataSource;
  board: MedicalBoardResponse;
  records: MedicalResponse[];
  emergency: MedicalResponse[];
  incidents: MedicalResponse[];
  notifications: MedicalResponse[];
  history: MedicalResponse[];
  statusEvents: MedicalStatusEvent[];
  reviewSummaries: MedicalReviewSummary[];
  guardianRecords: MedicalResponse[];
  studentRecords: MedicalResponse[];
  configuration: Array<{ key: string; value: string; evidence: string }>;
};

export const medicalCapabilities = [
  "medical.records",
  "medical.emergency_access",
  "medical.incident_logging",
  "medical.notifications",
  "medical.history",
  "medical.configuration",
];

export const medicalFallbackData: MedicalOperationsData = {
  schoolAccountId: "school-demo",
  dataSource: "fallback",
  board: {
    schoolAccountId: "school-demo",
    phase: "medical-emergency",
    status: "ready",
    capabilities: medicalCapabilities,
    profiles: 128,
    openEmergencySessions: 1,
    incidents: 4,
    notifications: 6,
    statusEvents: 4,
    reviewSummaries: 3,
  },
  records: [
    { medicalRecordId: "med-1", recordReference: "MED-2026-0001", studentProfileId: "student-amina", recordType: "profile", status: "Verified", severity: "Routine", visibleSummary: "Allergy plan visible to nurse and guardian.", expiresAt: null, auditTrail: ["medical-profile-updated", "guardian-visibility-reviewed"] },
  ],
  emergency: [
    { medicalRecordId: "emg-1", recordReference: "EMG-2026-0002", studentProfileId: "student-omar", recordType: "emergency-access", status: "Open", severity: "Urgent", visibleSummary: "Emergency access opened with minimum necessary data.", expiresAt: "2026-05-11T09:30:00Z", auditTrail: ["emergency-access-opened", "minimum-necessary-data-opened"] },
  ],
  incidents: [
    { medicalRecordId: "inc-1", recordReference: "INC-2026-0003", studentProfileId: "student-amina", recordType: "incident", status: "Open", severity: "High", visibleSummary: "Clinic incident logged and guardian notification queued.", expiresAt: null, auditTrail: ["incident-logged", "care-action-recorded"] },
  ],
  notifications: [
    { medicalRecordId: "medn-1", recordReference: "MEDN-2026-0004", studentProfileId: "student-amina", recordType: "notification", status: "Queued", severity: "High", visibleSummary: "Medical notification queued for approved guardian contact.", expiresAt: null, auditTrail: ["audience-minimized", "contact-attempt-required"] },
  ],
  history: [],
  statusEvents: [
    { medicalRecordId: "med-1", recordReference: "MED-2026-0001", studentProfileId: "student-amina", recordType: "profile", status: "Verified", severity: "Routine", sourceEventType: "medical-profile-upserted", notificationEligible: false, reviewRequired: false, occurredAt: "2026-05-11T08:00:00Z", availableForNotificationsAt: "2026-05-11T08:02:00Z" },
    { medicalRecordId: "emg-1", recordReference: "EMG-2026-0002", studentProfileId: "student-omar", recordType: "emergency-access", status: "Open", severity: "Urgent", sourceEventType: "mandatory-review-created", notificationEligible: false, reviewRequired: true, occurredAt: "2026-05-11T08:10:00Z", availableForNotificationsAt: "2026-05-11T08:12:00Z" },
    { medicalRecordId: "inc-1", recordReference: "INC-2026-0003", studentProfileId: "student-amina", recordType: "incident", status: "Open", severity: "High", sourceEventType: "incident-logged", notificationEligible: true, reviewRequired: false, occurredAt: "2026-05-11T08:20:00Z", availableForNotificationsAt: "2026-05-11T08:22:00Z" },
  ],
  reviewSummaries: [
    { medicalRecordId: "med-1", recordReference: "MED-2026-0001", studentProfileId: "student-amina", recordType: "profile", status: "Verified", severity: "Routine", reviewState: "none", lastEventType: "medical-profile-upserted", updatedAt: "2026-05-11T08:00:00Z" },
    { medicalRecordId: "emg-1", recordReference: "EMG-2026-0002", studentProfileId: "student-omar", recordType: "emergency-access", status: "Open", severity: "Urgent", reviewState: "mandatory-review-created", lastEventType: "mandatory-review-created", updatedAt: "2026-05-11T08:10:00Z" },
    { medicalRecordId: "inc-1", recordReference: "INC-2026-0003", studentProfileId: "student-amina", recordType: "incident", status: "Open", severity: "High", reviewState: "none", lastEventType: "incident-logged", updatedAt: "2026-05-11T08:20:00Z" },
  ],
  guardianRecords: [
    { medicalRecordId: "med-1", recordReference: "MED-2026-0001", studentProfileId: "student-amina", recordType: "profile", status: "Verified", severity: "Routine", visibleSummary: "Guardian-visible medical summary is current.", expiresAt: null, auditTrail: ["minimum-necessary-view"] },
  ],
  studentRecords: [
    { medicalRecordId: "med-1", recordReference: "MED-2026-0001", studentProfileId: "student-amina", recordType: "profile", status: "Verified", severity: "Routine", visibleSummary: "Student-visible medical summary is current.", expiresAt: null, auditTrail: ["minimum-necessary-view"] },
  ],
  configuration: [
    { key: "emergencyAccessMinutes", value: "30", evidence: "privacy-reviewed" },
    { key: "offlineCacheHours", value: "24", evidence: "stale-cache-warning" },
    { key: "breakGlassRoles", value: "school-nurse, emergency-authorized-staff", evidence: "review-required" },
  ],
};

medicalFallbackData.history = [...medicalFallbackData.records, ...medicalFallbackData.emergency, ...medicalFallbackData.incidents, ...medicalFallbackData.notifications];

export function medicalApiBaseUrl() {
  return serverApiBaseUrl();
}

export function medicalHeaders(schoolAccountId: string, actorReference = "medical-operator") {
  return {
    "content-type": "application/json",
    "x-school-account-id": schoolAccountId,
    "x-actor-reference": actorReference,
    ...serverApiAuthorizationHeader(),
  };
}

async function fetchMedicalJson<T>(path: string, schoolAccountId: string, actorReference?: string): Promise<T | null> {
  const baseUrl = medicalApiBaseUrl();
  if (!baseUrl) return null;
  const response = await fetch(`${baseUrl}${path}`, { headers: medicalHeaders(schoolAccountId, actorReference), cache: "no-store" });
  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadMedicalOperations(schoolAccountId = medicalFallbackData.schoolAccountId): Promise<MedicalOperationsData> {
  const [board, records, emergency, incidents, notifications, history, statusEvents, reviewSummaries, guardianRecords, studentRecords] = await Promise.all([
    fetchMedicalJson<MedicalBoardResponse>(medicalRoutes.school(schoolAccountId), schoolAccountId),
    fetchMedicalJson<MedicalResponse[]>(medicalRoutes.records(schoolAccountId), schoolAccountId),
    fetchMedicalJson<MedicalResponse[]>(medicalRoutes.emergency(schoolAccountId), schoolAccountId),
    fetchMedicalJson<MedicalResponse[]>(medicalRoutes.incidents(schoolAccountId), schoolAccountId),
    fetchMedicalJson<MedicalResponse[]>(medicalRoutes.notifications(schoolAccountId), schoolAccountId),
    fetchMedicalJson<MedicalResponse[]>(medicalRoutes.history(schoolAccountId), schoolAccountId),
    fetchMedicalJson<MedicalStatusEvent[]>(medicalRoutes.statusEvents(schoolAccountId), schoolAccountId),
    fetchMedicalJson<MedicalReviewSummary[]>(medicalRoutes.reviewSummaries(schoolAccountId), schoolAccountId),
    fetchMedicalJson<MedicalResponse[]>(medicalRoutes.guardian(), schoolAccountId, "guardian-demo"),
    fetchMedicalJson<MedicalResponse[]>(medicalRoutes.student(), schoolAccountId, "student-demo"),
  ]);

  const hasApiData = [board, records, emergency, incidents, notifications, history, statusEvents, reviewSummaries, guardianRecords, studentRecords].some((item) => item !== null);
  assertApiDataAvailable("Medical operations", [board, records, emergency, incidents, notifications, history, statusEvents, reviewSummaries, guardianRecords, studentRecords], medicalApiBaseUrl());
  if (!hasApiData) return { ...medicalFallbackData, schoolAccountId, dataSource: "fallback" };

  return {
    ...medicalFallbackData,
    schoolAccountId,
    dataSource: "api",
    board: board ?? { ...medicalFallbackData.board, schoolAccountId },
    records: records ?? [],
    emergency: emergency ?? [],
    incidents: incidents ?? [],
    notifications: notifications ?? [],
    history: history ?? [],
    statusEvents: statusEvents ?? [],
    reviewSummaries: reviewSummaries ?? [],
    guardianRecords: guardianRecords ?? [],
    studentRecords: studentRecords ?? [],
  };
}
