export type MobileRole =
  | "guardian"
  | "student"
  | "transport_driver"
  | "gate_access"
  | "canteen_cashier"
  | "teacher"
  | "medical_staff"
  | "complaint_handler"
  | "communication_sender"
  | "document_administrator"
  | "school_administrator"
  | "platform_support";

export type MobileWorkspace = {
  role: MobileRole;
  label: string;
  arabicLabel: string;
  features: string[];
  actions: string[];
  offline: boolean;
};

export const mobileWorkspaces: MobileWorkspace[] = [
  { role: "guardian", label: "Guardian", arabicLabel: "ولي الأمر", features: ["attendance", "transport", "wallet", "learning", "requests", "complaints", "documents"], actions: ["View linked students", "Submit request", "Submit complaint"], offline: false },
  { role: "student", label: "Student", arabicLabel: "الطالب", features: ["learning", "communications", "complaints", "documents"], actions: ["View learning", "Read messages", "View certificates"], offline: false },
  { role: "transport_driver", label: "Transport driver", arabicLabel: "سائق الحافلة", features: ["transport"], actions: ["View trip", "Boarding scan", "Drop scan", "Send location"], offline: true },
  { role: "gate_access", label: "Gate/access staff", arabicLabel: "بوابة المدرسة", features: ["attendance_access"], actions: ["NFC scan", "QR scan", "Review denial"], offline: true },
  { role: "canteen_cashier", label: "Canteen cashier", arabicLabel: "المقصف", features: ["wallet"], actions: ["Scan wallet", "Charge purchase", "Review decision"], offline: true },
  { role: "teacher", label: "Teacher", arabicLabel: "المعلم", features: ["learning", "attendance"], actions: ["View class", "Record behavior", "Review assignments"], offline: false },
  { role: "medical_staff", label: "Medical staff", arabicLabel: "العيادة", features: ["medical", "emergency"], actions: ["View medical profile", "Capture emergency", "Handover"], offline: true },
  { role: "complaint_handler", label: "Complaint handler", arabicLabel: "الشكاوى", features: ["complaints"], actions: ["Triage", "Assign", "Escalate"], offline: false },
  { role: "communication_sender", label: "Communication sender", arabicLabel: "الرسائل", features: ["communications"], actions: ["Compose", "Send", "Review delivery"], offline: false },
  { role: "document_administrator", label: "Document administrator", arabicLabel: "الوثائق", features: ["documents", "certificates"], actions: ["Review document", "Issue certificate", "Search"], offline: false },
  { role: "school_administrator", label: "School administrator", arabicLabel: "إدارة المدرسة", features: ["admin", "mobile_permissions"], actions: ["Assign roles", "Review release", "View metrics"], offline: false },
  { role: "platform_support", label: "Platform support", arabicLabel: "الدعم", features: ["support", "audit"], actions: ["Inspect device", "Review install", "Trace issue"], offline: false },
];

export const mobileReleases = [
  { id: "release-12", version: "12.0.0", code: 1200, status: "Active", audience: "school-demo pilot", checksum: "sha256-demo-phase12" },
  { id: "release-11", version: "11.0.0", code: 1100, status: "Superseded", audience: "internal", checksum: "sha256-demo-phase11" },
];

export const mobileSupportEvents = [
  { id: "evt-1", user: "guardian-demo", device: "device-guardian", version: "12.0.0", result: "allowed", reason: "ok" },
  { id: "evt-2", user: "student-demo", device: "device-student", version: "12.0.0", result: "blocked", reason: "FEATURE_DISABLED" },
  { id: "evt-3", user: "driver-demo", device: "device-driver", version: "12.0.0", result: "accepted", reason: "sync" },
];

export function mobileStats() {
  return {
    roles: mobileWorkspaces.length,
    rtlCoverage: "Arabic + English",
    activeRelease: mobileReleases[0].version,
    supportEvents: mobileSupportEvents.length,
  };
}
