export const communicationsRoutes = {
  school: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/communications`,
  guardianNotifications: () => "/api/v1/guardians/me/communications/notifications",
  studentNotifications: () => "/api/v1/students/me/communications/notifications",
};

export const communicationsDemoData = {
  schoolAccountId: "school-demo",
  metrics: [{ label: "Unread", value: "42" }, { label: "Broadcasts", value: "6" }, { label: "Delivery exceptions", value: "4" }],
  rows: ["Attendance source event - notification created", "Direct message - sent", "Emergency broadcast - published"],
};
