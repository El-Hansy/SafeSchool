export const complaintsRoutes = {
  school: (schoolAccountId: string) => `/api/v1/schools/${schoolAccountId}/complaints`,
  guardian: () => "/api/v1/guardians/me/complaints",
  student: () => "/api/v1/students/me/complaints",
};

export const complaintsDemoData = {
  schoolAccountId: "school-demo",
  metrics: [{ label: "Open complaints", value: "14" }, { label: "Escalated", value: "3" }, { label: "Pending feedback", value: "5" }],
  rows: ["CMP-2026-0001 - bullying concern - assigned", "CMP-2026-0002 - transport concern - escalated", "CMP-2026-0003 - canteen issue - resolved"],
};
