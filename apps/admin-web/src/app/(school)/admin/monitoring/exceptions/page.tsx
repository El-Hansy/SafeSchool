import { OperationalRoutePage } from "@/features/home";

export default function Page() {
  return (
    <OperationalRoutePage
      area="Administration / Monitoring"
      title="Operational Exceptions"
      detail="Review cross-module exceptions that need staff action without mutating attendance, transport, wallet, or document records."
    />
  );
}
