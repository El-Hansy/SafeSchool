export function NotificationDetailPanel({ notificationId }: { notificationId: string }) {
  return <section aria-label="Notification detail">{notificationId}</section>;
}

export function WithdrawDialog() {
  return <section aria-label="Withdraw notification">Withdraw</section>;
}

