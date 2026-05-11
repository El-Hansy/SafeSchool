import { NextResponse, type NextRequest } from "next/server";
import { serverApiAuthorizationHeader, serverApiBaseUrl } from "@/features/common/apiReadiness";

export const dynamic = "force-dynamic";

type ProxyPayload = {
  path?: unknown;
  schoolAccountId?: unknown;
  actorReference?: unknown;
  body?: unknown;
};

export async function POST(request: NextRequest) {
  const payload = (await request.json().catch(() => null)) as ProxyPayload | null;
  const path = typeof payload?.path === "string" ? payload.path : "";
  const schoolAccountId = typeof payload?.schoolAccountId === "string" ? payload.schoolAccountId : "";
  const actorReference = typeof payload?.actorReference === "string" ? payload.actorReference : "web-user";
  const baseUrl = serverApiBaseUrl();

  if (!baseUrl.trim()) {
    return NextResponse.json({ message: "SAFE_SCHOOL_API_BASE_URL or NEXT_PUBLIC_API_BASE_URL is required." }, { status: 500 });
  }

  if (!path.startsWith("/api/v1/") || path.includes("://")) {
    return NextResponse.json({ message: "SafeSchool API proxy rejected an invalid API path." }, { status: 400 });
  }

  if (!schoolAccountId.trim()) {
    return NextResponse.json({ message: "schoolAccountId is required." }, { status: 400 });
  }

  const response = await fetch(`${baseUrl}${path}`, {
    method: "POST",
    headers: {
      "content-type": "application/json",
      "x-school-account-id": schoolAccountId,
      "x-actor-reference": actorReference,
      ...serverApiAuthorizationHeader(),
    },
    body: JSON.stringify(payload?.body ?? {}),
    cache: "no-store",
  });

  const text = await response.text();
  return new NextResponse(text, {
    status: response.status,
    headers: {
      "content-type": response.headers.get("content-type") ?? "application/json",
    },
  });
}

