export const complaintsRoutes = {
  school: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/complaints`,
  assign: (schoolAccountId: string, complaintId: string) => `${complaintsRoutes.school(schoolAccountId)}/${complaintId}/assign`,
  escalate: (schoolAccountId: string, complaintId: string) => `${complaintsRoutes.school(schoolAccountId)}/${complaintId}/escalate`,
  resolve: (schoolAccountId: string, complaintId: string) => `${complaintsRoutes.school(schoolAccountId)}/${complaintId}/resolve`,
  trace: (schoolAccountId: string, complaintId: string) => `${complaintsRoutes.school(schoolAccountId)}/${complaintId}/trace`,
  guardian: () => "/api/v1/guardians/me/complaints",
  guardianFeedback: (complaintId: string) => `${complaintsRoutes.guardian()}/${complaintId}/feedback`,
  student: () => "/api/v1/students/me/complaints",
};

export const complaintsDemoData = {
  schoolAccountId: "school-demo",
  metrics: [{ label: "Open complaints", value: "14" }, { label: "Escalated", value: "3" }, { label: "Pending feedback", value: "5" }],
  rows: ["CMP-2026-0001 - bullying concern - assigned", "CMP-2026-0002 - transport concern - escalated", "CMP-2026-0003 - canteen issue - resolved"],
};

export type ComplaintDataSource = "api" | "fallback";
export type ComplaintAudience = "school" | "guardian" | "student";

export type ComplaintBoardResponse = {
  schoolAccountId: string;
  phase: string;
  status: string;
  capabilities: string[];
  open: number;
  escalated: number;
  pendingFeedback: number;
};

export type ComplaintResponse = {
  complaintId: string;
  trackingReference: string;
  status: string;
  priority: string;
  visibleSummary: string;
  auditTrail: string[];
};

export type ComplaintOperationsData = {
  schoolAccountId: string;
  dataSource: ComplaintDataSource;
  board: ComplaintBoardResponse;
  schoolComplaints: ComplaintResponse[];
  guardianComplaints: ComplaintResponse[];
  studentComplaints: ComplaintResponse[];
};

export const complaintCapabilities = [
  "complaints.submission",
  "complaints.categorization",
  "complaints.assignment",
  "complaints.escalation",
  "complaints.feedback_resolution",
  "complaints.history",
  "complaints.configuration",
  "complaints.review_summaries",
];

export const complaintsFallbackData: ComplaintOperationsData = {
  schoolAccountId: complaintsDemoData.schoolAccountId,
  dataSource: "fallback",
  board: {
    schoolAccountId: complaintsDemoData.schoolAccountId,
    phase: "complaints-escalations",
    status: "ready",
    capabilities: complaintCapabilities,
    open: 14,
    escalated: 3,
    pendingFeedback: 5,
  },
  schoolComplaints: [
    { complaintId: "cmp-1", trackingReference: "CMP-2026-0001", status: "Assigned", priority: "High", visibleSummary: "Bullying concern assigned to student wellbeing owner.", auditTrail: ["submitted", "categorized", "assigned"] },
    { complaintId: "cmp-2", trackingReference: "CMP-2026-0002", status: "Escalated", priority: "Urgent", visibleSummary: "Transport concern escalated for same-day review.", auditTrail: ["submitted", "assigned", "escalated"] },
    { complaintId: "cmp-3", trackingReference: "CMP-2026-0003", status: "Resolved", priority: "Normal", visibleSummary: "Canteen issue resolved with guardian-visible summary.", auditTrail: ["submitted", "resolved", "feedback-requested"] },
  ],
  guardianComplaints: [
    { complaintId: "cmp-1", trackingReference: "CMP-2026-0001", status: "InReview", priority: "High", visibleSummary: "Your complaint is assigned and under review.", auditTrail: ["submitted", "assigned"] },
  ],
  studentComplaints: [
    { complaintId: "cmp-2", trackingReference: "CMP-2026-0002", status: "Received", priority: "Normal", visibleSummary: "Your complaint was received.", auditTrail: ["submitted"] },
  ],
};

export function complaintsApiBaseUrl() {
  return process.env.NEXT_PUBLIC_API_BASE_URL ?? "";
}

export function complaintsHeaders(schoolAccountId: string, actorReference = "complaints-operator") {
  return {
    "content-type": "application/json",
    "x-school-account-id": schoolAccountId,
    "x-actor-reference": actorReference,
  };
}

async function fetchComplaintsJson<T>(path: string, schoolAccountId: string, actorReference?: string): Promise<T | null> {
  const baseUrl = complaintsApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: complaintsHeaders(schoolAccountId, actorReference),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadComplaintOperations(schoolAccountId = complaintsFallbackData.schoolAccountId): Promise<ComplaintOperationsData> {
  const [board, guardianComplaints, studentComplaints] = await Promise.all([
    fetchComplaintsJson<ComplaintBoardResponse>(complaintsRoutes.school(schoolAccountId), schoolAccountId),
    fetchComplaintsJson<ComplaintResponse[]>(complaintsRoutes.guardian(), schoolAccountId, "guardian-demo"),
    fetchComplaintsJson<ComplaintResponse[]>(complaintsRoutes.student(), schoolAccountId, "student-demo"),
  ]);

  const hasApiData = [board, guardianComplaints, studentComplaints].some((item) => item !== null);
  if (!hasApiData) return { ...complaintsFallbackData, schoolAccountId, dataSource: "fallback" };

  return {
    ...complaintsFallbackData,
    schoolAccountId,
    dataSource: "api",
    board: board ?? { ...complaintsFallbackData.board, schoolAccountId },
    guardianComplaints: guardianComplaints ?? complaintsFallbackData.guardianComplaints,
    studentComplaints: studentComplaints ?? complaintsFallbackData.studentComplaints,
  };
}
