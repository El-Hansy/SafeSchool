export type TransportApiError = { code: string; message: string; field?: string };
export type Page<T> = { items: T[]; page: number; pageSize: number; totalCount: number };
export type TransportDataSource = "api" | "fallback";
export type TransportEnumValue = string | number;

export type RouteResponse = {
  transportRouteId: string;
  schoolAccountId: string;
  routeName: string;
  routeCode: string;
  serviceDirection: TransportEnumValue;
  routeStatus: TransportEnumValue;
  routeVersion: string;
  updatedAt: string;
};

export type StopResponse = {
  transportStopId: string;
  schoolAccountId: string;
  stopName: string;
  stopCode: string;
  pickupAllowed: boolean;
  dropAllowed: boolean;
  stopStatus: TransportEnumValue;
};

export type RouteStopSequenceResponse = {
  routeStopSequenceId: string;
  transportRouteId: string;
  transportStopId: string;
  serviceDirection: TransportEnumValue;
  sequenceNumber: number;
  routeVersion: string;
  sequenceStatus: TransportEnumValue;
};

export type VehicleResponse = {
  transportVehicleId: string;
  schoolAccountId: string;
  vehicleName: string;
  vehicleCode: string;
  capacity: number;
  vehicleStatus: TransportEnumValue;
};

export type AssignmentResponse = {
  studentTransportAssignmentId: string;
  schoolAccountId: string;
  studentProfileId: string;
  transportRouteId: string;
  transportVehicleId?: string | null;
  serviceDirection: TransportEnumValue;
  validFrom: string;
  validTo?: string | null;
  visibilityState: TransportEnumValue;
  assignmentStatus: TransportEnumValue;
  reviewStatus: TransportEnumValue;
  updatedAt: string;
};

export type BoardingDropScanResponse = {
  boardingDropScanEventId: string;
  schoolAccountId: string;
  transportTripId: string;
  transportRouteId: string;
  routeStopSequenceId: string;
  studentProfileId: string;
  studentTransportAssignmentId?: string | null;
  credentialReference: string;
  scanDirection: TransportEnumValue;
  localScanTime: string;
  receivedAt: string;
  offlineCaptured: boolean;
  syncStatus: TransportEnumValue;
  scanDecision: TransportEnumValue;
  decisionReason: string;
  transportStatusAfter: TransportEnumValue;
  reviewStatus: TransportEnumValue;
};

export type LocationUpdateResponse = {
  transportLocationUpdateId: string;
  transportTripId: string;
  progressState: TransportEnumValue;
  freshnessStatus: TransportEnumValue;
  acceptanceStatus: TransportEnumValue;
  reportedAt: string;
  receivedAt: string;
  suppressionReason: string;
};

export type TripProgressResponse = {
  transportTripId: string;
  schoolAccountId: string;
  transportRouteId: string;
  transportVehicleId: string;
  tripStatus: TransportEnumValue;
  serviceDirection: TransportEnumValue;
  latestLocation?: LocationUpdateResponse | null;
  guardianVisibilityState: string;
};

export type EtaRecordResponse = {
  etaRecordId: string;
  schoolAccountId: string;
  transportTripId: string;
  transportRouteId: string;
  routeStopSequenceId: string;
  studentProfileId: string;
  estimatedArrivalTime?: string | null;
  etaState: TransportEnumValue;
  confidenceState: TransportEnumValue;
  freshnessStatus: TransportEnumValue;
  sourceLocationUpdateId?: string | null;
  calculatedAt: string;
  reviewStatus: TransportEnumValue;
};

export type TransportNotificationRecordResponse = {
  transportNotificationRecordId: string;
  schoolAccountId: string;
  guardianRecordId: string;
  studentProfileId: string;
  transportTripId?: string | null;
  eventType: TransportEnumValue;
  sourceEventReference: string;
  notificationStatus: TransportEnumValue;
  suppressionReason: string;
  visibleStatus: string;
  updatedAt: string;
};

export type TransportReviewSummary = {
  summaryScope: string;
  scopeReference: string;
  schoolAccountId: string;
  acceptedScans: number;
  needsReviewScans: number;
  currentLocations: number;
  staleLocations: number;
  visibleNotifications: number;
  openAnomalies: number;
  latestEvidenceAt: string;
};

export type TransportRuleSettingResponse = {
  ruleSettingId: string;
  schoolAccountId: string;
  status: TransportEnumValue;
  locationDetailRetentionDays: number;
  locationStalenessThreshold: string;
  changeReason: string;
  updatedAt: string;
};

export type TransportAnomalySummary = {
  anomalyId: string;
  anomalyType: string;
  severity: string;
  target: string;
  status: string;
  evidence: string;
};

export type TransportOperationsData = {
  schoolAccountId: string;
  dataSource: TransportDataSource;
  routes: RouteResponse[];
  stops: StopResponse[];
  routeStopSequences: RouteStopSequenceResponse[];
  vehicles: VehicleResponse[];
  assignments: AssignmentResponse[];
  scans: BoardingDropScanResponse[];
  trips: TripProgressResponse[];
  etaRecords: EtaRecordResponse[];
  notifications: TransportNotificationRecordResponse[];
  anomalies: TransportAnomalySummary[];
  reviewSummaries: TransportReviewSummary[];
  ruleSetting: TransportRuleSettingResponse;
};

export const transportApi = {
  base: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/transport`,
  rules: "/rule-settings/current",
  reviewSummaries: "/review-summaries",
  featureDisabled: (capability: string): TransportApiError => ({ code: "feature_disabled", message: `${capability} is disabled` }),
  guardianScope: (): TransportApiError => ({ code: "guardian_scope_denied", message: "Guardian scope does not include this student" })
};

export const transportRoutes = {
  base: transportApi.base,
  routes: (schoolAccountId: string) => `${transportApi.base(schoolAccountId)}/routes`,
  stops: (schoolAccountId: string) => `${transportApi.base(schoolAccountId)}/stops`,
  vehicles: (schoolAccountId: string) => `${transportApi.base(schoolAccountId)}/vehicles`,
  assignments: (schoolAccountId: string) => `${transportApi.base(schoolAccountId)}/assignments`,
  scanContextTrips: (schoolAccountId: string) => `${transportApi.base(schoolAccountId)}/scan-context-trips`,
  startScanContextTrip: (schoolAccountId: string, tripId: string) => `${transportApi.base(schoolAccountId)}/scan-context-trips/${tripId}/start`,
  endScanContextTrip: (schoolAccountId: string, tripId: string) => `${transportApi.base(schoolAccountId)}/scan-context-trips/${tripId}/end`,
  scanEvents: (schoolAccountId: string) => `${transportApi.base(schoolAccountId)}/scan-events`,
  locationUpdates: (schoolAccountId: string, tripId: string) => `${transportApi.base(schoolAccountId)}/trips/${tripId}/location-updates`,
  tripProgress: (schoolAccountId: string, tripId: string) => `${transportApi.base(schoolAccountId)}/trips/${tripId}/progress`,
  etaRecalculate: (schoolAccountId: string, tripId: string) => `${transportApi.base(schoolAccountId)}/trips/${tripId}/eta/recalculate`,
  notificationWithdraw: (schoolAccountId: string, notificationRecordId: string) => `${transportApi.base(schoolAccountId)}/notification-records/${notificationRecordId}/withdraw`,
  ruleSettings: (schoolAccountId: string) => `${transportApi.base(schoolAccountId)}${transportApi.rules}`,
  reviewSummaries: (schoolAccountId: string) => `${transportApi.base(schoolAccountId)}${transportApi.reviewSummaries}`,
  manualReviews: (schoolAccountId: string) => `${transportApi.base(schoolAccountId)}/manual-reviews`,
};

export const transportEnumValues = {
  routeStatus: { Draft: 0, Active: 1, Suspended: 2, Retired: 3 },
  vehicleStatus: { Draft: 0, Active: 1, Suspended: 2, Retired: 3 },
  serviceDirection: { Pickup: 0, Dropoff: 1, Both: 2, Combined: 3 },
  visibilityState: { GuardianVisible: 0, StaffOnly: 1, Suspended: 2 },
  assignmentStatus: { Draft: 0, Active: 1, Suspended: 2, Expired: 3, Removed: 4 },
  scanDirection: { Boarding: 0, Drop: 1 },
  scanMethod: { Nfc: 0, Qr: 1, Manual: 2 },
  locationProgressState: { NotStarted: 0, EnRoute: 1, ApproachingStop: 2, AtStop: 3, Delayed: 4, Completed: 5 },
};

const demoIds = {
  routeNorth: "11111111-1111-4111-8111-111111111111",
  routeEast: "12121212-1212-4121-8121-121212121212",
  stopGate: "22222222-2222-4222-8222-222222222222",
  stopLibrary: "23232323-2323-4232-8232-232323232323",
  stopCampus: "24242424-2424-4242-8242-242424242424",
  sequenceGate: "33333333-3333-4333-8333-333333333333",
  sequenceLibrary: "34343434-3434-4343-8343-343434343434",
  sequenceCampus: "35353535-3535-4353-8353-353535353535",
  vehicleBus12: "44444444-4444-4444-8444-444444444444",
  vehicleBus8: "45454545-4545-4454-8454-454545454545",
  assignmentAmina: "55555555-5555-4555-8555-555555555555",
  assignmentOmar: "56565656-5656-4565-8565-565656565656",
  tripNorth: "66666666-6666-4666-8666-666666666666",
  locationNorth: "77777777-7777-4777-8777-777777777777",
  etaAmina: "88888888-8888-4888-8888-888888888888",
  notificationAmina: "99999999-9999-4999-8999-999999999999",
};

export const transportDemoData: TransportOperationsData = {
  schoolAccountId: "school-demo",
  dataSource: "fallback",
  routes: [
    { transportRouteId: demoIds.routeNorth, schoolAccountId: "school-demo", routeName: "North Morning Route", routeCode: "NORTH-AM", serviceDirection: "Pickup", routeStatus: "Active", routeVersion: "v3", updatedAt: "2026-05-10T06:00:00Z" },
    { transportRouteId: demoIds.routeEast, schoolAccountId: "school-demo", routeName: "East Dropoff Route", routeCode: "EAST-PM", serviceDirection: "Dropoff", routeStatus: "Active", routeVersion: "v2", updatedAt: "2026-05-10T06:10:00Z" },
    { transportRouteId: "13131313-1313-4131-8131-131313131313", schoolAccountId: "school-demo", routeName: "West Morning Route", routeCode: "WEST-AM", serviceDirection: "Pickup", routeStatus: "Draft", routeVersion: "v1", updatedAt: "2026-05-10T06:20:00Z" },
  ],
  stops: [
    { transportStopId: demoIds.stopGate, schoolAccountId: "school-demo", stopName: "Gate 4 Community", stopCode: "GATE-4", pickupAllowed: true, dropAllowed: true, stopStatus: "Active" },
    { transportStopId: demoIds.stopLibrary, schoolAccountId: "school-demo", stopName: "Library Corner", stopCode: "LIBRARY", pickupAllowed: true, dropAllowed: true, stopStatus: "Active" },
    { transportStopId: demoIds.stopCampus, schoolAccountId: "school-demo", stopName: "North Campus", stopCode: "NORTH-CAMPUS", pickupAllowed: false, dropAllowed: true, stopStatus: "Active" },
  ],
  routeStopSequences: [
    { routeStopSequenceId: demoIds.sequenceGate, transportRouteId: demoIds.routeNorth, transportStopId: demoIds.stopGate, serviceDirection: "Pickup", sequenceNumber: 1, routeVersion: "v3", sequenceStatus: "Active" },
    { routeStopSequenceId: demoIds.sequenceLibrary, transportRouteId: demoIds.routeNorth, transportStopId: demoIds.stopLibrary, serviceDirection: "Pickup", sequenceNumber: 2, routeVersion: "v3", sequenceStatus: "Active" },
    { routeStopSequenceId: demoIds.sequenceCampus, transportRouteId: demoIds.routeNorth, transportStopId: demoIds.stopCampus, serviceDirection: "Pickup", sequenceNumber: 3, routeVersion: "v3", sequenceStatus: "Active" },
  ],
  vehicles: [
    { transportVehicleId: demoIds.vehicleBus12, schoolAccountId: "school-demo", vehicleName: "Bus 12", vehicleCode: "BUS-12", capacity: 36, vehicleStatus: "Active" },
    { transportVehicleId: demoIds.vehicleBus8, schoolAccountId: "school-demo", vehicleName: "Bus 08", vehicleCode: "BUS-08", capacity: 32, vehicleStatus: "Active" },
  ],
  assignments: [
    { studentTransportAssignmentId: demoIds.assignmentAmina, schoolAccountId: "school-demo", studentProfileId: "student-amina", transportRouteId: demoIds.routeNorth, transportVehicleId: demoIds.vehicleBus12, serviceDirection: "Pickup", validFrom: "2026-05-01", validTo: null, visibilityState: "GuardianVisible", assignmentStatus: "Active", reviewStatus: "NotRequired", updatedAt: "2026-05-10T06:10:00Z" },
    { studentTransportAssignmentId: demoIds.assignmentOmar, schoolAccountId: "school-demo", studentProfileId: "student-omar", transportRouteId: demoIds.routeNorth, transportVehicleId: demoIds.vehicleBus12, serviceDirection: "Pickup", validFrom: "2026-05-01", validTo: null, visibilityState: "GuardianVisible", assignmentStatus: "Active", reviewStatus: "NotRequired", updatedAt: "2026-05-10T06:10:00Z" },
  ],
  scans: [
    { boardingDropScanEventId: "aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa", schoolAccountId: "school-demo", transportTripId: demoIds.tripNorth, transportRouteId: demoIds.routeNorth, routeStopSequenceId: demoIds.sequenceGate, studentProfileId: "student-amina", studentTransportAssignmentId: demoIds.assignmentAmina, credentialReference: "NFC-AMINA-001", scanDirection: "Boarding", localScanTime: "2026-05-10T06:42:00Z", receivedAt: "2026-05-10T06:42:05Z", offlineCaptured: false, syncStatus: "Reconciled", scanDecision: "Accepted", decisionReason: "Active assignment and credential", transportStatusAfter: "Onboard", reviewStatus: "NotRequired" },
    { boardingDropScanEventId: "abababab-abab-4aba-8aba-abababababab", schoolAccountId: "school-demo", transportTripId: demoIds.tripNorth, transportRouteId: demoIds.routeNorth, routeStopSequenceId: demoIds.sequenceLibrary, studentProfileId: "student-omar", studentTransportAssignmentId: demoIds.assignmentOmar, credentialReference: "QR-OMAR-001", scanDirection: "Boarding", localScanTime: "2026-05-10T06:44:00Z", receivedAt: "2026-05-10T06:44:06Z", offlineCaptured: false, syncStatus: "Reconciled", scanDecision: "Accepted", decisionReason: "QR fallback accepted", transportStatusAfter: "Onboard", reviewStatus: "NotRequired" },
    { boardingDropScanEventId: "acacacac-acac-4aca-8aca-acacacacacac", schoolAccountId: "school-demo", transportTripId: demoIds.tripNorth, transportRouteId: demoIds.routeNorth, routeStopSequenceId: demoIds.sequenceGate, studentProfileId: "unknown", studentTransportAssignmentId: null, credentialReference: "NFC-UNKNOWN", scanDirection: "Boarding", localScanTime: "2026-05-10T06:47:00Z", receivedAt: "2026-05-10T06:47:10Z", offlineCaptured: false, syncStatus: "PartiallyReconciled", scanDecision: "NeedsReview", decisionReason: "Credential not matched", transportStatusAfter: "NeedsReview", reviewStatus: "NeedsReview" },
  ],
  trips: [
    { transportTripId: demoIds.tripNorth, schoolAccountId: "school-demo", transportRouteId: demoIds.routeNorth, transportVehicleId: demoIds.vehicleBus12, tripStatus: "Active", serviceDirection: "Pickup", guardianVisibilityState: "Exact Location Available", latestLocation: { transportLocationUpdateId: demoIds.locationNorth, transportTripId: demoIds.tripNorth, progressState: "ApproachingStop", freshnessStatus: "Current", acceptanceStatus: "Accepted", reportedAt: "2026-05-10T06:52:00Z", receivedAt: "2026-05-10T06:52:05Z", suppressionReason: "" } },
    { transportTripId: "67676767-6767-4676-8676-676767676767", schoolAccountId: "school-demo", transportRouteId: demoIds.routeEast, transportVehicleId: demoIds.vehicleBus8, tripStatus: "Planned", serviceDirection: "Dropoff", guardianVisibilityState: "ETA Only", latestLocation: null },
  ],
  etaRecords: [
    { etaRecordId: demoIds.etaAmina, schoolAccountId: "school-demo", transportTripId: demoIds.tripNorth, transportRouteId: demoIds.routeNorth, routeStopSequenceId: demoIds.sequenceGate, studentProfileId: "student-amina", estimatedArrivalTime: "2026-05-10T06:58:00Z", etaState: "Available", confidenceState: "High", freshnessStatus: "Current", sourceLocationUpdateId: demoIds.locationNorth, calculatedAt: "2026-05-10T06:52:05Z", reviewStatus: "NotRequired" },
    { etaRecordId: "89898989-8989-4898-8898-898989898989", schoolAccountId: "school-demo", transportTripId: demoIds.tripNorth, transportRouteId: demoIds.routeNorth, routeStopSequenceId: demoIds.sequenceLibrary, studentProfileId: "student-omar", estimatedArrivalTime: "2026-05-10T07:04:00Z", etaState: "Available", confidenceState: "Medium", freshnessStatus: "Current", sourceLocationUpdateId: demoIds.locationNorth, calculatedAt: "2026-05-10T06:52:05Z", reviewStatus: "NotRequired" },
  ],
  notifications: [
    { transportNotificationRecordId: demoIds.notificationAmina, schoolAccountId: "school-demo", guardianRecordId: "guardian-mariam", studentProfileId: "student-amina", transportTripId: demoIds.tripNorth, eventType: "Boarding", sourceEventReference: "scan-amina", notificationStatus: "Visible", suppressionReason: "", visibleStatus: "Amina boarded Bus 12", updatedAt: "2026-05-10T06:42:07Z" },
    { transportNotificationRecordId: "91919191-9191-4919-8919-919191919191", schoolAccountId: "school-demo", guardianRecordId: "guardian-saleh", studentProfileId: "student-omar", transportTripId: demoIds.tripNorth, eventType: "MaterialEtaChange", sourceEventReference: "eta-omar", notificationStatus: "Visible", suppressionReason: "", visibleStatus: "Omar ETA changed by 6 minutes", updatedAt: "2026-05-10T06:54:00Z" },
  ],
  anomalies: [
    { anomalyId: "anomaly-1", anomalyType: "Invalid credential", severity: "High", target: "NFC-UNKNOWN", status: "Open", evidence: "scan needs review" },
    { anomalyId: "anomaly-2", anomalyType: "Wrong stop", severity: "Medium", target: "student-omar", status: "Assigned", evidence: "stop mismatch" },
  ],
  reviewSummaries: [
    { summaryScope: "School", scopeReference: "school-demo", schoolAccountId: "school-demo", acceptedScans: 243, needsReviewScans: 3, currentLocations: 2, staleLocations: 0, visibleNotifications: 91, openAnomalies: 2, latestEvidenceAt: "2026-05-10T06:55:00Z" },
  ],
  ruleSetting: { ruleSettingId: "bcbcbcbc-bcbc-4bcb-8bcb-bcbcbcbcbcbc", schoolAccountId: "school-demo", status: "Active", locationDetailRetentionDays: 30, locationStalenessThreshold: "00:03:00", changeReason: "Fallback transport safety policy", updatedAt: "2026-05-10T05:00:00Z" },
};

export function transportApiBaseUrl() {
  return process.env.NEXT_PUBLIC_API_BASE_URL ?? "";
}

export function transportHeaders(schoolAccountId: string, actorReference = "transport-operator") {
  return {
    "content-type": "application/json",
    "x-school-account-id": schoolAccountId,
    "x-actor-reference": actorReference,
  };
}

async function fetchTransportJson<T>(path: string, schoolAccountId: string): Promise<T | null> {
  const baseUrl = transportApiBaseUrl();
  if (!baseUrl) return null;

  const response = await fetch(`${baseUrl}${path}`, {
    headers: transportHeaders(schoolAccountId),
    cache: "no-store",
  });

  if (!response.ok) return null;
  return (await response.json()) as T;
}

export async function loadSchoolTransportOperations(schoolAccountId = transportDemoData.schoolAccountId): Promise<TransportOperationsData> {
  const [routes, stops, vehicles, assignments, reviewSummaries, ruleSetting] = await Promise.all([
    fetchTransportJson<RouteResponse[]>(transportRoutes.routes(schoolAccountId), schoolAccountId),
    fetchTransportJson<StopResponse[]>(transportRoutes.stops(schoolAccountId), schoolAccountId),
    fetchTransportJson<VehicleResponse[]>(transportRoutes.vehicles(schoolAccountId), schoolAccountId),
    fetchTransportJson<AssignmentResponse[]>(transportRoutes.assignments(schoolAccountId), schoolAccountId),
    fetchTransportJson<TransportReviewSummary[]>(transportRoutes.reviewSummaries(schoolAccountId), schoolAccountId),
    fetchTransportJson<TransportRuleSettingResponse>(transportRoutes.ruleSettings(schoolAccountId), schoolAccountId),
  ]);

  const hasApiData = [routes, stops, vehicles, assignments, reviewSummaries, ruleSetting].some((item) => item !== null);
  if (!hasApiData) return { ...transportDemoData, schoolAccountId, dataSource: "fallback" };

  return {
    ...transportDemoData,
    schoolAccountId,
    dataSource: "api",
    routes: routes ?? transportDemoData.routes,
    stops: stops ?? transportDemoData.stops,
    vehicles: vehicles ?? transportDemoData.vehicles,
    assignments: assignments ?? transportDemoData.assignments,
    reviewSummaries: reviewSummaries ?? transportDemoData.reviewSummaries,
    ruleSetting: ruleSetting ?? transportDemoData.ruleSetting,
  };
}

export function transportLabel(value: TransportEnumValue | null | undefined, labels: Record<number, string> = {}) {
  if (value === null || value === undefined || value === "") return "None";
  if (typeof value === "number") return labels[value] ?? String(value);
  return value.replace(/([a-z])([A-Z])/g, "$1 $2");
}
