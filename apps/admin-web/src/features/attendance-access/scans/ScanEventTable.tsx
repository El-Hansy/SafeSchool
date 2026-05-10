import type { ScanEventSummary } from "./scansApi";

export function ScanDecisionBadge({ status }: { status: ScanEventSummary["status"] }) {
  return <span data-status={status}>{status}</span>;
}

export function ScanEventTable({ scans }: { scans: ScanEventSummary[] }) {
  return (
    <table>
      <thead>
        <tr>
          <th>Student</th>
          <th>Direction</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody>
        {scans.map((scan) => (
          <tr key={scan.scanEventId}>
            <td>{scan.studentProfileId}</td>
            <td>{scan.direction}</td>
            <td>
              <ScanDecisionBadge status={scan.status} />
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

export function ScanTracePanel({ references }: { references: string[] }) {
  return <ol>{references.map((reference) => <li key={reference}>{reference}</li>)}</ol>;
}

