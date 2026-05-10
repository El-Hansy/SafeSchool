class NotificationSummary { const NotificationSummary(this.reference, this.status); final String reference; final String status; }
class CommunicationsRepository { Future<List<NotificationSummary>> notifications() async => const [NotificationSummary('notification-1', 'Unread')]; Future<String> acknowledge(String reference) async => 'ack:$reference'; }
