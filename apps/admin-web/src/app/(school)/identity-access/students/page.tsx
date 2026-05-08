import { StudentProfileForm } from "@/features/identity-access/student-profiles/StudentProfileForm";
import { StudentProfileTable } from "@/features/identity-access/student-profiles/StudentProfileTable";
import type { StudentProfile } from "@/features/identity-access/student-profiles/studentProfilesApi";

const sampleProfiles: StudentProfile[] = [
  {
    studentProfileId: "demo-student-1",
    schoolAccountId: "demo-school",
    schoolStudentNumber: "S-1001",
    legalName: "Amina Hassan",
    gradeLevel: "5",
    enrollmentStatus: "Enrolled",
    profileStatus: "Active",
    duplicateReviewStatus: "Clear",
  },
];

export default function StudentProfilesPage() {
  return (
    <main style={{ padding: "32px", display: "grid", gap: "20px" }}>
      <h1 style={{ margin: 0 }}>Student Profiles</h1>
      <StudentProfileForm />
      <StudentProfileTable profiles={sampleProfiles} />
    </main>
  );
}
