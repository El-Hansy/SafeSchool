import type { AttendanceRecordSummary } from "./attendanceApi";

export function AttendanceStatusBadge({ status }: { status: AttendanceRecordSummary["status"] }) {
  return <span data-status={status}>{status}</span>;
}

export function AttendanceRecordTable({ records }: { records: AttendanceRecordSummary[] }) {
  return (
    <table>
      <tbody>
        {records.map((record) => (
          <tr key={record.attendanceRecordId}>
            <td>{record.studentProfileId}</td>
            <td>
              <AttendanceStatusBadge status={record.status} />
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

export function CorrectionDialog() {
  return <section aria-label="Correction">Correction</section>;
}

export function AttendanceSummaryCards() {
  return <section aria-label="Summary">Summary</section>;
}

