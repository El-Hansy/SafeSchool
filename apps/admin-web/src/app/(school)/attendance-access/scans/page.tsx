import { ScanEventTable, ScanTracePanel } from "../../../../features/attendance-access/scans/ScanEventTable";

export default function ScansPage() {
  return (
    <main>
      <h1>Scan review</h1>
      <ScanEventTable scans={[]} />
      <ScanTracePanel references={[]} />
    </main>
  );
}

