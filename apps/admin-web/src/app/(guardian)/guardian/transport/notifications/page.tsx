import { GuardianTransportNotificationList } from "../../../../../features/guardian-transport/notifications/GuardianTransportNotificationList";

export default function GuardianTransportNotificationsPage() {
  return <main><h1>Notifications</h1><GuardianTransportNotificationList records={["Boarding"]} /></main>;
}
