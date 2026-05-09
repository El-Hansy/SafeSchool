import { GuardianEntryExitList } from "../../../features/attendance-access/guardian-entry-exit/GuardianEntryExitList";

export default function GuardianEntryExitPage() {
  return (
    <main>
      <h1>Entry and exit</h1>
      <GuardianEntryExitList items={[]} />
    </main>
  );
}

