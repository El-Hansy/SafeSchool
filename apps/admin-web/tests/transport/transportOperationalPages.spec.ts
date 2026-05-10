import { readFileSync } from "node:fs";
import { join } from "node:path";
import { describe, expect, it } from "vitest";
import { guardianTransportApi, loadGuardianTransportData } from "../../src/features/guardian-transport/api/client";
import { loadSchoolTransportOperations, transportRoutes } from "../../src/features/transport/api/client";

describe("transport operational pages", () => {
  it("uses operational transport pages instead of static demo pages", () => {
    const pages = [
      "src/app/(school)/transport/page.tsx",
      "src/app/(school)/transport/routes/page.tsx",
      "src/app/(school)/transport/assignments/page.tsx",
      "src/app/(school)/transport/scans/page.tsx",
      "src/app/(school)/transport/trips/page.tsx",
      "src/app/(school)/transport/eta/page.tsx",
      "src/app/(school)/transport/notifications/page.tsx",
      "src/app/(guardian)/guardian/transport/page.tsx",
      "src/app/(guardian)/guardian/transport/progress/page.tsx",
      "src/app/(guardian)/guardian/transport/eta/page.tsx",
      "src/app/(guardian)/guardian/transport/notifications/page.tsx",
    ];

    for (const page of pages) {
      const source = readFileSync(join(process.cwd(), page), "utf8");
      expect(source).not.toContain("TransportDemo");
      expect(source).not.toContain("GuardianTransportDemo");
      expect(source).toMatch(/TransportOperationsPage|GuardianTransportExperience/);
    }
  });

  it("loads transport fallback data with operational records when the API base URL is not configured", async () => {
    const data = await loadSchoolTransportOperations("school-demo");

    expect(data.dataSource).toBe("fallback");
    expect(data.routes.length).toBeGreaterThan(0);
    expect(data.vehicles.length).toBeGreaterThan(0);
    expect(data.assignments.length).toBeGreaterThan(0);
    expect(data.scans.length).toBeGreaterThan(0);
    expect(data.trips.length).toBeGreaterThan(0);
    expect(data.etaRecords.length).toBeGreaterThan(0);
    expect(data.reviewSummaries.length).toBeGreaterThan(0);
  });

  it("keeps school actions and guardian reads on scoped transport routes", async () => {
    expect(transportRoutes.scanEvents("school-demo")).toBe("/api/v1/schools/school-demo/transport/scan-events");
    expect(transportRoutes.locationUpdates("school-demo", "trip-1")).toBe("/api/v1/schools/school-demo/transport/trips/trip-1/location-updates");
    expect(transportRoutes.etaRecalculate("school-demo", "trip-1")).toBe("/api/v1/schools/school-demo/transport/trips/trip-1/eta/recalculate");
    expect(guardianTransportApi.progress("student-amina", "trip-1")).toBe("/api/v1/guardians/me/students/student-amina/transport/trips/trip-1/progress");

    const guardianData = await loadGuardianTransportData("student-amina");
    expect(guardianData.dataSource).toBe("fallback");
    expect(guardianData.plan.assignments.every((assignment) => assignment.studentProfileId === "student-amina")).toBe(true);
    expect(guardianData.notifications.notifications.every((notification) => notification.studentProfileId === "student-amina")).toBe(true);
  });

  it("exposes driver scan and trip update forms through backend route helpers", () => {
    const source = readFileSync(join(process.cwd(), "src/features/transport/components/TransportActionForms.tsx"), "utf8");

    expect(source).toContain("transportRoutes.scanEvents");
    expect(source).toContain("transportRoutes.locationUpdates");
    expect(source).toContain("transportRoutes.scanContextTrips");
    expect(source).toContain("transportRoutes.etaRecalculate");
    expect(source).toContain("transportRoutes.manualReviews");
  });
});
