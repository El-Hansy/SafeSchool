import type { NotificationRecordSummary } from "./notificationsApi";

export function SuppressionReasonBadge({ reason }: { reason?: string }) {
  return <span>{reason ?? "visible"}</span>;
}

export function NotificationRecordTable({ records }: { records: NotificationRecordSummary[] }) {
  return (
    <table>
      <tbody>
        {records.map((record) => (
          <tr key={record.notificationRecordId}>
            <td>{record.studentProfileId}</td>
            <td>{record.guardianReference}</td>
            <td>{record.eligibilityStatus}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

