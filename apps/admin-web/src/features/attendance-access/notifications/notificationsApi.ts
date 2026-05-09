export const notificationRoutes = {
  list: "/notifications",
  withdraw: (notificationId: string) => `/notifications/${notificationId}/withdraw`,
};

export type NotificationRecordSummary = {
  notificationRecordId: string;
  studentProfileId: string;
  guardianReference: string;
  eligibilityStatus: "Eligible" | "Suppressed" | "Visible" | "Attempted" | "Failed" | "Withdrawn";
};

