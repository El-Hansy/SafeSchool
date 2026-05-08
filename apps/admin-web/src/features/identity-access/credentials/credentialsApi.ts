import { requestIdentityAccess } from "../api/client";

export type Credential = {
  identityCredentialId: string;
  studentProfileId: string;
  credentialType: "NfcCard" | "QrFallback";
  credentialReference: string;
  credentialStatus: string;
  validFrom: string;
  validUntil?: string;
  statusReason: string;
};

export const credentialRoutes = {
  issueNfc: (studentProfileId: string) => `/students/${studentProfileId}/credentials/nfc`,
  createQr: (studentProfileId: string) => `/students/${studentProfileId}/credentials/qr`,
  list: (studentProfileId: string) => `/students/${studentProfileId}/credentials`,
  suspend: (credentialId: string) => `/credentials/${credentialId}/suspend`,
  restore: (credentialId: string) => `/credentials/${credentialId}/restore`,
  replace: (credentialId: string) => `/credentials/${credentialId}/replace`,
  revoke: (credentialId: string) => `/credentials/${credentialId}/revoke`,
  statusSnapshot: "/credentials/status-snapshot",
};

export function listCredentials(schoolAccountId: string, studentProfileId: string) {
  return requestIdentityAccess<Credential[]>(credentialRoutes.list(studentProfileId), { schoolAccountId });
}
