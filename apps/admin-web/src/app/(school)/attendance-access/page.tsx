import { attendanceAccessArea } from "../../../features/attendance-access";

export default function AttendanceAccessPage() {
  return (
    <main>
      <h1>{attendanceAccessArea.title}</h1>
      <nav>
        <a href="/attendance-access/gates">Gates</a>
        <a href="/attendance-access/scans">Scans</a>
        <a href="/attendance-access/attendance">Attendance</a>
        <a href="/attendance-access/notifications">Notifications</a>
        <a href="/attendance-access/anomalies">Anomalies</a>
      </nav>
    </main>
  );
}

