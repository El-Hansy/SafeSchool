export function GuardianTransportNotificationList({ records }: { records: string[] }) { return <ul>{records.map((record) => <li key={record}>{record}</li>)}</ul>; }
