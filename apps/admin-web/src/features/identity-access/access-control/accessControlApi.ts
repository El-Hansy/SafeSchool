import { requestIdentityAccess } from "../api/client";

export type Role = {
  roleId: string;
  tenantId: string;
  roleKey: string;
  displayName: string;
  roleScope: string;
  roleStatus: string;
};

export type AccessDecision = {
  accessDecisionId: string;
  actorReference: string;
  attemptedAction: string;
  targetType: string;
  targetReference: string;
  decision: "Allowed" | "Denied";
  decisionReason: string;
  decidedAt: string;
};

export type AuditEvent = {
  auditEventId: string;
  eventCategory: string;
  eventType: string;
  actorReference: string;
  subjectType: string;
  subjectReference: string;
  reason: string;
  eventTime: string;
};

export const accessControlRoutes = {
  roles: "/roles",
  permissions: "/permissions",
  roleAssignments: "/role-assignments",
  accessDecisions: "/access-decisions",
  auditEvents: "/audit-events",
};

export function listRoles(schoolAccountId: string) {
  return requestIdentityAccess<Role[]>(accessControlRoutes.roles, { schoolAccountId });
}
