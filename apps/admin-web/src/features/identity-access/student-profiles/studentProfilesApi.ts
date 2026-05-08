import { requestIdentityAccess } from "../api/client";

export type StudentProfileStatus = "Draft" | "Active" | "Suspended" | "Deactivated" | "Archived";

export type StudentProfile = {
  studentProfileId: string;
  schoolAccountId: string;
  schoolStudentNumber: string;
  legalName: string;
  preferredName?: string;
  gradeLevel: string;
  enrollmentStatus: string;
  profileStatus: StudentProfileStatus;
  duplicateReviewStatus: string;
};

export type CreateStudentProfileInput = {
  schoolStudentNumber: string;
  legalName: string;
  preferredName?: string;
  dateOfBirth: string;
  gradeLevel: string;
  enrollmentStatus: string;
  profileStatus: StudentProfileStatus;
  reviewReason: string;
  clientRequestId: string;
};

export const studentProfileRoutes = {
  list: "/students",
  create: "/students",
  history: (studentProfileId: string) => `/students/${studentProfileId}/history`,
  deactivate: (studentProfileId: string) => `/students/${studentProfileId}/deactivate`,
};

export function listStudentProfiles(schoolAccountId: string) {
  return requestIdentityAccess<{ items: StudentProfile[] }>(studentProfileRoutes.list, { schoolAccountId });
}

export function createStudentProfile(schoolAccountId: string, body: CreateStudentProfileInput) {
  return requestIdentityAccess<StudentProfile>(studentProfileRoutes.create, {
    schoolAccountId,
    method: "POST",
    body: JSON.stringify({ ...body, externalIdentityReferences: [], campusOrDivision: null }),
  });
}
