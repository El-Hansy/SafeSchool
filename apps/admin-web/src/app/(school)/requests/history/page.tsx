import { RequestsOperationsPage } from "../../../../features/requests";
import type { RequestOperationsFilters } from "../../../../features/requests";

type QueryValue = string | string[] | undefined;

export default async function Page({ searchParams }: { searchParams: Promise<Record<string, QueryValue>> }) {
  const params = await searchParams;
  return <RequestsOperationsPage view="history" audience="school" filters={toFilters(params)} />;
}

function toFilters(params: Record<string, QueryValue>): RequestOperationsFilters {
  return {
    studentProfileId: first(params.studentProfileId),
    requestType: first(params.requestType),
    status: first(params.status),
    sourceEventType: first(params.sourceEventType),
    exceptionState: first(params.exceptionState),
  };
}

function first(value: QueryValue) {
  return Array.isArray(value) ? value[0] : value;
}
