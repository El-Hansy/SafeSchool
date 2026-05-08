import { describe, expect, it } from "vitest";
import { credentialRoutes } from "../../src/features/identity-access/credentials/credentialsApi";

describe("credential lifecycle routes", () => {
  it("keeps NFC, QR, lifecycle, and status snapshot routes in Phase 1 identity scope", () => {
    expect(credentialRoutes.issueNfc("student-1")).toBe("/students/student-1/credentials/nfc");
    expect(credentialRoutes.createQr("student-1")).toBe("/students/student-1/credentials/qr");
    expect(credentialRoutes.revoke("credential-1")).toBe("/credentials/credential-1/revoke");
    expect(credentialRoutes.statusSnapshot).toBe("/credentials/status-snapshot");
  });
});
