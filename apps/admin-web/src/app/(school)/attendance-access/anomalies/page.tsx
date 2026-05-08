import { AnomalyRunForm } from "../../../../features/attendance-access/anomalies/AnomalyRunForm";
import { AnomalyDetailPanel, AnomalyTable } from "../../../../features/attendance-access/anomalies/AnomalyTable";

export default function AnomaliesPage() {
  return (
    <main>
      <h1>Anomalies</h1>
      <AnomalyRunForm />
      <AnomalyTable anomalies={[]} />
      <AnomalyDetailPanel />
    </main>
  );
}

