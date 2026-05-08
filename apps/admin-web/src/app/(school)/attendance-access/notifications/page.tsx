import { NotificationDetailPanel, WithdrawDialog } from "../../../../features/attendance-access/notifications/NotificationDetailPanel";
import { NotificationRecordTable } from "../../../../features/attendance-access/notifications/NotificationRecordTable";

export default function NotificationsPage() {
  return (
    <main>
      <h1>Entry and exit notifications</h1>
      <NotificationRecordTable records={[]} />
      <NotificationDetailPanel notificationId="selected" />
      <WithdrawDialog />
    </main>
  );
}

