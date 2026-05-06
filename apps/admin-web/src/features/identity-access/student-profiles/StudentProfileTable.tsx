import type { StudentProfile } from "./studentProfilesApi";

type Props = {
  profiles: StudentProfile[];
};

export function StudentProfileTable({ profiles }: Props) {
  return (
    <table style={{ width: "100%", borderCollapse: "collapse", background: "#ffffff" }}>
      <thead>
        <tr>
          {["Student number", "Name", "Grade", "Status", "Duplicate review"].map((header) => (
            <th key={header} style={{ textAlign: "left", padding: "10px", borderBottom: "1px solid #d8dee8" }}>
              {header}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {profiles.map((profile) => (
          <tr key={profile.studentProfileId}>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{profile.schoolStudentNumber}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{profile.legalName}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{profile.gradeLevel}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{profile.profileStatus}</td>
            <td style={{ padding: "10px", borderBottom: "1px solid #eef2f7" }}>{profile.duplicateReviewStatus}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
