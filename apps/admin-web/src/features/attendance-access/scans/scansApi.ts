export const scanRoutes = {
  record: "/scans",
  offlineSync: "/scans/offline-sync",
  list: "/scans",
  detail: (scanEventId: string) => `/scans/${scanEventId}`,
  trace: (scanEventId: string) => `/scans/${scanEventId}/trace`,
};

export type ScanEventSummary = {
  scanEventId: string;
  studentProfileId: string;
  direction: "Entry" | "Exit";
  status: "Accepted" | "Denied" | "Flagged" | "Duplicate" | "NeedsReview";
};

