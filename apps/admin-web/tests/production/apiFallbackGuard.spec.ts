import { afterEach, describe, expect, it } from "vitest";
import { loadAdminOperations } from "../../src/features/administration/api/adminApi";
import { loadCommunicationsOperations } from "../../src/features/communications/api/communicationsApi";
import { loadComplaintOperations } from "../../src/features/complaints/api/complaintsApi";
import { loadDocumentsOperations } from "../../src/features/documents/api/documentStorageApi";
import { loadGuardianTransportData } from "../../src/features/guardian-transport/api/client";
import { loadGuardianWalletData } from "../../src/features/guardian-wallet/api/client";
import { loadLearningOperations } from "../../src/features/learning/api/learningApi";
import { loadMedicalOperations } from "../../src/features/medical/api/medicalApi";
import { loadRequestOperations } from "../../src/features/requests/api/requestsApi";
import { loadSchoolTransportOperations } from "../../src/features/transport/api/client";
import { loadSchoolWalletOperations, walletApiBaseUrl } from "../../src/features/wallet/api/client";
import { serverApiAuthorizationHeader } from "../../src/features/common/apiReadiness";

describe("production API fallback guard", () => {
  afterEach(() => {
    delete process.env.NEXT_PUBLIC_REQUIRE_API_DATA;
    delete process.env.SAFE_SCHOOL_API_BASE_URL;
    delete process.env.SAFE_SCHOOL_API_BEARER_TOKEN;
    delete process.env.NEXT_PUBLIC_API_BASE_URL;
  });

  it("keeps demo fallback available unless API data is required", async () => {
    await expect(loadSchoolWalletOperations()).resolves.toMatchObject({ dataSource: "fallback" });
  });

  it("blocks operational web loaders from showing fallback data when API data is required", async () => {
    process.env.NEXT_PUBLIC_REQUIRE_API_DATA = "1";

    const loaders = [
      loadAdminOperations,
      loadCommunicationsOperations,
      loadComplaintOperations,
      loadDocumentsOperations,
      loadGuardianTransportData,
      loadGuardianWalletData,
      loadLearningOperations,
      loadMedicalOperations,
      loadRequestOperations,
      loadSchoolTransportOperations,
      loadSchoolWalletOperations,
    ];

    for (const load of loaders) {
      await expect(load()).rejects.toThrow("NEXT_PUBLIC_API_BASE_URL");
    }
  });

  it("prefers server API base URL for container-side rendering", () => {
    process.env.SAFE_SCHOOL_API_BASE_URL = "http://api:8080";
    process.env.NEXT_PUBLIC_API_BASE_URL = "https://api.example.school";

    expect(walletApiBaseUrl()).toBe("http://api:8080");
  });

  it("keeps API bearer tokens server-side", () => {
    process.env.SAFE_SCHOOL_API_BEARER_TOKEN = "signed-access-token";

    expect(serverApiAuthorizationHeader()).toEqual({ authorization: "Bearer signed-access-token" });
  });
});
