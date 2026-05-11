import { MedicalOperationsPage } from "../../../../features/medical";
import type { MedicalOperationsFilters } from "../../../../features/medical";

type QueryValue = string | string[] | undefined;

export default async function Page({ searchParams }: { searchParams: Promise<Record<string, QueryValue>> }) {
  const params = await searchParams;
  return <MedicalOperationsPage view="history" audience="school" filters={toFilters(params)} />;
}

function toFilters(params: Record<string, QueryValue>): MedicalOperationsFilters {
  return {
    studentProfileId: first(params.studentProfileId),
    recordType: first(params.recordType),
    status: first(params.status),
    severity: first(params.severity),
    sourceEventType: first(params.sourceEventType),
    reviewState: first(params.reviewState),
  };
}

function first(value: QueryValue) {
  return Array.isArray(value) ? value[0] : value;
}
