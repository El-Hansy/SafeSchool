export function SeverityBadge({ severity }: { severity: string }) {
  return <span data-severity={severity}>{severity}</span>;
}

export function StatusBadge({ status }: { status: string }) {
  return <span data-status={status}>{status}</span>;
}

export function AnomalyTable({ anomalies }: { anomalies: Array<{ id: string; severity: string; status: string }> }) {
  return (
    <table>
      <tbody>
        {anomalies.map((anomaly) => (
          <tr key={anomaly.id}>
            <td>
              <SeverityBadge severity={anomaly.severity} />
            </td>
            <td>
              <StatusBadge status={anomaly.status} />
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

export function AnomalyDetailPanel() {
  return <section aria-label="Anomaly detail">Detail</section>;
}

