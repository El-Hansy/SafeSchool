import { assertApiDataAvailable } from "../../common/apiReadiness";

export const complaintsRoutes = {
  school: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/complaints`,
  detail: (schoolAccountId: string, complaintId: string) => `${complaintsRoutes.school(schoolAccountId)}/${complaintId}`,
  triage: (schoolAccountId: string) => `${complaintsRoutes.school(schoolAccountId)}/triage`,
  assigned: (schoolAccountId: string) => `${complaintsRoutes.school(schoolAccountId)}/assigned`,
  assignedDetail: (schoolAccountId: string, complaintId: string) => `${complaintsRoutes.assigned(schoolAccountId)}/${complaintId}`,
  escalations: (schoolAccountId: string) => `${complaintsRoutes.school(schoolAccountId)}/escalations`,
  exceptions: (schoolAccountId: string) => `${complaintsRoutes.school(schoolAccountId)}/exceptions`,
  summaries: (schoolAccountId: string) => `${complaintsRoutes.school(schoolAccountId)}/summaries`,
  category: (schoolAccountId: string, categoryId: string) => `${complaintsRoutes.school(schoolAccountId)}/configuration/categories/${categoryId}`,
  escalationRule: (schoolAccountId: string, ruleId: string) => `${complaintsRoutes.school(schoolAccountId)}/configuration/escalation-rules/${ruleId}`,
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
  triageComplaints: ComplaintResponse[];
  assignedComplaints: ComplaintResponse[];
  escalatedComplaints: ComplaintResponse[];
  exceptions: Array<{ exceptionReference: string; reason: string; owner: string; status: string }>;
  summaries: Array<{ summaryReference: string; status: string; audience: string; evidence: string }>;
  categories: Array<{ categoryId: string; name: string; ownerRole: string; sla: string; evidence: string[] }>;
  escalationRules: Array<{ ruleId: string; trigger: string; ownerRole: string; window: string; evidence: string[] }>;
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
  triageComplaints: [
    { complaintId: "cmp-1", trackingReference: "CMP-2026-0001", status: "NeedsTriage", priority: "High", visibleSummary: "Wellbeing category suggested; restricted details minimized.", auditTrail: ["submitted", "category-suggested"] },
  ],
  assignedComplaints: [
    { complaintId: "cmp-1", trackingReference: "CMP-2026-0001", status: "Assigned", priority: "High", visibleSummary: "Assigned to student wellbeing owner.", auditTrail: ["submitted", "categorized", "assigned"] },
  ],
  escalatedComplaints: [
    { complaintId: "cmp-2", trackingReference: "CMP-2026-0002", status: "Escalated", priority: "Urgent", visibleSummary: "Escalated for same-day transport review.", auditTrail: ["submitted", "assigned", "escalated"] },
  ],
  exceptions: [
    { exceptionReference: "cmp-exception-1", reason: "Duplicate client request conflict", owner: "Complaint reviewer", status: "ManualReviewRequired" },
  ],
  summaries: [
    { summaryReference: "cmp-summary-1", status: "Published", audience: "Guardian", evidence: "restricted-details-minimized" },
  ],
  categories: [
    { categoryId: "wellbeing", name: "Wellbeing", ownerRole: "Student wellbeing", sla: "1 school day", evidence: ["category-versioned", "routing-reviewed"] },
    { categoryId: "transport", name: "Transport", ownerRole: "Transport operations", sla: "Same day", evidence: ["owner-routed"] },
  ],
  escalationRules: [
    { ruleId: "urgent-overdue", trigger: "Urgent priority or overdue SLA", ownerRole: "Senior operations", window: "Same day", evidence: ["rule-versioned", "audit-written"] },
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
  const [board, guardianComplaints, studentComplaints, triageComplaints, assignedComplaints, escalatedComplaints, exceptions, summaries] = await Promise.all([
    fetchComplaintsJson<ComplaintBoardResponse>(complaintsRoutes.school(schoolAccountId), schoolAccountId),
    fetchComplaintsJson<ComplaintResponse[]>(complaintsRoutes.guardian(), schoolAccountId, "guardian-demo"),
    fetchComplaintsJson<ComplaintResponse[]>(complaintsRoutes.student(), schoolAccountId, "student-demo"),
    fetchComplaintsJson<ComplaintResponse[]>(complaintsRoutes.triage(schoolAccountId), schoolAccountId),
    fetchComplaintsJson<ComplaintResponse[]>(complaintsRoutes.assigned(schoolAccountId), schoolAccountId),
    fetchComplaintsJson<ComplaintResponse[]>(complaintsRoutes.escalations(schoolAccountId), schoolAccountId),
    fetchComplaintsJson<ComplaintOperationsData["exceptions"]>(complaintsRoutes.exceptions(schoolAccountId), schoolAccountId),
    fetchComplaintsJson<ComplaintOperationsData["summaries"]>(complaintsRoutes.summaries(schoolAccountId), schoolAccountId),
  ]);

  const hasApiData = [board, guardianComplaints, studentComplaints, triageComplaints, assignedComplaints, escalatedComplaints, exceptions, summaries].some((item) => item !== null);
  assertApiDataAvailable("Complaints operations", [board, guardianComplaints, studentComplaints, triageComplaints, assignedComplaints, escalatedComplaints, exceptions, summaries], complaintsApiBaseUrl());
  if (!hasApiData) return { ...complaintsFallbackData, schoolAccountId, dataSource: "fallback" };

  return {
    ...complaintsFallbackData,
    schoolAccountId,
    dataSource: "api",
    board: board ?? { ...complaintsFallbackData.board, schoolAccountId },
    guardianComplaints: guardianComplaints ?? complaintsFallbackData.guardianComplaints,
    studentComplaints: studentComplaints ?? complaintsFallbackData.studentComplaints,
    triageComplaints: triageComplaints ?? complaintsFallbackData.triageComplaints,
    assignedComplaints: assignedComplaints ?? complaintsFallbackData.assignedComplaints,
    escalatedComplaints: escalatedComplaints ?? complaintsFallbackData.escalatedComplaints,
    exceptions: exceptions ?? complaintsFallbackData.exceptions,
    summaries: summaries ?? complaintsFallbackData.summaries,
  };
}
