import { serverApiAuthorizationHeader, serverApiBaseUrl } from "../../common/apiReadiness";

export type IdentityAccessErrorCode =
  | "feature_disabled"
  | "missing_permission"
  | "tenant_mismatch"
  | "validation_failed"
  | "not_found"
  | "unknown";

export class IdentityAccessClientError extends Error {
  constructor(
    public readonly code: IdentityAccessErrorCode,
    message: string,
    public readonly status: number,
    public readonly target?: string,
  ) {
    super(message);
  }
}

export type IdentityAccessRequestOptions = RequestInit & {
  schoolAccountId: string;
  actorReference?: string;
};

export function identityAccessPath(schoolAccountId: string, path: string) {
  const normalized = path.startsWith("/") ? path : `/${path}`;
  return `/api/v1/schools/${encodeURIComponent(schoolAccountId)}/identity${normalized}`;
}

export function identityHeaders(options: IdentityAccessRequestOptions) {
  return {
    "content-type": "application/json",
    "x-school-account-id": options.schoolAccountId,
    ...(options.actorReference ? { "x-actor-reference": options.actorReference } : {}),
    ...serverApiAuthorizationHeader(),
    ...Object.fromEntries(new Headers(options.headers).entries()),
  };
}

export async function requestIdentityAccess<T>(path: string, options: IdentityAccessRequestOptions): Promise<T> {
  const baseUrl = serverApiBaseUrl();
  const response = await fetch(`${baseUrl}${identityAccessPath(options.schoolAccountId, path)}`, {
    ...options,
    headers: identityHeaders(options),
  });

  if (!response.ok) {
    const payload = await response.json().catch(() => ({}));
    throw new IdentityAccessClientError(
      payload.code ?? "unknown",
      payload.message ?? "Identity access request failed.",
      response.status,
      payload.target,
    );
  }

  return (await response.json()) as T;
}
