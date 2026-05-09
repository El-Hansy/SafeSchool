import { AttendanceRecordTable } from "../../../../features/attendance-access/attendance/AttendanceRecordTable";
import { AttendanceSessionForm, GenerationActionPanel } from "../../../../features/attendance-access/attendance/AttendanceSessionForm";

export default function AttendancePage() {
  return (
    <main>
      <h1>Attendance</h1>
      <AttendanceSessionForm />
      <GenerationActionPanel />
      <AttendanceRecordTable records={[]} />
    </main>
  );
}

