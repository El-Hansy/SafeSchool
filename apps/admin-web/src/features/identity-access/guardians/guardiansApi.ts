import { requestIdentityAccess } from "../api/client";

export type Guardian = {
  guardianId: string;
  tenantId: string;
  displayName: string;
  guardianStatus: string;
  identityReviewStatus: string;
};

export type GuardianLink = {
  guardianLinkId: string;
  studentProfileId: string;
  guardianId: string;
  relationshipType: string;
  accessScope: Record<string, string>;
  linkStatus: string;
  validFrom: string;
  validUntil?: string;
};

export const guardianRoutes = {
  guardians: "/guardians",
  createLink: (studentProfileId: string) => `/students/${studentProfileId}/guardian-links`,
  suspendLink: (guardianLinkId: string) => `/guardian-links/${guardianLinkId}/suspend`,
  removeLink: (guardianLinkId: string) => `/guardian-links/${guardianLinkId}/remove`,
};

export function listGuardians(schoolAccountId: string) {
  return requestIdentityAccess<Guardian[]>(guardianRoutes.guardians, { schoolAccountId });
}
